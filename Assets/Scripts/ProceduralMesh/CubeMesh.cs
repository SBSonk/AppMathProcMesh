using UnityEngine;

public class CubeMesh : ProceduralMesh
{
    protected override void BuildMesh()
    {
        verts = new Vector3[]
        {
            new(-1, -1, -1), new( 1, -1, -1), // 0, 1
            new( 1,  1, -1), new(-1,  1, -1), // 2, 3
            new(-1, -1,  1), new( 1, -1,  1), // 4, 5
            new( 1,  1,  1), new(-1,  1,  1)  // 6, 7
        };

        faces = new[]
        {
            new[] { 0, 1, 2, 3 }, // front
            new[] { 5, 4, 7, 6 }, // back
            new[] { 4, 0, 3, 7 }, // left
            new[] { 1, 5, 6, 2 }, // right
            new[] { 3, 2, 6, 7 }, // top
            new[] { 0, 4, 5, 1 }  // bottom
        };
    }
}