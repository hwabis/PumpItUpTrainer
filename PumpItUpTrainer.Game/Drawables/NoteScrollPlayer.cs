using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using PumpItUpTrainer.Game.Notes;

namespace PumpItUpTrainer.Game.Drawables
{
    public partial class NoteScrollPlayer : CompositeDrawable
    {
        private Drawable noteToDrawable(Note note)
        {
            switch (note)
            {
                case Note.P1DL:
                    return new DrawableArrowNote { Colour = Colour4.Blue, Rotation = -180, X = noteXPositions[0] };
                case Note.P1UL:
                    return new DrawableArrowNote { Colour = Colour4.Red, Rotation = -90, X = noteXPositions[1] };
                case Note.P1C:
                    return new DrawableSquareNote { X = noteXPositions[2] };
                case Note.P1UR:
                    return new DrawableArrowNote { Colour = Colour4.Red, Rotation = 0, X = noteXPositions[3] };
                case Note.P1DR:
                    return new DrawableArrowNote { Colour = Colour4.Blue, Rotation = 90, X = noteXPositions[4] };
                case Note.P2DL:
                    return new DrawableArrowNote { Colour = Colour4.Blue, Rotation = -180, X = noteXPositions[5] };
                case Note.P2UL:
                    return new DrawableArrowNote { Colour = Colour4.Red, Rotation = -90, X = noteXPositions[6] };
                case Note.P2C:
                    return new DrawableSquareNote { X = noteXPositions[7] };
                case Note.P2UR:
                    return new DrawableArrowNote { Colour = Colour4.Red, Rotation = 0, X = noteXPositions[8] };
                case Note.P2DR:
                    return new DrawableArrowNote { Colour = Colour4.Blue, Rotation = 90, X = noteXPositions[9] };
                default:
                    throw new Exception("d=====(￣▽￣*)b");
            }
        }

        private static int[] noteXPositions =
        [
            40 - 80 * 5,
            40 - 80 * 4,
            40 - 80 * 3,
            40 - 80 * 2,
            40 - 80 * 1,
            40,
            40 + 80 * 1,
            40 + 80 * 2,
            40 + 80 * 3,
            40 + 80 * 4
        ];

        private Container topRowNotesContainer = null!;

        private const int offset_from_top = 30;

        private Sample hitSample = null!;

        [BackgroundDependencyLoader]
        private void load(ISampleStore sampleStore)
        {
            hitSample = sampleStore.Get("soft-hitclap.wav");

            RelativeSizeAxes = Axes.Both;

            addTopRowNotes();
        }

        // returns the total time it will take
        public double GenerateAndPlayNotes(double bpm, double noteTravelTimeMs, int noteCount, Foot startingFoot, List<Note> allowedNotes)
        {
            ClearInternal();
            addTopRowNotes();

            List<Note> notes = NoteSequenceGenerator.GenerateNoteSequence(noteCount, startingFoot, allowedNotes);

            double nextNoteStartingTime = 1000;
            double msBetweenNotes = getTimeMsBetweenNotes(bpm);

            foreach (Note note in notes)
            {
                Drawable drawableNote;
                AddInternal(drawableNote = noteToDrawable(note));
                drawableNote.Y = 850; // idk lol anywhere off screen
                drawableNote.Delay(nextNoteStartingTime).Then().MoveToY(topRowNotesContainer.Position.Y, noteTravelTimeMs).Finally(d =>
                {
                    RemoveInternal(d, false); // bro idk what disposing immediately means but it crashes everything lol
                    hitSample.Play();
                });

                nextNoteStartingTime += msBetweenNotes;
            }

            return nextNoteStartingTime + noteTravelTimeMs - msBetweenNotes;
        }

        private void addTopRowNotes()
        {
            AddInternal(topRowNotesContainer = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Colour = Colour4.Gray,
                Y = offset_from_top,
                Children =
                [
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = -180, X = noteXPositions[0] },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = -90, X = noteXPositions[1] },
                    new DrawableSquareNote { Anchor = Anchor.Centre, X = noteXPositions[2] },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = 0, X = noteXPositions[3] },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = 90, X = noteXPositions[4] },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = -180, X = noteXPositions[5] },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = -90, X = noteXPositions[6] },
                    new DrawableSquareNote { Anchor = Anchor.Centre, X = noteXPositions[7] },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = 0, X = noteXPositions[8] },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = 90, X = noteXPositions[9] },
                ],
            });
        }

        private double getTimeMsBetweenNotes(double bpm)
        {
            double beatsPerSecond = bpm / 60;
            double secondsPerBeat = 1 / beatsPerSecond;
            double msPerBeat = secondsPerBeat * 1000;
            double msPerNote = msPerBeat / 4;

            return msPerNote;
        }
    }
}
