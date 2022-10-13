using System.Windows;

namespace EMS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void ExportToCsv(object sender, RoutedEventArgs e)
        {
            view.ExportToCsv(@"c:\data\grid_export.csv");
        }


        private void view_CurrentPageIndexChanging(object sender, DevExpress.Xpf.Editors.DataPager.DataPagerPageIndexChangingEventArgs e)
        {
        }
    }
}
