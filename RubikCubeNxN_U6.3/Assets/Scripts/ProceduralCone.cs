using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCone : MonoBehaviour
{
    [Range(3, 64)]
    public int sides = 32;

    public float radius = 0.1f;
    public float height = 0.2f;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = CreateCone(radius, height, sides);
    }

    public static Mesh CreateCone(float radius, float height, int sides)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Cone";

        List<Vector3> vertices = new();
        List<int> triangles = new();
        List<Vector3> normals = new();
        List<Vector2> uvs = new();

        //----------------------------------------------------
        // Side vertices
        //----------------------------------------------------

        Vector3 tip = new Vector3(0, height, 0);

        for (int i = 0; i < sides; i++)
        {
            float angle = Mathf.PI * 2f * i / sides;

            vertices.Add(new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius));
        }

        int tipIndex = vertices.Count;
        vertices.Add(tip);

        //----------------------------------------------------
        // Side triangles
        //----------------------------------------------------

        for (int i = 0; i < sides; i++)
        {
            int next = (i + 1) % sides;

            triangles.Add(i);
            triangles.Add(next);
            triangles.Add(tipIndex);
        }

        //----------------------------------------------------
        // Bottom center
        //----------------------------------------------------

        int centerIndex = vertices.Count;
        vertices.Add(Vector3.zero);

        for (int i = 0; i < sides; i++)
        {
            int next = (i + 1) % sides;

            triangles.Add(centerIndex);
            triangles.Add(next);
            triangles.Add(i);
        }

        //----------------------------------------------------
        // UV + Normals
        //----------------------------------------------------

        for (int i = 0; i < vertices.Count; i++)
        {
            normals.Add(Vector3.up);
            uvs.Add(new Vector2(vertices[i].x, vertices[i].z));
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        return mesh;
    }
}