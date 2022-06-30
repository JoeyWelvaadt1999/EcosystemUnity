using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EntityObject {
    public GameObject Object;
    public int SpawnChance;
}
public class EntitySpawner : MonoBehaviour
{
    [Header("Spawn chance must be equal or less than 100")]
    [SerializeField]private List<EntityObject> _spawns;
    [SerializeField]private Vector2Int _gridSize;
    private WorldGrid _grid;
    // Start is called before the first frame update
    void Start()
    {
        _grid = GetComponent<WorldGrid>();
        _spawns.Sort((x, y) => x.SpawnChance.CompareTo( y.SpawnChance));
        
        

        for(int y = -_gridSize.y/2; y < _gridSize.y/2; y++) {
            for(int x = -_gridSize.x/2; x < _gridSize.x/2; x++) {
                if(x % 10 == 0 && y % 10 == 0) {
                    if(_grid.GetNodeFromPosition(new Vector3(x, 0, y)).Walkable) {
                        int rand = (int)Random.Range(0, 1000);
                        int totalSpawnChance = 0;
                        for (int i = 0; i < _spawns.Count; i++)
                        {
                            if(rand > totalSpawnChance && rand < totalSpawnChance + _spawns[i].SpawnChance) {
                                Instantiate(_spawns[i].Object, new Vector3(x, 2, y), Quaternion.identity);
                                Debug.Log(_spawns[i].Object.name);
                            } else {
                                totalSpawnChance += _spawns[i].SpawnChance;
                            }
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
