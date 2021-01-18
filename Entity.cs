using System;

namespace BeachWeatherStations
{
    public  class Entry
    {
        public string StationName { get; set; }
        public DateTimeOffset MeasurementTimestamp { get; set; }
        public float AirTemperature { get; set; }
        public float WetBulbTemperature { get; set; }
        public float Humidity { get; set; }
        public float RainIntensity { get; set; }
        public float IntervalRain { get; set; }
        public float TotalRain { get; set; }
        public float PrecipitationType { get; set; }
        public float WindDirection { get; set; }
        public float WindSpeed { get; set; }
        public float MaximumWindSpeed { get; set; }
        public float BarometricPressure { get; set; }
        public float SolarRadiation { get; set; }
        public float Heading { get; set; }
        public string BatteryLife { get; set; }
        public string MeasurementTimestampLabel { get; set; }
        public string MeasurementId { get; set; }
    }
}