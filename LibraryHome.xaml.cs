using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
using System.Windows.Threading;
using LibraryTry3.Domain;
using LibraryTry3.Views;

namespace LibraryTry3
{
    /// <summary>
    /// Interaction logic for LibraryHome.xaml
    /// </summary>
    public partial class LibraryHome : Window
    {
        //private static LibraryHome _obj;
        public AddBookView addBookView = new AddBookView();
        public ReaderView readerView= new ReaderView();
        public Administrator currentAdmin;
        public HomeView homeView = new HomeView();
        private string flag = "home";


        public LibraryHome(Administrator admin)
        {
            InitializeComponent();
            currentAdmin = admin;
            loginName.Content = admin.FirstName + " " + admin.LastName;
            
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Interval = new TimeSpan(0,0,1);
            timer.Start();
            /*List<Book> books = Globals.context.BookList.ToList();
            latestBookDisplay.ItemsSource = books;*/
            
            displayUserControl.Children.Remove(addBookView);
            displayUserControl.Children.Remove(readerView);
            displayUserControl.Children.Add(homeView);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            TimerBlock.Text = DateTime.Now.ToString();
        }


        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnNormal_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(sender.ToString());
            if (flag == "book")
            {

            }
            else
            {
                flag = "book";
                displayUserControl.Children.Add(addBookView);
                displayUserControl.Children.Remove(readerView);
                displayUserControl.Children.Remove(homeView);
            }

        }

        private void btnReaders_Click(object sender, RoutedEventArgs e)
        {
            if (flag == "reader")
            {

            }
            else
            {
                flag = "reader";
                displayUserControl.Children.Remove(addBookView);
                displayUserControl.Children.Add(readerView);
                displayUserControl.Children.Remove(homeView);
            }
            
                

        }


        private void btnHome_click(object sender, RoutedEventArgs e)
        {
            if (flag == "home")
            {

            }
            else
            {
                flag = "home";
                displayUserControl.Children.Remove(addBookView);
                displayUserControl.Children.Remove(readerView);
                displayUserControl.Children.Add(homeView);
            }
            

        }

        private void Btn_Main_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
            
            

        }

    }
}
