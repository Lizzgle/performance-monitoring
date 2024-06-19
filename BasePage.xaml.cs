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
    /// Логика взаимодействия для BasePage.xaml
    /// </summary>
    public partial class BasePage : Page
    {
        private MainWindow mainWindow;

        private int timeElapsed;
        public BasePage(MainWindow mainWindow)
        {
            InitializeComponent();

            // Запуск обновления данных каждую секунду
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdatePerformanceData;
            timer.Start();
            this.mainWindow = mainWindow;
        }

        public virtual void UpdatePerformanceData(object sender, EventArgs e)
        {
            if (timeElapsed > 30) // Отображаем последние 60 секунд
            {
                LimitChart();
            }
            timeElapsed++;
        }

        public void LimitChart()
        {
            string[] labels = new string[timeElapsed];

            for (int i = 0; i < timeElapsed; i++)
            {
                labels[i] = (timeElapsed - 30 + i).ToString(); // Просто номер секунды
            }
            // Присваивание меток оси X для каждого графика
            UsageChart.AxisX[0].Labels = labels;

        }
    }
}
