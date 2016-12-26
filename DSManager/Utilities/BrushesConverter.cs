using System.Windows.Media;

namespace DSManager.Utilities {
    public static class BrushesConverter {
        public static Brush GetDarker(Brush brush) {
            if(Brushes.SeaGreen.Equals(brush))
                return Brushes.Green;
            if(Brushes.SteelBlue.Equals(brush))
                return Brushes.RoyalBlue;
            if(Brushes.LightCoral.Equals(brush))
                return Brushes.Coral;
            if(Brushes.BlueViolet.Equals(brush))
                return Brushes.DarkViolet;
            if(Brushes.Brown.Equals(brush))
                return Brushes.DarkRed;

            return Brushes.Transparent;
        }

        public static Brush GetLighter(Brush brush) {
            if (Brushes.Green.Equals(brush))
                return Brushes.SeaGreen;
            if (Brushes.RoyalBlue.Equals(brush))
                return Brushes.SteelBlue;
            if (Brushes.Coral.Equals(brush))
                return Brushes.LightCoral;
            if (Brushes.DarkViolet.Equals(brush))
                return Brushes.BlueViolet;
            if(Brushes.DarkRed.Equals(brush))
                return Brushes.Brown;

            return Brushes.Transparent;
        }
    }
}