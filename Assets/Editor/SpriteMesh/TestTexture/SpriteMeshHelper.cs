#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;

public class SpriteMeshHelper
{
    /// <summary>
    /// 生成sprite animation 的mesh
    /// 
    /// </summary>
    /// <param name="tex">sprite animation 纹理</param>
    /// <param name="distVault"></param>
    /// <param name="eulerVault"></param>
    /// <param name="dilate"></param>
    /// <param name="size"></param>
    /// <param name="row">排</param>
    /// <param name="col">列</param>
    /// <param name="countDir">动画的方向几个</param>
    /// <param name="depth"></param>
    /// <param name="wing"></param>
    /// <returns></returns>
    public static Mesh Generate(Texture2D tex, int distVault, int eulerVault, int dilate, float size,int row,int col,int countDir=16,int depth=8,int wing=8)
    {
        // 得到格子数据
        float[,] alphas=  GenerateAlphaTex(tex, row, col);
        int width = alphas.GetLength(0);
        int height = alphas.GetLength(1);

        List<TexPoint> points = SampleTexture.Test(alphas,countDir,depth,wing);

        SimplifySample.Simplify(points, distVault, eulerVault, dilate);

        int maxX = width - 1;
        int maxY = height - 1;
        //int uvX = tex.width - 1;
        //int uvY = tex.height - 1;
        //int offsetY = height * (row - 1);
        int count = points.Count;
        List<Vector2> pts = new List<Vector2>(count);
        List<Vector2> uvs = new List<Vector2>(count);
        for (int i = 0; i < count; i++)
        {
            pts.Add(points[i].Pos);
            uvs.Add(points[i].UV(maxX,maxY));
        }

        PolyPartSet set = MonotonePartition.DoPartition(pts);
        set.Triangulate(pts);

        float sizeY = size * height / width;
        Vector3[] vertices = new Vector3[count];
        Vector3[] normals = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            vertices[i] = new Vector3((pts[i].x/maxX -0.5f) * size, (pts[i].y/maxY - 0.5f) * size, 0);
            normals[i] = Vector3.forward;
        }

        Mesh mesh = new Mesh();

        mesh.triangles = null;
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs.ToArray();
        mesh.triangles = set.tris.ToArray();

        return mesh;
    }
    /// <summary>
    /// 算法，把纹理分成row * col 块，
    /// 取相同位置的像素alpha值，全都小于阀值为0，否则为1
    /// </summary>
    /// <param name="tex">动画纹理</param>
    /// <param name="row">排</param>
    /// <param name="col">列</param>
    /// <returns></returns>
    public static float[,] GetAlphas(Texture2D tex, int row, int col)
    {
        int texWidth = tex.width;
        int texHeight = tex.height;
        int tileWitdh = texWidth / col;
        int tileHeight = texHeight / row;
        float vault = 0.1f;
        float[,] alphas = new float[tileWitdh, tileHeight];
        int idxW, idxH;
        for (int w = 0; w < texWidth; w++)
        {
            idxW = w % tileWitdh;
            for (int h = 0; h < texHeight; h++)
            {
                idxH = h % tileHeight;
                if (tex.GetPixel(w, h).a - vault > 0)
                    alphas[idxW, idxH] = 1f;
            }
        }
        return alphas;
    }
    /// <summary>
    /// 按分块获取纹理的alpha 通道，
    /// 判断所有分块对应位置alpha 值，有大于指定阈值范围1
    /// 返回分块后alpha 进行叠加后通透情况。
    /// 用于后续得到一个最大的mesh
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="row">排</param>
    /// <param name="col">列</param>
    /// <returns></returns>
    public static float[,] GenerateAlphaTex(Texture2D tex, int row, int col)
    {
        float[,] alphas = GetAlphas(tex, row, col);
        return alphas;
    }
}
#endif