using MySql.Data.MySqlClient;
using Mysqlx.Expr;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;


namespace База_данных_фирмы
{
    internal static class DatabaseConnector
    {
        private static string server = "localhost";
        private static string database = "factory";
        private static string user = "";
        private static string password = "";
        private static MySqlConnection connection = null;
        private static readonly object connectionLock = new object();
        private static Dictionary<string, string> ShowingTables = new Dictionary<string, string>() {
                                                                                                   {"root",$"SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{database}'"},
                                                                                                   {"Начальник_производства",$"SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{database}'" },
                                                                                                   {"Менеджер",$"SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{database}' and table_name in ('наряды', 'детали', 'содержание_наряда')" },
                                                                                                   {"Клиент",$"SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{database}' and table_name in ('детали')" } };


        internal static Dictionary<string, List<string>> DefaultKeys = new Dictionary<string, List<string>>() {                               {"детали", new List<string>() {"100", "АБрикос","что-то" ,"124"} },
                                                                                                                                              {"журнал_операций", new List<string>(){"54","24", "23", "7" } },
                                                                                                                                              {"запросы_инструментов", new List<string>(){ "34", "3"} },
                                                                                                                                              {"запросы_сырья", new List<string>() { "4", "1"} },
                                                                                                                                              {"инструменты", new List<string>() { "44", "Палка", "Чтобы палкать"} },
                                                                                                                                              {"наряды", new List<string>() { "55", "2024-06-11"} },
                                                                                                                                              {"операции", new List<string>() { "77", "Промывка"} },
                                                                                                                                              {"содержание_запросов_инструментов", new List<string>(){ "355", "4", "6"} },
                                                                                                                                              {"содержание_запросов_сырья", new List<string>() { "444", "2", "4", "13"} },
                                                                                                                                              {"содержание_наряда", new List<string>() { "23", "4", "4"} },
                                                                                                                                              {"сырьё", new List<string>() { "52", "Оксид водорода", "Водичка", "13"} },
                                                                                                                                              {"цеха", new List<string>()  {"432", "Цехище", "Типа большой цех", "43" } } };
        //Подключение
        public static bool Connect(string user_, string password_)
        {
            lock (connectionLock)
            {
                if (connection != null && connection.State == ConnectionState.Open) return true; //Уже подключены
                user = user_;
                password = password_;
                string connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};";
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
                    return false;
                }
            }
        }

        //Зачем это было сделано?
        public static void Disconnect()
        {
            lock (connectionLock)
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }
        }

        //Вывод списка пользователей
        public static DataTable GetUserListData()
        {
            lock (connectionLock)
            {
                if (connection == null || connection.State != ConnectionState.Open)
                    throw new InvalidOperationException("Не установлено соединение с базой данных.");
                try
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM user_list", connection))
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка получения данных: {ex.Message}");
                    return null;
                }
            }
        }

        //Вывод доступных таблиц
        public static List<string> GetAvailableTables()
        {
            lock (connectionLock)
            {
                if (connection == null || connection.State != ConnectionState.Open)
                    throw new InvalidOperationException("Не установлено соединение с базой данных.");
                string query = "";
                List<string> tables = new List<string>();
                try
                {
                    query = ShowingTables[user];
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                    return tables;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка получения списка таблиц: {ex.Message}");
                    return null;
                }
            }
        }



        //Выгрузка из таблицы
        public static DataTable GetTableData(string tableName)
        {
            lock (connectionLock)
            {
                if (connection == null || connection.State != ConnectionState.Open)
                    throw new InvalidOperationException("Не установлено соединение с базой данных.");
                try
                {
                    using (MySqlCommand command = new MySqlCommand($"SELECT * FROM {tableName}", connection))
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка получения данных из таблицы {tableName}: {ex.Message}");
                    return null;
                }
            }
        }

        //Любые команды
        public static bool ExecuteSqlCommand(string sqlCommand)
        {
            lock (connectionLock)
            {
                if (connection == null || connection.State != ConnectionState.Open)
                    throw new InvalidOperationException("Не установлено соединение с базой данных.");
                try
                {
                    using (MySqlCommand command = new MySqlCommand(sqlCommand, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show($"Неужели сработало?");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка выполнения SQL-команды: {ex.Message}");
                    return false;
                }
            }
        }

        //Смена пароля
        public static bool ChangePassword(string OldPassword, string NewPassword)
        {
            if (OldPassword == password)
            {
                lock (connectionLock)
                {
                    if (connection == null || connection.State != ConnectionState.Open)
                    {
                        throw new InvalidOperationException("Не установлено соединение с базой данных.");
                    }

                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(($"ALTER USER '{user}'@'{server}' IDENTIFIED BY '{NewPassword}';"), connection))
                        {
                            command.ExecuteNonQuery();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка выполнения SQL-команды: {ex.Message}");
                        return false;
                    }
                }
            }
            else return false;
        }


        //Удаление строк
        public static void DeleteRows(string ListDoDelete, string TableName)
        {
            if (Checker.CheckString(ListDoDelete))
                ExecuteSqlCommand($"DELETE FROM {TableName} WHERE ID IN ({ListDoDelete});");
            else
                MessageBox.Show("Перепроверьте ввод");
        }




        public static bool DeleteRow(string tableName, string selectedRowId, string columnName = "ID")
        {
            lock (connectionLock)
            {
                if (connection == null || connection.State != ConnectionState.Open)
                {
                    MessageBox.Show("Не установлено соединение с базой данных.");
                    return false;
                }

                try
                {
                    string query = $"DELETE FROM {tableName} WHERE {columnName} = '{selectedRowId}'";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Строка успешно удалена.");
                            return true;
                        }
                        else
                        {
                            MessageBox.Show($"Строка не найдена или не удалена.");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления строки: {ex.Message}");
                    return false;
                }
            }
        }




        //Получение имён столбцов
        public static List<string> GetColumnNames(string tableName, string databaseName = null)
        {
            List<string> columnNames = new List<string>();
            databaseName = database;
            lock (connectionLock)
            {
                using (MySqlCommand command = new MySqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = @databaseName AND TABLE_NAME = @tableName", connection))
                {
                    command.Parameters.AddWithValue("@databaseName", databaseName);
                    command.Parameters.AddWithValue("@tableName", tableName);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnNames.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return columnNames;
        }


        //Замена значений
        public static void ReplaceStringValues(string tableName, List<string> RowNames, List<string> NewValues, int ID)
        {
            string smth = $"Update {tableName} set ";
            smth += Checker.UnionLists(NewValues, RowNames);
            smth += $" where ({RowNames[0]} = '{ID}');";
            ExecuteSqlCommand(smth);
        }

        public static void AddValue(string tableName, List<string> RowNames, List<string> NewValues)
        {
            string Command = $"Insert into {tableName} ";
            Command += Checker.QueueLists(NewValues, RowNames);
            ExecuteSqlCommand(Command);
        }

        public static DataTable OperationsOfShops()
        {
            lock (connectionLock)
            {
                if (connection == null || connection.State != ConnectionState.Open)
                    throw new InvalidOperationException("Не установлено соединение с базой данных.");
                try
                {
                    using (MySqlCommand command = new MySqlCommand($" SELECT c.Название AS Название_цеха,  GROUP_CONCAT(o.Название ORDER BY o.Название SEPARATOR ', ') AS Названия_операций FROM  Цеха c JOIN  (SELECT DISTINCT ID_цеха FROM Журнал_операций) AS jc ON c.ID = jc.ID_цеха LEFT JOIN  Журнал_операций jo ON c.ID = jo.ID_цеха LEFT JOIN  Операции o ON jo.ID_операции = o.ID GROUP BY  c.ID, c.Название;  ", connection))
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка запроса: {ex.Message}");
                    return null;
                }
            }
        }


        public static void ExportDataGridToExcel(DataGrid dataGrid, string filePath)
        {
            try
            {
                // Проверяем, что DataGrid не пустой
                if (dataGrid.Items.Count == 0)
                {
                    throw new InvalidOperationException("DataGrid is empty.");
                }


                // Создаем DataTable из данных DataGrid
                DataTable dataTable = new DataTable();
                foreach (DataColumn column in dataGrid.Columns.Cast<DataGridTextColumn>().Select(c => new DataColumn(c.Header as string ?? "Column")).ToList())
                {
                    dataTable.Columns.Add(column);
                }


                foreach (var item in dataGrid.Items)
                {
                    DataRow newRow = dataTable.NewRow();
                    // Обработка разных типов данных в ячейках
                    foreach (var column in dataGrid.Columns)
                    {
                        var cellContent = column.GetCellContent(item);
                        if (cellContent != null)
                        {
                            //Обработка возможных null-значений
                            string cellValue = cellContent.ToString();
                            newRow[column.Header as string ?? "Column"] = cellValue; // Учитываем возможность null для заголовка столбца
                        }
                        else
                        {
                            // Обработка пустых или не поддерживаемых типов данных
                            newRow[column.Header as string ?? "Column"] = DBNull.Value;
                        }
                    }
                    dataTable.Rows.Add(newRow);
                }


                FileInfo file = new FileInfo(filePath);
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Data");


                    // Заполнение Excel-таблицы данными из DataTable
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);


                    excelPackage.Save();
                }


                MessageBox.Show("Данные успешно сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }




        public static DataTable ExecuteRichTextBoxQuery(RichTextBox richTextBox)
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                MessageBox.Show("Подключение к базе данных не открыто. Убедитесь, что вы вызвали InitializeConnection.");
                return null;
            }
            DataTable dataTable = null;
            string sqlQuery = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.Trim();
            if (string.IsNullOrEmpty(sqlQuery))
            {
                MessageBox.Show("Запрос пустой.");
                return null;
            }

            try
            {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    if (sqlQuery.ToUpper().Contains("SELECT"))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            dataTable = new DataTable();
                            adapter.Fill(dataTable);
                        }
                    }
                    else
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        MessageBox.Show($"Выполнено, затронуто строк: {rowsAffected}");
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Ошибка MySQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Общая ошибка: {ex.Message}");
            }

            return dataTable;
        }

    }
}
