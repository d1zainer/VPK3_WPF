using System.Windows;


namespace TPCHR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool Choised = false; //флаг для логина и пароля
        Menu menu = new Menu();

        public MainWindow()
        {          
            InitializeComponent();
        }

        private void bRegistration_Click(object sender, RoutedEventArgs e)
        {
            Choised = false;
            tbPass2.IsEnabled = true;
            tbPass2.Visibility = Visibility.Visible;
            pass2.Visibility = Visibility.Visible;
        }

        private void bLogin_Click(object sender, RoutedEventArgs e)
        {
            Choised = true;     
            tbPass2.IsEnabled = false;
            tbPass2.Visibility = Visibility.Collapsed;
            pass2.Visibility = Visibility.Collapsed;
        }

        private void Registration(string tbL, string tbP1, string tbP2)
        {
            if (tbL == "")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (tbP1 == tbP2)
            {
                
                /*
                проверить логин на незанятость если такого
               нет зарегистрировать пользователя, иначе сообщить об ошибке
                */
            }
            else
            {
                MessageBox.Show("Пароли различны");
                return;
            }
            CloseApplication(tbL);
        }
        private void Logining(string tbL, string tbP1)
        {
            if (tbL == "")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (tbP1 == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            CloseApplication(tbL);
        }
   
        private void bEnter_Click_1(object sender, RoutedEventArgs e)
        {

            if (Choised == false) //если регистрация
            {
                Registration(tbLogin.Text, tbPass1.Text, tbPass2.Text);
            }
            else
            {
                Logining(tbLogin.Text, tbPass1.Text);
            }
        }
        private void CloseApplication(string tbL)
        {
            menu.Left = this.Left; // Установка положения нового окна по горизонтали
            menu.Top = this.Top;
            
            menu.Show();
            menu.lUser.Content = "Текущий пользователь: " + tbL;
            Close();
        }
    }
}
