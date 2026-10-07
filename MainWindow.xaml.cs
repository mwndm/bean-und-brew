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
            /*
            var grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;

            var lbl = new Label();
            lbl.Content = "comic sans ms";
            lbl.FontFamily = new FontFamily("Comic Sans MS");
            grid.Children.Add(lbl);
            */

            //this.MainFrame.Content = grid;
            this.MainFrame.Content = new Pages.HomePage();

            var btnCount = 4;
            for (int i = 0; i < btnCount; i++) {
                //var inst = new Button();
                //inst.Content = $"Button {i + 1}";
                //inst.HorizontalAlignment = HorizontalAlignment.Left;
                //inst.VerticalAlignment = VerticalAlignment.Bottom;
                //inst.Margin = new Thickness(150.0 * i, 0, 0, 0);
                //inst.Width = 150;
                //inst.Height = 150;
                //this.AddChild(inst);
                /*
                 * 
        <Button Content="Button" HorizontalAlignment="Left" Margin="0,0,0,0" VerticalAlignment="Bottom" Height="130" Width="150"/>
        <Button Content="Button" HorizontalAlignment="Left" Margin="150,0,0,0" VerticalAlignment="Bottom" Height="130" Width="150"/>
        <Button Content="Button" HorizontalAlignment="Left" Margin="300,0,0,0" VerticalAlignment="Bottom" Height="130" Width="150"/>
        <Button Content="Button" HorizontalAlignment="Left" Margin="450,0,0,0" VerticalAlignment="Bottom" Height="130" Width="150"/>
                 */
            }
        }

        
    }
}