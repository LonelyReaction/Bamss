using System.Windows.Navigation;

namespace Sample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            this.ShowsNavigationUI = false;
            this.Navigate( new Page1() );
        }
    }
}
