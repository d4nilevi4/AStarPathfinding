﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace AStar
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField] private float nodeRadius;
        [SerializeField] private LayerMask unwalkableMask;

        private Node[,] _grid;

        private float _nodeDiameter;
        private int _gridSizeX;
        private int _gridSizeY;
        public List<Node> Path { get; set; }

        private void Start()
        {
            _nodeDiameter = nodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
            CreateGrid();
        }

        private void CreateGrid()
        {
            _grid = new Node[_gridSizeX, _gridSizeY];
            var worldBottomLeft =
                transform.position
                - Vector3.right * gridWorldSize.x / 2
                - Vector3.forward * gridWorldSize.y / 2;


            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    var worldPoint = worldBottomLeft 
                                     + Vector3.right * (x * _nodeDiameter + nodeRadius) 
                                     + Vector3.forward * (y * _nodeDiameter + nodeRadius);
                    var walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                    _grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        public Node NodeFromWorldPoint(Vector3 worldPoint)
        {
            var percentX = (worldPoint.x + gridWorldSize.x / 2) / gridWorldSize.x; 
            var percentY = (worldPoint.z + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            
            var x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
            var y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
            return _grid[x, y];
        }

        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>(8);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if(x == 0 && y == 0)
                        continue;

                    var checkX = node.X + x;
                    var checkY = node.Y + y;

                    if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                    {
                        neighbours.Add(_grid[checkX, checkY]);
                    }
                }
            }
            
            return neighbours;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (_grid != null)
            {
                foreach (var node in _grid)
                {
                    Gizmos.color = node.Walkable ? Color.white : Color.red;
                    if(Path.Contains(node))
                        Gizmos.color = Color.black;
                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter-.1f));
                }
            }
        }
    }
}