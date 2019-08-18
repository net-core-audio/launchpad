using PortMidi;

namespace Launchpad
{
    public class Hit
    {
        public long X { get; private set; }
        public long Y { get; private set; }
        public bool IsHitted { get; private set; }

        public static Hit Build(Event midiEvent)
        {
            var hit = new Hit
            {
                IsHitted = midiEvent.Data2 > 0
            };
            
            if (!hit.IsHitted)
            {
                return hit;
            }

            if (midiEvent.Status == 176)
            {
                hit.X = midiEvent.Date1 - 104;
                hit.Y = 8;
            }
            else
            {
                hit.X = midiEvent.Date1 % 16;
                hit.Y = (midiEvent.Date1 - hit.X) / 16;
            }

            return hit;
        }
    }
}