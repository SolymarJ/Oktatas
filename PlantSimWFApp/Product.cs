namespace PlantSimWFApp;

public class Product
{

    // the time spent on the production line
    public double TimeOnProductionLine;

    // list of stations the product has been through
    public List<Station> VisitedStations;

    // list of stations where the product has been stuck, and the time spent stuck
    public Dictionary<Station, double> StuckStations;

    // the current station of the product
    private Station _currentStation;
    
    public Product(Station curSt)
    {
        TimeOnProductionLine = 0;
        VisitedStations = new List<Station>();
        StuckStations = new Dictionary<Station, double>();
        _currentStation = curSt;
    }

    public void MoveTo(Station newSt)
    {
        // add the current station to the list of stations the product has been through
        VisitedStations.Add(_currentStation);
        _currentStation.Product = null;
        _currentStation.ProductCount++;

        // set the current station to the new station
        _currentStation = newSt;
        _currentStation.Product = this;

        // if the new station is the first station, reset the time spent on the production line, the list of visited stations, and the list of stuck stations
        if (_currentStation.StLayout.GetColumn(_currentStation.Panel) == 1)
        {
            LogInfo();
            TimeOnProductionLine = 0;
            VisitedStations = new List<Station>();
            StuckStations = new Dictionary<Station, double>();          
        }

        LogStats();
    }

    public void LogStuckStation(Station st, double time)
    {
        StuckStations.Add(st, time);
    }
    
    public void RemoveFromLine()
    {
        // remove the product from the production line
        VisitedStations.Add(_currentStation);
        _currentStation.Product = null;
        _currentStation.ProductCount++;

        LogInfo();
        LogStats();
    }

    public void LogInfo()
    {
        if (StuckStations.Count > 0)
        {
            _currentStation.ProductInfo.AppendText("Product got stuck at the following stations: \n");
            foreach (KeyValuePair<Station, double> kvp in StuckStations)
            {
              
                _currentStation.ProductInfo.AppendText($"{kvp.Key.Name} for {kvp.Value.ToString("F2")} seconds\n");
            }
            _currentStation.ProductInfo.AppendText("------\n");

            // also write this to a file
            using StreamWriter sw = File.AppendText("stuckstats.txt");
            sw.WriteLine("Product got stuck at the following stations: ");
            foreach (KeyValuePair<Station, double> kvp in StuckStations)
            {
                sw.WriteLine($"{kvp.Key.Name} for {kvp.Value.ToString("F2")} seconds");
            }
            sw.WriteLine("------");
        }

    }

    public void LogStats()
    {
        // log all stations' stats (how many products have been through so far)

        // clear the stats
        _currentStation.Stats.Clear();
        _currentStation.Stats.AppendText("Products per station:\n");
        foreach (Station st in _currentStation.AllStations.Stations)
        {
            _currentStation.Stats.AppendText($"{st.Name}: {st.ProductCount}\n");
        }

        // log total stuck time for each station
        _currentStation.Stats.AppendText("------\n");
        _currentStation.Stats.AppendText("Total stuck time per station:\n");
        foreach (Station st in _currentStation.AllStations.Stations)
        {          
            _currentStation.Stats.AppendText($"{st.Name}: {st.TotalStuckTime.ToString("F2")} seconds\n");
        }

        // also write this to a file
        using (StreamWriter sw = File.CreateText("info.txt")) sw.WriteLine("");
        using (StreamWriter sw = File.AppendText("info.txt"))
        {

            sw.WriteLine("Products per station:");
            foreach (Station st in _currentStation.AllStations.Stations)
            {
                sw.WriteLine($"{st.Name}: {st.ProductCount}");
            }

            sw.WriteLine("------");
            sw.WriteLine("Total stuck time per station:");
            foreach (Station st in _currentStation.AllStations.Stations)
            {
                sw.WriteLine($"{st.Name}: {st.TotalStuckTime.ToString("F2")} seconds");
            }
        }
    }
}
