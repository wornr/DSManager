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

namespace DSManager.Controls.WatermarkTextBox {
    /// <summary>
    /// Interaction logic for WatermarkTextBox.xaml
    /// </summary>
    public partial class WatermarkTextBox : UserControl {
        public WatermarkTextBox() {
            InitializeComponent();
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(string.Empty));

        public string Watermark {
            get { return GetValue(WatermarkProperty).ToString(); }
            set { SetValue(WatermarkProperty, value); }
        }
    }
}
