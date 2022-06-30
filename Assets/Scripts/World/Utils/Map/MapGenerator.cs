using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]private NoiseDataObject _noiseData;

    public bool AutoUpdate;

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(_noiseData.Width, _noiseData.Height, _noiseData.Seed, _noiseData.Scale, _noiseData.Octaves, _noiseData.Persistance, _noiseData.Lacunarity, _noiseData.Offset);

        MapDisplay display = FindObjectOfType<MapDisplay>();

        display.DrawNoiseMap(noiseMap);
    }

    
}
