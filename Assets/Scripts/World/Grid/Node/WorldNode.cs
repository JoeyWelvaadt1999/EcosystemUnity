using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNode {
    private Vector3 _position;
    public Vector3 Position {
        get { return _position; }
    }
    private Vector2Int _gridPosition;
    public Vector2Int GridPosition {
        get { return _gridPosition; }
    }

    private bool _walkable;
    public bool Walkable {
        get { return _walkable; }
        set { _walkable = value; }
    }

    private WorldNode _parent;
    public WorldNode Parent {
        get { return _parent; }
        set { _parent = value; }
    }

    private int _gCost;
    public int GCost {
        get { return _gCost; }
        set { _gCost = value; }
    }
    private int _hCost;
    public int HCost {
        get { return _hCost; }
        set { _hCost = value; }
    }

    public int FCost {
        get {
            return _gCost + _hCost;
        }
    }

    public WorldNode (bool walkable, Vector3 position, Vector2Int gridPosition) {
        _position = position;
        _walkable = walkable;
        _gridPosition = gridPosition;
    }
}