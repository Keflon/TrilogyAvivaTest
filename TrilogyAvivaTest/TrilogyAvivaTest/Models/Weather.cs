namespace TrilogyAvivaTest.Models
{
    public class Weather
    {
        public Weather(int id, string main, string description, string icon)
        {
            Id = id;
            Main = main;
            Description = description;
            Icon = icon;
        }

        public int Id { get; }
        public string Main { get; }
        public string Description { get; }
        public string Icon { get; }
    }
}
