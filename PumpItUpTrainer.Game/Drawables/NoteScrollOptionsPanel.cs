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
    public partial class NoteScrollOptionsPanel : FillFlowContainer
    {
        private NoteScrollPlayer notePlayer;

        private TextBox bpm = null!;
        private TextBox scrollTimeMs = null!;
        private TextBox noteCount = null!;
        private Dropdown<Foot> startingFoot = null!;
        private Dropdown<NoteConfig> noteConfigs = null!;
        private Button playButton = null!;

        public NoteScrollOptionsPanel(NoteScrollPlayer notePlayer)
        {
            this.notePlayer = notePlayer;
            Direction = FillDirection.Vertical;
            AutoSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal([
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
                    Items = [
                        Foot.Left,
                        Foot.Right,
                    ]
                },
                noteConfigs = new BasicDropdown<NoteConfig>
                {
                    Width = 100,
                    Margin = new(5),
                    Items = [
                        NoteConfig.InMiddle6,
                        NoteConfig.Single5,
                        NoteConfig.InMiddle4,
                        NoteConfig.Left3,
                        NoteConfig.Right3,
                        NoteConfig.All10
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

                            this.FadeOut(0).Delay(totalTime).Then().FadeIn(0);

                            // none of the things i tried below worked LOL
                            // Hide();
                            // Schedule(_ => Show(), totalTime);
                            // this.FadeOut(0).Then().Delay(totalTime).Finally(_ => Show());
                        }
                        catch (ArgumentNullException) { }
                        catch (FormatException) { }
                    }
                }
            ]);
        }

        private Dictionary<NoteConfig, List<Note>> configNotes = new()
        {
            { NoteConfig.InMiddle6, [Note.P1C, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C]},
            { NoteConfig.Single5, [Note.P1DL, Note.P1UL, Note.P1C, Note.P1UR, Note.P1DR]},
            { NoteConfig.InMiddle4, [Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL]},
            { NoteConfig.Left3, [Note.P1C, Note.P1UR, Note.P1DR]},
            { NoteConfig.Right3, [Note.P2DL, Note.P2UL, Note.P2C]},
            { NoteConfig.All10, [Note.P1DL, Note.P1UL, Note.P1C, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C, Note.P2UR, Note.P2DR]},
        };

        private enum NoteConfig
        {
            InMiddle6,
            Single5,
            InMiddle4,
            Left3,
            Right3,
            All10,
        }
    }
}
