namespace StarterCode_WayPoints
{
    public class Node
    {
        public WayPoint Data { get; set; }
        public Node Next { get; set; }

        public Node(WayPoint data)
        {
            Data = data;
            Next = null;
        }
    }
}
