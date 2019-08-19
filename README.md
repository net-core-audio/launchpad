# Launchpad
A library can enable to talk to your Launchpad Mini device in .NET & .NET Core. You can light buttons, listen pressed keys

Developed this library on [Novation Launchpad Mini](https://novationmusic.com/launch/launchpad-mini) But you can contribute to extend library for another launchpads

## Getting Started

1. You have to install "PortMidi" library on your computer

### Linux (Ubuntu)

```bash
$ apt-get install libportmidi0
```

### Mac OS

```bash
$ brew install portmidi
```

### Windows

```cmd
choco install portmidi
```

2. Install launchpad library on your project

```bash
$ dotnet add package launchpad
```

## Example

Basic example usage of launchpad library

```cs
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
```

## Contributing

* If you want to contribute to codes, create pull request
* If you find any bugs or error, create an issue

## License

This project is licensed under the MIT License
