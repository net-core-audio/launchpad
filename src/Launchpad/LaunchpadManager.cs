using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PortMidi;

namespace Launchpad
{
    public class LaunchpadManager
    {
        private List<MidiDeviceInfo> MidiDevices { get; set; }
        private MidiInput Input { get; set; }
        private MidiOutput Output { get; set; }
        private CancellationTokenSource ListenTask { get; set; } = new CancellationTokenSource();

        public bool FindDevice()
        {
            MidiDevices = MidiDeviceManager.AllDevices?.Where(x => x.Name == "Launchpad Mini").ToList();

            return MidiDevices?.Count > 0;
        }

        public bool Open()
        {
            var inputDevice = MidiDevices.FirstOrDefault(device => device.IsInput);

            if (inputDevice.Equals(default))
            {
                throw new Exception("Cannot find any device");
            }
            Input = MidiDeviceManager.OpenInput(inputDevice.ID);

            var outputDevice = MidiDevices.FirstOrDefault(device => device.IsOutput);
            if (outputDevice.Equals(default))
            {
                throw new Exception("Cannot find any device");
            }
            Output = MidiDeviceManager.OpenOutput(outputDevice.ID);

            return Input != null && Output != null;
        }

        public void Clear()
        {
            var midiMessage = new MidiMessage(0xb0, 0, 0);
            Output.Write(new MidiEvent
            {
                Message = midiMessage
            });
        }

        public void Close()
        {
            Input?.Close();
            Output?.Close();
        }

        public void Light(Hit hit, LaunchpadColor color)
        {
            var note = (int) (hit.X + 16 * hit.Y);
            var velocity = color.ToVelocity();
            var midiMessage = new MidiMessage(0x90, note, velocity);
            Output.Write(new MidiEvent()
            {
                Message = midiMessage
            });
        }

        public void ListenStop()
        {
            ListenTask.Cancel();
        }

        public void ListenStart(Action<LaunchpadEventArgs> action)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (ListenTask.IsCancellationRequested)
                    {
                        ListenTask.Token.ThrowIfCancellationRequested();
                    }

                    Thread.Sleep(100);
                    if (!Input.HasData)
                    {
                        continue;
                    }

                    var data = new byte[64];
                    var midiEvent = Input.ReadEvent(data, 0, data.Length);
                    var hit = Hit.Build(midiEvent);
                    if (hit.IsHitted)
                    {
                        action.Invoke(new LaunchpadEventArgs()
                        {
                            Hit = hit
                        });
                    }
                }
            }, ListenTask.Token);
        }
    }
}