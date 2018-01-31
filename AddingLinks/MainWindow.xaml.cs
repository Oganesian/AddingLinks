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
using System.Collections;
using System.IO;

namespace AddingLinks
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string text;
        static byte[] XML;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string strDoc = @"C:\Users\bezim\Desktop\aaa.docx"; 
            FileStream file = new FileStream(@"C:\Users\bezim\Desktop\e.txt", FileMode.Open);
            XML = new byte[file.Length];
            file.Read(XML, 0, XML.Length);
            AddTheListOfSourcesUsed(strDoc);
            string pattern = @"\[(\d|\d\d|\d\d\d|\d\d\d\d)\]";
            Regex regex = new Regex(pattern);
            int differentMatches = 0;          
            ArrayList array = new ArrayList();
            foreach (Match match in regex.Matches(text))
            {
                if (!array.Contains(match.ToString()))
                {
                    differentMatches++;
                    array.Add(match.ToString());
                    Console.WriteLine(match);
                }
            }
            this.label1.Text = "В тексте найдено " + differentMatches.ToString() + 
                " различных ссылок на список использованных источников";
            this.label1.Visibility = Visibility.Visible;
        }

        public static void AddTheListOfSourcesUsed(string filepath)
        {
            WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(filepath, true);

            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            DocumentFormat.OpenXml.Wordprocessing.Paragraph para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
            DocumentFormat.OpenXml.Wordprocessing.Run run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());

            text = body.InnerText;

            string result = System.Text.Encoding.UTF8.GetString(XML);
            body.InnerXml += result;

            wordprocessingDocument.Close();
        }
    }
}
