using System.Xml;
using static System.Collections.Specialized.BitVector32;

namespace PlantSimWFApp;

// -- LINKED LIST OF STATIONS --
public class LinkedListOfStations
{
    public LinkedList<Station> Stations { get; set; }

    public LinkedListOfStations()
    {
        Stations = new LinkedList<Station>();
    }

    // log all stations
    public void LogAllStations()
    {
        foreach (Station st in Stations) st.LogStation();
    }

    // save all stations to an xml file
    public void SaveToXml()
    {
        
        XmlWriterSettings settings = new()
        {
            Indent = true,
            IndentChars = "\t"
        };
        
        using XmlWriter writer = XmlWriter.Create("stations.xml", settings);
        
        writer.WriteStartDocument();
        writer.WriteStartElement("Stations");
        
        foreach (Station st in Stations)
        {
            
            writer.WriteStartElement("Station");
            writer.WriteElementString("Name", st.Name);
            writer.WriteElementString("X", st.X.ToString());
            writer.WriteElementString("Y", st.Y.ToString());

            // write all children
            writer.WriteStartElement("Children");
            foreach (Station child in st.Children) writer.WriteElementString("Child", child.Name);            
            writer.WriteEndElement();
            
            writer.WriteElementString("CycleTime", st.Timer.CycleTime.ToString());
            writer.WriteElementString("Efficiency", st.Efficiency.ToString());
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteEndDocument();
    }

    // load all stations from an xml file
    public void LoadFromXml()
    {
        XmlDocument doc = new();
        doc.Load("stations.xml");
        XmlNodeList stations = doc.GetElementsByTagName("Station");


        // get the form from the first station
        Form form = Stations.First().Form;
        
        // get the log form from the first station
        Form logForm = Stations.First().LogForm;

        // delete all stations
        Stations.First().DeleteStation();
        
        foreach (XmlNode station in stations)
        {
            string name = station["Name"].InnerText;
            int x = int.Parse(station["X"].InnerText);
            int y = int.Parse(station["Y"].InnerText);
            double cycleTime = double.Parse(station["CycleTime"].InnerText);
            double efficiency = double.Parse(station["Efficiency"].InnerText);
                       
            // create the station
            Station st = new(x, y, 120, 120, name, this, form, logForm);
            
            // set the cycle time and efficiency           
            st.Timer.SetCycleTime(cycleTime);
            st.Efficiency = efficiency;
            st.CycleTimeNumericUpDown.Value = (decimal)cycleTime;
            st.EfficiencyNumericUpDown.Value = (decimal)efficiency * 100;                                       
        }

        // add children to the stations
        foreach (XmlNode station in stations)
        {
            // add the children
            XmlNodeList? children = station["Children"]?.ChildNodes;
            if (children != null)
            {
                foreach (XmlNode child in children)
                {
                    string childName = child.InnerText;
                    Station childStation = Stations.First(st => st.Name == childName);
                    Station parentStation = Stations.First(st => st.Name == station["Name"].InnerText);
                    parentStation.Children.Add(childStation);
                }
            }
        }

        // go through each station and delete buttons accordingly

        foreach (Station s in Stations)
        {
            foreach (Station st in Stations)
            {
                // if there is a station above
                if (st.StLayout.GetColumn(st.Panel) == s.Y && st.StLayout.GetRow(st.Panel) == s.X - 1)
                {
                    s.Up.Button.Visible = false;
                }

                // if there is a station below
                if (st.StLayout.GetColumn(st.Panel) == s.Y && st.StLayout.GetRow(st.Panel) == s.X + 1)
                {
                    s.Down.Button.Visible = false;
                }

                // if there is a station to the left
                if (st.StLayout.GetColumn(st.Panel) == s.Y + 1 && st.StLayout.GetRow(st.Panel) == s.X)
                {
                    s.Right.Button.Visible = false;
                }
                
                if (st.StLayout.GetColumn(st.Panel) == s.Y + 2)
                {
                    break;
                }

            }
            // hide the connect to first button
            s.ConnectToFirstButton.Visible = false;

        }

        Stations.Last().ConnectToFirstButton.Visible = true;
    }
}



// -- STATION --
public class Station
{
    // coordinates of the station
    public int X { get; }
    public int Y { get; }

    // size of the station
    private readonly int _width;
    private readonly int _height;

    // the station's name
    public string Name { get; set; }

    // the window that the station is in
    public Form Form { get; }

    // the window where the log is
    public Form LogForm { get; }

    // the table layout panel that the station is in
    public TableLayoutPanel StLayout { get; }

