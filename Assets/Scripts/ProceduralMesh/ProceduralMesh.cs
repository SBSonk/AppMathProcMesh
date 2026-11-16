using System.Collections.Generic;
using UnityEngine;

public abstract class ProceduralMesh : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Material lineMat;
    [SerializeField] private Vector3 position = Vector3.zero;
    [SerializeField] private float scale = 1f;
    [SerializeField, Range(0f, 360f)] private float yRotation = 0f;

    protected Vector3[] verts;
    protected int[][] faces;

    private Mesh mesh;
    private readonly List<Vector3> actualPosition = new();
    private readonly List<int> visibleEdges = new();

    protected virtual void Awake()
    {
        mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = lineMat;

        BuildMesh();
    }

    protected virtual void LateUpdate()
    {
        if (verts == null || faces == null) return;

        actualPosition.Clear();
        foreach (var point in verts)
        {
            actualPosition.Add(Project(point));
        }

        mesh.Clear();
        mesh.vertices = actualPosition.ToArray();
        visibleEdges.Clear();

        foreach (var f in faces)
        {
            if (!FacesCamera(actualPosition[f[0]], actualPosition[f[1]], actualPosition[f[2]])) continue;

            int edgeCount = f.Length;
            for (int i = 0; i < edgeCount; i++)
            {
                visibleEdges.Add(f[i]);
                visibleEdges.Add(f[(i + 1) % edgeCount]);
            }
        }

        mesh.SetIndices(visibleEdges, MeshTopology.Lines, 0);
        mesh.RecalculateBounds();
    }

    protected abstract void BuildMesh();

    private Vector2 Project(Vector3 point)
    {
        point *= 0.5f;

        // rotate around Y axis
        float rad = yRotation * Mathf.Deg2Rad;
        float xRot = point.x * Mathf.Cos(rad) - point.z * Mathf.Sin(rad);
        float zRot = point.x * Mathf.Sin(rad) + point.z * Mathf.Cos(rad);
        point = new Vector3(xRot, point.y, zRot);

        point += position;

        float focalLength = PerspectiveCamera.Instance != null
            ? PerspectiveCamera.Instance.FocalLength
            : 20f;

        // Perspective projection
        float p = focalLength / (focalLength + point.z);

        return new Vector2(point.x, point.y) * scale * p;
    }

    private bool FacesCamera(Vector2 a, Vector2 b, Vector2 c)
    {
        return (b - a).x * (c - a).y - (b - a).y * (c - a).x > 0;
    }
}