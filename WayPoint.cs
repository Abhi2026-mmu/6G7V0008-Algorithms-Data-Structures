using System;

namespace StarterCode_WayPoints
{
    public class WayPoint
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Elevation { get; set; }
        public string Description { get; set; }

        public WayPoint(string name, string code, string latitude, string longitude, int elevation, string description)
        {
            Name = name;
            Code = code;
            Latitude = latitude;
            Longitude = longitude;
            Elevation = elevation;
            Description = description;
        }

        public override string ToString()
        {
            return $"{{ {Name}, {Code}, pos[{Longitude},{Latitude}], h:{Elevation}m, {Description} }}";
        }
    }
}
