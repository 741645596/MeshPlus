#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PolyPart
{
    public List<int> partA;
    public List<int> partB;
    public PolyPart(List<int> ids, int idxA, int idxB)
    {
        int count = ids.Count;

        partA = new List<int>(count);
        for (int i = 0, c = idxA; i < count; i++, c = (c + 1) % count)
        {
            partA.Add(ids[c]);
            if (c == idxB)
                break;
        }

        partB = new List<int>(count);
        for (int i = 0, c = idxB; i < count; i++, c = (c + 1) % count)
        {
            partB.Add(ids[c]);
            if (c == idxA)
                break;
        }
    }
    public PolyPart(List<Vector2> pts, int idxA, int idxB)
    {
        int count = pts.Count;

        partA = new List<int>(count);
        for (int i = 0, c = idxA; i < count; i++, c = (c - 1 + count) % count)
        {
            partA.Add(c);
            if (c == idxB)
                break;
        }

        partB = new List<int>(count);
        for (int i = 0, c = idxA; i < count; i++, c = (c + 1) % count)
        {
            partB.Add(c);
            if (c == idxB)
                break;
        }
    }
    public void DrawPoly(List<Vector2> points)
    {

        int countA = partA.Count;
        int countB = partB.Count;

        Handles.color = Color.blue;
        for (int i = 0; i < countA; i++)
        {
            Handles.DrawLine(points[partA[i]], points[partA[(i + 1) % countA]]);
        }

        Handles.color = Color.red;
        for (int i = 0; i < countB; i++)
        {
            Handles.DrawLine(points[partB[i]], points[partB[(i + 1) % countB]]);
        }
    }
}
#endif