using System.Collections.Generic;
using UnityEngine;

public class SphereMesh : ProceduralMesh
{
    [Header("Sphere Settings")]
    [Range(2, 64)] [SerializeField] private int rings = 12;
    [Range(3, 64)] [SerializeField] private int segments = 24;

    private int lastRings;
    private int lastSegments;

    protected override void LateUpdate()
    {
        if (rings != lastRings || segments != lastSegments)
        {
            BuildMesh();
        }

        base.LateUpdate();
    }

    protected override void BuildMesh()
    {
        int ringCount = Mathf.Max(2, rings);
        int segmentCount = Mathf.Max(3, segments);

        lastRings = rings;
        lastSegments = segments;

        var vertList = new List<Vector3>();
        var faceList = new List<int[]>();

        // Top pole (index 0)
        vertList.Add(new Vector3(0f, 1f, 0f));

        // Intermediate latitude rings
        for (int r = 1; r < ringCount; r++)
        {
            float phi = Mathf.PI * r / ringCount;
            float y = Mathf.Cos(phi);
            float ringRadius = Mathf.Sin(phi);

            for (int s = 0; s < segmentCount; s++)
            {
                float theta = 2f * Mathf.PI * s / segmentCount;
                float x = ringRadius * Mathf.Sin(theta);
                float z = -ringRadius * Mathf.Cos(theta);
                vertList.Add(new Vector3(x, y, z));
            }
        }

        // Bottom pole
        int bottomPoleIndex = vertList.Count;
        vertList.Add(new Vector3(0f, -1f, 0f));

        int RingStart(int r) => 1 + (r - 1) * segmentCount;

        // Top cap triangles
        int firstRingStart = RingStart(1);
        for (int s = 0; s < segmentCount; s++)
        {
            int nextS = (s + 1) % segmentCount;
            faceList.Add(new[] { 0, firstRingStart + s, firstRingStart + nextS });
        }

        // Middle quads
        for (int r = 1; r < ringCount - 1; r++)
        {
            int upperStart = RingStart(r);
            int lowerStart = RingStart(r + 1);

            for (int s = 0; s < segmentCount; s++)
            {
                int nextS = (s + 1) % segmentCount;
                faceList.Add(new[]
                {
                    lowerStart + s,
                    lowerStart + nextS,
                    upperStart + nextS,
                    upperStart + s
                });
            }
        }

        // Bottom cap triangles
        int lastRingStart = RingStart(ringCount - 1);
        for (int s = 0; s < segmentCount; s++)
        {
            int nextS = (s + 1) % segmentCount;
            faceList.Add(new[] { bottomPoleIndex, lastRingStart + nextS, lastRingStart + s });
        }

        verts = vertList.ToArray();
        faces = faceList.ToArray();
    }
}