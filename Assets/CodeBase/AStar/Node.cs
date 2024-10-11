using UnityEngine;

namespace AStar
{
    public class Node
    {
        public Node Parent { get; private set; }
        public bool Walkable { get; private set; }
        public Vector3 WorldPosition { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int GCost { get; private set; }
        public int HCost { get; private set; }
        public int FCost => GCost + HCost;

        public Node(bool walkable, Vector3 worldPosition, int x, int y)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
            X = x;
            Y = y;
        }

        public void UpdateNode(Node parent, int gCost, int hCost)
        {
            Parent = parent;
            GCost = gCost;
            HCost = hCost;
        }
    }
}