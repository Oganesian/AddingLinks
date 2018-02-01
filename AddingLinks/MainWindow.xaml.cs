using System.Windows;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using Microsoft.Win32;

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
            if (!File.Exists(filePath1.Text))
            {
                MessageBox.Show("Указан неверный путь к файлу", "Неверный путь", MessageBoxButton.OK, MessageBoxImage.Error);
                if (!IsOpenFileDialog())
                {
                    this.Close();
                }
            }

            AddTheListOfSourcesUsed(this.filePath1.Text);
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
                }
            }
            Window1 window = new Window1(differentMatches, filePath1.Text);
            window.Show();
            this.Close();
        }

        public void AddTheListOfSourcesUsed(string filepath)
        {
            if (!File.Exists(filePath1.Text)) Application.Current.Shutdown();
            WordprocessingDocument wordprocessingDocument =
                    WordprocessingDocument.Open(filepath, true);
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            DocumentFormat.OpenXml.Wordprocessing.Paragraph para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                DocumentFormat.OpenXml.Wordprocessing.Run run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
            text = body.InnerText;

            wordprocessingDocument.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IsOpenFileDialog();
        }
        private bool IsOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Документы Word (*.docx)|*.docx|(*.doc)|*.doc";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                this.filePath1.Text = openFileDialog.FileName;
                return true;
            }
            return false;
        }
    }
}
