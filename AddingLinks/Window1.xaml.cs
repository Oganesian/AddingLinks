using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AddingLinks
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        static byte[] XML;
        int currentIndex = 1;
        int totalIndexes;
        string filepath;
        string res;

        public Window1(int indexes, string fp)
        {
            InitializeComponent();
            totalIndexes = indexes;
            filepath = fp;
            Title = indexes + " ссылок";
            sourceXFromX.Content = "Формарование ссылки 1 из " + indexes;

            FileStream file = new FileStream(@"XML\first.xml", FileMode.Open);
            XML = new byte[file.Length];
            file.Read(XML, 0, XML.Length);
            file.Close();
            res = System.Text.Encoding.UTF8.GetString(XML);

        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if(totalIndexes != currentIndex)
            {
                Application.Current.Shutdown();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                thirdLabel.Content = "Гиперссылка";
                fourthBox.Visibility = Visibility.Hidden;
                fifthBox.Visibility = Visibility.Hidden;
                fourthLabel.Visibility = Visibility.Hidden;
                fifthLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                thirdLabel.Content = "Автор(-ы)";
                fourthBox.Visibility = Visibility.Visible;
                fifthBox.Visibility = Visibility.Visible;
                fourthLabel.Visibility = Visibility.Visible;
                fifthLabel.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sourceXFromX.Content = "Формарование ссылки " + ++currentIndex + " из " + totalIndexes;

            FileStream file = new FileStream(@"XML\item.xml", FileMode.Open);
            XML = new byte[file.Length];
            file.Read(XML, 0, XML.Length);
            string tempXML = System.Text.Encoding.UTF8.GetString(XML);
            file.Close();
            string temp = "";
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    temp += secondBox.Text + " [Электронный ресурс]/ Режим доступа: "
                        + thirdBox.Text + "."; break;
                case 1:
                     temp += thirdBox.Text + ". " + secondBox.Text +
                        " [Текст] / " + fourthBox.Text + " — " + fifthBox.Text + "с.";
                    break;
                default: break;
            }

            tempXML = tempXML.Replace("123456654321", temp);
            secondBox.Text = "";
            fourthBox.Text = "";
            thirdBox.Text = "";
            fifthBox.Text = "";

            res += tempXML;

            if (currentIndex == totalIndexes)
            {
                FileStream file1 = new FileStream(@"XML\last.xml", FileMode.Open);
                XML = new byte[file1.Length];
                file1.Read(XML, 0, XML.Length);
                file1.Close();
                string lastXML = System.Text.Encoding.UTF8.GetString(XML);
                res += lastXML;
                AddLinks();
                MessageBoxResult result = MessageBox.Show("Работа сделана. Хотите использовать программу повторно?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }
        private void AddLinks()
        {
            WordprocessingDocument wordprocessingDocument =
                   WordprocessingDocument.Open(filepath, true);
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            DocumentFormat.OpenXml.Wordprocessing.Paragraph para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
            DocumentFormat.OpenXml.Wordprocessing.Run run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());

            body.InnerXml += res;

            wordprocessingDocument.Close();
        }
    }
}
