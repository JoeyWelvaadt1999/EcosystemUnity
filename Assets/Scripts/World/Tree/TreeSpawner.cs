using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private GameObject _tree;
    [Range(0f,100f)][SerializeField] private float _spawnPercentage;
    private WorldGrid _grid;
    private void Start() {
        _grid = FindObjectOfType<WorldGrid>();
        for(int y = 0; y < _gridSize.y; y++) {
            for(int x = 0; x < _gridSize.x; x++) {
                Vector3 newPos = new Vector3(Mathf.Floor(x  + 0.5f - Mathf.Floor(_gridSize.x / 2f)), 1, Mathf.Floor(y + 0.5f - Mathf.Floor(_gridSize.y / 2f)));
                WorldNode node = _grid.GetNodeFromPosition(newPos);
                List<WorldNode> neighbours = _grid.GetNeighboursFromNode(node);
                
                if(node.Walkable && _grid.CheckNeighbours(neighbours) == 8) {
                    System.Random rand = new System.Random();
                    if(Random.Range(0f, 100f) <= _spawnPercentage) {
                        node.Walkable = false;
                        Instantiate(_tree, newPos, Quaternion.identity);
                    }
                }
            }
        }
    }
}
