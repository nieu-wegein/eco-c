using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;


namespace WindowsFormsApp1
{
    class Book
    {
        /*public string Title { get; set; }
        public string Authors { get; set; }
        public string Year { get; set; }
        public string Price { get; set; }*/

        public Dictionary<string, string> bookInfo = new Dictionary<string, string>();


        public string Category { get; set; }
        public string Cover { get; set; }


        public Book()
        {
            bookInfo["title"] = "Название книги";
            bookInfo["authors"] = "Авторы";
            this.Category = "Категория";
            bookInfo["price"] = "Цена";
        }



        public Book(XmlElement bookNode)
        {
            if (bookNode.HasAttribute("category"))
                this.Category = bookNode.Attributes.GetNamedItem("category").Value;

            if (bookNode.HasAttribute("cover"))
                this.Cover = bookNode.Attributes.GetNamedItem("cover").Value;

            foreach (XmlNode infoNode in bookNode)
            {
                
               switch (infoNode.Name)
                {
                    case "title": bookInfo["title"] = infoNode.InnerText; break;
                    case "author":
                        if (bookInfo.ContainsKey("authors"))
                                {
                                    bookInfo["authors"] += "; " + infoNode.InnerText; break;
                                }
                                else
                                {
                                     bookInfo["authors"] = infoNode.InnerText; break;
                                }
                    case "year": bookInfo["year"] = infoNode.InnerText; break;
                    case "price": bookInfo["price"] = infoNode.InnerText; break;
                }  
                
            }
        }




        public static XmlElement toXml(Book book, XmlDocument xmldocument) {

            XmlElement bookNode = xmldocument.CreateElement("book");

            if (book.Category != null)
            {
                bookNode.SetAttribute("category", book.Category);
            }

            if (book.Cover != null)
            {
                bookNode.SetAttribute("cover", book.Cover);
            }

            foreach (var info in book.bookInfo)
            {
                if (info.Key == "authors")
                {
                    string[] authorsList = info.Value.Split(new char[] {';'});
                    foreach (string author in authorsList)
                    {
                        XmlElement element = xmldocument.CreateElement("author");
                        element.InnerText = author;
                        bookNode.AppendChild(element);
                    }
                }
                else
                {
                    XmlElement element = xmldocument.CreateElement(info.Key);
                    element.InnerText = info.Value;
                    bookNode.AppendChild(element);
                } 
            }
            return bookNode;
        }
    }
}
