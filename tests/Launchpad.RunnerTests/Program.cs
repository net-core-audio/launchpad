using System;
using System.Collections.Generic;

namespace Launchpad.RunnerTests
{
    class Program
    {
        static readonly LaunchpadManager LaunchpadManager = new LaunchpadManager();
        static readonly Dictionary<string, int> Histories = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            if (!LaunchpadManager.FindDevice())
            {
                Console.WriteLine("Cannot find any Launchpad Device");
                return;
            }

            if (!LaunchpadManager.Open())
            {
                Console.WriteLine("Cannot open Launchpad device");
                return;
            }

            LaunchpadManager.Clear();

            LaunchpadManager.ListenStart(KeyPressed);
            Console.ReadKey();
            LaunchpadManager.ListenStop();
        }


        static void KeyPressed(LaunchpadEventArgs e)
        {
            string key = $"X:{e.Hit.X} Y:{e.Hit.Y}";

            if (!Histories.ContainsKey(key))
            {
                Histories.Add(key, 1);
            }

            Console.WriteLine($"Pressed: {key}");
            LaunchpadManager.Light(e.Hit, (LaunchpadColor) Histories[key]);
            if ((LaunchpadColor) Histories[key] == LaunchpadColor.Red)
            {
                Histories[key] = 0;
            }
            else
            {
                Histories[key]++;
            }
        }
    }
}