using DataCenter.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace ScheduleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Загрузка данных для ComboBox'ов при загрузке окна
            await LoadInitialDataAsync();
        }

        private async Task LoadInitialDataAsync()
        {
            try
            {
                // Загрузка специализаций
                var specialitiesResponse = await _client.GetAsync("http://localhost:8080/api/specialities");
                if (specialitiesResponse.IsSuccessStatusCode)
                {
                    var specialitiesJson = await specialitiesResponse.Content.ReadAsStringAsync();
                    var specialities = JsonConvert.DeserializeObject<List<Speciality>>(specialitiesJson);
                    specialityComboBox.ItemsSource = specialities;
                    specialityComboBox.DisplayMemberPath = "Title";
                    specialityComboBox.SelectedValuePath = "ID";
                }

                // Подобным образом реализуйте загрузку и отображение данных о врачах, если необходимо
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading initial data: {ex.Message}");
            }
        }


        private async void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранных значений и формирование запроса к API
            var selectedDate = datePicker.SelectedDate;
            // Предположим, что фильтрация по специальности и врачам уже реализована в вашем API

            if (selectedDate.HasValue)
            {
                var response = await _client.GetAsync($"http://localhost:8080/api/schedules?date={selectedDate.Value:yyyy-MM-dd}");
                if (response.IsSuccessStatusCode)
                {
                    var schedulesJson = await response.Content.ReadAsStringAsync();
                    var schedules = JsonConvert.DeserializeObject<List<ScheduleDTO>>(schedulesJson);

                    dailyScheduleDataGrid.ItemsSource = schedules.Where(s => s.ScheduleDate.Value.Date == selectedDate.Value.Date).ToList();
                    weeklyScheduleDataGrid.ItemsSource = schedules.Where(s => s.ScheduleDate.Value.Date >= selectedDate.Value.Date && s.ScheduleDate.Value.Date <= selectedDate.Value.Date.AddDays(7)).ToList();
                }
                else
                {
                    MessageBox.Show("Error getting schedule.");
                }
            }
        }
    }
}
