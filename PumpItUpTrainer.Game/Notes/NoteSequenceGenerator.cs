using System;
using System.Collections.Generic;
using System.Linq;

namespace PumpItUpTrainer.Game.Notes
{
    public static class NoteSequenceGenerator
    {
        private static Dictionary<Note, List<Note>> nextNotesLastFootLeft = new()
        {
            { Note.P1DL, [Note.P1UL, Note.P1C, Note.P1UR, Note.P1DR] },
            { Note.P1UL, [Note.P1DL, Note.P1C, Note.P1UR, Note.P1DR] },
            { Note.P1C, [Note.P1DL, Note.P1UL, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL] },
            { Note.P1UR, [Note.P1C, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C] },
            { Note.P1DR, [Note.P1C, Note.P1UR, Note.P2DL, Note.P2UL, Note.P2C] },
            { Note.P2DL, [Note.P1UR, Note.P2UL, Note.P2C, Note.P2UR, Note.P2DR] },
            { Note.P2UL, [Note.P1DR, Note.P2DL, Note.P2C, Note.P2UR, Note.P2DR] },
            { Note.P2C, [Note.P2DL, Note.P2UL, Note.P2UR, Note.P2DR] },
            { Note.P2UR, [Note.P2C, Note.P2DR] },
            { Note.P2DR, [Note.P2C, Note.P2UR] },
        };

        private static Dictionary<Note, List<Note>> nextNotesLastFootRight = [];

        private static Dictionary<Note, List<Note>> nextNotesLastFootLeftHard = new()
        {
            { Note.P1UR, [Note.P1DL] },
            { Note.P1DR, [Note.P1UL] },
            { Note.P2DL, [Note.P1C] },
            { Note.P2UL, [Note.P1C] },
            { Note.P2C, [Note.P1UR, Note.P1DR] },
            { Note.P2UR, [Note.P2DL] },
            { Note.P2DR, [Note.P2UL] },
        };

        private static Dictionary<Note, List<Note>> nextNotesLastFootRightHard = [];

        private static Dictionary<Note, Note> horizontalFlips = new()
        {
            { Note.P1DL, Note.P2DR },
            { Note.P1UL, Note.P2UR },
            { Note.P1C, Note.P2C },
            { Note.P1UR, Note.P2UL },
            { Note.P1DR, Note.P2DL },
            { Note.P2DL, Note.P1DR },
            { Note.P2UL, Note.P1UR },
            { Note.P2C, Note.P1C },
            { Note.P2UR, Note.P1UL },
            { Note.P2DR, Note.P1DL },
        };

        private static Random random = new();

        static NoteSequenceGenerator()
        {
            foreach (var entry in nextNotesLastFootLeft)
            {
                Note flippedNote = horizontalFlips[entry.Key];
                List<Note> flippedValues = nextNotesLastFootLeft[flippedNote].Select(n => horizontalFlips[n]).ToList();
                nextNotesLastFootRight[entry.Key] = flippedValues;
            }

            foreach (var entry in nextNotesLastFootLeftHard)
            {
                Note flippedNote = horizontalFlips[entry.Key];

                if (nextNotesLastFootLeftHard.ContainsKey(flippedNote))
                {
                    List<Note> flippedValues = nextNotesLastFootLeftHard[flippedNote].Select(n => horizontalFlips[n]).ToList();
                    nextNotesLastFootRightHard[entry.Key] = flippedValues;
                }
            }
        }

        public static List<Note> GenerateNoteSequence(int noteCount, Foot startingFoot, List<Note> allowedNotes, bool hardModeOn, bool allowWideSwings)
        {
            if (noteCount < 1)
            {
                return new();
            }

            List<Note> generatedNotes = [allowedNotes[random.Next(allowedNotes.Count)]];

            Foot nextFoot = swapFoot(startingFoot);

            for (int i = 1; i < noteCount; i++)
            {
                Note lastNote = generatedNotes.Last();

                List<Note> candidateNotes = (nextFoot == Foot.Left ? nextNotesLastFootRight[lastNote] : nextNotesLastFootLeft[lastNote]).ToList();

                if (hardModeOn)
                {
                    bool hardListExists = nextFoot == Foot.Left ?
                        nextNotesLastFootRightHard.ContainsKey(lastNote) : nextNotesLastFootLeftHard.ContainsKey(lastNote);

                    if (hardListExists)
                    {
                        List<Note> hardCandidateNotes =
                            (nextFoot == Foot.Left ? nextNotesLastFootRightHard[lastNote] : nextNotesLastFootLeftHard[lastNote]).ToList();
                        candidateNotes.AddRange(hardCandidateNotes);
                    }
                }

                allowOnlyAllowedNotes(allowedNotes, candidateNotes);
                banCandidateNotesCausingBannedPatterns(generatedNotes, swapFoot(nextFoot), candidateNotes, allowWideSwings);

                if (candidateNotes.Count == 0)
                    throw new Exception("???");

                if (candidateNotes.Contains(generatedNotes.Last()))
                    throw new Exception("???");

                generatedNotes.Add(getRandomNextNote(generatedNotes, candidateNotes, 4));

                nextFoot = swapFoot(nextFoot);
            }

            return generatedNotes;
        }

