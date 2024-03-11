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
                var response = await _client.GetAsync("http://localhost:8080/api/specialities");
                if (response.IsSuccessStatusCode)
                {
                    var specialitiesJson = await response.Content.ReadAsStringAsync();
                    var specialities = JsonConvert.DeserializeObject<List<Speciality>>(specialitiesJson);
                    specialityComboBox.ItemsSource = specialities.ToList();
                }
                else
                {
                    MessageBox.Show("Error retrieving data about specialties.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private async void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранных значений и формирование запроса к API
            var selectedDate = datePicker.SelectedDate;
            var selectedSpeciality = specialityComboBox.SelectedItem as string;
            var selectedDoctor = doctorComboBox.SelectedItem as string;

            if (selectedDate.HasValue)
            {
                var response = await _client.GetAsync($"http://localhost:8080/api/schedules?date={selectedDate.Value:yyyy-MM-dd}");
                if (response.IsSuccessStatusCode)
                {
                    var schedulesJson = await response.Content.ReadAsStringAsync();
                    // Десериализация и обновление UI
                }
                else
                {
                    MessageBox.Show("Error getting schedule.");
                }
            }
        }
    }
}