    // where the console is logged
    public RichTextBox StInfo { get; }

    // where product info is logged
    public RichTextBox ProductInfo { get; }

    // where the stats are logged
    public RichTextBox Stats { get; }
    
    // the station's panel
    public Panel? Panel { get; private set; }

    // the station's buttons
    public StationButton? Up;
    public StationButton? Down;
    public StationButton? Right;
    public Button ConnectToFirstButton;

    // a timer for the station
    public StationTimer Timer;

    // a numeric up down to set the cycle time
    public NumericUpDown? CycleTimeNumericUpDown;

    // the station's children
    public List<Station> Children;

    // list of all stations
    public LinkedListOfStations? AllStations;

    // a progress bar to show the progress of the station
    public ProgressBar? ProgressBar;
    
    //// if the station has product
    //public bool HasProduct;

    // the station's product
    public Product? Product;

    // count how many product have been through this station
    public int ProductCount;

    // total stuck time
    public double TotalStuckTime = 0;

    // efficiency, as a percentage - this means that the cycle time is between eff * cycle time and cycle time
    public double Efficiency = 100;

    // a numeric up down to set the efficiency
    public NumericUpDown? EfficiencyNumericUpDown;

    // a label to show the current cycle time
    public Label? CycleTimeLabel;
    
    public Station(int x, int y, int width, int height, string name, LinkedListOfStations LLOS, Form form, Form logForm)
    {
        X = x;
        Y = y;
        _width = width;
        _height = height;
        Name = name;
        Form = form;
        LogForm = logForm;
        StLayout = (TableLayoutPanel)form.Controls.Find("StationLayout", true)[0];
        StInfo = (RichTextBox)logForm.Controls.Find("StationInfo", true)[0];
        ProductInfo = (RichTextBox)logForm.Controls.Find("ProductInfo", true)[0];
        Stats = (RichTextBox)logForm.Controls.Find("Statistics", true)[0];

        AllStations = LLOS;
        //HasProduct = false;
        
        Timer = new(this);
        
        Children = new List<Station>();
        AllStations.Stations.AddLast(this);
        
        PlaceOnForm();

    }

    private void ConnectToFirstStation(object? sender, EventArgs e)
    {
        Children.Add(AllStations.Stations.First.Value);

        // make all buttons invisible
        Up.Button.Visible = false;
        Down.Button.Visible = false;
        Right.Button.Visible = false;

        // make the connect to first button invisible
        ConnectToFirstButton.Visible = false;

    }

    // place the station
    private void PlaceOnForm()
    {

        const int offset = 5;
        const int buttonSize = 25;
        
        // create the station
        Panel = new Panel
        {
            Location = new Point(X, Y),
            Size = new Size(_width, _height),
            Name = Name,
            BorderStyle = BorderStyle.FixedSingle
        };

        // add 3 buttons to the form, one for each direction
        Up = new StationButton("U", _width / 2 + buttonSize / 2 + offset, offset, this);
        Down = new StationButton("D", _width / 2 + buttonSize / 2 + offset, _height - buttonSize - offset - 10, this);
        Right = new StationButton("R", _width - buttonSize - offset * 2, _height / 2 - buttonSize / 2 - 5, this);

        // add a button to connect to the first station
        ConnectToFirstButton = new Button
        {
            Text = "C",
            Size = new Size(25, 25),
            Location = new Point(0, 30)
        };
        ConnectToFirstButton.Click += ConnectToFirstStation;
        Panel.Controls.Add(ConnectToFirstButton);

        // add a progress bar
        ProgressBar = new ProgressBar
        {
            Location = new Point(0, _height - 10),
            Size = new Size(_width, 10),
            Minimum = 1,
            Maximum = 100,
            Value = 1
        };
        
        Panel.Controls.Add(ProgressBar);

        // add the cycle numeric up down to the station
        CycleTimeNumericUpDown = new NumericUpDown
        {
            Location = new Point(0, _height - 60),
            Size = new Size(40, 120),
            Minimum = 0,
            Maximum = 1000,
            Increment = 1,
            DecimalPlaces = 2,
            Value = 1,    
        };
        
        // onchange event for the numeric up down
        CycleTimeNumericUpDown.ValueChanged += CycleTimeChanged;

        Panel.Controls.Add(CycleTimeNumericUpDown);

        // add the efficiency numeric up down to the station in the bottom left corner
        EfficiencyNumericUpDown = new NumericUpDown
        {
            Location = new Point(0, _height - 35),
            Size = new Size(40, 120),
            Minimum = 0,
            Maximum = 100,
            Increment = 1,
            DecimalPlaces = 0,
            Value = 100,
        };
        
        Efficiency = 1;
        
        // onchange event for the numeric up down
        EfficiencyNumericUpDown.ValueChanged += EfficiencyChanged;

        Panel.Controls.Add(EfficiencyNumericUpDown);

        // add a label to show the current cycle time in the bottom right corner
        CycleTimeLabel = new Label
        {
            Location = new Point(5, 5),
            Size = new Size(40, 15),
            Text = "1"
        };
        Panel.Controls.Add(CycleTimeLabel);

        // display name of the station in the middle of the station
        Label label = new()
        {
            Text = Name,
            Location = new Point(0, 0),
            Size = new Size(_width, _height-20),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent
        };

        // add an event handler for when a keyboard button is pressed
        (Panel as Control).KeyPress += new KeyPressEventHandler(Panel_KeyDown);
        
        // focus on the panel on mousedown
        label.MouseDown += new MouseEventHandler(Label_MouseClick);

        Panel.Controls.Add(label);
        
        StLayout.Controls.Add(Panel, Y, X);
    }
    
