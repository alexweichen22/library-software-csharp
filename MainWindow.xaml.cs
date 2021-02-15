using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
using LibraryTry3.Domain;
using LibraryTry3.Tools;
using Border = System.Windows.Controls.Border;
using Window = System.Windows.Window;

namespace LibraryTry3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Administrator> administrators = Globals.context.AdminList.ToList();
            if (administrators.Count == 0)
            {
                Administrator admin = new Administrator
                {

                    Password = "000000",
                    FirstName = "Devin",
                    LastName = "Williams"
                };
                Globals.context.AdminList.Add(admin);
                Globals.context.SaveChanges();
            }
            

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {

            //MessageBox.Show(this.Width.ToString());
            if (this.Width < 400)
            {
                lbLib.Visibility = Visibility.Hidden;
                btnMinus.Visibility = Visibility.Hidden;
                btnNormal.Visibility = Visibility.Hidden;
            }
            else
            {
                lbLib.Visibility = Visibility.Visible;
                btnMinus.Visibility = Visibility.Visible;
                btnNormal.Visibility = Visibility.Visible;
            }

        }

        private void btnNoraml_Click(object sender, RoutedEventArgs e)
        {
            
                /*this.Width = SystemParameters.WorkArea.Width;
                this.Height = SystemParameters.WorkArea.Height;
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                Left = 0;
                Top = 0;*/
                //this.WindowState = WindowState.Maximized;
                /*var screenWidth = SystemParameters.WorkArea.Width + SystemParameters.BorderWidth;
               
                MessageBox.Show(this.Width  + " : " + screenWidth );
                
                if (this.Width == SystemParameters.BorderWidth)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }*/
                this.WindowState = WindowState.Maximized;





        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnMinize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void backNormal_Click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Normal;

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userType = comboUserType.Text;
            string idText = txtID.Text;
            int id;
            if (!int.TryParse(idText, out id))
            {
                MessageBox.Show("Invalid Password");
                return;
            }
            string password = txtPsd.Password.ToString();
            if (userType.Equals("Administrator"))
            {
                List<Administrator> administrators = Globals.context.AdminList.ToList();
                foreach (var administrator in administrators)
                {
                    if (password == administrator.Password)
                    {
                        LibraryHome libraryHome = new LibraryHome(administrator);
                        libraryHome.Show();
                        this.Hide();
                    }
                }

                
                
            }
        }
    }
}
