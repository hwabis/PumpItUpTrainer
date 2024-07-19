using System;
using System.Collections.Generic;
using System.Linq;

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

        // Banned patterns:
        // 1. [Right foot] DL C UL
        // 2. [Right foot] UL C DL
        // 3. [Left foot] DR C UR
        // 4. [Left foot] UR C DR
        public static List<Note> GenerateNoteSequence(int noteCount, Foot startingFoot, List<Note> allowedNotes, bool repeatsAllowed)
        {
            if (noteCount < 1)
            {
                throw new Exception("🤔");
            }

            List<Note> generatedNotes = new();

            List<Note> leftFootStartingNotes = [Note.P1DL, Note.P1UL, Note.P1C, Note.P1UR, Note.P1DR];
            List<Note> rightFootStartingNotes = [Note.P2DL, Note.P2UL, Note.P2C, Note.P2UR, Note.P2DR];
            Foot currentFoot;

            if (startingFoot == Foot.Left)
            {
                generatedNotes.Add(leftFootStartingNotes[random.Next(5)]);
                currentFoot = Foot.Right;
            }
            else
            {
                generatedNotes.Add(rightFootStartingNotes[random.Next(5)]);
                currentFoot = Foot.Left;
            }

            for (int i = 1; i < noteCount; i++)
            {
                Note mostRecentNote = generatedNotes.Last();

                if (currentFoot == Foot.Left)
                {
                    List<Note> candidateNotes = nextNotesCurrentFootLeft[mostRecentNote];
                    generatedNotes.Add(candidateNotes[random.Next(candidateNotes.Count)]);
                    currentFoot = Foot.Right;
                }
                else
                {
                    List<Note> candidateNotes = nextNotesCurrentFootRight[mostRecentNote];
                    generatedNotes.Add(candidateNotes[random.Next(candidateNotes.Count)]);
                    currentFoot = Foot.Left;
                }
            }

            return generatedNotes;
        }
    }
}
