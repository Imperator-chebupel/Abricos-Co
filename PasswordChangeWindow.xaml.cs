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
    /// Логика взаимодействия для PasswordChangeWindow.xaml
    /// </summary>
    public partial class PasswordChangeWindow : Window
    {
        public PasswordChangeWindow()
        {
            InitializeComponent();
            this.Title = "Смена пароля";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((CurrentPassword.Password != "") || (NewPassword1.Password != "") || (NewPassword2.Password != ""))
            {
                if (NewPassword1.Password != NewPassword2.Password)
                    MessageBox.Show("Новые пароли не совпадают!");
                else 
                {
                    if (DatabaseConnector.ChangePassword(CurrentPassword.Password,NewPassword1.Password))
                        MessageBox.Show("Смена пароля прошла успешно");
                    else
                        MessageBox.Show("Что-то пошло не так!");
                }
            }
            else
                MessageBox.Show("Заполинте все поля!");

        }
    }
}
