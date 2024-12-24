using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class WelcomeWindow : Window
    {
        string selectedTable;
        string selectedOrder;
        public WelcomeWindow(string databaseName)
        {
            InitializeComponent();
            //txtwelcome.Text = $"Подключено к базе данных: {databaseName}";
            this.Title = "Абрикос";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadUserData();
            LoadTables();
            LoadOrders();
        }
        //private void LoadUserData()
        //{
        //        DataTable userData = DatabaseConnector.GetUserListData();
        //        if (userData != null)
        //        {
        //            DataGrid1.ItemsSource = userData.DefaultView;
        //        }
        //        else
        //        {
        //            // Обработка ошибки
        //            MessageBox.Show("Ошибка загрузки данных.");
        //        }
        //}

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonList1_Click(object sender, RoutedEventArgs e)
        {
            Tab1.SelectedIndex = 0;
        }

        private void ButtonList2_Click(object sender, RoutedEventArgs e)
        {
            Tab1.SelectedIndex = 1;
        }

        private void ButtonList3_Click(object sender, RoutedEventArgs e)
        {
            Tab1.SelectedIndex = 2;
        }

        private void ButtonList4_Click(object sender, RoutedEventArgs e)
        {
            Tab1.SelectedIndex = 3;
        }

        private void ButtonList5_Click(object sender, RoutedEventArgs e)
        {
            Tab1.SelectedIndex = 4;
        }

        private void ButtonList6_Click(object sender, RoutedEventArgs e)
        {
            Tab1.SelectedIndex = 5;
        }

        private void LoadTables()
        {
            List<string> tables = DatabaseConnector.GetAvailableTables();
            if (tables != null)
            {
                ComboBox1.ItemsSource = tables;
                //if (tables.Count > 0)
                //{
                //    ComboBox1.SelectedIndex = 0; // Выберите первую таблицу по умолчанию
                //}
            }

            //List<KeyValuePair<string, string>> tableNames = DatabaseConnector.GetAvailableTablesWithDisplayNames();
            //if (tableNames != null)
            //{
            //    ComboBox1.ItemsSource = tableNames;
            //    ComboBox1.DisplayMemberPath = "Value";
            //    ComboBox1.SelectedValuePath = "Key";
            //    selectedTable = "Key";
            //}
        }

        private void tableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTable = ComboBox1.SelectedItem as string;
            if (selectedTable != null)
            {
                LoadTableData(selectedTable);
            }
            //if (ComboBox1.SelectedItem is KeyValuePair<string, string> selectedItem)
            //{
            //    string selectedTableName = selectedItem.Key;
            //    // Дальнейшая работа с выбранной таблицей
            //}
        }

        private void LoadTableData(string tableName)
        {
                DataTable tableData = DatabaseConnector.GetTableData(tableName);
                if (tableData != null)
                {
                    DataGrid2.ItemsSource = tableData.DefaultView;
                }
                else
                {
                    MessageBox.Show("Ошибка загрузки данных.");
                }
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordChangeWindow passwordChangeWindow = new PasswordChangeWindow();
            passwordChangeWindow.Show();
        }

        private void CommandButton_Click(object sender, RoutedEventArgs e)
        {
            //string sqlCommand = new TextRange(ComandBox.Document.ContentStart, ComandBox.Document.ContentEnd).Text;
            //if (string.IsNullOrWhiteSpace(sqlCommand))
            //{
            //    MessageBox.Show("Пожалуйста, введите SQL-запрос.");
            //    return;
            //}
            //try
            //{
            //    bool success = DatabaseConnector.ExecuteSqlCommand(sqlCommand);
            //    if (success)
            //    {
            //        MessageBox.Show("Всё прошло успешно!");
            //    }
            //}
            //catch (InvalidOperationException ex)
            //{
            //    MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Произошла ошибка: {ex.Message}");
            //}
            DataTable resultTable = DatabaseConnector.ExecuteRichTextBoxQuery(ComandBox);
            if (resultTable != null)
            {
                DataGridForRangomRequests.ItemsSource = resultTable.DefaultView;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteWindow deleteWindow = new DeleteWindow(selectedTable);
            deleteWindow.Show();

            //DataRowView selectedRow = SomeDataGrid.SelectedItem as DataRowView;
            //string firstCellValue = selectedRow.Row.ItemArray[0].ToString(); // Используйте "?." для избежания null reference
            //DatabaseConnector.DeleteRow(selectedTable, firstCellValue);

            //if (SomeDataGrid.SelectedItem != null)
            //{
            //    DataRowView selectedRow = SomeDataGrid.SelectedItem as DataRowView;
            //    if (selectedRow != null)
            //    {
            //        string firstCellValue = selectedRow.Row.ItemArray[0]?.ToString(); // Используйте "?." для избежания null reference
            //        DatabaseConnector.DeleteRow(selectedTable, firstCellValue);
            //    }
            //}
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTableData(selectedTable);
        }

        private void Replace_Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> Sus = DatabaseConnector.GetColumnNames(selectedTable);
            ReplaceWindow RW = new ReplaceWindow(selectedTable, Sus);
            RW.Show();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> Sus = DatabaseConnector.GetColumnNames(selectedTable);
            InsertWindow IW = new InsertWindow(selectedTable, Sus);
            IW.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoadOrders()
        {
            List<string> Sourse = new List<string> {"Операции цехов"};
            ComboBoxOfDocuments.ItemsSource = Sourse;
            if (Sourse.Count > 0)
            {
                ComboBox1.SelectedIndex = 0; // Выберите первую таблицу по умолчанию
            }
        }

        private void ComboBoxOfDocuments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOrder = ComboBox1.SelectedItem as string;
            if (selectedTable != "Операции цехов")
            {
                LoadOrders(selectedTable);
            }
        }


        private void LoadOrders(string tableName)
        {
            DataTable tableData = DatabaseConnector.OperationsOfShops();
            if (tableData != null)
            {
                SomeDataGrid.ItemsSource = tableData.DefaultView;
            }
            else
            {
                MessageBox.Show("Ошибка загрузки данных.");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnector.ExportDataGridToExcel(DataGridForRangomRequests, "Файлы");
        }
    }
}
