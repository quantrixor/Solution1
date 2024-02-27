using DataCenter.Model;
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

namespace DesktopTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TrackerService _trackerService;
        //7460 × 2580
        private double originalImageWidth = 7460;
        private double originalImageHeight = 2580;

        public MainWindow()
        {
            InitializeComponent();
            _trackerService = new TrackerService();
        }

        // Обработчик нажатия кнопки
        private async void buttonUpdateLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<SecurityAccessLog> jsonResponse = await _trackerService.GetTrackingDataAsync();
                DisplayPeopleOnCanvas(jsonResponse);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void DisplayPeopleOnCanvas(List<SecurityAccessLog> securityAccessLogs)
        {
            try
            {
                // Очистите предыдущие маркеры
                TrackingCanvas.Children.Clear();

                foreach (var person in securityAccessLogs)
                {
                    int scudNumber = person.LastSecurityPointNumber ?? 0;

                    // Предположим, что функция GetCanvasCoordinatesFromScudNumber возвращает точку на канвасе для данного СКУД
                    Point location = GetCanvasCoordinatesFromScudNumber(scudNumber);

                    // Создаем визуальный маркер для человека
                    Ellipse personMarker = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Red // Выберите цвет в зависимости от роли
                    };

                    // Установка координат маркера на канвасе
                    Canvas.SetLeft(personMarker, location.X);
                    Canvas.SetTop(personMarker, location.Y);

                    // Добавление маркера на канвас
                    TrackingCanvas.Children.Add(personMarker);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private Dictionary<int, Point> scudNumberToCanvasCoordinates = new Dictionary<int, Point>()
        {
            { 15, new Point(1177, 1646) },
            { 21, new Point(2179, 930) },
            { 22, new Point(2809, 925) },
        };

        private Point GetCanvasCoordinatesFromScudNumber(int scudNumber)
        {
            try
            {
                if (scudNumberToCanvasCoordinates.TryGetValue(scudNumber, out Point imageCoordinates))
                {
                    // Если изображение масштабируется, применяем масштабирование
                    double scaleX = TrackingCanvas.ActualWidth / originalImageWidth;
                    double scaleY = TrackingCanvas.ActualHeight / originalImageHeight;

                    // Преобразование координат изображения в координаты канваса
                    double canvasX = imageCoordinates.X * scaleX;
                    double canvasY = imageCoordinates.Y * scaleY;

                    return new Point(canvasX, canvasY);
                }
                else
                {
                    return new Point(0, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new Point(0, 0);
            }
          
        }

    }
}
