using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoiseData", menuName="Noise/Create Noise Data", order = 1)]
public class NoiseDataObject : ScriptableObject
{
    public int Width;
    public int Height;
    public float Scale;
    public int Octaves;
    [Range(0,1)]public float Persistance;
    public float Lacunarity;
    public int Seed;
    public Vector2 Offset;
}
