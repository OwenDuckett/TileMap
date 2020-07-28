using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public static Pathfinding Instance { get; private set; }

    public Pathfinding(int width, int height)
    {
        Instance = this;
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> grid, int x, int y) => new PathNode(grid, x, y));
    }

    public List<Vector3> FindPath(Vector3 startVector, Vector3 endVector)
    {
        grid.GetXY(startVector, out int startX, out int startY);
        grid.GetXY(endVector, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null)
            return null;
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();

            foreach (PathNode node in path)
            {
                vectorPath.Add(new Vector3(node.x, node.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * 0.5f);
            }

            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculatefCost();
                pathNode.previousNode = null;

            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculatefCost();


        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestfCost(openList);

            if(currentNode == endNode)
            {
                return CalculatedPAth(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode node in GetNeighbours(currentNode))
            {
                if (closedList.Contains(node))
                    continue;

                if(!node.isWalkable)
                {
                    closedList.Add(node);
                    continue;
                }


                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, node);

                if(tentativeGCost < node.gCost)
                {
                    node.previousNode = currentNode;
                    node.gCost = tentativeGCost;
                    node.hCost = CalculateDistance(node, endNode);
                    node.CalculatefCost();

                    if (!openList.Contains(node))
                        openList.Add(node);
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbours(PathNode pathNode)
    {
        List<PathNode> neighbours = new List<PathNode>();

        if(pathNode.x - 1 >= 0)
        {
            neighbours.Add(GetNode(pathNode.x - 1, pathNode.y));

            if (pathNode.y - 1 >= 0)
                neighbours.Add(GetNode(pathNode.x - 1, pathNode.y - 1));

            if (pathNode.y + 1 < grid.GetHeight())
                neighbours.Add(GetNode(pathNode.x - 1, pathNode.y + 1));
        }

        if(pathNode.x + 1 < grid.GetWidth())
        {
            neighbours.Add(GetNode(pathNode.x + 1, pathNode.y));

            if (pathNode.y - 1 >= 0)
                neighbours.Add(GetNode(pathNode.x + 1, pathNode.y - 1));

            if (pathNode.y + 1 < grid.GetHeight())
                neighbours.Add(GetNode(pathNode.x + 1, pathNode.y + 1));
        }

        if (pathNode.y - 1 >= 0)
            neighbours.Add(GetNode(pathNode.x, pathNode.y - 1));

        if (pathNode.y + 1 < grid.GetHeight())
            neighbours.Add(GetNode(pathNode.x, pathNode.y + 1));


        return neighbours;
    }

    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatedPAth(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();

        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
        if (a == null || b == null)
            return 0;

        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int distance = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRIGHT_COST * distance;
    }

    private PathNode GetLowestfCost(List<PathNode> pathNodes)
    {
        PathNode lowNode = pathNodes[0];
        for (int i = 1; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].fCost < lowNode.fCost)
            {
                lowNode = pathNodes[i];
            }
        }

        return lowNode;
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }
}

public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int y;
    public bool isWalkable;

    public int gCost;
    public int hCost;
    public int fCost;
    public int costMod = 0;

    public PathNode previousNode;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        isWalkable = true;
    }

    public override string ToString()
    {
        return x + "," + y;
    }

    public void CalculatefCost()
    {
        fCost = gCost + hCost + costMod;
    }

    public void SetWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void IncreaseCostMod()
    {
        costMod += 1;
    }
}
