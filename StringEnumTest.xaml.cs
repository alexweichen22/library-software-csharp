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
using LibraryTry3.Tools;

namespace LibraryTry3
{
    /// <summary>
    /// Interaction logic for StringEnumTest.xaml
    /// </summary>
    public partial class StringEnumTest : Window
    {
        public StringEnumTest()
        {
            InitializeComponent();
            List<Book> books = Globals.context.BookList.ToList();
            borrowedBookList.ItemsSource = books;
        }
    }
}
