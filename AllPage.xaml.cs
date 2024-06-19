using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace performance_monitoring
{
    /// <summary>
    /// Логика взаимодействия для AllPage.xaml
    /// </summary>
    public partial class AllPage : Page
    {
        private float totalMemory;

        private ChartValues<float> cpuUsageValues;
        private ChartValues<float> ramUsageValues;
        private ChartValues<float> diskUsageValues;

        private MainWindow mainWindow;

        private int timeElapsed;

        public AllPage(MainWindow mainWindow)
        {
            InitializeComponent();

            // Получение общего объема оперативной памяти
            totalMemory = Ram.GetTotalPhysicalMemory();

            // Инициализация коллекций для графиков
            cpuUsageValues = new ChartValues<float>();
            ramUsageValues = new ChartValues<float>();
            diskUsageValues = new ChartValues<float>();


            SettingUpCharts();


            // Запуск обновления данных каждую секунду
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdatePerformanceData;
            timer.Start();
            this.mainWindow = mainWindow;
        }
        public void SettingUpCharts()
        {
            // Настройка графиков
            UsageChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "CPU Usage",
                    Values = cpuUsageValues
                },
                new LineSeries
                {
                    Title = "RAM Usage",
                    Values = ramUsageValues
                },
                new LineSeries
                {
                    Title = "Disk Usage",
                    Values = diskUsageValues
                }
            };
        }

        public void UpdatePerformanceData(object sender, EventArgs e)
        {
            //Получение текущих значений CPU и памяти
            float cpuUsage = mainWindow.GetCpuUsage();
            float ramUsage = mainWindow.GetRamUsage();
            float diskUsage = mainWindow.GetDiskUsage();

            // Вычисление процента использования оперативной памяти
            float ramUsagePercent = ((totalMemory - ramUsage) / totalMemory) * 100;

            // Добавление значений в коллекции для графиков
            cpuUsageValues.Add(cpuUsage);
            ramUsageValues.Add(ramUsagePercent);
            diskUsageValues.Add(diskUsage);

            LimitChart();

            cpuUsageValues.RemoveAt(0);
            ramUsageValues.RemoveAt(0);
            diskUsageValues.RemoveAt(0);

        }

        public void LimitChart()
        {
            if (timeElapsed > 30) // Отображаем последние 60 секунд
            {
                string[] labels = new string[timeElapsed];

                for (int i = 0; i < timeElapsed; i++)
                {
                    labels[i] = (timeElapsed - 30 + i).ToString(); // Просто номер секунды
                }
                // Присваивание меток оси X для каждого графика
                UsageChart.AxisX[0].Labels = labels;
            }
            timeElapsed++;
        }
    }
}
