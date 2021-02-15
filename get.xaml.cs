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
using LibraryTry3.Domain;
using Xceed.Wpf.Toolkit.Primitives;

namespace LibraryTry3
{
    /// <summary>
    /// Interaction logic for get.xaml
    /// </summary>
    public partial class get : Window
    {
        public List<Book> books;
        public get()
        {
            InitializeComponent();
            books = Globals.context.BookList.ToList();
            Bookchecklistbox.ItemsSource = books;
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = Bookchecklistbox.SelectedItems;
            if (selectedItems != null)
            {
                foreach (var item in selectedItems)
                {
                    if (item is Book)
                    {
                        Book book = (Book)item;
                        books.Remove(book);
                    }
                }

                Globals.context.SaveChanges();
                MessageBox.Show("delete");
                
                Bookchecklistbox.ItemsSource = Globals.context.BookList.ToList();
            }
        }
    }
}
