using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct NoiseInfo {
    public GameObject Obj;
    public float NoiseScale;
    public bool Walkable;
}

public class WorldGrid : MonoBehaviour
{
    [SerializeField] private NoiseInfo[] _info; 
    [SerializeField] private NoiseDataObject _noiseData;
    [SerializeField] private Vector2Int _gridSize;
    private WorldNode [,] _grid;
    private float[,] _noiseMap;

    private Vector3 _initialPosition;
    private Vector2Int _gridPosition;
    [SerializeField] private Transform _testPos;

    // Start is called before the first frame update
    void Awake()
    {
        CreateGrid();
    }

    private void InitializeGrid () {
        _initialPosition = new Vector3(transform.position.x + 0.5f - Mathf.Floor(_gridSize.x / 2f), transform.position.y, transform.position.z + 0.5f - Mathf.Floor(_gridSize.y / 2f));
        _grid = new WorldNode[_gridSize.x, _gridSize.y];
        _noiseMap = Noise.GenerateNoiseMap(_noiseData.Width, _noiseData.Height, _noiseData.Seed, _noiseData.Scale, _noiseData.Octaves, _noiseData.Persistance, _noiseData.Lacunarity, _noiseData.Offset);
    }

    private NoiseInfo PickTile (float value) {
        NoiseInfo result = new NoiseInfo();
        for (int i = 0; i < _info.Length;) {
            if(i < _info.Length - 1) {
                if(value > _info[i].NoiseScale && value < _info[i + 1].NoiseScale) {
                    result = _info[i];
                    i = _info.Length;
                } else {
                    i++;
                }
            } else {
                result = _info[i];
                i = _info.Length;
            }
        }

        return result;
    }

    public WorldNode GetNodeFromPosition(Vector3 position) {
        Vector2Int gridPos = new Vector2Int((int)(position.x + Mathf.Floor(_gridSize.x / 2f)), (int)(position.z + Mathf.Floor(_gridSize.y / 2f)));
        _gridPosition = gridPos;

        return _grid[gridPos.x, gridPos.y];
    }

    public List<WorldNode> GetNeighboursFromNode(WorldNode node) {
        List<WorldNode> neighbours = new List<WorldNode>();
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if(x == 0 && y == 0) {
                    continue;
                }

                int checkX = node.GridPosition.x + x;
                int checkY = node.GridPosition.y + y;

                if(checkX >= 0 && checkX < _gridSize.x && checkY >= 0 && checkY < _gridSize.y) {
                    neighbours.Add(_grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Returns an int between 0 and 8 for the total neighbours walkable
    /// </summary>
    
    public int CheckNeighbours(List<WorldNode> neighbours) {
        int walkable = 0;
        for(int i = 0; i < neighbours.Count; i++) {
            if(neighbours[i].Walkable) 
                walkable++;
        }
        return walkable;
    }

    /// <summary>
    /// Gets the first walkable neighbour it can find from certain node
    /// </summary>
    public WorldNode GetFirstWalkableNeighbour(List<WorldNode> neighbours) {
        WorldNode walkable = null;
        foreach(WorldNode node in neighbours) {
            if(node.Walkable) {
                walkable = node;
                break;
            }
        }
        return walkable;
    }

    private void CreateGrid() {
        InitializeGrid();

        for(int y = 0; y < _gridSize.y; y++) {
            for(int x = 0; x < _gridSize.x; x++) {
                Vector3 position = new Vector3(_initialPosition.x + x, _initialPosition.y, _initialPosition.z + y);
                NoiseInfo info = PickTile(_noiseMap[x,y]);
                _grid[x, y] = new WorldNode(info.Walkable, position, new Vector2Int(x, y));
                Instantiate(info.Obj, position, Quaternion.identity, transform);
            }
        }
    }

    public List<WorldNode> path;
    private void OnDrawGizmos() {
        
        if(_grid != null) {
            GetNodeFromPosition(_testPos.position);

            foreach(WorldNode node in _grid) {
                // Vector3 newPos = new Vector3(_initialPosition.x + x, _initialPosition.y, _initialPosition.z + y);
                
                if(_gridPosition.x == node.GridPosition.x && _gridPosition.y == node.GridPosition.y) {
                    Gizmos.color = Color.red;
                } else if (!node.Walkable) {
                    Gizmos.color = Color.blue;
                }
                else {
                    Gizmos.color = Color.green;
                } 

                if(path != null) {
                    if(path.Contains(node)) {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(node.Position, new Vector3(0.8f, 1.8f, 0.8f));
            }
        }      
            
    }
}
