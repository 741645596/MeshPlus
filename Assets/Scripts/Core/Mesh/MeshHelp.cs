using UnityEngine;
using RayGraphics.Math;
using RayGraphics.Geometric;
public partial class MeshHelp 
{
    /// <summary>
    /// 创建挡格mesh
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static Mesh Create(Circle2D circle)
    {
        Vector3 center = new Vector3((float)circle.Center.x, 0.1f, (float)circle.Center.y);
        float radius = (float)circle.radius;
        return CreateCircleMesh(center, radius);
    }
    /// <summary>
    /// 创建圆形mesh
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    private static Mesh CreateCircleMesh(Vector3 center, float radius)
    {
        Mesh mesh = new Mesh();
        float angle = Mathf.Deg2Rad * 1.0f;
        Double2 dir = Double2.right * radius;
        Vector3[] vertices = new Vector3[361];
        vertices[360] = center;
        for (int i = 0; i < 360; i++)
        {
            Double2 newPos = Double2.Rotate(dir, i * angle);
            vertices[i] = center + new Vector3((float)newPos.x, 0, (float)newPos.y);
        }

        int[] triangles = new int[360 * 3];
        for (int i = 0; i < 360; i++)
        {
            int index = i * 3;
            triangles[index] = 360;
            triangles[index + 1] = i;
            triangles[index + 2] = (i + 1) > 359 ? 0: (i + 1);
        }

        Vector2[] uvs = new Vector2[361];
        for (int i = 0; i < 361; i++)
        {
            Vector3 diff = (vertices[i] - center) / radius * 0.5f;
            uvs[i] = new Vector2(diff.x + 0.5f, diff.z + 0.5f);
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        return mesh;
    }
}
