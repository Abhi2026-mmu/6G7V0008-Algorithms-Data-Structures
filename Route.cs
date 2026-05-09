using System;

namespace StarterCode_WayPoints
{
    public class Route
    {
        public string Name { get; set; }
        private Node head;

        public Route(string name)
        {
            Name = name;
            head = null;
        }

        public void AddFront(WayPoint wp)
        {
            Node newNode = new Node(wp);
            newNode.Next = head;
            head = newNode;
        }

        public void AddEnd(WayPoint wp)
        {
            Node newNode = new Node(wp);
            if (head == null)
            {
                head = newNode;
                return;
            }
            Node current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = newNode;
        }

        // Inserts at a specific 1-based position.
        // If position is 1, inserts at front.
        // If position is larger than the number of items, inserts at the end.
        public void InsertAt(int position, WayPoint wp)
        {
            if (position <= 1 || head == null)
            {
                AddFront(wp);
                return;
            }

            Node newNode = new Node(wp);
            Node current = head;
            int currentPos = 1;

            while (current.Next != null && currentPos < position - 1)
            {
                current = current.Next;
                currentPos++;
            }

            newNode.Next = current.Next;
            current.Next = newNode;
        }

        public void Remove(string waypointName)
        {
            if (head == null) return;

            if (head.Data.Name.Equals(waypointName, StringComparison.OrdinalIgnoreCase))
            {
                head = head.Next;
                return;
            }

            Node current = head;
            while (current.Next != null && !current.Next.Data.Name.Equals(waypointName, StringComparison.OrdinalIgnoreCase))
            {
                current = current.Next;
            }

            if (current.Next != null)
            {
                current.Next = current.Next.Next;
            }
            else
            {
                Console.WriteLine($"Waypoint {waypointName} not found in the route.");
            }
        }

        public void Reverse()
        {
            Node prev = null;
            Node current = head;
            Node next = null;

            while (current != null)
            {
                next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }
            head = prev;
        }

        public void DisplayRoute()
        {
            Console.WriteLine($"\n--- Route: {Name} ---");
            if (head == null)
            {
                Console.WriteLine("The route is empty.");
                return;
            }

            Node current = head;
            int i = 1;
            while (current != null)
            {
                Console.WriteLine($"{i}. {current.Data.ToString()}");
                current = current.Next;
                i++;
            }
            Console.WriteLine("-----------------------");
        }
    }
}
