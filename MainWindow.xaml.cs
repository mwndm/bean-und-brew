using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace beanbrew
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;

            var grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;

            var lbl = new Label();
            lbl.Content = "comic sans ms";
            lbl.FontFamily = new FontFamily("Comic Sans MS");
            grid.Children.Add(lbl);

            this.MainFrame.Content = grid;
        }
    }
}