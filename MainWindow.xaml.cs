using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace beanbrew {
    public partial class MainWindow : Window {
        public class NavbarButtonData {
            public required Button ButtonInst { get; set; }
            public required Rectangle ButtonBacking { get; set; }
            public required string ButtonName { get; set; }
            public required Page Target { get; set; }
        }

        private int CurNavButton = -1;
        private List<NavbarButtonData> NavButtons = [];

        public MainWindow() {
            this.InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            this.MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            this.NavButtons = [
                new NavbarButtonData {
                    ButtonInst = this.xBtnHome, ButtonBacking = this.xRecHomeBg, ButtonName = "Home", Target = new Pages.HomePage()
                },
                new NavbarButtonData {
                    ButtonInst = this.xBtnBook, ButtonBacking = this.xRecBookBg, ButtonName = "Book", Target = new Pages.BookPage()
                },
                new NavbarButtonData {
                    ButtonInst = this.xBtnOrder, ButtonBacking = this.xRecOrderBg, ButtonName = "Order", Target = new Pages.OrderPage()
                },
                new NavbarButtonData {
                    ButtonInst = this.xBtnRewards, ButtonBacking = this.xRecRewardsBg, ButtonName = "Rewards", Target = new Pages.RewardsPage()
                }
            ];
            this.GoToNavIdx(0);
        }

        private Color GetColorWithAltAlpha(Color color, byte alpha) {
            return Color.FromArgb(alpha, color.R, color.G, color.B);
        }

        private Color GetBackingColorWithAltAlpha(Rectangle backing, byte alpha) {
            return this.GetColorWithAltAlpha(((SolidColorBrush)backing.Fill).Color, alpha);
        }

        private void GoToNavIdx(int idx) {
            if (this.CurNavButton == idx) {
                return;
            }

            var duration = new Duration(TimeSpan.FromMilliseconds(200));
            if (this.CurNavButton >= 0) {
                Rectangle oldBacking = this.NavButtons[this.CurNavButton].ButtonBacking;
                oldBacking.Fill.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation {
                    To = this.GetBackingColorWithAltAlpha(oldBacking, 0),
                    Duration = duration
                });
            }

            this.MainFrame.Content = this.NavButtons[idx].Target;
            Rectangle newBacking = this.NavButtons[idx].ButtonBacking;
            newBacking.Fill.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation {
                To = this.GetBackingColorWithAltAlpha(newBacking, 255),
                Duration = duration
            });
            this.CurNavButton = idx;
        }

        private void xBtnHome_Click(object sender, RoutedEventArgs e) {
            this.GoToNavIdx(0);
        }

        private void xBtnBook_Click(object sender, RoutedEventArgs e) {
            this.GoToNavIdx(1);
        }

        private void xBtnOrder_Click(object sender, RoutedEventArgs e) {
            this.GoToNavIdx(2);
        }

        private void xBtnRewards_Click(object sender, RoutedEventArgs e) {
            this.GoToNavIdx(3);
        }
    }
}