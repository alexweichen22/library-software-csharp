using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibraryTry3.Domain;
using LibraryTry3.Tools;
using Microsoft.Win32;

namespace LibraryTry3.Views
{
    /// <summary>
    /// Interaction logic for ReaderView.xaml
    /// </summary>
    public partial class ReaderView : UserControl
    {
        public byte[] currenPhoto;
        public Reader CurrentReader;
        public IQueryable<Loan> currentReaderActiveLoans;
        public ReaderView()
        {
            InitializeComponent();
            comboGender.ItemsSource = Enum.GetValues(typeof(Reader.GenderEnum));
            comboProvince.ItemsSource = Enum.GetValues(typeof(Address.ProvinceEnum));

            List<Reader> readers = Globals.context.ReaderList.ToList();
            ReaderDataGrid.ItemsSource = readers;
        }

        private void Btn_AddReader_OnClick(object sender, RoutedEventArgs e)
        {
            var cityText = txtCity.Text;
            var emailText = txtEmail.Text;
            var firstText = txtFirst.Text;
            var idText = txtID.Text;
            /*if (idText.Length == 0)
            {
                MessageBox.Show("Id number is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int idNum;
            if (!int.TryParse(idText, out idNum))
            {
                MessageBox.Show("Please input the numbers", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<Reader> readers = Globals.context.ReaderList.ToList();
            int total = readers.Count;
            if (idNum <= total)
            {
                MessageBox.Show($"Please input number more than {total}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }*/

            var lastText = txtLast.Text;
            var phoneText = txtPhone.Text;
            var postcodeText = txtPostcode.Text;
            var roomNum = txtRoomNo.Text;
            var street1Text = txtStreet1.Text;
            var street2Text = txtStreet2.Text;
            var genderText = comboGender.SelectedItem;
            var provinceSelectedItem = comboProvince.SelectedItem;
            DateTime? selectedDate = birthDatePicker.SelectedDate;
            DateTime? startDate = startDatePiker.SelectedDate;
            if (!ValidateFields())
            {
                return;
            }
            //MessageBox.Show(date.ToString());
            Reader reader = new Reader
            {
                ReaderID = "GLCS"+idText,
                CurrentLoanNum = 0,
                FirstName = firstText,
                LastName = lastText,
                Gender = (Reader.GenderEnum)genderText,
                DateOfBirth = (DateTime)selectedDate,
                StartDate = (DateTime)startDate,
                Email = emailText,
                Photo = currenPhoto,
                Phone = phoneText,
                Password = firstText.Substring(0,1).ToUpper() + lastText.Substring(0,1).ToLower()
                         + selectedDate.ToString().Substring(0,2),


            };
            Address address = new Address
            {
                AddressID = "GLCS" + idText,
                RoomNum = roomNum,
                AddressLine1 = street1Text,
                AddressLine2 = street2Text,
                City = cityText,
                Province = (Address.ProvinceEnum) provinceSelectedItem,
                Postcode = postcodeText,
                Country = "Canada"
                

            };
            Globals.context.AddressList.Add(address);
            Globals.context.ReaderList.Add(reader);
            ClearInput();
            
            Globals.context.SaveChanges();
            MessageBox.Show("Add successfully", "Information", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void BtnReaderPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.jpeg;*.gif;*.png|All Files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    currenPhoto = File.ReadAllBytes(openFileDialog.FileName);
                    txtPhoto.Visibility = Visibility.Hidden;
                    BitmapImage bitmap = LibUtils.ByteArrayToBitmapImage(currenPhoto);
                    readerPhoto.Source = bitmap;
                }
                catch (Exception exception) when (exception is SystemException || exception is IOException)
                {
                    MessageBox.Show(exception.Message, "File reading failed", MessageBoxButton.OK,
                        MessageBoxImage.Error);

                }
            }
        }
        

