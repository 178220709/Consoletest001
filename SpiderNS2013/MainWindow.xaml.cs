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

namespace SpiderNS2013
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartSpider_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 173; i < 400; i++)
            {
                var people = new PeopleNS();
                var url = http://www.zhaogangren.com/vote/qingxin2013/?mod=qxinfo&app=info&uid=260
            }
        }
    }

    public  class PeopleNS
    {

    }
}
