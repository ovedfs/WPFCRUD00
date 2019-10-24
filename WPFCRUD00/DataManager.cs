using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFCRUD00
{
    public class DataManager
    {
        protected static string connectionString = "Data Source=library.db;Version=3;";

        public static DataView GetData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM books", connection);
                connection.Close();

                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                return dataSet.Tables[0].DefaultView;
            }
        }

        public static int Insert(Book book)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                int result = -1;

                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "INSERT INTO books(title, author, isbn) VALUES (@title, @author, @isbn)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@author", book.Author);
                    cmd.Parameters.AddWithValue("@isbn", book.ISBN);
                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return -1;
                    }
                }
                connection.Close();

                return result;
            }
        }

        public static int Update(Book book)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                int result = -1;

                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "UPDATE books SET title = @title, author = @author, isbn = @isbn WHERE id = @id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@author", book.Author);
                    cmd.Parameters.AddWithValue("@isbn", book.ISBN);
                    cmd.Parameters.AddWithValue("@id", book.Id);
                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return -1;
                    }
                }
                connection.Close();

                return result;
            }
        }

        public static int Delete(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                int result = -1;

                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "DELETE FROM books WHERE id = @id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return -1;
                    }
                }
                connection.Close();

                return result;
            }
        }

        public static Book Find(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM books WHERE id = @id LIMIT 1", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Book book = new Book
                        {
                            Title = reader[1].ToString(),
                            Author = reader[2].ToString(),
                            ISBN = reader[3].ToString(),
                            Id = Int32.Parse(reader[0].ToString())
                        };

                        return book;
                    }
                }
                connection.Close();
                
                return new Book();
            }
        }
    }
}
