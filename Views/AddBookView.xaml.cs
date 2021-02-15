using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using Xceed.Wpf.AvalonDock.Controls;

namespace LibraryTry3.Views
{
    /// <summary>
    /// Interaction logic for AddBookView.xaml
    /// </summary>
    public partial class AddBookView : UserControl
    {
        private byte[] currentCover;

        private List<string> readerIdList = new List<string>();

        public object selectedItem;

        public Book CurrenBook;
        public List<string> fieldList = new List<string>()
        {
            "Book ID",
            "Name",
            "Author",
            "Publisher",
            "ISBN",
            "Call Number",
            "Publish Date",
            "Shelf Date",
            "Language",
            "Collection",
            "Category"
        };

        //private readonly List<Book> books;
        public AddBookView()
        {
            InitializeComponent();
            comboxField.ItemsSource = fieldList;
            
            comboCategory.ItemsSource = Enum.GetValues(typeof(Book.CategoryEnum));
            comboCollection.ItemsSource = Enum.GetValues(typeof(Book.CollectionEnum));
            comboLanguage.ItemsSource = Enum.GetValues(typeof(Book.LanguageEnum));
            List<Reader> readers = Globals.context.ReaderList.ToList();
            foreach (var reader in readers)
            {
                readerIdList.Add(reader.ReaderID);
            }

            comboReaderID.ItemsSource = readerIdList;

            List<Book> bookList = Globals.context.BookList.ToList();

            BookDataGrid.ItemsSource = bookList;

        }
        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            if (!isValidField())
            {
                return;
            }
            var authorText = txtAuthor.Text;
            var bookName = txtBookName.Text;
            var callNo = txtCallNum.Text;
            var publisherText = txtPublisher.Text;
            var copiesText = txtCopies.Text;
            int copies;
            if (!int.TryParse(copiesText, out copies))
            {
                MessageBox.Show("Copies should be an integer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (copies < 0)
            {
                MessageBox.Show("Copies must be a positive integer.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var isbnText = txtISBN.Text;
            var priceText = txtPrice.Text;
            double price;
            if (!double.TryParse(priceText, out price))
            {
                MessageBox.Show("Price must be a decimal", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (price < 0)
            {
                MessageBox.Show("Price must be a positive number.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var summaryText = txtSummary.Text;
            DateTime shelfDate = (DateTime)shelfDatePicker.SelectedDate;
            DateTime publishDate =(DateTime)publishDatePicer.SelectedDate;
            var categorySelectedItem =(Book.CategoryEnum) comboCategory.SelectedItem;
            var collectionSelectedItem = (Book.CollectionEnum)comboCollection.SelectedItem;
            var languageSelectedItem = (Book.LanguageEnum)comboLanguage.SelectedItem;
            Book newBook = new Book
            {
                BookName = bookName,
                Author = authorText,
                Publisher = publisherText,
                ISBN = isbnText,
                CallNum = callNo,
                Copies = copies,
                Price = price,
                Summary = summaryText,
                ShelfDate = shelfDate,
                PublishDate = publishDate,
                Category = categorySelectedItem,
                Collection = collectionSelectedItem,
                Language = languageSelectedItem,
                Cover = currentCover

            };
            Globals.context.BookList.Add(newBook);
            Globals.context.SaveChanges();
            ClearInput();
            MessageBox.Show("Save successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

        private void ClearInput()
        {
            txtAuthor.Clear();
            txtBookName.Clear();
            txtCallNum.Clear();
            txtCopies.Clear();
            txtISBN.Clear();
            txtPrice.Clear();
            txtPublisher.Clear();
            txtSummary.Clear();
            comboCategory.Text = "";
            comboCollection.Text = null;
            comboLanguage.Text = null;
            publishDatePicer.Text = null;
            shelfDatePicker.Text = null;
            coverImg.Source = null;
        }

        private void btnCover_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.jpeg;*.gif;*.png|All Files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    currentCover = File.ReadAllBytes(openFileDialog.FileName);
                    coverTxt.Visibility = Visibility.Hidden;
                    BitmapImage bitmap = LibUtils.ByteArrayToBitmapImage(currentCover);
                    coverImg.Source = bitmap;
                }
                catch (Exception exception) when (exception is SystemException || exception is IOException)
                {
                    MessageBox.Show(exception.Message, "File reading failed", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    
                }
            }
        }

        private void BookDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            selectedItem = BookDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                if (selectedItem is Book)
                {
                    authorTitle.Content = "Author: ";
                    pubTitle.Content = "Publisher: ";
                    ISBNTitle.Content = "ISBN: ";
                    callTitle.Content = "Call Num: ";
                    languageTitle.Content = "Language: ";
                    sumTitle.Content = "Summary: ";
                    lbSummary.Visibility = Visibility.Visible;
                    issueDate.Visibility = Visibility.Visible;
                    comboReaderID.Visibility = Visibility.Visible;
                    btnIssueBook.Visibility = Visibility.Visible;
                    CurrenBook = (Book) selectedItem;
                    tbBookName.Text = CurrenBook.BookName;
                    lbAuthor.Content = CurrenBook.Author;
                    lbPublisher.Content = CurrenBook.Publisher;
                    lbCallNum.Content = CurrenBook.CallNum;
                    lbLanguage.Content = CurrenBook.Language.ToString();
                    lbSummary.Text = CurrenBook.Summary;
                    lbISBN.Content = CurrenBook.ISBN;
                    selectedBookCover.Source = LibUtils.ByteArrayToBitmapImage(CurrenBook.Cover);

                }
            }
        }


        private void BtnIssueBook_OnClick(object sender, RoutedEventArgs e)
        {
            var issueDateSelectedDate = issueDate.SelectedDate;
            var selectedReaderID = comboReaderID.SelectedItem;
            var num = CurrenBook.Copies;
            if (num == 0)
            {
                MessageBox.Show("No book on shelf, you can't issue.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Reader reader = Globals.context.ReaderList.
                Where(r => (r.ReaderID == (string)selectedReaderID))
                .FirstOrDefault();
            

            Loan loan = new Loan
            {
                LoanDate = (DateTime)issueDateSelectedDate,
                ReaderID = selectedReaderID.ToString(),

                BookID = CurrenBook.BookID,

            };
            List<Loan> loans = Globals.context.LoanList.Where(l => l.ReturnDate == null && (l.ReaderID == selectedReaderID)).ToList();
            foreach (var loanItem in loans)
            {
                if (loan.BookID == loanItem.BookID)
                {
                    MessageBox.Show("You have borrowed this book.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
            }

            ++reader.CurrentLoanNum;
            if (reader.CurrentLoanNum> 5)
            {
                MessageBox.Show("Every reader can borrow maximum 5 books.", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                reader.CurrentLoanNum--;
                return;
            }

            CurrenBook.Copies--;
            Globals.context.LoanList.Add(loan);
            Globals.context.SaveChanges();
            string bookName = CurrenBook.BookName;
            string readerName = reader.FirstName + " " + reader.LastName;
            string output = bookName + "\r\n" + "has been issued to " + "\r\n" + readerName;
            MessageBox.Show(output,"Information",MessageBoxButton.OK,
                MessageBoxImage.Information);
            BookDataGrid.ItemsSource = Globals.context.BookList.ToList();

        }

        private void Btn_EditBook_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrenBook == null)
            {
                MessageBox.Show("You have not chosen any book", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            EditBookDialog editBookDialog = new EditBookDialog(CurrenBook);
            editBookDialog.ShowDialog();

            BookDataGrid.ItemsSource = Globals.context.BookList.ToList();

        }

        private void BtnClear_OnClick(object sender, RoutedEventArgs e)
        {
            ClearInput();
        }

        private void BtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            List<Book> books = Globals.context.BookList.ToList();
            BookDataGrid.ItemsSource = books;
            BookDataGrid.SelectedIndex = -1;
        }

        private void Btn_delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (BookDataGrid.SelectedIndex == -1)
            {
                MessageBox.Show("You have not chosen any record.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedItems = BookDataGrid.SelectedItems;

            if (selectedItems.Count > 0)
            {
                var result = MessageBox.Show("Are you sure to delete the record?", "Question", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (var item in selectedItems)
                    {
                        if (item is Book)
                        {
                            Book book = (Book)item;
                            Globals.context.BookList.Remove(book);
                            Globals.context.SaveChanges();
                        }

                    }
                    clearIssueBook();
                }
                
                List<Book> list = Globals.context.BookList.ToList();
                BookDataGrid.ItemsSource = list;
                
            }
        }

        private void clearIssueBook()
        {
            lbAuthor.Content = "";
            lbSummary.Clear();
            lbCallNum.Content = "";
            lbLanguage.Content = "";
            lbISBN.Content = "";
            lbPublisher.Content = "";
            callTitle.Content = "";
            languageTitle.Content = "";
            selectedBookCover.Source = null;
            sumTitle.Content = "";
            lbSummary.Visibility = Visibility.Hidden;
            authorTitle.Content = "";
            pubTitle.Content = "";
            tbBookName.Text = "";
            ISBNTitle.Content = "";
            lbLanguage.Content = "";
            txtSummary.Clear();
            btnIssueBook.Visibility = Visibility.Hidden;
            comboReaderID.Visibility = Visibility.Hidden;
            issueDate.Visibility = Visibility.Hidden;

        }


        private bool isValidField()
        {
            
            var authorText = txtAuthor.Text;
            if (authorText.Length == 0)
            {
                MessageBox.Show("Author is mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            string pattern = @"^[A-Za-z\s\.\-]{3,}$";
            if (!Regex.IsMatch(authorText, pattern))
            {
                MessageBox.Show("Author Name must be more than 3 characters and dot, slash and space.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bookName = txtBookName.Text;
            if (bookName.Length == 0)
            {
                MessageBox.Show("Book name is mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            string pattern1 = @"^[\w\s\:\,\.-]+$";
            if (!Regex.IsMatch(bookName, pattern1))
            {
                MessageBox.Show("Book Name must be 1 to 250 characters and .-.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var callNo = txtCallNum.Text;
            if (callNo.Length == 0)
            {
                MessageBox.Show("Call Number is mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            string pattern2 = @"^[\w\s\/.-]{3,120}$";
            if (!Regex.IsMatch(callNo, pattern2))
            {
                MessageBox.Show("Call number must be 3 to 120 characters and .-/.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            
            if (txtPublisher.Text.Length == 0)
            {
                MessageBox.Show("Publisher is mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            var publisherText = txtPublisher.Text;
            string pattern3 = @"^[A-Za-z\s\.-:]{1,120}$";
            if (!Regex.IsMatch(publisherText, pattern3))
            {
                MessageBox.Show("Publisher must be 1 to 120 characters and .-.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            var copiesText = txtCopies.Text;
            if (copiesText.Length == 0)
            {
                MessageBox.Show("Copies is mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var isbnText = txtISBN.Text;
            if (isbnText.Length == 0)
            {
                MessageBox.Show("ISBN is mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string pattern4 = @"^\d{10}(\d{3})?$";
            if (!Regex.IsMatch(isbnText, pattern4))
            {
                MessageBox.Show("ISBN must be 10 or 13 numbers", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var priceText = txtPrice.Text;
            if (priceText.Length == 0)
            {
                MessageBox.Show("Price is mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            DateTime?shelfDate = shelfDatePicker.SelectedDate;
            if (shelfDatePicker.Text.Length == 0)
            {
                MessageBox.Show("Please choose shelf date.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            DateTime?publishDate = publishDatePicer.SelectedDate;
            if (publishDatePicer.Text.Length == 0)
            {
                MessageBox.Show("Please choose publish date.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            var categorySelectedItem = (Book.CategoryEnum)comboCategory.SelectedItem;
            if (comboCategory.SelectedIndex == -1)
            {
                MessageBox.Show("You must choose a category", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            var collectionSelectedItem = (Book.CollectionEnum)comboCollection.SelectedItem;
            if (comboCollection.SelectedIndex == -1)
            {
                MessageBox.Show("You must choose a collection", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            var languageSelectedItem = (Book.LanguageEnum)comboLanguage.SelectedItem;
            if (comboLanguage.SelectedIndex == -1)
            {
                MessageBox.Show("You must choose a language", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            return true;

        }

        private void BtnSearch_Book_OnClick(object sender, RoutedEventArgs e)
        {
            List<Book> searchedBookList = new List<Book>();
            if (comboxField.SelectedItem == null)
            {
                MessageBox.Show("You have not chosen any field.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string selectedField = comboxField.SelectedItem.ToString();
            var contextBookList = Globals.context.BookList;
            var searchText = txtSearchField.Text;
            var searchComboValue = comboxSearchTxt.SelectedItem;
            

            //MessageBox.Show(selectedField);
            switch (selectedField)
            {
                case "Book ID":
                    if (!GetSearchValue())
                    {
                        return;
                    }

                    int id;
                    if (!int.TryParse(searchText, out id))
                    {
                        MessageBox.Show("ID must be a positive integer.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    searchedBookList = contextBookList.Where(b => id == b.BookID).ToList();
                    break;
                case "Name":
                    if (!GetSearchValue())
                    {
                        return;
                    }
                    searchedBookList = contextBookList.Where(book => book.BookName.ToLower().Contains(searchText.ToLower()))
                        .ToList();
                    break;
                case "Author":
                    if (!GetSearchValue())
                    {
                        return;
                    }
                    searchedBookList = contextBookList.Where(book => book.Author.ToLower().Contains(searchText.ToLower())).ToList();
                    break;
                case "Publisher":
                    if (!GetSearchValue())
                    {
                        return;
                    }
                    searchedBookList = contextBookList.Where(b => b.Publisher.ToLower().Contains(searchText.ToLower())).ToList();
                    break;
                case "ISBN":
                    if (!GetSearchValue())
                    {
                        return;
                    }
                    searchedBookList = contextBookList.Where(b => b.ISBN.ToLower().Contains(searchText.ToLower())).ToList();
                    break;
                case "Call Number":
                    if (!GetSearchValue())
                    {
                        return;
                    }
                    searchedBookList = contextBookList.Where(b => b.CallNum.ToLower().Contains(searchText.ToLower())).ToList();
                    break;
                case "Publish Date":
                    DateTime? selectedDate = dateFieldPicker.SelectedDate;
                    if (selectedDate == null)
                    {
                        MessageBox.Show("Please choose a date.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    searchedBookList = contextBookList.Where(b => b.PublishDate == selectedDate).ToList();
                    break;
                case "Shelf Date":
                    DateTime? selectedShelfDate = dateFieldPicker.SelectedDate;
                    if (selectedShelfDate == null)
                    {
                        MessageBox.Show("Please choose a date.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    searchedBookList = contextBookList.Where(b => b.ShelfDate == selectedShelfDate).ToList();
                    break;
                case "Language":
                    if (!GetComboxText())
                    {
                        return;
                    }
                    searchedBookList = contextBookList.Where(b => b.Language.ToString() == searchComboValue.ToString()).ToList();
                    break;
                case "Collection":
                    if (!GetComboxText())
                    {
                        return;
                    }
                    searchedBookList = contextBookList.Where(b => b.Collection.ToString() == searchComboValue.ToString()).ToList();
                    break;
                case "Category":
                    if (!GetComboxText())
                    {
                        return;
                    }
                    searchedBookList = contextBookList.Where(b => b.Category.ToString() == searchComboValue.ToString()).ToList();
                    break;
                default:
                    return;
            }
            BookDataGrid.ItemsSource = searchedBookList;
        }

        private void BtnClear_Search_OnClick(object sender, RoutedEventArgs e)
        {
            comboxField.SelectedIndex = -1;
            txtSearchField.Text = "";
            comboxSearchTxt.SelectedIndex = -1;
            dateFieldPicker.SelectedDate = null;
            txtSearchField.Visibility = Visibility.Visible;
            comboxSearchTxt.Visibility = Visibility.Hidden;
            dateFieldPicker.Visibility = Visibility.Hidden;
            BookDataGrid.ItemsSource = Globals.context.BookList.ToList();
        }
        

        private bool GetComboxText()
        {
            var item = comboxSearchTxt.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Please choose the value.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool GetSearchValue()
        {
            var searchText = txtSearchField.Text;

            if (searchText.Length == 0)
            {
                MessageBox.Show("Please input the search content", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }

            return true;
        }


        private void ComboxField_OnDropDownClosed(object sender, EventArgs e)
        {
            if (comboxField.SelectedItem == null)
            {
                MessageBox.Show("You have not chosen any search field.");
                return;
            }

            var selectedItem = comboxField.SelectedItem.ToString();
            switch (selectedItem)
            {
                case "Publish Date":
                case "Shelf Date":
                    dateFieldPicker.Visibility = Visibility.Visible;
                    txtSearchField.Visibility = Visibility.Hidden;
                    comboxSearchTxt.Visibility = Visibility.Hidden;
                    break;
                case "Language":
                    comboxSearchTxt.Visibility = Visibility.Visible;
                    dateFieldPicker.Visibility = Visibility.Hidden;
                    txtSearchField.Visibility = Visibility.Hidden;
                    comboxSearchTxt.ItemsSource = Enum.GetValues(typeof(Book.LanguageEnum));
                    break;

                case "Collection":
                    comboxSearchTxt.Visibility = Visibility.Visible;
                    dateFieldPicker.Visibility = Visibility.Hidden;
                    txtSearchField.Visibility = Visibility.Hidden;
                    comboxSearchTxt.ItemsSource = Enum.GetValues(typeof(Book.CollectionEnum));
                    break;
                case "Category":
                    comboxSearchTxt.Visibility = Visibility.Visible;
                    dateFieldPicker.Visibility = Visibility.Hidden;
                    txtSearchField.Visibility = Visibility.Hidden;
                    comboxSearchTxt.ItemsSource = Enum.GetValues(typeof(Book.CategoryEnum));
                    break;

                default:
                    comboxSearchTxt.Visibility = Visibility.Hidden;
                    dateFieldPicker.Visibility = Visibility.Hidden;
                    txtSearchField.Visibility = Visibility.Visible;
                    break;
            }
        }
        

        private void BookDataGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                ;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    ;
                    if (!dgr.IsMouseOver)
                    {
                        (dgr as DataGridRow).IsSelected = false;
                    }
                }
            }

            if (BookDataGrid.SelectedItem == null)
            {
                clearIssueBook();
            }
        }
    }
}
