using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer.Misc
{
    public class PathfindNode
    {
        public Point Position;

        public bool Walkable;

        public PathfindNode[] Neighbors;

        public PathfindNode Parent;

        public bool isEnd;

        public bool InOpenList;

        public bool InClosedList;

        public float DistanceToGoal;

        public float DistanceTraveled;
    }

    public class PathfindAStar
    {
        public PathfindNode[,] searchNodes;

        private int levelWidth;

        private int levelHeight;

        private List<PathfindNode> openList = new List<PathfindNode>();

        private List<PathfindNode> closedList = new List<PathfindNode>();

        float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) +
                   Math.Abs(point1.Y - point2.Y);
        }

        public void InitializeSinglePathNode(int x, int y)
        {
            //var tiles = FieldMap.Tiles;

            PathfindNode node = new PathfindNode();
            node.Position = new Point(x, y);

            //node.Walkable = tiles[x, y].IsWalkableTileType;

        }

        public void InitializeAllPathNodes(int mapWidth, int mapHeight)
        {
            levelWidth = mapWidth;
            levelHeight = mapHeight;

            searchNodes = new PathfindNode[levelWidth, levelHeight];

            //var tiles = FieldMap.Tiles;

            for(int y = 0; y < levelHeight; y++)
            {
                for(int x = 0; x < levelWidth; x++)
                {
                    PathfindNode node = new PathfindNode();
                    node.Position = new Point(x, y);

                    //node.Walkable = tiles[x, y].IsWalkableTileType;

                    if(node.Walkable == true)
                    {
                        node.Neighbors = new PathfindNode[4];
                        searchNodes[x, y] = node;
                    }
                }
            }

            for(int x = 0; x < levelWidth; x++)
            {
                for(int y = 0; y < levelHeight; y++)
                {
                    PathfindNode node = searchNodes[x, y];

                    if(node == null || !node.Walkable)
                    {
                        continue;
                    }

                    Point[] neighbors = new Point[]
                    {
                        //  horizontal & vertical movement
                        new Point (x, y - 1),
                        new Point (x, y + 1),
                        new Point (x - 1, y),
                        new Point (x + 1, y),

                        //  diagonal movement
                        //new Point (x + 1, y + 1),
                        //new Point (x + 1, y - 1),
                        //new Point (x - 1, y + 1),
                        //new Point (x + 1, y - 1),
                    };

                    for(int i = 0; i < neighbors.Length; i++)
                    {
                        Point position = neighbors[i];

                        if(position.X < 0 || position.X > levelWidth - 1 ||
                            position.Y < 0 || position.Y > levelHeight - 1)
                        {
                            continue;
                        }

                        PathfindNode neighbor = searchNodes[position.X, position.Y];

                        if(neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }

                        node.Neighbors[i] = neighbor;
                    }
                }
            }
        }

        void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for(int x = 0; x < levelWidth; x++)
            {
                for(int y = 0; y < levelHeight; y++)
                {
                    PathfindNode node = searchNodes[x, y];

                    if(node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;
                }
            }
        }

        List<Point> FindFinalPath(PathfindNode startNode, PathfindNode endNode)
        {
            closedList.Add(endNode);

            PathfindNode parentTile = endNode.Parent;
            if(parentTile == null)
            {
                return new List<Point>();
            }

            while(parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            List<Point> finalPath = new List<Point>();

            for(int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Point(closedList[i].Position.X/* * 32*/,
                                          closedList[i].Position.Y/* * 32*/));
            }

            return finalPath;
        }

        PathfindNode FindBestNode()
        {
            PathfindNode currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            for(int i = 0; i < openList.Count; i++)
            {
                if(openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }

        public List<Point> FindPath(Point startPoint, Point endPoint)
        {
            if(startPoint == endPoint)
            {
                return new List<Point>();
            }

            ResetSearchNodes();

            PathfindNode startNode = searchNodes[startPoint.X, startPoint.Y];
            PathfindNode endNode = searchNodes[endPoint.X, endPoint.Y];

            if(startNode == null)
            {
                Console.WriteLine("STARTNODE NULL(unit(s) blocked) - crash");
                return new List<Point>();
            }

            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            PathfindNode nearestNode = startNode;  // preparing nearestNode 

            int count = openList.Count;

            while(openList.Count > 0)
            {
                PathfindNode currentNode = FindBestNode();

                if(currentNode == null)
                {
                    break;
                }

                if(currentNode == endNode)
                {
                    return FindFinalPath(startNode, endNode);
                }

                for(int i = 0; i < currentNode.Neighbors.Length; i++)
                {
                    PathfindNode neighbor = currentNode.Neighbors[i];

                    if(neighbor == null)
                    {
                        continue;
                    }

                    float distanceTraveled = currentNode.DistanceTraveled + 1;

                    float heuristic = Heuristic(neighbor.Position, endPoint);

                    if(neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        neighbor.DistanceTraveled = distanceTraveled;
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        neighbor.Parent = currentNode;
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    else if(neighbor.InOpenList || neighbor.InClosedList)
                    {
                        if(neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;
                            neighbor.Parent = currentNode;

                        }
                    }
                    // if it could not find destination(endNode/endPoint), caluclate nearest node
                    //if(endNode != null)
                    //    if(Heuristic(neighbor.Position, endNode.Position) < (Heuristic(nearestNode.Position, endNode.Position)))
                    //        nearestNode = neighbor;

                }

                openList.Remove(currentNode);
                currentNode.InClosedList = true;

            }

            endNode = nearestNode;

            if(endNode == null)
            {
                Console.WriteLine("null");
                return new List<Point>();
            }

            return FindFinalPath(startNode, endNode);
        }

    }
}
