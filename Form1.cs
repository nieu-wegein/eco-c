using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
       
        List<Book> booksList = new List<Book>();  //Список всех книг
        string CurrentFilePath = null;
        bool isChanged = false;


        
        private void OpenXmlButton_Click(object sender, EventArgs e)
        {
            LoadBooks();
        }

        private void SaveXmlButton_Click(object sender, EventArgs e)
        {
            SaveBooks();
        }

        private void SaveHtmlButton_Click(object sender, EventArgs e)
        {
            SaveHtml();
        }

        private void DeleteBooklButton_Click(object sender, EventArgs e)
        {
            DeleteCurrentBook();
        }

        private void AddBookButton_Click(object sender, EventArgs e)
        {
            InsertEmptyBook();
        }


        // При редактировании любой ячейки меняется свойство книги в списке. Свойство определяется по названию столбца, а сама книга - по номеру строки.
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isChanged = true;
                string header = dataGridView1.Columns[e.ColumnIndex].HeaderText;
                switch (header)
                {
                    case "Книга": booksList[e.RowIndex].bookInfo["title"] = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString(); break;
                    case "Автор": booksList[e.RowIndex].bookInfo["authors"] = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString(); break;
                    case "Категория": booksList[e.RowIndex].Category = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString(); break;
                    case "Цена": booksList[e.RowIndex].bookInfo["price"] = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString(); break;
                }
            }

        }



        private string ShowDialogWindow(FileDialog dialog)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return null;
        }


        //Открывает файл и создает для каждого xml-элемента book объект книги
        private void LoadBooks()
        {
            string Path = ShowDialogWindow(openFileDialog);

            if (Path != null)
            {
                CurrentFilePath = Path;
                this.Text = CurrentFilePath;
                booksList = new List<Book>();

                XmlDocument xmlfile = new XmlDocument();
                xmlfile.Load(CurrentFilePath);

                foreach (XmlElement bookNode in xmlfile.DocumentElement)
                {
                    if (bookNode.Name == "book")
                    {
                        Book book = new Book(bookNode);
                        AddBook(book);
                    };
                }
                isChanged = false;
                DisplayBooks();
            }
        }


        //Сохраняет отчет из текущего xml-файла
        private void SaveHtml()
        {
        
                if (CurrentFilePath != null)
                {
                    if (isChanged == false)
                    {
                        XslCompiledTransform transform = new XslCompiledTransform();
                        transform.Load("template.xsl");
                        try
                        {
                            transform.Transform(CurrentFilePath, ShowDialogWindow(saveHtmlDialog));
                        }
                        catch { return; }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Все изменения будут добавлены в xml-файл. Продолжить?", " ", MessageBoxButtons.OKCancel);
                        if (result == DialogResult.OK)
                        {
                            XmlDocument xmlfile = createXmlDocument();
                            xmlfile.Save(CurrentFilePath);
                            isChanged = false;
                            SaveHtml();
                        }
                        else return;
                    }
                }
                else
                {
                    MessageBox.Show("Перед созданием отчета необходимо сохранить xml-файл");
                }
            

        }


        //Сохраняет список книг в новый xml-файл
        private void SaveBooks()
        {
            string Path = ShowDialogWindow(saveFileDialog);

            if (Path != null)
            {
                CurrentFilePath = Path;
                this.Text = CurrentFilePath;
                XmlDocument xmlfile = createXmlDocument();
                xmlfile.Save(CurrentFilePath);
                isChanged = false;
            }
        }


        //Создает xml-элемент для каждого объекта из списка книг и возврашает xml-документ
        private XmlDocument createXmlDocument()
        {
            XmlDocument xmldocument = new XmlDocument();
            XmlDeclaration declaration = xmldocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldocument.AppendChild(declaration);

            XmlElement bookstoreNode = xmldocument.CreateElement("bookstore");

            foreach (Book book in booksList)
            {
                XmlElement bookNode = Book.toXml(book, xmldocument);
                bookstoreNode.AppendChild(bookNode);
            }

            xmldocument.AppendChild(bookstoreNode);
            return xmldocument;
        }





        private void InsertEmptyBook()
        {
           Book book = new Book();
            AddBook(book);
            DisplayBooks();

        }

        private void DeleteCurrentBook()
        {
            if (dataGridView1.CurrentRow != null)
            {
                RemoveBook(dataGridView1.CurrentRow.Index);
                DisplayBooks();
            }
        }

        private void DisplayBooks()
        {
            dataGridView1.Rows.Clear();
            foreach (Book book in booksList)
            {
                dataGridView1.Rows.Add(book.bookInfo["title"], book.bookInfo["authors"], book.Category, book.bookInfo["price"]);
            }
        }




    
        private void AddBook(Book book)
        {
            booksList.Add(book);
            isChanged = true;
        }

        private void RemoveBook(int index)
        {
            booksList.RemoveAt(index);
            isChanged = true;
        }

    }
}
