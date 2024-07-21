using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using PumpItUpTrainer.Game.Notes;
using static PumpItUpTrainer.Game.Drawables.NoteCheckboxRowSelector;

namespace PumpItUpTrainer.Game.Drawables
{
    public partial class NoteCheckboxRowSelector : FillFlowContainer<AssociatedNoteCheckbox>
    {
        public NoteCheckboxRowSelector()
        {
            AutoSizeAxes = Axes.Both;
            Direction = FillDirection.Horizontal;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal([
                new AssociatedNoteCheckbox(Note.P1DL),
                new AssociatedNoteCheckbox(Note.P1UL),
                new AssociatedNoteCheckbox(Note.P1C),
                new AssociatedNoteCheckbox(Note.P1UR),
                new AssociatedNoteCheckbox(Note.P1DR),
                new AssociatedNoteCheckbox(Note.P2DL),
                new AssociatedNoteCheckbox(Note.P2UL),
                new AssociatedNoteCheckbox(Note.P2C),
                new AssociatedNoteCheckbox(Note.P2UR),
                new AssociatedNoteCheckbox(Note.P2DR),
            ]);
        }

        public List<Note> GetSelectedNotes()
        {
            List<Note> selectedNotes = [];

            foreach (AssociatedNoteCheckbox checkBox in Children)
            {
                if (checkBox.Current.Value == true)
                {
                    selectedNotes.Add(checkBox.AssociatedNote);
                }
            }

            return selectedNotes;
        }

        public partial class AssociatedNoteCheckbox : BasicCheckbox
        {
            public Note AssociatedNote { get; private set; }

            public AssociatedNoteCheckbox(Note associatedNote)
            {
                AssociatedNote = associatedNote;
                Margin = new(1);
            }
        }
    }
}
