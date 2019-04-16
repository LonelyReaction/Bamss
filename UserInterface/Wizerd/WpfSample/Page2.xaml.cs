using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sample
{
    /// <summary>
    /// Page2.xaml の相互作用ロジック
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();

            // タイトルの変更
            this.WindowTitle = "ぺーじ２";
        }

        private void ReturnPage_Click( object sender, RoutedEventArgs e )
        {
            // 前のページに戻る
            if( NavigationService.CanGoBack )
                NavigationService.GoBack();
        }
    }
}
