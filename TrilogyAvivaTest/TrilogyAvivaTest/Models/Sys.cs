namespace TrilogyAvivaTest.Models
{
    public class Sys
    {
        public Sys(int type, int id, string country, int sunrise, int sunset)
        {
            Type = type;
            Id = id;
            Country = country;
            Sunrise = sunrise;
            Sunset = sunset;
        }

        public int Type { get;  }
        public int Id { get;  }
        public string Country { get;  }
        public int Sunrise { get;  }
        public int Sunset { get;  }
    }
}
