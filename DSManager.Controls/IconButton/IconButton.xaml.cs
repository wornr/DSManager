using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DSManager.Controls.IconButton {
    /// <summary>
    /// Interaction logic for IconButton.xaml
    /// </summary>
    public partial class IconButton : UserControl {
        public IconButton() {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register("IsDefault", typeof(bool), typeof(IconButton), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsCancelProperty = DependencyProperty.Register("IsCancel", typeof(bool), typeof(IconButton), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(IconButton), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(IconButton), new FrameworkPropertyMetadata(string.Empty));

        public bool IsDefault {
            get { return GetValue(IsDefaultProperty).Equals(true); }
            set { SetValue(IsDefaultProperty, value); }
        }
        public bool IsCancel {
            get { return GetValue(IsCancelProperty).Equals(true); }
            set { SetValue(IsCancelProperty, value); }
        }
        public ImageSource ImageSource {
            get { return GetValue(ImageSourceProperty) as ImageSource; }
            set { SetValue(ImageSourceProperty, value); }
        }
        public string Text {
            get { return GetValue(TextProperty).ToString(); }
            set { SetValue(TextProperty, value); }
        }
    }
}
