namespace Launchpad
{
    public static class LaunchpadColorExtensions
    {
        public static int ToVelocity(this LaunchpadColor color)
        {
            switch (color)
            {
                case LaunchpadColor.None:
                    return 12;
                case LaunchpadColor.Red:
                    return 15;
                case LaunchpadColor.Green:
                    return 61;
                case LaunchpadColor.Yellow:
                    return 63;
                default:
                    return 12;
            }
            
        }
    }
}