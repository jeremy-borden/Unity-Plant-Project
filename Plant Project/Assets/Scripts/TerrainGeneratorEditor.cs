using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( TerrainGenerator ) )]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI ()
    {

        TerrainGenerator terrainGenerator = ( TerrainGenerator )target;

        if ( DrawDefaultInspector() )
        {
            if ( terrainGenerator.autoUpdate )
            {
                terrainGenerator.Generate();

            }
        }

        if ( GUILayout.Button( "Generate" ) )
        {
            terrainGenerator.Generate();
            Debug.Log( "Generated" );
        }
    }
}