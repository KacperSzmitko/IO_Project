using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Views 
{
    public partial class Login : UserControl 
    {
        private ServerConnection conn;
        public Login() {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            conn = new ServerConnection("localhost", 17777);
        }

        private void LoginButton_Clicked(object sender, RoutedEventArgs e) {
            ServerCommands.LoginCommand(conn, LoginBox.Text, PasswordBox.Password);
        }

    }
}
