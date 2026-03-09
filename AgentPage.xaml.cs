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

namespace Yakupova_Glazki
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();
            var currentAgents = Yakupova_GlazkiEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgents;
            ComboType.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;
            UpdateService();
        }

        private void UpdateService()
        {
            try
            {
                var currentAgents = Yakupova_GlazkiEntities.GetContext().Agent.ToList();
                if (ComboType.SelectedItem != null)
                {
                    string selectedType = (ComboType.SelectedItem as TextBlock).Text;

                    if (selectedType != "Все типы")
                    {
                        currentAgents = currentAgents.Where(p => p.AgentTypeTitle == selectedType).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(TBSearch.Text))
                {
                    string searchText = TBSearch.Text.ToLower();
                    string cleanedSearchPhone = searchText
                        .Replace("+7", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace("-", "")
                        .Replace(" ", "");
                       

                    currentAgents = currentAgents.Where(p => (p.Title != null && p.Title.ToLower().Contains(searchText)) ||
                            (p.Email != null && p.Email.ToLower().Contains(searchText)) ||
                        (p.Phone != null && p.Phone
                        .Replace("+7", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace("-", "")
                        .Replace(" ", "").
                       
                        Contains(cleanedSearchPhone))
                    ).ToList();
                }
                if (ComboSort.SelectedIndex == 0)
                {

                }
                if (ComboSort.SelectedIndex == 1)
                {
                    currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
                }
                if (ComboSort.SelectedIndex == 2)
                {
                    currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
                }
                if (ComboSort.SelectedIndex == 3)
                {
                    currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
                }
                if (ComboSort.SelectedIndex == 4)
                {
                    currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
                }
                if (ComboSort.SelectedIndex == 5)
                {
                    currentAgents = currentAgents.OrderBy(p => p.Discount).ToList();
                }
                if (ComboSort.SelectedIndex == 6)
                {
                    currentAgents = currentAgents.OrderByDescending(p => p.Discount).ToList();
                }


                AgentListView.ItemsSource = currentAgents;
                AgentListView.ItemsSource = currentAgents;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }

        }


        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateService();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateService();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateService();
        }
        private void ChangePage(int direction, int? selectedPage)

{
    if (PageListBox == null) return;

    double AgentCount = 10;
    CountRecords = TableList.Count;
    CountPage = (int)Math.Ceiling((double)CountRecords / AgentCount); //кол-во записей делить на кол-во агентов на одной странице (10)
    if (selectedPage.HasValue) //текущая страница
    {
        CurrentPage = selectedPage.Value;
    }
    else
    {
       switch(direction)
        {
            case 1: CurrentPage--; break;
            case 2: CurrentPage++; break;   
        }
    }

    //границы
    if (CurrentPage < 0)
    {
        CurrentPage = 0;
    }
    if (CurrentPage >= CountPage)
    {
        CurrentPage = CountPage - 1;
    }

    CurrentPageList = TableList
        .Skip(CurrentPage * 10)
        .Take(10)
        .ToList();
    PageListBox.Items.Clear(); //обновление списка на странице
    for (int i = 1; i<= CountPage; i++)
    {
        PageListBox.Items.Add(i);
    }
    PageListBox.SelectedIndex = CurrentPage;
    AgentListView.ItemsSource = CurrentPageList;
    AgentListView.Items.Refresh();
}

private void PageListBox_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
{
    ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);

}

private void RightDirButton_Click(object sender, RoutedEventArgs e)
{
    ChangePage(2, null);
}

private void LeftDirButton_Click(object sender, RoutedEventArgs e)
{
    ChangePage(1, null);
}
 private void Button_Click(object sender, RoutedEventArgs e)
 {
     Meneger.MainFrame.Navigate(new AddEdit(null));
 }
    }
}
