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
using System.Windows.Shapes;
using AlphaS.CoreNS;
using Microsoft.Win32;

namespace AlphaS.Forms
{
    /// <summary>
    /// StockListWindow.xaml 的互動邏輯
    /// </summary>
    public partial class StockListWindow : Window
    {
        MainWindow mainWindow;
        public StockListWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            this.Left = Core.settingManager.getSetting("StockListWindowPostitionLeft").getIntFromString();
            this.Top = Core.settingManager.getSetting("StockListWindowPostitionTop").getIntFromString();
        }

        private void LoadStockList(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            var defaultFolder = Core.settingManager.getSetting("StockListDefaultFolder");
            if (defaultFolder != "")
            {
                openFileDialog.InitialDirectory = defaultFolder;
            }
            else
            {
                openFileDialog.InitialDirectory = Core.DEFAULT_FOLDER;
            }
            if (openFileDialog.ShowDialog() == true)
            {
                Core.settingManager.saveSetting("StockListDefaultFolder", openFileDialog.FileName.getFileFolderFromPath());
                Core.stockListManager.loadStockList(openFileDialog.FileName);
                stockListView.ItemsSource = Core.stockListManager.stockList;
                stockListView.Items.Refresh();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Core.settingManager.saveSetting("StockListWindowPostitionLeft", this.Left.ToString());
            Core.settingManager.saveSetting("StockListWindowPostitionTop", this.Top.ToString());
        }
    }
}