    private void Label_MouseClick(object? sender, MouseEventArgs e)
    {
        // on left click, focus on the panel
        if (e.Button == MouseButtons.Left) Panel.Focus();

        // on right click, make the label editable
        if (e.Button == MouseButtons.Right)
        {
            Label label = (Label)sender;
            label.Click += new EventHandler(Label_Edit);
        }
        
    }

    private void Label_Edit(object? sender, EventArgs e)
    {
        Label label = (Label)sender;
        label.Click -= new EventHandler(Label_Edit);

        // create a textbox in place of the label
        TextBox textBox = new TextBox
        {
            Text = label.Text,
            Location = new Point(45, 42),
            Size = new Size(30, 30),
            TextAlign = HorizontalAlignment.Center,
        };
        
        Panel.Controls.Add(textBox);
        Panel.Controls.Remove(label);
        textBox.KeyDown += new KeyEventHandler(FinishEditing);

    }

    private void FinishEditing(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Return)
        {
            {
                TextBox textBox = (TextBox)sender;
                Label label = new Label
                {
                    Text = textBox.Text,
                    Location = new Point(0, 0),
                    Size = new Size(_width, _height - 20),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                Name = textBox.Text;
                label.MouseDown += new MouseEventHandler(Label_MouseClick);
                Panel.Controls.Remove(textBox);
                Panel.Controls.Add(label);
            }
        }
    }

    private void Panel_KeyDown(object? sender, KeyPressEventArgs e)
    {
        // delete station if backspace is pressed
        if (e.KeyChar == (char)Keys.Back)
        {
            DeleteStation();
        }
    }

    public void DeleteStation()
    {
        // delete panel from form
        StLayout.Controls.Remove(Panel);


        foreach (Station s in AllStations.Stations)
        {
            // if there is a station above, make the down button visible on the above station
            if (s.StLayout.GetColumn(s.Panel) == Y && s.StLayout.GetRow(s.Panel) == X - 1) s.Down.Button.Visible = true;

            // if there is a station below, make the up button visible on the below station
            if (s.StLayout.GetColumn(s.Panel) == Y && s.StLayout.GetRow(s.Panel) == X + 1) s.Up.Button.Visible = true;

            // if there is a station to the left, make the right button on that station
            if (s.StLayout.GetColumn(s.Panel) == Y - 1 && s.StLayout.GetRow(s.Panel) == X) s.Right.Button.Visible = true;
            
            s.Children.Remove(this);

        }

        // remove this station from the list of all stations
        AllStations.Stations.Remove(this);

        
    }

    private void EfficiencyChanged(object? sender, EventArgs e)
    {
        Efficiency = (double)EfficiencyNumericUpDown!.Value / 100;
        StInfo.AppendText($"{this.Name} efficiency changed to {Efficiency}\n");
    }

    // log station info in console
    public void LogStation()
    {

        // log station info in the rich textbox
        StInfo.AppendText($"Station: {Name}\nX: {X}\nY: {Y}\nChildren: \n");
        foreach (Station st in Children)
        {
            StInfo.AppendText(st.Name + "\n");
        }

        StInfo.AppendText("\n------\n");
    }

    private void CycleTimeChanged(object sender, EventArgs e)
    {
        double CycleTime = (double)CycleTimeNumericUpDown!.Value;
        Timer.SetCycleTime(CycleTime);
        StInfo.AppendText($"{this.Name} cycle time changed to {Timer.CycleTime}\n");
    }

}




