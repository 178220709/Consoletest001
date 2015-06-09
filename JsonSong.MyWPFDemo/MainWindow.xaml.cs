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
using JsonSong.SpiderApp.MyTask;
using Suijing.Utils.sysTools;

namespace JsonSong.MyWPFDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LogHelper.SetConfig();
        }

        private async void StartSpiderTask_Click(object sender, RoutedEventArgs e)
        {
           var controller = new TaskController();
           await  controller.StartHahaTask();
        }

        private async void StartSync_Click(object sender, RoutedEventArgs e)
        {
            var source = this.TxtSourcePath.Text;
            var target = this.TxtTargetPath.Text;
           
        }
    }
}