        private void ReaderDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var selectedItem = ReaderDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                if (selectedItem is Reader)
                {
                    CurrentReader = (Reader) selectedItem;
                    List<Book> toBeReturnedBooks = GetToBeReturnedBooks();
                    /*var bookList = Globals.context.BookList.Join(currentReaderActiveLoans,
                        book => book.BookID,
                        loan => loan.BookID,
                        (book, loan) => new
                        {
                            LoanID = loan.LoanID,
                            BookID = loan.BookID,
                            BookName = book.BookName,
                            Author = book.Author,
                            Publisher = book.Publisher,
                            ISBN = book.ISBN,
                            CallNum = book.CallNum,
                            Cover = book.Cover,
                            LoanDate = loan.LoanDate

                        }).ToList();*/
                    borrowedBookCheckBox.ItemsSource = toBeReturnedBooks;
                    GetCurrentReaderLoanHistory();
                }
            }
            
        }

        private void GetCurrentReaderLoanHistory()
        {
            List<Loan> loans = Globals.context.LoanList.Where(l => l.ReaderID == CurrentReader.ReaderID).ToList();
            LoanDataGrid.ItemsSource = loans;
        }

        private void BtnReturn_OnClick(object sender, RoutedEventArgs e)
        {
            var borrowedBooks = borrowedBookCheckBox.SelectedItems;
            MessageBox.Show(borrowedBooks.Count.ToString());
            if (borrowedBooks.Count == 0)
            {
                MessageBox.Show("You have not chosen any book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime? loanReturnDate = returnDatePicker.SelectedDate;
            if (loanReturnDate == null)
            {
                MessageBox.Show("You have not chosen the return date.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var borrowedBook in borrowedBooks)
            {
                if (borrowedBook is Book)
                {
                    Book book = (Book) borrowedBook;
                    book.Copies++;
                    CurrentReader.CurrentLoanNum--;
                    ICollection<Loan> bookLoanList = book.LoanList;
                    var loans = bookLoanList.Where(l => l.ReaderID == CurrentReader.ReaderID);
                    foreach (var loan in loans)
                    {
                        loan.ReturnDate = loanReturnDate;
                    }
                }
            }
            
            Globals.context.SaveChanges();
            MessageBox.Show("Return successfully.","Information",MessageBoxButton.OK,
                MessageBoxImage.Information);
            ReaderDataGrid.ItemsSource = Globals.context.ReaderList.ToList();
            var toBeReturnedBooks = GetToBeReturnedBooks();
            borrowedBookCheckBox.ItemsSource = toBeReturnedBooks;
            GetCurrentReaderLoanHistory();

        }

        private List<Book> GetToBeReturnedBooks()
        {
            currentReaderActiveLoans = Globals.context.LoanList
                .Where(loan => (loan.ReaderID == CurrentReader.ReaderID && loan.ReturnDate == null));

            List<Book> books = Globals.context.BookList.ToList();
            List<Book> toBeReturnedBooks = new List<Book>();
            foreach (var activeLoan in currentReaderActiveLoans)
            {
                foreach (var book in books)
                {
                    if (book.BookID == activeLoan.BookID)
                    {
                        toBeReturnedBooks.Add(book);
                    }
                }
            }

            return toBeReturnedBooks;
        }

        private void Btn_clear_reader_OnClick(object sender, RoutedEventArgs e)
        {
            ClearInput();
        }

        private void ClearInput()
        {
            txtCity.Clear();
            txtEmail.Clear();
            txtFirst.Clear();
            txtID.Clear();
            txtLast.Clear();
            txtPhone.Clear();
            txtPhoto.Visibility = Visibility.Visible;
            txtPostcode.Clear();
            txtRoomNo.Clear();
            txtStreet1.Clear();
            txtStreet2.Clear();
            txtEmail.Clear();
            birthDatePicker.Text = "";
            startDatePiker.SelectedDate = null;
            comboGender.Text = "";
            comboProvince.Text = "";
            readerPhoto.Source = null;
        }

        private void Btn_refresh_readers_OnClick(object sender, RoutedEventArgs e)
        {
            List<Reader> readers = Globals.context.ReaderList.ToList();
            ReaderDataGrid.ItemsSource = readers;
           // ReaderDataGrid.SelectedIndex = -1;
            ReaderDataGrid.SelectedItem = null;
        }

        private void Btn_edit_reader_OnClick(object sender, RoutedEventArgs e)
        {
            if (ReaderDataGrid.SelectedIndex == -1)
            {
                MessageBox.Show("You have not chosen any reader.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            EditReaderDialog editReaderDialog = new EditReaderDialog(CurrentReader);
            editReaderDialog.ShowDialog();
            ReaderDataGrid.ItemsSource = Globals.context.ReaderList.ToList();
        }

        private void Btn_delete_reader_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = ReaderDataGrid.SelectedItems;
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("You have not chosen any reader.", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            var result = MessageBox.Show("Are you sure to delete permanently?", "Information",
                MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (MessageBoxResult.Yes == result)
            {
                foreach (var selectedItem in selectedItems)
                {
                    if (selectedItem is Reader)
                    {
                        Reader reader = (Reader)selectedItem;
                        Globals.context.ReaderList.Remove(reader);
                    }
                }

            }

            Globals.context.SaveChanges();
            ReaderDataGrid.ItemsSource = Globals.context.ReaderList.ToList();
        }

        private void ReaderDataGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

            if (ReaderDataGrid.SelectedItem == null)
            {
                borrowedBookCheckBox.ItemsSource = null;
                LoanDataGrid.ItemsSource = null;
            }

        }

        private bool ValidateFields()
        {
            var cityText = txtCity.Text;
            if (cityText.Length == 0)
            {
                MessageBox.Show("City is a mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string cityPattern = @"^[A-Za-z\s\.-:]{3,}$";
            if (!Regex.IsMatch(cityText, cityPattern))
            {
                MessageBox.Show("City must be at least three letters with space,and special characters",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            var emailText = txtEmail.Text;
            if (emailText.Length == 0)
            {
                MessageBox.Show("Email is a mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            if (!Regex.IsMatch(emailText, emailPattern))
            {
                MessageBox.Show("You email is not right, please input again.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            var firstText = txtFirst.Text;
            if (firstText.Length == 0)
            {
                MessageBox.Show("First Name is a mandatory field",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string namePattern = @"^[A-Z][a-z/s]{2,}$";
            if (!Regex.IsMatch(firstText, namePattern))
            {
                MessageBox.Show("First name must have at least three letter, and first letter is uppercase",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var lastText = txtLast.Text;
            if (lastText.Length == 0)
            {
                MessageBox.Show("Last Name is a mandatory field",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(lastText, namePattern))
            {
                MessageBox.Show("Last name must have at least three letter, and first letter is uppercase",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            var idText = txtID.Text;
            if (idText.Length == 0)
            {
                MessageBox.Show("Id number is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int idNum;
            if (!int.TryParse(idText, out idNum))
            {
                MessageBox.Show("Please input the numbers", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            List<Reader> readers = Globals.context.ReaderList.ToList();
            int total = readers.Count;
            if (idNum <= total)
            {
                MessageBox.Show($"Please input number more than {total}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var phoneText = txtPhone.Text;
            if (phoneText.Length == 0)
            {
                MessageBox.Show("Phone is a mandatory field",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string phonePattern = @"^\d{10}$";
            if (!Regex.IsMatch(phoneText, phonePattern))
            {
                MessageBox.Show("Phone number must be 10 numbers.", "Errror",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var postcodeText = txtPostcode.Text;
            if (postcodeText.Length == 0)
            {
                MessageBox.Show("Postcode is a mandatory field",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string codePattern = @"^[A-Z]\d[A-Z]\s\d[A-Z]\d$";
            if (!Regex.IsMatch(postcodeText, codePattern))
            {
                MessageBox.Show("Postal code must be like this A8A 9H9", "Errror",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var roomTxt = txtRoomNo.Text;
            if (roomTxt.Length == 0)
            {
                MessageBox.Show("Room Number is a mandatory field",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string roomPattern = @"[\w\-.]{2,}$";
            if (!Regex.IsMatch(roomTxt, roomPattern))
            {
                MessageBox.Show("Post code is a combination of letters, numbers and special charachters like .-, at least 2", "Errror",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var street1Text = txtStreet1.Text;
            if (street1Text.Length == 0)
            {
                MessageBox.Show("Street 1 is a mandatory field",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string streetPattern = @"[\w\s\.-]{2,}$";
            if (!Regex.IsMatch(street1Text, streetPattern))
            {
                MessageBox.Show("Street must be at least two letters, number,  with space and special characters",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            /*var street2Text = txtStreet2.Text;
            if (!Regex.IsMatch(street2Text, streetPattern))
            {
                MessageBox.Show("Street must be at least two letters with space and special characters",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }*/

            DateTime? birthDate = birthDatePicker.SelectedDate;
            if (birthDate == null)
            {
                MessageBox.Show("You must choose a date", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            DateTime? startDate = startDatePiker.SelectedDate;
            if (startDate == null)
            {
                MessageBox.Show("You must choose a date", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            var selectedGender = comboGender.SelectedItem;
            if (selectedGender == null)
            {
                MessageBox.Show("You must choose the gender", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            var selectedProvince = comboProvince.SelectedItem;
            if (selectedProvince == null)
            {
                MessageBox.Show("You must choose the province", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            return true;
        }

    }
}
