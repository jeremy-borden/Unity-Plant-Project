using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class TriangleMesh : MonoBehaviour
{
    Mesh mesh;

    Vector3[] verticies;
    int[] triangles;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateTri();
        UpdateMesh();
    }

    void Update()
    {

    }

    void CreateTri()
    {
        verticies = new Vector3[]{
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(Mathf.Sqrt(3)/2f,0, 0.5f),


        };

        triangles = new int[]{
            0,1,2,
        };
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
