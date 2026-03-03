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
    }
}
