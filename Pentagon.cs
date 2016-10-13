using UnityEngine;
using System.Collections;


public class Pentagon : MonoBehaviour
{

    [HideInInspector]
    public Vector3 centre;
    [HideInInspector]
    public static float radius = 40;

    private int vertices_count = 5;

    float angle;

    public Vector3[] vertices;

    public Vector3[] directions;

    public Vector2[] uvs;

    int[] triangles =
    {
        0,4,1,
        0,3,2,
        0,2,1,
        1,4,3,
    };



    void Awake()
    {
        vertices = new Vector3[vertices_count];
        directions = new Vector3[vertices_count];
        uvs = new Vector2[vertices_count];

        angle = 2 * Mathf.PI / vertices_count;
        centre = transform.position;
        MakePentagon();
    }

    //Making Mesh for Back Face Adding collider to the mesh
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;

        int total_vertices_count = vertices.Length;

        Vector3[] newVertices = new Vector3[total_vertices_count * 2];
        Vector3[] newNormals = new Vector3[total_vertices_count * 2];




        for (int i = 0; i < total_vertices_count; i++)
        {
            newVertices[i] = newVertices[i + total_vertices_count] = vertices[i];
            newNormals[i] = normals[i];
            newNormals[i + total_vertices_count] = -normals[i];//Other face's normals reversed
        }

        int[] triangles = mesh.triangles;
        int triangle_count = triangles.Length;
        int[] newTriangles = new int[triangle_count * 2];

        for (int i = 0; i < triangle_count; i += 3)
        {
            newTriangles[i] = triangles[i];
            newTriangles[i + 1] = triangles[i + 1];
            newTriangles[i + 2] = triangles[i + 2];

            int j = i + triangle_count;

            newTriangles[j] = triangles[i] + total_vertices_count;
            newTriangles[j + 2] = triangles[i + 1] + total_vertices_count;
            newTriangles[j + 1] = triangles[i + 2] + total_vertices_count;
        }
        mesh.vertices = newVertices;
        mesh.normals = newNormals;
        mesh.triangles = newTriangles;

        MeshCollider meshCollider = GetComponent<MeshFilter>().gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    //Making Pentagon 
    public void MakePentagon()
    {

        for (int i = 0; i < vertices_count; i++)
        {
            vertices[i].x = centre.x + radius * Mathf.Cos(i * angle);
            vertices[i].z = centre.z + radius * Mathf.Sin(i * angle);
            vertices[i].y += centre.y;
        }

        for (int i = 0; i < vertices_count; i++)
        {
            directions[i] = (vertices[(i + 1) % vertices_count] - vertices[i]).normalized;
        }

        Mesh mesh = new Mesh();
        mesh.name = "Pentagon";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;

    }



    //Calculates vertices for pentagon's rotation
    public void CalculateVerticesWithRotation(Vector3 rotation)
    {

        for (int i = 0; i < vertices_count; i++)
        {
            //Z AXIS ROTATITON
            vertices[i] = new Vector3(vertices[i].x * Mathf.Cos(transform.rotation.z) - vertices[i].y * Mathf.Sin(transform.rotation.z),
                                      vertices[i].x * Mathf.Sin(transform.rotation.z) + vertices[i].y * Mathf.Cos(transform.rotation.z),
                                      vertices[i].z);
            //Y AXIS ROTATION
            vertices[i] = new Vector3(vertices[i].z * Mathf.Sin(transform.rotation.y) + vertices[i].x * Mathf.Cos(transform.rotation.y),
                                      vertices[i].y,
                                      vertices[i].z * Mathf.Cos(transform.rotation.y) - vertices[i].x * Mathf.Sin(transform.rotation.y));
            //X AXIS ROTATION
            vertices[i] = new Vector3(vertices[i].x,
                                      vertices[i].y * Mathf.Cos(transform.rotation.x) - vertices[i].z * Mathf.Sin(transform.rotation.x),
                                      vertices[i].y * Mathf.Sin(transform.rotation.x) + vertices[i].z * Mathf.Cos(transform.rotation.x));


        }

    }

    void OnCollisionEnter(Collision Other)
    {
        if (Other.gameObject.CompareTag("Player"))
        {
            if (transform.name == "Root")
            {
                Other.gameObject.GetComponent<Rigidbody>().AddForce(-transform.up * 100f, ForceMode.VelocityChange);
            }
            else
            {
                if(transform.parent.name == "UpPart")
                {
                    Other.gameObject.GetComponent<Rigidbody>().AddForce(Other.transform.up * 100f, ForceMode.VelocityChange);
                }
                else
                {
                    Other.gameObject.GetComponent<Rigidbody>().AddForce(-Other.transform.up * 100f, ForceMode.VelocityChange);
                }


            }
        }

    }


}
