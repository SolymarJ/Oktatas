namespace PlantSimWFApp;

public class StationTimer
{

    // the station's timer
    public System.Windows.Forms.Timer? _timer;

    // the station's cycle time
    public double CycleTime;

    // the station's cycle time with efficiency
    public double CycleTimeWithEfficiency;
    // the station
    private Station _station;

    private System.Windows.Forms.Timer? _rescanTimer;
    private double _timeStuck = 0.1;
    
    // a timer for the progress bar
    private System.Windows.Forms.Timer? _progressBarTimer;
    private const int ProgressBarMinimumValue = 1;

    // constructor
    public StationTimer(Station station)
    {
        _station = station;
        CycleTime = 1;
        _timer = new System.Windows.Forms.Timer();
        
        _progressBarTimer = new System.Windows.Forms.Timer();
        _progressBarTimer.Interval = 1000;

        _timer.Tick += TimerFinished;
        _progressBarTimer.Tick += ProgressBarTimerTick;
        
    }

    private void ProgressBarTimerTick(object? sender, EventArgs e)
    {

        // if the progress bar is not full, increment the progress bar every 1s
        if (_station.ProgressBar.Value < _station.ProgressBar.Maximum)
        {
            _station.ProgressBar.Value++;
        }

    }

    public void StartTimer(Product? product)
    {
        if (product != null) _station.Product = product;
        else if (_station.Product != null) return;
        else _station.Product = new Product(_station);
        
        // start the timer
        _timer.Start();
        
        // set the progress bar to 0
        _progressBarTimer.Start();
            
        // make the station green
        _station.Panel.BackColor = Color.Green;
    }

    public void StopTimer()
    {
        
        // stop the timer
        _timer.Stop();
        
    }

    public void SetCycleTime(double cycleTime)
    {
        
        // set the cycle time
        CycleTime = cycleTime;
        
        // set the timer interval
        _timer.Interval = (int)(CycleTime * 1000);
        _station.CycleTimeLabel.Text = CycleTime.ToString("F2");
        if (_timer.Interval < 1000)
        {
            _station.ProgressBar.Maximum = 1;
        }
        else
        {
            _station.ProgressBar.Maximum = (int)CycleTime;

        }
    }

    public void SetCycleTimeWithEfficiency(double cycleTime)
    {
        
        CycleTimeWithEfficiency = cycleTime;

        // set the timer CycleTimeWithEfficiency
        _timer.Interval = (int)(CycleTimeWithEfficiency * 1000);
        _station.CycleTimeLabel.Text = CycleTimeWithEfficiency.ToString();
        if (_timer.Interval < 1000)
        {
            _station.ProgressBar.Maximum = 1;
        }
        else
        {
            _station.ProgressBar.Maximum = (int)CycleTimeWithEfficiency;

        }

    }

    // event handler for when the timer finishes
    private void TimerFinished(object? sender, EventArgs e)
    {
        
        // stop the timers
        _timer.Stop();
        _progressBarTimer.Stop();

        // make the station transparent
        _station.Panel.BackColor = Color.Transparent;

        double HigherBound = (2 - _station.Efficiency) * CycleTime;
        double LowerBound = CycleTime;
        
        // set random cycle time based on efficiency, minimum value is cycletime, maximum value is 2-efficiency*cycletime
        _station.Timer.SetCycleTimeWithEfficiency(new Random().NextDouble() * (HigherBound - LowerBound) + LowerBound);

        // if the station has no children
        if (_station.Children.Count == 0)
        {
            // remove the product from the line
            _station.Product.RemoveFromLine();

            // set the progress bar value to 0
            _station.ProgressBar.Value = ProgressBarMinimumValue;
            
            return;
        }

        foreach (Station st in _station.Children)
        {
            if (st.Product == null)
            {
                _timeStuck = 0.1;
                _station.Product.MoveTo(st);
                
                // start the child's timer
                st.Timer.StartTimer(st.Product);

                // set the progress bar value to 0
                _station.ProgressBar.Value = ProgressBarMinimumValue;

                return;
            }
        }

        _station.Panel.BackColor = Color.Red;

        // start the rescan timer
        _rescanTimer = new System.Windows.Forms.Timer();
        _rescanTimer.Interval = 100;
        _rescanTimer.Tick += Rescan;
        _rescanTimer.Start();


    }

    private void Rescan(object? sender, EventArgs e)
    {
        // start counting the time the product has been stuck
        foreach (Station st in _station.Children)
        {
            if (st.Product == null)
            {

                // stop the rescan timer
                _rescanTimer.Stop();
                
                // set the progress bar to 0 and stop the timer
                _station.ProgressBar.Value = ProgressBarMinimumValue;
                _progressBarTimer.Stop();
                
                _station.Product.LogStuckStation(_station, _timeStuck);
                _station.TotalStuckTime += _timeStuck;
                _timeStuck = 0.1;

                _station.Product.MoveTo(st);
               
                _station.Panel.BackColor = Color.Transparent;

                // start the child's timer
                st.Timer.StartTimer(st.Product);
                         
                double HigherBound = (2 - _station.Efficiency) * CycleTime;
                double LowerBound = CycleTime;
                
                // set random cycle time based on efficiency, minimum value is cycletime, maximum value is 1-efficiency*cycletime
                _station.Timer.SetCycleTimeWithEfficiency(new Random().NextDouble() * (HigherBound - LowerBound) + LowerBound);
                                     
                break;
            }
            else
            {
                _timeStuck += 0.1;
            }

        }
    }
}
