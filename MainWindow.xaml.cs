using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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
        private bool IsTransitioning = false;
        private CancellationTokenSource? ScrollAnimationCancellation;

        public MainWindow() {
            this.InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
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
                    ButtonInst = this.xBtnRewards, ButtonBacking = this.xRecRewardsBg, ButtonName = "Rewards", Target = new Pages.LoginPage()
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

        private void AnimateBackingAlpha(Rectangle backing, byte targetAlpha) {
            if (backing.Fill is not SolidColorBrush currentBrush) {
                currentBrush = new SolidColorBrush(Colors.Transparent);
                backing.Fill = currentBrush;
            }

            var animation = new ColorAnimation {
                To = this.GetBackingColorWithAltAlpha(backing, targetAlpha),
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new CubicEase {
                    EasingMode = EasingMode.EaseInOut
                }
            };

            currentBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        private void MainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) {
            if (e.ExtentHeightChange == 0 && e.ViewportHeightChange == 0 && e.VerticalChange == 0) {
                return;
            }

            if (this.MainScrollViewer.ScrollableHeight <= 1) {
                this.ScrollDownArrow.Opacity = 0;
                this.ScrollDownArrow.Visibility = Visibility.Collapsed;
                return;
            }

            this.ScrollDownArrow.Visibility = Visibility.Visible;
            double progress = this.MainScrollViewer.VerticalOffset / (this.MainScrollViewer.ViewportHeight * 0.5);
            this.ScrollDownArrow.Opacity = Math.Clamp(1.0 - progress, 0.0, 1.0);

            if (progress >= 1.0) {
                this.ScrollDownArrow.Visibility = Visibility.Collapsed;
            }
        }

        private async void ScrollDownArrow_Click(object sender, RoutedEventArgs e) {
            this.ScrollAnimationCancellation?.Cancel();

            using var cancellation = new CancellationTokenSource();
            this.ScrollAnimationCancellation = cancellation;

            double startOffset = this.MainScrollViewer.VerticalOffset;
            double targetOffset = Math.Min(startOffset + this.MainScrollViewer.ViewportHeight, this.MainScrollViewer.ScrollableHeight);
            if (Math.Abs(targetOffset - startOffset) < 0.1) {
                return;
            }

            const double durationMilliseconds = 450;
            var stopwatch = Stopwatch.StartNew();
            try {
                while (stopwatch.Elapsed.TotalMilliseconds < durationMilliseconds) {
                    double progress = stopwatch.Elapsed.TotalMilliseconds / durationMilliseconds;
                    progress = Math.Clamp(progress, 0.0, 1.0);
                    double easedProgress = 1.0 - Math.Pow(1.0 - progress, 3.0);
                    double currentOffset = startOffset + ((targetOffset - startOffset) * easedProgress);
                    this.MainScrollViewer.ScrollToVerticalOffset(currentOffset);
                    await Task.Delay(16, cancellation.Token);
                }

                this.MainScrollViewer.ScrollToVerticalOffset(targetOffset);
            }
            catch (TaskCanceledException) {
                // A new click started another scroll animation.
            }
            finally {
                if (ReferenceEquals(this.ScrollAnimationCancellation, cancellation)) {
                    this.ScrollAnimationCancellation = null;
                }
            }
        }

        private void GoToNavIdx(int idx) {
            if (idx < 0 || idx >= this.NavButtons.Count) {
                return;
            }

            if (idx == this.CurNavButton || this.IsTransitioning) {
                return;
            }

            if (this.CurNavButton >= 0) {
                this.AnimateBackingAlpha(this.NavButtons[this.CurNavButton].ButtonBacking, 0);
            }

            this.AnimateBackingAlpha(this.NavButtons[idx].ButtonBacking, 255);
            Page newPage = this.NavButtons[idx].Target;
            if (this.CurNavButton == -1) {
                this.CurrentPageFrame.Content = newPage;
                this.CurNavButton = idx;
                return;
            }

            this.IsTransitioning = true;
            int direction = idx > this.CurNavButton ? 1 : -1;

            double width = this.MainFrame.ActualWidth;
            if (width <= 0) {
                width = this.MainFrame.RenderSize.Width;
            }

            Page oldPage = this.NavButtons[this.CurNavButton].Target;
            this.OldPageFrame.Content = oldPage;
            this.CurrentPageFrame.Content = newPage;
            this.OldPageFrame.RenderTransform = new TranslateTransform(0, 0);
            this.CurrentPageFrame.RenderTransform = new TranslateTransform(direction * width, 0);

            var duration = TimeSpan.FromMilliseconds(280);
            var oldTransform = this.OldPageFrame.RenderTransform;
            var newTransform = this.CurrentPageFrame.RenderTransform;

            var easing = new CubicEase {
                EasingMode = EasingMode.EaseInOut
            };

            var oldAnimation = new DoubleAnimation {
                To = -direction * width,
                Duration = duration,
                EasingFunction = easing
            };

            var newAnimation = new DoubleAnimation {
                To = 0,
                Duration = duration,
                EasingFunction = easing
            };

            newAnimation.Completed += (s, e) => {
                oldTransform.BeginAnimation(TranslateTransform.XProperty, null);
                newTransform.BeginAnimation(TranslateTransform.XProperty, null);

                this.OldPageFrame.Content = null;

                this.OldPageFrame.RenderTransform = Transform.Identity;
                this.CurrentPageFrame.RenderTransform = Transform.Identity;

                this.CurNavButton = idx;
                this.IsTransitioning = false;
            };

            oldTransform.BeginAnimation(TranslateTransform.XProperty, oldAnimation);
            newTransform.BeginAnimation(TranslateTransform.XProperty, newAnimation);
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