using LiveCharts;
using LiveCharts.Wpf;
using System.Diagnostics;
using System.Text;
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
using System.Management;

namespace performance_monitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        private PerformanceCounter diskCounter;

        private float totalMemory;

        private ChartValues<float> cpuUsageValues;
        private ChartValues<float> ramUsageValues;
        private ChartValues<float> diskUsageValues;

        private ChartValues<int> fpsValues;
        private int timeElapsed;

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация счетчиков производительности
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

            // Получение общего объема оперативной памяти
            totalMemory = Ram.GetTotalPhysicalMemory();

            // Инициализация коллекций для графиков
            cpuUsageValues = new ChartValues<float>();
            ramUsageValues = new ChartValues<float>();
            diskUsageValues = new ChartValues<float>();

            fpsValues = new ChartValues<int>();



            // Настройка графиков
            cpuUsageChart.Series = new SeriesCollection
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

            // Запуск обновления данных каждую секунду
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdatePerformanceData;
            timer.Start();

        }

        private void UpdatePerformanceData(object sender, EventArgs e)
        {
            // Получение текущих значений CPU и памяти
            float cpuUsage = cpuCounter.NextValue();
            float ramUsage = ramCounter.NextValue();
            float diskUsage = diskCounter.NextValue();

            // Вычисление процента использования оперативной памяти
            float ramUsagePercent = ((totalMemory - ramUsage) / totalMemory) * 100;

            // Добавление значений в коллекции для графиков
            cpuUsageValues.Add(cpuUsage);
            ramUsageValues.Add(ramUsagePercent);
            diskUsageValues.Add(diskUsage);


            // Ограничение количества отображаемых значений на графике
            if (timeElapsed > 30) // Отображаем последние 60 секунд
            {
                string[] labels = new string[timeElapsed];

                for (int i = 0; i < timeElapsed; i++)
                {
                    labels[i] = (timeElapsed - 30 + i).ToString(); // Просто номер секунды
                }
                // Присваивание меток оси X для каждого графика
                cpuUsageChart.AxisX[0].Labels = labels;
                //ramUsageChart.AxisX[0].Labels = labels;
                //diskUsageChart.AxisX[0].Labels = labels;
                // fpsChart.AxisX[0].Labels = labels;

                cpuUsageValues.RemoveAt(0);
                ramUsageValues.RemoveAt(0);
                diskUsageValues.RemoveAt(0);
                // fpsValues.RemoveAt(0);

            }

            timeElapsed++;

        }

    }
}