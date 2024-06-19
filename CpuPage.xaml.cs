using LiveCharts.Wpf;
using LiveCharts;
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
using System.Windows.Threading;

namespace performance_monitoring
{
    /// <summary>
    /// Логика взаимодействия для CpuPage.xaml
    /// </summary>
    public partial class CpuPage : BasePage
    {

        private ChartValues<float> cpuUsageValues;

        private MainWindow mainWindow;

        private int timeElapsed;

        public CpuPage(MainWindow mainWindow) : base(mainWindow)
        {
            // InitializeComponent();

            // Инициализация коллекций для графиков
            cpuUsageValues = new ChartValues<float>();

            SettingUpCharts();

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
            };
        }

        public override void UpdatePerformanceData(object sender, EventArgs e)
        {
            //Получение текущих значений CPU и памяти
            float cpuUsage = mainWindow.GetCpuUsage();

            // Добавление значений в коллекции для графиков
            cpuUsageValues.Add(cpuUsage);

            // Ограничение количества отображаемых значений на графике
            if (timeElapsed > 30) // Отображаем последние 60 секунд
            {
                base.LimitChart();
                cpuUsageValues.RemoveAt(0);
            }
            timeElapsed++;
        }
    }
}
