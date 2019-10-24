using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFCRUD00
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RefreshGrid();
            Clear();
        }

        private void RefreshGrid()
        {
            dataGrid.ItemsSource = DataManager.GetData();
        }

        private void Clear()
        {
            lblID.Content = String.Empty;
            txtTitle.Text = String.Empty;
            txtAuthor.Text = String.Empty;
            txtISBN.Text = String.Empty;

            EnableTxtFields(false);
        }

        private void EnableTxtFields(bool state)
        {
            txtTitle.IsEnabled = state;
            txtAuthor.IsEnabled = state;
            txtISBN.IsEnabled = state;
        }

        private bool ValidateForm()
        {
            if (String.IsNullOrEmpty(txtTitle.Text) || String.IsNullOrEmpty(txtAuthor.Text) || String.IsNullOrEmpty(txtISBN.Text))
            {
                MessageBox.Show("Todos los datos son obligatorios", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        private void ShowBook(Book book)
        {
            lblID.Content = book.Id;
            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            txtISBN.Text = book.ISBN;
        }

        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EnableTxtFields(true);
            txtTitle.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                if (! String.IsNullOrEmpty(lblID.Content.ToString()))
                {
                    UpdateBook();
                }
                else
                {
                    InsertBook();
                }
            }
        }

        private void InsertBook()
        {
            Book book = new Book
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                ISBN = txtISBN.Text
            };

            if (DataManager.Insert(book) != -1)
            {
                MessageBox.Show("Libro registrado correctamente", "Nuevo libro", MessageBoxButton.OK, MessageBoxImage.Information);
                Clear();
                RefreshGrid();
                return;
            }
            else
            {
                MessageBox.Show("El Libro NO pudo ser registrado correctamente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void UpdateBook()
        {
            Book book = new Book
            {
                Id = Int32.Parse(lblID.Content.ToString()),
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                ISBN = txtISBN.Text
            };

            if (DataManager.Update(book) != -1)
            {
                MessageBox.Show("Libro actualizado correctamente", "Actualizar libro", MessageBoxButton.OK, MessageBoxImage.Information);
                Clear();
                RefreshGrid();
                return;
            }
            else
            {
                MessageBox.Show("El Libro NO pudo ser actualizado correctamente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView dataRow = (DataRowView)dataGrid.SelectedItem;
            int id = Int32.Parse(dataRow.Row.ItemArray[0].ToString());

            Book book = DataManager.Find(id);
            ShowBook(book);
            EnableTxtFields(true);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(lblID.Content.ToString()))
            {
                MessageBox.Show("Selecciona de la tabla el libro que quieres eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("¿Desea eliminar este libro?", "Eliminar libro", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int id = Int32.Parse(lblID.Content.ToString());
                DataManager.Delete(id);
                Clear();
                RefreshGrid();
            }
        }
    }
}
