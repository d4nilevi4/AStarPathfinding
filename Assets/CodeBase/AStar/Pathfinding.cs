using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField] private Transform seeker, target;
        private Grid _grid;


        private void Awake()
        {
            _grid = GetComponent<Grid>();
        }

        private void Update()
        {
            FindPath(seeker.position, target.position);
        }

        private void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            var startNode = _grid.NodeFromWorldPoint(startPos);
            var targetNode = _grid.NodeFromWorldPoint(targetPos);

            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                //var openNode = openSet.Aggregate((node1, node2) => node1.FCost < node2.FCost ? node1 : node2);
                var currentNode = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost ||
                        openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (var neighbour in _grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                        continue;

                    var newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.UpdateNode(currentNode, newMovementCostToNeighbour,
                            GetDistance(neighbour, targetNode));
                        
                        if(!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        private void RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            
            path.Reverse();
            _grid.Path = path;
        }

        private int GetDistance(Node from, Node to)
        {
            var dstX = Mathf.Abs(from.X - to.X);
            var dstY = Mathf.Abs(from.Y - to.Y);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }

            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}