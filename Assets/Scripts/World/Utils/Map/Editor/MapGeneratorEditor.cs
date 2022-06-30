using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
   public override void OnInspectorGUI() {
       MapGenerator gen = (MapGenerator)target;
       if(DrawDefaultInspector()) {
           if(gen.AutoUpdate) {
               gen.GenerateMap();
           }
       }



       if(GUILayout.Button("Generate")) {
           gen.GenerateMap();
       }
   }
}
