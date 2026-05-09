using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace StarterCode_WayPoints
{
    internal class Program
    {
        //directory containing file
        static string FILE_PATH = "..\\..\\..\\"; //set path to solution directory
        static string fileName = "UK_waypoints.csv"; //file to read

        static WayPointArray allWaypoints;
        static List<Route> routes = new List<Route>();
        static Route currentRoute = null;

        static void Main(string[] args)
        {
            LoadWayPoints(FILE_PATH + fileName);
            RunMenu();
        }

        static void LoadWayPoints(string fileName)
        {
            if (!File.Exists(fileName))
            {
                // Fallback in case of running from bin/Debug directly
                if (File.Exists("UK_waypoints.csv"))
                {
                    fileName = "UK_waypoints.csv";
                }
                else
                {
                    Console.WriteLine("File not found: " + fileName);
                    allWaypoints = new WayPointArray(10);
                    return;
                }
            }

            string[] linesInFile = File.ReadAllLines(fileName); 
            allWaypoints = new WayPointArray(linesInFile.Length);

            int lineNumber = 0;
            foreach (string line in linesInFile)
            {
                lineNumber++; 
                if (lineNumber != 1 && line != "") 
                {
                    string[] featuresInLine = line.Split(','); 
                    string name = featuresInLine[0];
                    string code = featuresInLine[1];
                    string latitude = featuresInLine[3];
                    string longitude = featuresInLine[4];
                    int elevation = convertElevationToMeters(featuresInLine[5]); 
                    string description = buildDescription(featuresInLine);

                    WayPoint wp = new WayPoint(name, code, latitude, longitude, elevation, description);
                    allWaypoints.Add(wp);
                }
            }
            Console.WriteLine($"Loaded {allWaypoints.Count} unique waypoints into the array.");
        }

        static void RunMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- GPS Route Creation Tool ---");
                Console.WriteLine("1. Display all waypoints");
                Console.WriteLine("2. Search waypoints (by name)");
                Console.WriteLine("3. Search waypoints (by partial name)");
                Console.WriteLine("4. Search waypoints (under given height)");
                Console.WriteLine("5. Create a new route");
                Console.WriteLine("6. Edit current route (Add to end)");
                Console.WriteLine("7. Edit current route (Insert at position)");
                Console.WriteLine("8. Edit current route (Remove by name)");
                Console.WriteLine("9. Display current route");
                Console.WriteLine("10. Reverse current route");
                Console.WriteLine("11. Select/Switch current route");
                Console.WriteLine("12. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        allWaypoints.DisplayAll();
                        break;
                    case "2":
                        Console.Write("Enter exact waypoint name: ");
                        string exactName = Console.ReadLine();
                        WayPoint foundExact = allWaypoints.SearchByName(exactName);
                        if (foundExact != null)
                            Console.WriteLine(foundExact.ToString());
                        else
                            Console.WriteLine("Waypoint not found.");
                        break;
                    case "3":
                        Console.Write("Enter partial waypoint name: ");
                        string partial = Console.ReadLine();
                        WayPoint[] foundPartial = allWaypoints.SearchByPartialName(partial);
                        if (foundPartial.Length > 0)
                        {
                            foreach (var wp in foundPartial)
                                Console.WriteLine(wp.ToString());
                            Console.WriteLine($"Found {foundPartial.Length} waypoints.");
                        }
                        else
                            Console.WriteLine("No waypoints found.");
                        break;
                    case "4":
                        Console.Write("Enter maximum height (in meters): ");
                        if (int.TryParse(Console.ReadLine(), out int maxHeight))
                        {
                            WayPoint[] foundHeight = allWaypoints.SearchByHeight(maxHeight);
                            if (foundHeight.Length > 0)
                            {
                                foreach (var wp in foundHeight)
                                    Console.WriteLine(wp.ToString());
                                Console.WriteLine($"Found {foundHeight.Length} waypoints under {maxHeight}m.");
                            }
                            else
                                Console.WriteLine("No waypoints found under this height.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid height input.");
                        }
                        break;
                    case "5":
                        Console.Write("Enter new route name: ");
                        string routeName = Console.ReadLine();
                        Route newRoute = new Route(routeName);
                        routes.Add(newRoute);
                        currentRoute = newRoute;
                        Console.WriteLine($"Route '{routeName}' created and set as current.");
                        break;
                    case "6":
                        if (currentRoute == null) { Console.WriteLine("No current route selected. Please create or select a route first."); break; }
                        Console.Write("Enter waypoint name to add to end: ");
                        string wpAdd = Console.ReadLine();
                        WayPoint addWp = allWaypoints.SearchByName(wpAdd);
                        if (addWp != null)
                        {
                            currentRoute.AddEnd(addWp);
                            Console.WriteLine($"Waypoint '{addWp.Name}' added to route.");
                        }
                        else
                            Console.WriteLine("Waypoint not found in data structure.");
                        break;
                    case "7":
                        if (currentRoute == null) { Console.WriteLine("No current route selected. Please create or select a route first."); break; }
                        Console.Write("Enter waypoint name to insert: ");
                        string wpIns = Console.ReadLine();
                        WayPoint insWp = allWaypoints.SearchByName(wpIns);
                        if (insWp != null)
                        {
                            Console.Write("Enter position (1 for front, etc.): ");
                            if (int.TryParse(Console.ReadLine(), out int pos))
                            {
                                currentRoute.InsertAt(pos, insWp);
                                Console.WriteLine($"Waypoint '{insWp.Name}' inserted at position {pos}.");
                            }
                            else
                                Console.WriteLine("Invalid position.");
                        }
                        else
                            Console.WriteLine("Waypoint not found in data structure.");
                        break;
                    case "8":
                        if (currentRoute == null) { Console.WriteLine("No current route selected. Please create or select a route first."); break; }
                        Console.Write("Enter waypoint name to remove: ");
                        string wpRem = Console.ReadLine();
                        currentRoute.Remove(wpRem);
                        break;
                    case "9":
                        if (currentRoute == null) { Console.WriteLine("No current route selected. Please create or select a route first."); break; }
                        currentRoute.DisplayRoute();
                        break;
                    case "10":
                        if (currentRoute == null) { Console.WriteLine("No current route selected. Please create or select a route first."); break; }
                        currentRoute.Reverse();
                        Console.WriteLine("Route reversed.");
                        currentRoute.DisplayRoute();
                        break;
                    case "11":
                        if (routes.Count == 0)
                        {
                            Console.WriteLine("No routes created yet.");
                            break;
                        }
                        Console.WriteLine("Available routes:");
                        for (int i = 0; i < routes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {routes[i].Name}");
                        }
                        Console.Write("Select route number: ");
                        if (int.TryParse(Console.ReadLine(), out int routeNum) && routeNum >= 1 && routeNum <= routes.Count)
                        {
                            currentRoute = routes[routeNum - 1];
                            Console.WriteLine($"Current route set to '{currentRoute.Name}'.");
                        }
                        else
                            Console.WriteLine("Invalid selection.");
                        break;
                    case "12":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static string buildDescription(string[] featuresInLine)
        {
            StringBuilder description = new StringBuilder();
            int arrayPosition = 11; 
            string descriptionPart;
            while (arrayPosition < featuresInLine.Length)
            {
                descriptionPart = featuresInLine[arrayPosition];
                if (isText(descriptionPart))
                {
                    description.Append(descriptionPart + ",");
                }
                arrayPosition++;
            }
            return description.ToString().TrimEnd(',');
        }

        static Boolean isText(string str)
        {
            if (str == "" || str == " ")
                return false;
            return true;
        }

        static int convertElevationToMeters(string elevationStr)
        {
            if (string.IsNullOrWhiteSpace(elevationStr)) return 0;
            char[] unitChars = { 'f', 't', 'M', 'm' };
            if (elevationStr.ToLower().EndsWith("m"))
            {
                if (Double.TryParse(elevationStr.TrimEnd(unitChars), out double valM))
                    return (int)valM;
                return 0;
            }

            if (Double.TryParse(elevationStr.TrimEnd(unitChars), out double elevationFeet))
                return (int)(elevationFeet / 3.142); //constant for ft to m
            
            return 0;
        }
    }
}
