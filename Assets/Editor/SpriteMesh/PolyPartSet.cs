#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PolyPartSet
{
    public List<List<int>> toTests;
    public List<List<int>> monotones;
    public List<int> tris;

    public PolyPartSet()
    {
        monotones = new List<List<int>>(10);
        toTests = new List<List<int>>(10);
    }
    public void AddTestPoly(List<int> ids)
    {
        toTests.Add(ids);
    }
    public void AddMonotone(List<int> ids)
    {
        if(ids.Count==0)
        {
            Debug.Log("<Color=red> 多边形检测异常</Color>");
            return;
        }
        monotones.Add(ids);
    }
    public List<int> Next()
    {
        if(toTests.Count>0)
        {
            List<int> poly = toTests[0];
            toTests.RemoveAt(0);
            return poly;
        }
        return null;
    }
    public void Triangulate(List<Vector2> points)
    {
        tris = new List<int>(1000);
        List<int> ids;
        for(int i=0;i<monotones.Count;i++)
        {
            ids = monotones[i];
            float max = float.MinValue;
            int apex = -1;
            for(int d=0;d<ids.Count;d++)
            {
                if(points[ids[d]].y>max)
                {
                    max = points[ids[d]].y;
                    apex = d;
                }
            }
            Triangulate(points, ids, apex);
        }
    }
    public void Triangulate(List<Vector2> points,List<int> ids,int apexIdx)
    {
        List<int> tris = new List<int>(30);
        int len = ids.Count;
        int rightIdx = (apexIdx + 1) % len;
        int leftIdx = (apexIdx - 1 + len) % len;

        int ptApex = ids[apexIdx];
        int ptRight = ids[rightIdx];
        int ptLeft = ids[leftIdx];

        Vector2 dir, mid;
        while (true)
        {
            tris.Add(ptApex);
            if (points[ptRight].y > points[ptLeft].y)
            {
                int rightIdx_ = (rightIdx + 1) % len;
                int ptRight_ = ids[rightIdx_];
                dir = points[ptRight_] - points[ptRight];
                dir = new Vector2(-dir.y, dir.x);
                mid = (points[ptRight_] + points[ptRight]) / 2f;
                if (points[ptRight_].y > points[ptLeft].y && Vector2.Dot(dir, points[ptApex] - mid) < 0)
                {
                    tris.Add(ptRight);
                    tris.Add(ptRight_);
                }
                else
                {
                    tris.Add(ptRight);
                    tris.Add(ptLeft);
                    ptApex = ptRight;
                }
                rightIdx = (rightIdx + 1) % len;
                ptRight = ids[rightIdx];
            }
            else
            {
                int leftIdx_ = (leftIdx - 1 + len) % len;
                int ptLeft_ = ids[leftIdx_];
                dir = points[ptLeft] - points[ptLeft_];
                dir = new Vector2(-dir.y, dir.x);
                mid = (points[ptLeft] + points[ptLeft_]) / 2f;
                if (points[ptLeft_].y > points[ptRight].y && Vector2.Dot(dir, points[ptApex] - mid) < 0)
                {
                    tris.Add(ptLeft_);
                    tris.Add(ptLeft);
                }
                else
                {
                    tris.Add(ptRight);
                    tris.Add(ptLeft);
                    ptApex = ptLeft;
                }
                leftIdx = (leftIdx - 1 + len) % len;
                ptLeft = ids[leftIdx];
            }
            if (ptLeft == ptRight)
            {
                break;
            }
        }
        this.tris.AddRange(tris);
    }
    public void DrawPloy(List<Vector2> pts,Color[] tints,float space)
    {
        
        int countPoly = monotones.Count;
        int countTint = tints.Length;

        List<int> ids;
        int idxA, idxB, idxC, idxD;
        Vector2 dirA,dirB;
        Vector2 p1, p2, p3, p4;
        Vector2 sum = Vector2.zero;
        for(int p=0,c=0;p<countPoly;p++,c=(c+1)%countTint)
        {
            ids = monotones[p];
            Handles.color = tints[c];
            int count = ids.Count;

            sum = Vector2.zero;
            for(int k=0;k<count;k++)
            {
                sum += pts[ids[k]];

                idxA = ids[(k - 1 + count) % count];
                idxB = ids[k];
                idxC = ids[(k + 1) % count];
                idxD = ids[(k + 2) % count];
                p1 = pts[idxA];
                p2 = pts[idxB];
                p3 = pts[idxC];
                p4 = pts[idxD];

                dirA = p3 - p1;
                dirB = p4 - p2;
                dirA = new Vector2(dirA.y, -dirA.x);
                dirB = new Vector2(dirB.y, -dirB.x);
                p2 = p2 + dirA.normalized * space;
                p3 = p3 + dirB.normalized * space;

                Handles.DrawLine(p2, p3);
            }
            sum = sum / count;
            
            Handles.Label(new Vector3(sum.x, sum.y, 0), "poly:" + p);
        }
    }
    public void DrawMesh(List<Vector2> pts,float space)
    {
        space = -space;
        int countTri = tris.Count;
        Vector2 p1, p2, p3, pCenter;

        for(int i=0;i<countTri/3;i++)
        {
            p1 = pts[tris[i*3]];
            p2 = pts[tris[i*3 + 1]];
            p3 = pts[tris[i*3 + 2]];

            pCenter = (p1 + p2 + p3) / 3f;
            p1 = p1 + (p1 - pCenter).normalized * space;
            p2 = p2 + (p2 - pCenter).normalized * space;
            p3 = p3 + (p3 - pCenter).normalized * space;

            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);
            Handles.DrawLine(p3, p1);


            Handles.Label(new Vector3(pCenter.x,pCenter.y,0), "" + i);
        }
    }
}
#endif
