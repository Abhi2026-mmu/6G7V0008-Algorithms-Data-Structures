using System;

namespace StarterCode_WayPoints
{
    public class WayPointArray
    {
        private WayPoint[] waypoints;
        private int count;

        public int Count { get { return count; } }

        public WayPointArray(int size)
        {
            waypoints = new WayPoint[size];
            count = 0;
        }

        public void Add(WayPoint wp)
        {
            if (!Contains(wp.Name))
            {
                if (count < waypoints.Length)
                {
                    waypoints[count] = wp;
                    count++;
                }
                else
                {
                    Console.WriteLine("Data structure is full.");
                }
            }
        }

        public bool Contains(string name)
        {
            for (int i = 0; i < count; i++)
            {
                if (waypoints[i].Name == name) return true;
            }
            return false;
        }

        public void DisplayAll()
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(waypoints[i].ToString());
            }
        }

        public WayPoint SearchByName(string name)
        {
            for (int i = 0; i < count; i++)
            {
                if (waypoints[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return waypoints[i];
            }
            return null;
        }

        public WayPoint[] SearchByPartialName(string partialName)
        {
            WayPoint[] results = new WayPoint[count];
            int resCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (waypoints[i].Name.StartsWith(partialName, StringComparison.OrdinalIgnoreCase))
                {
                    results[resCount++] = waypoints[i];
                }
            }
            WayPoint[] finalRes = new WayPoint[resCount];
            Array.Copy(results, finalRes, resCount);
            return finalRes;
        }

        public WayPoint[] SearchByHeight(int maxHeight)
        {
            WayPoint[] results = new WayPoint[count];
            int resCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (waypoints[i].Elevation < maxHeight)
                {
                    results[resCount++] = waypoints[i];
                }
            }
            WayPoint[] finalRes = new WayPoint[resCount];
            Array.Copy(results, finalRes, resCount);
            return finalRes;
        }
    }
}
