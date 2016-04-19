namespace MSI_Keyboard_Manager
{
    class RegionSetting
    {
        public Constants.Regions Region;
        public Constants.Intensities Intensity;
        public Constants.Colors PrimaryColor;
        public Constants.Colors SecondaryColor;

        public RegionSetting(Constants.Regions region, Constants.Intensities intensity,
            Constants.Colors primaryColor,
            Constants.Colors secondaryColor)
        {
            Region = region;
            Intensity = intensity;
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
        }
    }
}