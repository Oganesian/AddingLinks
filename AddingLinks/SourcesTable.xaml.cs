using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace AddingLinks
{
    /// <summary>
    /// Логика взаимодействия для SourcesTable.xaml
    /// </summary>
    public partial class SourcesTable
    {
        public SourcesTable()
        {
            InitializeComponent();
            TableGrid.Items.Clear();
        }

        private void SelectFromDB(object sender, RoutedEventArgs e)
        {
            if (TableGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите строку, данными из которой хотите заполнить форму", "Строка не выбрана", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int count = TableGrid.Columns.Count;
                string[] rows = new string[count];
                for (int i = 0; i < count; i++)
                {
                    rows[i] = ((DataRowView)TableGrid.SelectedItem).Row[i].ToString();
                }

                Window1 owner = (Window1)this.Owner;

                owner.FillFromDB(rows[0], rows[1], rows[2], rows[3], rows[4], rows[5], rows[6]);
                owner.Activate();

                Close();
            }
        }

        private void SearchInDB(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.Owner = this;
            searchWindow.Show();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Activate();
        }

        private void UpdateTable(object sender, RoutedEventArgs e)
        {
            string SQLQuery = "SELECT * FROM literature";
            string connStr = @"Database = listofsources; Data Source = localhost; User Id = root; Password =";

            MySqlLib.MySqlData.MySqlExecuteData.MyResultData result = new MySqlLib.MySqlData.MySqlExecuteData.MyResultData();
            result = MySqlLib.MySqlData.MySqlExecuteData.SqlReturnDataset(SQLQuery, connStr);
            if (result.HasError == false)
            {
                TableGrid.ItemsSource = result.ResultData.DefaultView;
            }
            else
            {
                MessageBox.Show("Что-то не так с базой данных", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
