using UnityEngine;

public class PyramidMesh : ProceduralMesh
{
    protected override void BuildMesh()
    {
        verts = new Vector3[]
        {
            new(-1, -1, -1), // 0: front-left base
            new( 1, -1, -1), // 1: front-right base
            new( 1, -1,  1), // 2: back-right base
            new(-1, -1,  1), // 3: back-left base
            new( 0,  1,  0)  // 4: apex
        };

        faces = new[]
        {
            new[] { 0, 1, 4 },    // front triangle
            new[] { 1, 2, 4 },    // right triangle
            new[] { 2, 3, 4 },    // back triangle
            new[] { 3, 0, 4 },    // left triangle
            new[] { 0, 3, 2, 1 }  // bottom quad
        };
    }
}