using osu.Framework.Platform;
using osu.Framework;
using PumpItUpTrainer.Game;

namespace PumpItUpTrainer.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"PumpItUpTrainer"))
            using (osu.Framework.Game game = new PumpItUpTrainerGame())
                host.Run(game);
        }
    }
}
