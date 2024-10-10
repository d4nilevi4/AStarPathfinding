using UnityEngine;

namespace AStar
{
    public class Node
    {
        public bool Walkable { get; private set; }
        public Vector3 WorldPosition { get; private set; }

        public Node(bool walkable, Vector3 worldPosition)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
        }
    }
}

