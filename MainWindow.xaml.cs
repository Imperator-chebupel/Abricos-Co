using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace База_данных_фирмы
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Абрикос";
            this.ResizeMode = ResizeMode.NoResize;
            this.Width = 800;
            this.Height = 480;
            this.BorderBrush = Brushes.LightCyan;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string server = "наш_сервер";
            string database = textbox1.Text; //База данных определяется логином
            string user = textbox1.Text;     //Логин определяет базу данных
            string password = passwordbox.Password;


            //DatabaseConnector connector = new DatabaseConnector();
            MySqlConnection connection;
            if (DatabaseConnector.Connect(user, password))
            {
                MessageBox.Show($"Успешное подключение к базе данных {database}!");
                var welcomeWindow = new WelcomeWindow(database); //Передаем имя базы данных
                welcomeWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к базе данных.");
            }
        }

    }
}