        private static void allowOnlyAllowedNotes(List<Note> allowedNotes, List<Note> notesToFilter)
        {
            foreach (Note note in notesToFilter.ToList())
            {
                if (!allowedNotes.Contains(note))
                {
                    notesToFilter.Remove(note);
                }
            }
        }

        private static void banCandidateNotesCausingBannedPatterns(
            List<Note> noteSequence, Foot lastNoteFoot, List<Note> candidateNotesToBanFrom, bool allowWideSwings)
        {
            if (noteSequence.Count <= 1)
            {
                return;
            }

            Note lastNote = noteSequence.Last();
            Note secondToLastNote = noteSequence[^2];

            // Ban patterns that would only be comfortable if you spin around
            // [RLR] DL C UL
            // [RLR] UL C DL
            // [LRL] DR C UR
            // [LRL] UR C DR

            if (lastNoteFoot == Foot.Left)
            {
                if (lastNote == Note.P1C)
                {
                    if (secondToLastNote == Note.P1DL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1UL);
                    }
                    else if (secondToLastNote == Note.P1UL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1DL);
                    }
                }
                else if (lastNote == Note.P2C)
                {
                    if (secondToLastNote == Note.P2DL || secondToLastNote == Note.P1DR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2UL);
                        candidateNotesToBanFrom.Remove(Note.P1UR);
                    }
                    else if (secondToLastNote == Note.P2UL || secondToLastNote == Note.P1UR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2DL);
                        candidateNotesToBanFrom.Remove(Note.P1DR);
                    }
                }
            }
            else if (lastNoteFoot == Foot.Right)
            {
                if (lastNote == Note.P1C)
                {
                    if (secondToLastNote == Note.P1DR || secondToLastNote == Note.P2DL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1UR);
                        candidateNotesToBanFrom.Remove(Note.P2UL);
                    }
                    else if (secondToLastNote == Note.P1UR || secondToLastNote == Note.P2UL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1DR);
                        candidateNotesToBanFrom.Remove(Note.P2DL);
                    }
                }
                else if (lastNote == Note.P2C)
                {
                    if (secondToLastNote == Note.P2DR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2UR);
                    }
                    else if (secondToLastNote == Note.P2UR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2DR);
                    }
                }
            }

            if (!allowWideSwings)
            {
                // Ban 180 degree twists

                if (lastNote == Note.P1C)
                {
                    if (secondToLastNote == Note.P1DL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1UR);
                    }
                    else if (secondToLastNote == Note.P1UL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1DR);
                    }
                    else if (secondToLastNote == Note.P1DR || secondToLastNote == Note.P2DL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1UL);
                    }
                    else if (secondToLastNote == Note.P1UR || secondToLastNote == Note.P2UL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1DL);
                    }
                }
                else if (lastNote == Note.P2C)
                {
                    if (secondToLastNote == Note.P2DR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2UL);
                    }
                    else if (secondToLastNote == Note.P2UR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2DL);
                    }
                    else if (secondToLastNote == Note.P1DR || secondToLastNote == Note.P2DL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2UR);
                    }
                    else if (secondToLastNote == Note.P1UR || secondToLastNote == Note.P2UL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2DR);
                    }
                }

                // Ban ... long horizontal swings.

                if (lastNote == Note.P1C)
                {
                    if (secondToLastNote == Note.P2UL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1UL);
                    }
                    if (secondToLastNote == Note.P2DL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1DL);
                    }
                }
                else if (lastNote == Note.P2C)
                {
                    if (secondToLastNote == Note.P1UR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2UR);
                    }
                    if (secondToLastNote == Note.P1DR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2DR);
                    }
                }
            }
        }

        // Less likely to use a candidate note that matches the second to last note.
        // Nobody wants to play endless trills :)
        private static Note getRandomNextNote(List<Note> existingNotes, List<Note> candidateNotes, int additionalNonRepeatWeight)
        {
            if (existingNotes.Count < 2)
                return candidateNotes[random.Next(candidateNotes.Count)];

            Note secondToLastNote = existingNotes[^2];
            List<Note> candidateNoteWeighted = [];

            foreach (Note candidateNote in candidateNotes)
            {
                candidateNoteWeighted.Add(candidateNote);

                if (candidateNote != secondToLastNote)
                {
                    for (int i = 0; i < additionalNonRepeatWeight; i++)
                    {
                        candidateNoteWeighted.Add(candidateNote);
                    }
                }
            }

            return candidateNoteWeighted[random.Next(candidateNoteWeighted.Count)];
        }

        private static Foot swapFoot(Foot currentFoot)
        {
            return currentFoot == Foot.Left ? Foot.Right : Foot.Left;
        }
    }
}
