namespace TrilogyAvivaTest.Models
{
    public class Main
    {
        public Main(double temp, double feels_like, double temp_min, double temp_max, int pressure, int humidity)
        {
            Temp = temp;
            Feels_like = feels_like;
            Temp_min = temp_min;
            Temp_max = temp_max;
            Pressure = pressure;
            Humidity = humidity;
        }

        public double Temp { get; }
        public double Feels_like { get; }
        public double Temp_min { get; }
        public double Temp_max { get; }
        public int Pressure { get; }
        public int Humidity { get; }
    }
}
