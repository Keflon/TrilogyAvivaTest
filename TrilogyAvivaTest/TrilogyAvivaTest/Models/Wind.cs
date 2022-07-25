namespace TrilogyAvivaTest.Models
{
    public class Wind
    {
        public Wind(double speed, int deg)
        {
            Speed = speed;
            Deg = deg;
        }

        public double Speed { get; }
        public int Deg { get;  }
    }
}
