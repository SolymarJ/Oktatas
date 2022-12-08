namespace PlantSimWFApp;

public partial class StationsForm : Form
{

    LinkedListOfStations _stations = new();
    public StationsForm()
    {
        InitializeComponent();

        LogForm logForm = new(_stations);
        logForm.Show();
        _ = new Station(3, 1, 120, 120, "st1", _stations, this, logForm);

    }

    private void SaveXmlButton_Click(object sender, EventArgs e)
    {
        // save the stations to an xml file
        _stations.SaveToXml();
    }

    private void LoadFromXmlButton_Click(object sender, EventArgs e)
    {
        // load the stations from an xml file
        _stations.LoadFromXml();
    }

    private void SpeedUpTimers_Click(object sender, EventArgs e)
    {
        //foreach (Station st in _stations.Stations)
        //{
        //    if (st == _stations.Stations.Last())
        //    {
        //        break;
        //    }
        //    st.Timer.StartTimer(null);
        //}

        // speed up all timers by 10x
        foreach (Station st in _stations.Stations)
        {
            st.Timer.SetCycleTime(st.Timer.CycleTime / 10);
            st.CycleTimeNumericUpDown.Value = (decimal)st.Timer.CycleTime;
        }
        _stations.Stations.First().Timer.StartTimer(null);
        Button button = (Button)sender;
        button.Text = "Slow down timers to normal speed";
        button.Click -= SpeedUpTimers_Click;
        button.Click += SlowDownTimers;
    }

    private void SlowDownTimers(object? sender, EventArgs e)
    {
        foreach (Station st in _stations.Stations)
        {
            st.Timer.SetCycleTime(st.Timer.CycleTime * 10);
            st.CycleTimeNumericUpDown.Value = (decimal)st.Timer.CycleTime;
        }
        Button button = (Button)sender;
        button.Text = "Speed up timers by 10x";
        button.Click -= SlowDownTimers;
        button.Click += SpeedUpTimers_Click;

    }
}