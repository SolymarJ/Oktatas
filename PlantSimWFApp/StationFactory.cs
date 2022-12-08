namespace PlantSimWFApp;

public class StationFactory
{
    public static Station CreateStation(int x, int y, int width, int height, string name, LinkedListOfStations LLOS, Form form, Form logForm)
    {
        return new Station(x, y, width, height, name, LLOS, form, logForm);
    }
}