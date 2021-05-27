#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;


public class SimplifySample
{
    /// <summary>
    /// 调整顶点列表，并进行适当扩展
    /// </summary>
    /// <param name="points"></param>
    /// <param name="dist"></param>
    /// <param name="euler"></param>
    /// <param name="dilate"></param>
    public static void Simplify(List<TexPoint> points, float dist, float euler, float dilate)
    {
        bool dirty = true;
        while (dirty)
        {
            dirty = DoSimplfy(points, dist);
        }

        dirty = true;
        while (dirty)
        {
            dirty = DoAngle(points, euler);
        }
        DoDilate(points, dilate);

        dirty = true;
        while (dirty)
        {
            dirty = DoAngle(points, euler);
        }
        //DoBorder(points, width, height);
    }
    /// <summary>
    /// 调整顶点列表顺序，保证逆时针
    /// </summary>
    /// <param name="points"></param>
    /// <param name="vault"></param>
    /// <returns></returns>
    private static bool DoSimplfy(List<TexPoint> points, float vault)
    {
        int count = points.Count;
        float sqr = vault * vault;
        List<int> deltas = new List<int>(count);

        TexPoint tp_1, tp1, tp2;
        Vector2Int p_1, p1, p2;
        Vector2Int vec2_1;
        Vector2 vec;
        Vector2 dir2_1;
        Vector2 dir;

        Vector2Int vec1_1, vec21;

        for (int i = 0; i < count; i++)
        {
            tp_1 = points[(i - 1 + count) % count];
            tp1 = points[i];
            tp2 = points[(i + 1) % count];
            p_1 = tp_1.pixel;
            p1 = tp1.pixel;
            p2 = tp2.pixel;

            vec1_1 = p1 - p_1;
            vec21 = p2 - p1;
            vec2_1 = p2 - p_1;
            if (vec1_1.sqrMagnitude < sqr || vec21.sqrMagnitude < sqr)
            {
                dir2_1 = new Vector2(-vec2_1.y, vec2_1.x);
                vec = p1 * 2 - (p_1 + p2);
                dir = new Vector2(vec.x, vec.y) / 2f;
                if (Vector2.Dot(dir, dir2_1) <= 0)
                {
                    deltas.Add(i);
                }
            }
        }
        bool dirty = deltas.Count > 0;
        int atIdx;
        int acc = 0;
        for (int d = 0; d < deltas.Count; d++)
        {
            atIdx = deltas[d];
            atIdx = atIdx - acc;
            points.RemoveAt(atIdx);
            acc += 1;
        }
        return dirty;
    }
    /// <summary>
    /// 调整顶点顺序，保证夹角
    /// </summary>
    /// <param name="points"></param>
    /// <param name="euler"></param>
    /// <returns></returns>
    private static bool DoAngle(List<TexPoint> points, float euler)
    {
        float dotVault = Mathf.Cos(euler * Mathf.Deg2Rad);
        int count = points.Count;

        //顺序 添加{0-Count}
        List<int> deltas = new List<int>(count);

        TexPoint tp_1, tp1, tp2;
        Vector2Int p_1, p1, p2;

        Vector2Int vec2_1;
        Vector2Int vec1_1, vec21;

        Vector2 dir2_1;
        Vector2 dir1_1, dir21;
        Vector2 vec;
        Vector2 dir;

        for (int i = 0; i < count; i++)
        {
            tp_1 = points[(i - 1 + count) % count];
            tp1 = points[i];
            tp2 = points[(i + 1) % count];
            p_1 = tp_1.pixel;
            p1 = tp1.pixel;
            p2 = tp2.pixel;

            vec1_1 = p1 - p_1;
            vec21 = p2 - p1;
            vec2_1 = p2 - p_1;
            dir1_1 = new Vector2(vec1_1.x, vec1_1.y).normalized;
            dir21 = new Vector2(vec21.x, vec21.y).normalized;

            if (Vector2.Dot(dir21, dir1_1) > dotVault)
            {
                dir2_1 = new Vector2(-vec2_1.y, vec2_1.x);
                vec = p1 * 2 - (p_1 + p2);
                dir = new Vector2(vec.x, vec.y) / 2f;
                if (Vector2.Dot(dir, dir2_1) <= 0)
                {
                    deltas.Add(i);
                }
            }
        }

        bool dirty = deltas.Count > 0;
        int atIdx;
        int acc = 0;
        for (int d = 0; d < deltas.Count; d++)
        {
            atIdx = deltas[d];
            atIdx = atIdx - acc;
            points.RemoveAt(atIdx);
            acc += 1;
        }
        return dirty;
    }
    /// <summary>
    /// 进行扩张
    /// </summary>
    /// <param name="points"></param>
    /// <param name="dilate"></param>
    private static void DoDilate(List<TexPoint> points, float dilate)
    {
        int count = points.Count;

        TexPoint tp_1, tp1, tp2;
        Vector2Int p_1, p1, p2;

        Vector2Int vec2_1;
        Vector2Int vec1_1, vec12;

        Vector2 dir2_1;
        Vector2 dir1_1, dir12;
        Vector2 vec;
        Vector2 dir;
        Vector2 dirA;

        Vector2 expend;
        Vector2Int offset;

        for (int i = 0; i < count; i++)
        {
            tp_1 = points[(i - 1 + count) % count];
            tp1 = points[i];
            tp2 = points[(i + 1) % count];
            p_1 = tp_1.pixel;
            p1 = tp1.pixel;
            p2 = tp2.pixel;

            vec1_1 = p1 - p_1;
            vec12 = p1 - p2;
            vec2_1 = p2 - p_1;
            dir1_1 = new Vector2(vec1_1.x, vec1_1.y).normalized;
            dir12 = new Vector2(vec12.x, vec12.y).normalized;
            dirA = (dir1_1 + dir12).normalized;

            dir2_1 = new Vector2(-vec2_1.y, vec2_1.x);
            vec = p1 * 2 - (p_1 + p2);
            dir = new Vector2(vec.x, vec.y) / 2f;
            if (Vector2.Dot(dir, dir2_1) < 0)
            {
                expend = -dirA * dilate;
            }
            else
            {
                expend = dirA * dilate;
            }
            offset = new Vector2Int((int)expend.x, (int)expend.y);
            tp1.pixel = tp1.pixel + offset;
        }
    }
    private static void DoBorder(List<TexPoint> points, int width, int height)
    {
        int count = points.Count;
        int maxX = width - 1;
        int maxY = height - 1;

        int first = -1;
        for (int i = 0; i < count; i++)
        {
            if (!points[i].IsDirty(maxX, maxY))
            {
                first = i;
                break;
            }
        }
        if (first == -1)
        {
            TexPoint quadA = new TexPoint(0, 0, new Vector2Int(0, 0), 0);
            TexPoint quadB = new TexPoint(0, 0, new Vector2Int(0, maxY), 0);
            TexPoint quadC = new TexPoint(0, 0, new Vector2Int(maxX, maxY), 0);
            TexPoint quadD = new TexPoint(0, 0, new Vector2Int(maxX, 0), 0);
            points.Clear();
            points.Add(quadA);
            points.Add(quadB);
            points.Add(quadC);
            points.Add(quadD);

            return;
        }
        for(int i=0;i<count;i++)
        {
            points[i].Clamp(maxX, maxY);
        }

    }
}
#endif