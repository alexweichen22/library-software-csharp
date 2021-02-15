using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace LibraryTry3
{
    /// <summary>
    /// Interaction logic for EditReaderDialog.xaml
    /// </summary>
    public partial class EditReaderDialog : Window
    {
        public Reader CurrentReader;
        public byte[] currenPhoto;
        public EditReaderDialog(Reader reader)
        {
            InitializeComponent();
            CurrentReader = reader;
            currenPhoto = CurrentReader.Photo;
            comboGender.ItemsSource = Enum.GetValues(typeof(Reader.GenderEnum));
            comboProvince.ItemsSource = Enum.GetValues(typeof(Address.ProvinceEnum));
            tbID.Text = CurrentReader.ReaderID;
            txtCity.Text = CurrentReader.Address.City;
            txtEmail.Text = CurrentReader.Email;
            txtFirst.Text = CurrentReader.FirstName;
            txtLast.Text = CurrentReader.LastName;
            txtPhone.Text = CurrentReader.Phone;
            txtPostcode.Text = CurrentReader.Address.Postcode;
            txtRoomNo.Text = CurrentReader.Address.RoomNum;
            txtStreet1.Text = CurrentReader.Address.AddressLine1;
            txtStreet2.Text = CurrentReader.Address.AddressLine2;
            txtPhoto.Visibility = Visibility.Hidden;
            readerPhoto.Source = LibUtils.ByteArrayToBitmapImage(currenPhoto);
            birthDatePicker.SelectedDate = CurrentReader.DateOfBirth;
            startDatePiker.SelectedDate = CurrentReader.StartDate;
            comboGender.Text = CurrentReader.Gender.ToString();
            comboProvince.Text = CurrentReader.Address.Province.ToString();

        }

        private void Btn_close_edit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_update_reader_OnClick(object sender, RoutedEventArgs e)
        {
            if (txtCity.Text.Length == 0)
            {
                MessageBox.Show("City is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.Address.City = txtCity.Text;
            if (txtEmail.Text.Length == 0)
            {
                MessageBox.Show("Email is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.Email = txtEmail.Text;

            if (txtFirst.Text.Length == 0)
            {
                MessageBox.Show("First Name is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.FirstName = txtFirst.Text;

            /*var idText = txtID.Text;
            if (idText.Length == 0)
            {
                MessageBox.Show("Reader ID is the mandatory field", "Error",
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
            }

            CurrentReader.ReaderID = "GLCS" + idNum;*/

            var lastText = txtLast.Text;
            if (lastText.Length == 0)
            {
                MessageBox.Show("Last Name is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.LastName = lastText;

            var phoneText = txtPhone.Text;
            if (phoneText.Length == 0)
            {
                MessageBox.Show("Phone is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.Phone = phoneText;

            var postcodeText = txtPostcode.Text;
            if (postcodeText.Length == 0)
            {
                MessageBox.Show("Post Code is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.Address.Postcode = postcodeText;

            var roomText = txtRoomNo.Text;
            if (roomText.Length == 0)
            {
                MessageBox.Show("Room Num is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.Address.RoomNum = roomText;

            var street1Text = txtStreet1.Text;
            if (street1Text.Length == 0)
            {
                MessageBox.Show("Address Line1 is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.Address.AddressLine1 = street1Text;

            var street2Text = txtStreet2.Text;
            CurrentReader.Address.AddressLine2 = street2Text;

            var selectedGender = comboGender.SelectedItem;
            if (selectedGender == null)
            {
                MessageBox.Show("Gender is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CurrentReader.Gender = (Reader.GenderEnum)selectedGender;

            var selectedProvince = comboProvince.SelectedItem;
            if (selectedProvince == null)
            {
                MessageBox.Show("Pronvince is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.Address.Province = (Address.ProvinceEnum) selectedProvince;

            DateTime? selectedDate = birthDatePicker.SelectedDate;
            if (selectedDate == null)
            {
                MessageBox.Show("Birth date is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.DateOfBirth = (DateTime)selectedDate;

            DateTime? startDate = startDatePiker.SelectedDate;
            if (startDate == null)
            {
                MessageBox.Show("Start Date is the mandatory field", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentReader.StartDate = (DateTime) startDate;

            Globals.context.SaveChanges();
            DialogResult = true;
        }

        private void Btn_clear_reader_info_OnClick(object sender, RoutedEventArgs e)
        {
            txtCity.Clear();
            txtEmail.Clear();
            txtFirst.Clear();
            tbID.Text = "";
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

        private void Btn_update_photo_OnClick(object sender, RoutedEventArgs e)
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


        private void EditReaderDialog_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
