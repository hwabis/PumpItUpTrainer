using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using PumpItUpTrainer.Game.Notes;

namespace PumpItUpTrainer.Game.Drawables
{
    public partial class NoteVisualizerOptionsPanel : Container
    {
        private NoteVisualizer noteVisualizer;

        private FillFlowContainer optionsContainer = null!;
        private TextBox bpm = null!;
        private TextBox scrollTimeMs = null!;
        private TextBox noteCount = null!;
        private Dropdown<Foot> startingFoot = null!;
        private NoteCheckboxRowSelector selectedNotes = null!;
        private Checkbox hardMode = null!;
        private Button playButton = null!;

        private Button stopButton = null!;

        public NoteVisualizerOptionsPanel(NoteVisualizer noteVisualizer)
        {
            this.noteVisualizer = noteVisualizer;
            AutoSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(optionsContainer = new FillFlowContainer
            {
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Both,
                Children =
                [
                    new TextWithAdjacentDrawable("BPM: ", bpm = new BasicTextBox
                    {
                        Size = new(50, 25)
                    }),
                    new TextWithAdjacentDrawable("Scroll time (ms): ", scrollTimeMs = new BasicTextBox
                    {
                        Size = new(50, 25)
                    }),
                    new TextWithAdjacentDrawable("Note count: ", noteCount = new BasicTextBox
                    {
                        Size = new(50, 25)
                    }),
                    new TextWithAdjacentDrawable("Starting foot: ", startingFoot = new BasicDropdown<Foot>
                    {
                        Width = 100,
                        Margin = new(5),
                        Items =
                                [
                                    Foot.Left,
                                    Foot.Right,
                                ]
                    }),
                    new TextWithAdjacentDrawable("Available notes:", selectedNotes = new NoteCheckboxRowSelector
                    {
                        Margin = new(5)
                    }),
                    new TextWithAdjacentDrawable("Include large crossovers:", hardMode = new BasicCheckbox()),
                    playButton = new BasicButton
                    {
                        Size = new(50, 25),
                        Margin = new(5),
                        Text = "Play",
                        Action = () =>
                        {
                            if (selectedNotes.GetSelectedNotes().Count <= 2)
                            {
                                return;
                            }

                            try
                            {
                                double totalTime = noteVisualizer.GenerateAndPlayNotes(
                                    int.Parse(bpm.Text),
                                    int.Parse(scrollTimeMs.Text),
                                    int.Parse(noteCount.Text),
                                    startingFoot.Current.Value,
                                    selectedNotes.GetSelectedNotes(),
                                    hardMode.Current.Value);

                                optionsContainer.FadeOut().Delay(totalTime).Then().FadeIn().Finally(_ => stopButton.Hide());
                                stopButton.Show();

                                // none of the things i tried below worked LOL
                                // Hide();
                                // Schedule(_ => Show(), totalTime);
                                // this.FadeOut(0).Then().Delay(totalTime).Finally(_ => Show());
                            }
                            catch (ArgumentNullException) { }
                            catch (FormatException) { }
                        }
                    },
                ]
            }
            );

            AddInternal(stopButton = new BasicButton
            {
                Size = new(50, 25),
                Text = "Stop",
                Action = () =>
                {
                    stopButton.Hide();
                    noteVisualizer.Stop();
                    optionsContainer.Show();
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            stopButton.Hide();
        }

        private partial class TextWithAdjacentDrawable : FillFlowContainer
        {
            private string text;
            private Drawable adjacentDrawable;

            public TextWithAdjacentDrawable(string text, Drawable adjacentDrawable)
            {
                this.text = text;
                this.adjacentDrawable = adjacentDrawable;

                AutoSizeAxes = Axes.Both;
                Direction = FillDirection.Horizontal;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Children =
                [
                    new SpriteText
                    {
                        Text = text,
                    },
                    adjacentDrawable,
                ];
            }
        }
    }
}
