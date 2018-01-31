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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace AddingLinks
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string text;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // CreateWordprocessingDocument(@"C:\Users\bezim\Desktop\Invoice.docx"); создание файла
        
            string strDoc = @"C:\Users\bezim\Desktop\dsa.docx"; 
            string strTxt = "Append text in body - OpenAndAddTextToWordDocument";
            OpenAndAddTextToWordDocument(strDoc, strTxt);
            string pattern = @"\[(\d|\d\d|\d\d\d|\d\d\d\d)\]";
            Regex regex = new Regex(pattern);
            int counter = 0;
            foreach (Match match in regex.Matches(text))
            {
                counter++;
            }
            this.label1.Text = "В тексте найдено "+counter.ToString()+" ссылок на список использованных источников";
            this.label1.Visibility = Visibility.Visible;
        }
        public static void OpenAndAddTextToWordDocument(string filepath, string txt) // добавление текста в существующий файл
        {
            // Open a WordprocessingDocument for editing using the filepath.
            WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(filepath, true);

            // Assign a reference to the existing document body.
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            // Add new text.
            //text = body.InnerXml;
            text = body.InnerText;
            DocumentFormat.OpenXml.Wordprocessing.Paragraph para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
            DocumentFormat.OpenXml.Wordprocessing.Run run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
            run.AppendChild(new Text(txt));
            
            // Close the handle explicitly.
            wordprocessingDocument.Close();
        }
        public static void CreateWordprocessingDocument(string filepath) // создание файла
        {
            // Create a document by supplying the filepath. 
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(filepath, WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                DocumentFormat.OpenXml.Wordprocessing.Paragraph para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                DocumentFormat.OpenXml.Wordprocessing.Run run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                run.AppendChild(new Text("Create text in body - CreateWordprocessingDocument"));
            }
        }
    }
}
