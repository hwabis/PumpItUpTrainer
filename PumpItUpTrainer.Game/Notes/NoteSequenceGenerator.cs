using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace PumpItUpTrainer.Game.Notes
{
    internal static class NoteSequenceGenerator
    {
        private static Dictionary<Note, List<Note>> nextNotesCurrentFootLeft = new()
        {
            { Note.P1DL, [Note.P1UL, Note.P1C, Note.P1UR, Note.P1DR] },
            { Note.P1UL, [Note.P1DL, Note.P1C, Note.P1UR, Note.P1DR] },
            { Note.P1C, [Note.P1DL, Note.P1UL, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL] },
            { Note.P1UR, [Note.P1C, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C] },
            { Note.P1DR, [Note.P1C, Note.P1UR, Note.P2DL, Note.P2UL, Note.P2C] },
            { Note.P2DL, [Note.P1UR, Note.P2UR, Note.P2C, Note.P2UR, Note.P2DR] },
            { Note.P2UL, [Note.P1DR, Note.P2DL, Note.P2C, Note.P2UR, Note.P2DR] },
            { Note.P2C, [Note.P2DL, Note.P2UL, Note.P2UR, Note.P2DL] },
            { Note.P2UR, [Note.P2C, Note.P2DR] },
            { Note.P2DR, [Note.P2C, Note.P2UR] },
        };

        private static Dictionary<Note, List<Note>> nextNotesCurrentFootRight = [];

        private static Dictionary<Note, Note> horizontalFlips = new()
        {
            { Note.P1DL, Note.P2DR },
            { Note.P1UL, Note.P2UR },
            { Note.P1C, Note.P2C },
            { Note.P1UR, Note.P2UL },
            { Note.P1DR, Note.P2DL },
            { Note.P2DL, Note.P1DL },
            { Note.P2UL, Note.P1UR },
            { Note.P2C, Note.P1C },
            { Note.P2UR, Note.P1UL },
            { Note.P2DR, Note.P1DL },
        };

        private static Random random = new();

        static NoteSequenceGenerator()
        {
            foreach (var entry in nextNotesCurrentFootLeft)
            {
                List<Note> flippedValues = entry.Value.Select(n => horizontalFlips[n]).ToList();
                nextNotesCurrentFootRight[entry.Key] = flippedValues;
            }
        }

        public static List<Note> GenerateNoteSequence(int noteCount, Foot startingFoot, List<Note> allowedNotes)
        {
            if (noteCount < 1)
            {
                return new();
            }

            List<Note> generatedNotes = new();

            List<Note> leftFootStartingNotes = [Note.P1DL, Note.P1UL, Note.P1C, Note.P1UR, Note.P1DR];
            List<Note> rightFootStartingNotes = [Note.P2DL, Note.P2UL, Note.P2C, Note.P2UR, Note.P2DR];

            allowOnlyAllowedNotes(allowedNotes, leftFootStartingNotes);
            allowOnlyAllowedNotes(allowedNotes, rightFootStartingNotes);

            generatedNotes.Add(startingFoot == Foot.Left ? leftFootStartingNotes[leftFootStartingNotes.Count] : rightFootStartingNotes[rightFootStartingNotes.Count]);

            Foot currentFoot = swapFoot(startingFoot);

            for (int i = 1; i < noteCount; i++)
            {
                Note currentNote = generatedNotes.Last();

                List<Note> candidateNotes = currentFoot == Foot.Left ? nextNotesCurrentFootLeft[currentNote] : nextNotesCurrentFootRight[currentNote];

                allowOnlyAllowedNotes(allowedNotes, candidateNotes);
                banCandidateNotesCausingBannedPatterns(generatedNotes, currentFoot, candidateNotes);

                generatedNotes.Add(candidateNotes[random.Next(candidateNotes.Count)]);

                currentFoot = swapFoot(currentFoot);
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

        // Banned patterns:
        // [RLR] DL C UL
        // [RLR] UL C DL
        // [LRL] DR C UR
        // [LRL] UR C DR
        private static void banCandidateNotesCausingBannedPatterns(List<Note> noteSequence, Foot currentFoot, List<Note> candidateNotesToBanFrom)
        {
            if (noteSequence.Count <= 1)
            {
                return;
            }

            Note lastNote = noteSequence.Last();
            Note secondToLastNote = noteSequence[^2];

            if (currentFoot == Foot.Left)
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
                    if (secondToLastNote == Note.P2DL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2UL);
                    }
                    else if (secondToLastNote == Note.P2UL)
                    {
                        candidateNotesToBanFrom.Remove(Note.P2DL);
                    }
                }
            }
            else if (currentFoot == Foot.Right)
            {
                if (lastNote == Note.P1C)
                {
                    if (secondToLastNote == Note.P1DR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1UR);
                    }
                    else if (secondToLastNote == Note.P1UR)
                    {
                        candidateNotesToBanFrom.Remove(Note.P1DR);
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
        }

        private static Foot swapFoot(Foot currentFoot)
        {
            return currentFoot == Foot.Left ? Foot.Right : Foot.Left;
        }
    }
}
