using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public GameObject prefab;

    Color[] vertexColors;
    private float minY;
    private float maxY;

    [Range( 1, 40 )]
    public int size;
    public float amplitude;
    public float noiseScale;
    public int octaves;
    [Range( 0, 1 )]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;

    public Gradient gradient;

    public bool autoUpdate;

    public void Generate ()
    {

        int xLength = ( int )( size * Mathf.Sqrt( 3 ) );
        int zLength = size;
        float[,] noiseMap = Noise.GenerateNoiseMap( xLength, zLength, seed, noiseScale, octaves, persistance, lacunarity, offset );

        for ( int x = 0; x < xLength; x++ )
        {
            for ( int z = 0; z < zLength; z++ )
            {
                GameObject terrainObject = Instantiate( prefab, //
                new Vector3( x * ( Mathf.Sqrt( 3 ) / 2 ), 1, z * 1.5f ), //
                Quaternion.Euler( 0, ( ( x + z ) % 2 ) * 180, 0 ), //
                this.transform );

                terrainObject.transform.localScale += new Vector3( 0, amplitude * noiseMap[ x, z ], 0 );
                terrainObject.transform.localPosition += new Vector3( 0, amplitude * noiseMap[ x, z ] / 2f, 0 );
            }
        }
        GenerateMesh();
        ClearChildren();
        GenerateMeshColor( transform.GetComponent<MeshFilter>().sharedMesh );

    }

    private void GenerateMesh ()
    {
        gameObject.GetComponent<MeshFilter>().sharedMesh.Clear();
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[ meshFilters.Length ];
        int i = 0;
        while ( i < meshFilters.Length )
        {
            combine[ i ].mesh = meshFilters[ i ].sharedMesh;
            combine[ i ].transform = meshFilters[ i ].transform.localToWorldMatrix;
            meshFilters[ i ].gameObject.SetActive( false );
            i++;
        }
        transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes( combine );
        transform.gameObject.SetActive( true );


    }

    private void GenerateMeshColor ( Mesh mesh )
    {
        Vector3[] verts = mesh.vertices;
        int vC = mesh.vertexCount;
        vertexColors = new Color[ vC ];
        for ( int i = 0; i < vC; i++ )
        {
            vertexColors[ i ] = gradient.Evaluate( Mathf.InverseLerp( 0, amplitude, verts[ i ].y ) );
        }
        mesh.colors = vertexColors;
    }

    private void ClearChildren ()
    {
        while ( transform.childCount > 0 )
        {
            DestroyImmediate( transform.GetChild( 0 ).gameObject );
        }
    }

    private void OnValidate ()
    {
        if ( size < 1 )
        {
            size = 1;
        }
        if ( lacunarity < 1 )
        {
            lacunarity = 1;
        }
        if ( octaves < 0 )
        {
            octaves = 0;
        }
        if ( amplitude < 0 )
        {
            amplitude = 0;
        }
        if ( noiseScale < 0 )
        {
            noiseScale = 0;
        }
    }
}
