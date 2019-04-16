using System.Windows;
using System.Windows.Controls;

namespace Sample
{
    /// <summary>
    /// Page1.xaml の相互作用ロジック
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();

            // タイトルの変更
            this.WindowTitle = "ぺーじ1";
        }

        private void NextPage_Click( object sender, RoutedEventArgs e )
        {
            // ぺーじ２に移動
            this.NavigationService.Navigate( new Page2() );
        }

        private void Cancel_Click( object sender, RoutedEventArgs e )
        {
            // アプリケーションの終了はこれでいいのかわからない
            Application.Current.Shutdown();
        }
    }
}
