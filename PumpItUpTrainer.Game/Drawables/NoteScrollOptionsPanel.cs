using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using PumpItUpTrainer.Game.Notes;

namespace PumpItUpTrainer.Game.Drawables
{
    public partial class NoteScrollOptionsPanel : Container
    {
        private NoteScrollPlayer notePlayer;

        private FillFlowContainer optionsContainer = null!;
        private TextBox bpm = null!;
        private TextBox scrollTimeMs = null!;
        private TextBox noteCount = null!;
        private Dropdown<Foot> startingFoot = null!;
        private Dropdown<NoteConfig> noteConfigs = null!;
        private Button playButton = null!;

        private Button stopButton = null!;

        public NoteScrollOptionsPanel(NoteScrollPlayer notePlayer)
        {
            this.notePlayer = notePlayer;
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
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Children =
                        [
                            new SpriteText
                            {
                                Text = "BPM: "
                            },
                            bpm = new BasicTextBox
                            {
                                Size = new(50, 25)
                            }
                        ]
                    },
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Children =
                        [
                            new SpriteText
                            {
                                Text = "Scroll time (ms): "
                            },
                            scrollTimeMs = new BasicTextBox
                            {
                                Size = new(50, 25)
                            }
                        ]
                    },
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Children =
                        [
                            new SpriteText
                            {
                                Text = "Note count: "
                            },
                            noteCount = new BasicTextBox
                            {
                                Size = new(50, 25)
                            }
                        ]
                    },
                    startingFoot = new BasicDropdown<Foot>
                    {
                        Width = 100,
                        Margin = new(5),
                        Items =
                        [
                            Foot.Left,
                            Foot.Right,
                        ]
                    },
                    noteConfigs = new BasicDropdown<NoteConfig>
                    {
                        Width = 100,
                        Margin = new(5),
                        Items =
                        [
                            NoteConfig.Middle6,
                            NoteConfig.MiddleLeft5,
                            NoteConfig.MiddleRight5,
                            NoteConfig.Single5,
                            NoteConfig.Middle4,
                            NoteConfig.Left3,
                            NoteConfig.Right3,
                            NoteConfig.Left7,
                            NoteConfig.Right7,
                            NoteConfig.All10,
                        ]
                    },
                    playButton = new BasicButton
                    {
                        Size = new(50, 25),
                        Margin = new(5),
                        Text = "Play",
                        Action = () =>
                        {
                            try
                            {
                                double totalTime = notePlayer.GenerateAndPlayNotes(
                                    int.Parse(bpm.Text),
                                    int.Parse(scrollTimeMs.Text),
                                    int.Parse(noteCount.Text),
                                    startingFoot.Current.Value,
                                    configNotes[noteConfigs.Current.Value]);

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
                    notePlayer.Stop();
                    optionsContainer.Show();
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            stopButton.Hide();
        }

        private Dictionary<NoteConfig, List<Note>> configNotes = new()
        {
            { NoteConfig.Middle6, [Note.P1C, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C]},
            { NoteConfig.MiddleLeft5, [Note.P1C, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL]},
            { NoteConfig.MiddleRight5, [Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C]},
            { NoteConfig.Single5, [Note.P1DL, Note.P1UL, Note.P1C, Note.P1UR, Note.P1DR]},
            { NoteConfig.Middle4, [Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL]},
            { NoteConfig.Left3, [Note.P1C, Note.P1UR, Note.P1DR]},
            { NoteConfig.Right3, [Note.P2DL, Note.P2UL, Note.P2C]},
            { NoteConfig.Left7, [Note.P1DL, Note.P1UL, Note.P1C, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL]},
            { NoteConfig.Right7, [Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C, Note.P2UR, Note.P2DR]},
            { NoteConfig.All10, [Note.P1DL, Note.P1UL, Note.P1C, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C, Note.P2UR, Note.P2DR]},
        };

        // todo: come up with a way to independently select notes so we don't need a config for every possible combination 🤣
        private enum NoteConfig
        {
            Middle6,
            MiddleLeft5,
            MiddleRight5,
            Single5,
            Middle4,
            Left3,
            Right3,
            Left7,
            Right7,
            All10,
        }
    }
}
