using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlantSimWFApp
{
    public partial class LogForm : Form       
    {
        
        private LinkedListOfStations _stations;
        
        public LogForm(LinkedListOfStations stations)
        {
            InitializeComponent();
            _stations = stations;          
        }

        private void logStationButton_Click(object sender, EventArgs e)
        {
            StationInfo.Clear();
            _stations.LogAllStations();
        }

        private void StartTimerButton_Click(object sender, EventArgs e)
        {
            ProductIntervalTimer.Interval = (int)(ProductInterval.Value * 1000);
            ProductIntervalTimer.Start();
        }

        private void ProductIntervalTimer_Tick(object sender, EventArgs e)
        {
            // start a timer of a station in the first column which has no product
            foreach (Station s in _stations.Stations)
            {
                if (s.Product == null && s.StLayout.GetColumn(s.Panel) == 1)
                {
                    s.Timer.StartTimer(null);
                    break;
                }
            }
        }

        private void StopSendButton_Click(object sender, EventArgs e)
        {
            ProductIntervalTimer.Stop();
        }

    }
}
