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
using System.Windows.Shapes;

namespace AddingLinks
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        private void SearchInDB(object sender, RoutedEventArgs e)
        {
            string SQLQuery = "SELECT * FROM `literature` WHERE ";
            string connStr = @"Database = listofsources; Data Source = localhost; User Id = root; Password =";

            SourcesTable win = (SourcesTable)this.Owner;

            for (int i = 0; i < 7; i++)
            {
                TextBox tb = (TextBox)MainGrid.Children[i];
                if (tb.Text != null && tb.Text != "")
                {
                    SQLQuery += "`" + win.TableGrid.Columns[i].Header + "` = " + "'" + tb.Text + "' AND ";
                }
            }
            SQLQuery += "replaceme";
            SQLQuery = SQLQuery.Replace("AND replaceme", "");
          
            MySqlLib.MySqlData.MySqlExecuteData.MyResultData result = new MySqlLib.MySqlData.MySqlExecuteData.MyResultData();
            result = MySqlLib.MySqlData.MySqlExecuteData.SqlReturnDataset(SQLQuery, connStr);
            if (result.HasError == false)
            {
                win.TableGrid.ItemsSource = result.ResultData.DefaultView;
                win.Activate();
                Close();
            }
            else
            {
                MessageBox.Show(result.ErrorText);
            }
        }

        private void PreviewTextInputDigit(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Activate();
        }
    }
}
