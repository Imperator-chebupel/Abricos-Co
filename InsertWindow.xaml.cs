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

namespace База_данных_фирмы
{
    /// <summary>
    /// Логика взаимодействия для InsertWindow.xaml
    /// </summary>
    public partial class InsertWindow : Window
    {
        private string ST;
        private List<string> STList;
        public InsertWindow(string selectedTable, List<string> Names)
        {
            InitializeComponent();
            ST = selectedTable;
            SillyTextBox.Text = string.Join("; ", Names);
            STList = Names;
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> Changes = (InsertTextBox.Text.Split("; ")).ToList();
            DatabaseConnector.AddValue(ST, STList, Changes);
            //MessageBox.Show( Checker.QueueLists(Changes, STList));
        }
    }
}
