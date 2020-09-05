using System.Collections.Generic;
using UnityEngine;

public partial class MeshHelp
{
    public static Mesh CreateLine(List<Vector3> lv, float Width, bool IsPull = false)
    {
        if (lv == null || lv.Count < 2)
            return null;
        Mesh mesh = new Mesh();

        int vlen = lv.Count;
        Vector3[] vertices = new Vector3[vlen * 2];
        Vector2[] uvs = new Vector2[vlen * 2];

        float dis = 0;

        for (int i = 0; i < vlen; i++)
        {
            if (i == 0)
            {
                dis = 0;
                Vector3 dir = lv[1] - lv[0];
                vertices[i] = CalcLinePoint(lv[0], dir, -Width, IsPull);
                uvs[i] = new Vector2(0.25f, dis);
                vertices[i + vlen] = CalcLinePoint(lv[0], dir, Width, IsPull);
                uvs[i + vlen] = new Vector2(0.75f, dis);
            }
            else if (i == vlen - 1)
            {
                Vector3 dir = lv[i] - lv[i - 1];
                dis += Vector3.Distance(lv[i], lv[i - 1]) / (2 * Width);
                vertices[i] = CalcLinePoint(lv[i], dir, -Width, IsPull);
                uvs[i] = new Vector2(0.25f, dis);
                vertices[i + vlen] = CalcLinePoint(lv[i], dir, Width, IsPull);
                uvs[i + vlen] = new Vector2(0.75f, dis);
            }
            else
            {
                // 采取均值
                Vector3 dir = (lv[i + 1] - lv[i]).normalized + (lv[i] - lv[i - 1]).normalized;
                dis += Vector3.Distance(lv[i], lv[i - 1]) / (2 * Width);
                vertices[i] = CalcLinePoint(lv[i], dir, -Width, IsPull);
                uvs[i] = new Vector2(0.25f, dis);
                vertices[i + vlen] = CalcLinePoint(lv[i], dir, Width, IsPull);
                uvs[i + vlen] = new Vector2(0.75f, dis);
            }
        }

        int[] triangles = new int[(vlen - 1) * 6];
        for (int i = 0, vi = 0; vi < vlen - 1; i += 6, vi++)
        {
            triangles[i] = vi;
            triangles[i + 1] = vi + vlen;
            triangles[i + 2] = vi + 1;

            triangles[i + 3] = vi + vlen;
            triangles[i + 4] = vi + 1 + vlen;
            triangles[i + 5] = vi + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }

    /// <summary>
    /// 计算线上的另外一个点
    /// </summary>
    /// <param name="start"></param>
    /// <param name="dir"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    private  static Vector3 CalcLinePoint(Vector3 start, Vector3 dir, float width, bool IsPull)
    {
        Vector3 normal = Vector3.Cross(dir, new Vector3(0, 1, 0)).normalized;
        if (IsPull == false)
        {
            return start + normal * width;
        }
        else
        {
            return start + normal * width + new Vector3(0, 0.1f, 0);
        }
    }
}
