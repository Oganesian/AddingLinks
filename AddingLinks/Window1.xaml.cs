using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AddingLinks
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1
    {
        string connStr = @"Database = listofsources; Data Source = localhost; User Id = root; Password =";
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
            Title = "Найдено ссылок: " + indexes;
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
                Height = 300;
                FromDB.Visibility = Visibility.Hidden;
                Next.Margin = new Thickness(0, 0, 0, 30);
                LinkOrAuthors.SetValue(TextBoxHelper.WatermarkProperty, "Гиперссылка");
                YearBox.Visibility = Visibility.Hidden;
                Pages.Visibility = Visibility.Hidden;
                City.Visibility = Visibility.Hidden;
                Publisher.Visibility = Visibility.Hidden;
                Volume.Visibility = Visibility.Hidden;
            }
            else
            {
                Height = 440;
                FromDB.Visibility = Visibility.Visible;
                Next.Margin = new Thickness(180, 0, 0, 30);
                LinkOrAuthors.SetValue(TextBoxHelper.WatermarkProperty, "Автор(-ы)");
                YearBox.Visibility = Visibility.Visible;
                Pages.Visibility = Visibility.Visible;
                City.Visibility = Visibility.Visible;
                Publisher.Visibility = Visibility.Visible;
                Volume.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxes(comboBox1.SelectedIndex))
            {
                sourceXFromX.Content = "Формарование ссылки " + ++currentIndex + " из " + totalIndexes;
                if (currentIndex > totalIndexes) sourceXFromX.Content = "Формарование ссылки " + totalIndexes + " из " + totalIndexes;
                FileStream file = new FileStream(@"XML\item.xml", FileMode.Open);
                XML = new byte[file.Length];
                file.Read(XML, 0, XML.Length);
                string tempXML = System.Text.Encoding.UTF8.GetString(XML);
                file.Close();
                string temp = "";
                string SQLQuery = "";
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        temp += Name.Text + " [Электронный ресурс]/ Режим доступа: "
                            + LinkOrAuthors.Text + ".";

                        SQLQuery = "INSERT INTO electronic_resource VALUES('" + 
                            Name.Text + "', '" + LinkOrAuthors.Text + "')";
                        break;
                    case 1:
                        temp += LinkOrAuthors.Text;
                        if (!LinkOrAuthors.Text.EndsWith(".")) temp += ". ";
                        temp += " " + Name.Text + " [Текст] / – " +
                        City.Text + ": " + Publisher.Text + ", " + YearBox.Text + ". – " +
                        Pages.Text + " с.";
                        if (Volume.Text != null && Volume.Text != "")
                        {
                            temp += " – " + Volume.Text + " т.";
                        }

                        SQLQuery = "INSERT INTO literature VALUES(";
                        SQLQuery += LinkOrAuthors.Text == "" ? "NULL" : "'" + LinkOrAuthors.Text + "'";
                        SQLQuery += ", '" + Name.Text + "', '" +
                            City.Text + "', '" + YearBox.Text + "', '" + Publisher.Text + "', ";
                        SQLQuery += Volume.Text == "" ? "NULL" : "'" + Volume.Text + "'";
                        SQLQuery += ", '" + Pages.Text + "')";
                        break;
                    default: break;
                }

                MySqlLib.MySqlDataC.MySqlExecute.MyResult SQLResult = new MySqlLib.MySqlDataC.MySqlExecute.MyResult();
                SQLResult = MySqlLib.MySqlDataC.MySqlExecute.SqlNoneQuery(SQLQuery, connStr);

                tempXML = tempXML.Replace("123456654321", temp);
                Name.Text = "";
                YearBox.Text = "";
                LinkOrAuthors.Text = "";
                Pages.Text = "";
                Volume.Text = "";
                City.Text = "";
                Publisher.Text = "";

                res += tempXML;

                if (currentIndex > totalIndexes)
                {
                    AddLinks();
                    MessageBoxResult result = MessageBox.Show("Готово", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }
        }

        private bool checkBoxes(int index)
        {
            List<string> list = new List<string>();
            bool smthAintFilled = false;
            switch (index)
            {
                case 0:
                    if (Name.Text == "" || Name.Text == null)
                    {
                        smthAintFilled = true;
                        list.Add("Имя");
                    }
                    if(LinkOrAuthors.Text == "" || LinkOrAuthors.Text == null)
                    {
                        smthAintFilled = true;
                        list.Add("Ссылка");
                    }
                    if (smthAintFilled == true)
                    {
                        if (list.Count == 1)
                            MessageBox.Show("Обязательное поле «" + list[0] + "» не заполнено", "Заполните поле", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                            MessageBox.Show("Обязательные поля «Имя» и «Ссылка» не заполнены", "Заполните поля", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    break;
               case 1:
                    if (Name.Text == "" || Name.Text == null)
                    {
                        smthAintFilled = true;
                        list.Add("Имя");
                    }
                    if (YearBox.Text == "" || YearBox.Text == null)
                    {
                        smthAintFilled = true;
                        list.Add("Год");
                    }
                    if (City.Text == "" || City.Text == null)
                    {
                        smthAintFilled = true;
                        list.Add("Город");
                    }
                    if (Publisher.Text == "" || Publisher.Text == null)
                    {
                        smthAintFilled = true;
                        list.Add("Издательство");
                    }
                    if (Pages.Text == "" || Pages.Text == null)
                    {
                        smthAintFilled = true;
                        list.Add("Страница(-ы)");
                    }
                    if (smthAintFilled == true)
                    {
                        if (list.Count == 1)
                            MessageBox.Show("Обязательное поле «" + list[0] + "» не заполнено", "Заполните поле", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            string temp = "Обязательные поля ";
                            foreach(string s in list)
                            {
                                temp += "«" + s + "», ";
                            }
                            temp += "replaceme";
                            temp = temp.Replace("», replaceme", "» не заполнены");
                            MessageBox.Show(temp, "Заполните поля", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        return false;
                    }
                    break; 
            }
            return true;
        }

        private void AddLinks()
        {
            WordprocessingDocument wordprocessingDocument =
                   WordprocessingDocument.Open(filepath, true);
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            DocumentFormat.OpenXml.Wordprocessing.Paragraph para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
            DocumentFormat.OpenXml.Wordprocessing.Run run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());

            System.Random rand = new System.Random();
            int r = rand.Next(1, 11111111);
            res = res.Replace("<w:numId w:val=\"1\" />", "<w:numId w:val=\"" + r + "\" />");

            body.InnerXml += res;

            wordprocessingDocument.Close();
        }

        private void PreviewTextInputDigit(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void From_DB(object sender, RoutedEventArgs e)
        {
            string SQLQuery = "SELECT * FROM literature";
            
            MySqlLib.MySqlData.MySqlExecuteData.MyResultData result = new MySqlLib.MySqlData.MySqlExecuteData.MyResultData();
            result = MySqlLib.MySqlData.MySqlExecuteData.SqlReturnDataset(SQLQuery, connStr);
            if (result.HasError == false)
            {
                SourcesTable win = new SourcesTable();
                win.Show();
                win.Owner = this;
                win.TableGrid.ItemsSource = result.ResultData.DefaultView;
                win.TableGrid.Columns[6].Width = 183;
            }
            else
            {
                MessageBox.Show("Что-то не так с базой данных", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void FillFromDB(string Authors, string _Name, string _City, string Year, string _Publisher, string _Volume, string _Pages)
        {
            LinkOrAuthors.Text = Authors;
            Name.Text = _Name;
            City.Text = _City;
            YearBox.Text = Year;
            Publisher.Text = _Publisher;
            Volume.Text = _Volume;
            Pages.Text = _Pages;
        }
    }
}
