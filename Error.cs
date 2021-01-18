namespace BeachWeatherStations
{
    public class Error
    {
        public string Code { get; set; }
        public bool ErrorError { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public string Query { get; set; }
        public Position Position { get; set; }
    }

    public class Position
    {
    }
}