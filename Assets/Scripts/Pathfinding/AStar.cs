using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pathfinding algoritm for entities.
/// </summary>
/// <remarks></remarks>

public class AStar : MonoBehaviour
{
    private WorldGrid _worldGrid;
    [SerializeField] private Transform _testObj;
    [SerializeField] private Transform _testObj2;

    private void Awake()
    {
        _worldGrid = FindObjectOfType<WorldGrid>();
    }

    private void Update()
    {
        // FindPath(_testObj2.transform.position, _testObj.transform.position);
    }

    /// <summary>
    /// Find a path of walkable nodes, it searches for the shortest path 
    /// using a certain cost calculation for nodes.
    /// </summary>
    /// <param name="start">Start location</param>
    /// <param name="end">End location</param>
    /// <returns>A path of world nodes</returns>
    /// <remarks></remarks>
    public List<WorldNode> FindPath(Vector3 start, Vector3 end)
    {
        WorldNode startNode = _worldGrid.GetNodeFromPosition(start);
        WorldNode endNode = _worldGrid.GetNodeFromPosition(end);

        List<WorldNode> openSet = new List<WorldNode>();
        HashSet<WorldNode> closedSet = new HashSet<WorldNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            WorldNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {

                return RetracePath(startNode, endNode);
            }

            foreach (WorldNode neighbour in _worldGrid.GetNeighboursFromNode(currentNode))
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCost = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newMovementCost < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCost;
                    neighbour.HCost = GetDistance(neighbour, endNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }

            }
        }
        return new List<WorldNode>();
    }

    /// <summary>
    /// Find path gets the path in reverse that is why the path has to be reversed. That is what this function does.
    /// By going through the parents of world nodes and reversing that list it gets the path in correct order.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns>List of world nodes</returns>
    /// <remarks></remarks>
    private List<WorldNode> RetracePath(WorldNode start, WorldNode end)
    {
        List<WorldNode> path = new List<WorldNode>();
        WorldNode current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        _worldGrid.path = path;
        return path;
    }

    private int GetDistance(WorldNode a, WorldNode b)
    {
        int distX = Mathf.Abs(a.GridPosition.x - b.GridPosition.x);
        int distY = Mathf.Abs(a.GridPosition.y - b.GridPosition.y);
        if (distX > distY)
        {
            return (14 * distY) + (10 * (distX - distY));
        }
        else
        {
            return (14 * distX) + (10 * (distY - distX));
        }
    }
}

