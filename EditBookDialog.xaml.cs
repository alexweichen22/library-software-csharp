using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using LibraryTry3.Domain;
using LibraryTry3.Tools;
using Microsoft.Win32;
using Enum = System.Enum;

namespace LibraryTry3
{
    /// <summary>
    /// Interaction logic for EditBookDialog.xaml
    /// </summary>
    public partial class EditBookDialog : Window
    {
        public byte[] currenPhoto;
        public Book currenBook;
        public EditBookDialog(Book book)
        {
            InitializeComponent();
            currenBook = book;
            comboCategory.ItemsSource = Enum.GetValues(typeof(Book.CategoryEnum));
            comboCollection.ItemsSource = Enum.GetValues(typeof(Book.CollectionEnum));
            comboLanguage.ItemsSource = Enum.GetValues(typeof(Book.LanguageEnum));
            txtAuthor.Text = book.Author;
            txtBookName.Text = book.BookName;
            txtCallNum.Text = book.CallNum;
            txtCopies.Text = book.Copies.ToString();
            txtISBN.Text = book.ISBN;
            txtPrice.Text = book.Price.ToString();
            txtPublisher.Text = book.Publisher;
            txtSummary.Text = book.Summary;
            coverImg.Source = LibUtils.ByteArrayToBitmapImage(book.Cover);
            comboCategory.Text = book.Category.ToString();
            comboCollection.Text = book.Collection.ToString();
            comboLanguage.Text = book.Language.ToString();
            publishDatePicer.Text = book.PublishDate.ToString();
            shelfDatePicker.Text = book.ShelfDate.ToString();
            coverTxt.Visibility = Visibility.Hidden;

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
                    currenPhoto = File.ReadAllBytes(openFileDialog.FileName);
                    coverTxt.Visibility = Visibility.Hidden;
                    BitmapImage bitmap = LibUtils.ByteArrayToBitmapImage(currenPhoto);
                    coverImg.Source = bitmap;
                }
                catch (Exception exception) when (exception is SystemException || exception is IOException)
                {
                    MessageBox.Show(exception.Message, "File reading failed", MessageBoxButton.OK,
                        MessageBoxImage.Error);

                }
            }
        }

        private void ClearnInput()
        {
            txtAuthor.Clear();
            txtBookName.Clear();
            txtCallNum.Clear();
            txtCopies.Clear();
            txtISBN.Clear();
            txtPrice.Clear();
            txtPublisher.Clear();
            txtSummary.Clear();
            comboCategory.SelectedItem = null;
            comboCollection.SelectedItem = null;
            comboLanguage.SelectedItem = null;
            publishDatePicer.SelectedDate = null;
            shelfDatePicker.SelectedDate = null;
            coverImg.Source = null;
            coverTxt.Visibility = Visibility.Visible;
        }

        private void BtnClear_OnClick(object sender, RoutedEventArgs e)
        {
            
            ClearnInput();
        }

        private void Btn_UpdateBook_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isValidField())
            {
                return;
            }
            if (txtCallNum.Text.Length != 0)
            {
                currenBook.CallNum = txtCallNum.Text;
            }

            if (txtAuthor.Text.Length != 0)
            {
                currenBook.Author = txtAuthor.Text;
            }

            if (txtBookName.Text.Length != 0)
            {
                currenBook.BookName = txtBookName.Text;
            }

            if (comboCategory.SelectedIndex != -1)
            {
                currenBook.Category = (Book.CategoryEnum)comboCategory.SelectedItem;
            }

            if (comboCollection.SelectedIndex != -1)
            {
                currenBook.Collection = (Book.CollectionEnum)comboCollection.SelectedItem;
            }

            if (comboLanguage.SelectedIndex != -1)
            {
                currenBook.Language = (Book.LanguageEnum)comboLanguage.SelectedItem;
            }

            if (currenPhoto != null)
            {
                currenBook.Cover = currenPhoto;
            }

            if (txtPublisher.Text.Length != 0)
            {
                currenBook.Publisher = txtPublisher.Text;
            }

            if (txtISBN.Text.Length != 0)
            {
                currenBook.ISBN = txtISBN.Text;
            }

            DateTime? selectedDate = publishDatePicer.SelectedDate;
            currenBook.PublishDate = (DateTime)selectedDate;
            DateTime? dateTime = shelfDatePicker.SelectedDate;
            currenBook.ShelfDate = (DateTime)dateTime;
            if (txtSummary.Text.Length != 0)
            {
                currenBook.Summary = txtSummary.Text;
            }
            string copytext = txtCopies.Text;
            int copy;
            if (!int.TryParse(copytext, out copy))
            {
                MessageBox.Show("Copies must be a positive integer number",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currenBook.Copies = copy;

            string priceText = txtPrice.Text;
            double price;
            if (!double.TryParse(priceText, out price))
            {
                MessageBox.Show("Copies must be a number",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currenBook.Price = price;

            
            Globals.context.SaveChanges();
            MessageBox.Show("update successfully!");
            DialogResult = true;


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
            string pattern1 = @"^[A-Za-z0-9\s\:\,\.-]{1,}$";
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
            string pattern2 = @"^[A-Za-z0-9\s\/.-]{3,120}$";
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
            string pattern3 = @"^[A-Za-z\s\.-]{1,120}$";
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

            DateTime shelfDate = (DateTime)shelfDatePicker.SelectedDate;
            if (shelfDatePicker.Text.Length == 0)
            {
                MessageBox.Show("Please choose shelf date.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            DateTime publishDate = (DateTime)publishDatePicer.SelectedDate;
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

        private void EditBookDialog_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
