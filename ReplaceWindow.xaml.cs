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
    /// Логика взаимодействия для ReplaceWindow.xaml
    /// </summary>
    public partial class ReplaceWindow : Window
    {
        private string TableName;
        private List<string> Keys;
        public ReplaceWindow(string ST, List<string> Names)
        {
            InitializeComponent();
            StrangeTextBlock.Text = string.Join("; ",Names);
            TableName = ST;
            ChangeTextBox.Text = string.Join("; ", DatabaseConnector.DefaultKeys[TableName]);
            Keys = Names;
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            int ID = Int32.Parse(IDBox.Text);
            List<string> Changes = (ChangeTextBox.Text.Split(';')).ToList();
            DatabaseConnector.ReplaceStringValues(TableName, Keys,Changes, ID);
        }
    }
}
