using System.Security.Cryptography.X509Certificates;

namespace PlantSimWFApp;

// -- STATION BUTTON --
public class StationButton
{

    // type of button
    private string? Type { get; set; }

    // coordinates of the button
    private int X { get; set; }
    private int Y { get; set; }

    // panel of the station of the button
    private Station St { get; set; }

    public Button Button { get; set; }
    
    // constructor
    public StationButton(string? type, int x, int y, Station station)
    {

        Type = type;
        X = x;
        Y = y;
        St = station;

        PlaceOnStation();

    }

    // place the button on the station
    private void PlaceOnStation()
    {
        // create a button
        Button = new()
        {
            Text = Type,
            Size = new Size(25, 25),
            Location = new Point(X, Y)
        };
        Button.Click += CreateNewStation;

        // add the button to the station
        St.Panel?.Controls.Add(Button);
    }

    // create a new station based on the type of the button    
    private void CreateNewStation(object? sender, EventArgs e)
    {

        const int maxRows = 10;
        const int maxCols = 50;
        const int stWidth = 120;
        const int stHeight = 120;
        const int penWidth = 1;
        
        Graphics g = St.Form.CreateGraphics();
        Pen p = new(Color.Black, penWidth);

        // increment station name by one, based on the length of all stations
        int index = St.AllStations?.Stations.Count ?? 0;
        string newName = $"st{index + 1}";

        // get the row number of the station in the table layout panel
        int row = St.StLayout.GetRow(St.Panel);
        int col = St.StLayout.GetColumn(St.Panel);

        Station st;
        Station newSt;

        switch (Type)
        {
            case "U":

                if (row == 0) break;
                
                newSt = StationFactory.CreateStation(row - 1, col, stWidth, stHeight, newName, St.AllStations, St.Form, St.LogForm);
                newSt.Down.Button.Visible = false;
                newSt.Right.Button.Visible = false;

                newSt.ConnectToFirstButton.Visible = false;
                St.ConnectToFirstButton.Visible = false;

                // add this new station as a child of all stations in the previous column
                foreach (Station s in St.AllStations.Stations)
                {
                    if (s != St && s != newSt && s.StLayout.GetColumn(s.Panel) == col - 1)
                    {
                        s.Children.Add(newSt);
                    }
                }

                // add all stations in the next column as a child of this new station
                foreach (Station s in St.AllStations.Stations)
                {
                    if (s != St && s != newSt && s.StLayout.GetColumn(s.Panel) == col + 1)
                    {
                        newSt.Children.Add(s);
                    }
                }
                break;

            case "D":

                if (row == maxRows) break;

                newSt = StationFactory.CreateStation(row + 1, col, stWidth, stHeight, newName, St.AllStations, St.Form, St.LogForm);
                newSt.Up.Button.Visible = false;
                newSt.Right.Button.Visible = false;

                newSt.ConnectToFirstButton.Visible = false;
                St.ConnectToFirstButton.Visible = false;

                // add this new station as a child of all stations in the previous column
                foreach (Station s in St.AllStations.Stations)
                {
                    if (s != St && s != newSt && s.StLayout.GetColumn(s.Panel) == col - 1)
                    {
                        s.Children.Add(newSt);
                    }
                }

                // add all stations in the next column as a child of this new station
                foreach (Station s in St.AllStations.Stations)
                {
                    if (s != St && s != newSt && s.StLayout.GetColumn(s.Panel) == col + 1)
                    {
                        newSt.Children.Add(s);
                    }
                }
                
                break;

            case "R":

                if (col == maxCols) break;

                newSt = StationFactory.CreateStation(row, col + 1, stWidth, stHeight, newName, St.AllStations, St.Form, St.LogForm);

                St.ConnectToFirstButton.Visible = false;
                
                St.Children.Add(newSt);

                // get all stations that are parallel with this one, and connect them to the new station
                foreach (Station s in St.AllStations.Stations)
                {
                    if (s != St && s != newSt && s.StLayout.GetColumn(s.Panel) == col)
                    {
                        s.Children.Add(newSt);
                    }
                }

                break;

        }
        
        // make the button disappear
        ((Button)sender).Visible = false;


    }

}

