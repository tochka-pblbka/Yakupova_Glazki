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
            TableList = currentAgents;
            ChangePage(0, 0);
        }

        int CountRecords; // записи
        int CountPage;
        int CurrentPage = 0; // текущ стр 

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
      

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


                TableList = currentAgents;

                ChangePage(0, 0);



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
                switch (direction)
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
            for (int i = 1; i <= CountPage; i++)
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
        public void RefreshAgents()
        {
            UpdateService();
        }



        private void ChangePriorityBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count == 0)
                return;

            // Находим максимальный приоритет среди выбранных
            int maxPriority = 0;
            foreach (Agent selectedAgent in AgentListView.SelectedItems)
            {
                if (selectedAgent.Priority > maxPriority)
                {
                    maxPriority = selectedAgent.Priority;
                }
            }

            // Создаем и открываем окно ввода нового приоритета
            // Вам нужно создать это окно (PriorChange.xaml) аналогично коду подруги
            PriorChange priorWindow = new PriorChange(maxPriority);
            priorWindow.ShowDialog();

            // Получаем новый приоритет из окна
            if (int.TryParse(priorWindow.TBPriority.Text, out int newPriority))
            {
                // Обновляем приоритет для всех выбранных агентов
                foreach (Agent agent in AgentListView.SelectedItems)
                {
                    agent.Priority = newPriority;
                }

                try
                {
                    Yakupova_GlazkiEntities.GetContext().SaveChanges();
                    UpdateService(); // Обновляем список
                    MessageBox.Show("Приоритеты обновлены");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                // Полностью очищаем локальный кэш
                var context = Yakupova_GlazkiEntities.GetContext();

                // Отсоединяем ВСЕ загруженные сущности
                foreach (var entry in context.ChangeTracker.Entries().ToList())
                {
                    entry.State = EntityState.Detached;
                }
                UpdateService();
            }
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count > 0)
            {
                ChangePriorityBtn.Visibility = Visibility.Visible;
            }
            else
            {
                ChangePriorityBtn.Visibility = Visibility.Hidden;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Meneger.MainFrame.Navigate(new AddEdit(AgentListView.SelectedItem as Agent));
        }
    }
}
