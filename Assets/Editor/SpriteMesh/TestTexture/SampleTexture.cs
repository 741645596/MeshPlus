#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;


public class SampleTexture
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="countDir"></param>
    /// <param name="depth"></param>
    /// <param name="wing"></param>
    /// <param name="steps"></param>
    /// <returns></returns>
    public static List<TexPoint> Test(float[,] tex,int countDir=16,int depth=4,int wing=8, int steps = -1)
    {
        int width = tex.GetLength(0);
        int height = tex.GetLength(1);

        TestDirs dirs = new TestDirs(countDir);
        Samples samples = new Samples(countDir, depth, wing);
        List<TexPoint> points = new List<TexPoint>(100);
        bool startDone = false;
        float first = tex[0, height / 2];
        TexPoint fromPt = new TexPoint(0, -1, new Vector2Int(0, height / 2), first);
        if (fromPt.grade > 0)
        {
            points.Add(fromPt);
            startDone = true;
        }
        float sqr = (depth) * (depth)/4f;
        TexPoint startPoint = fromPt;
        int maxStep = (width + height) * depth;
        if (steps > 0) maxStep = steps;

        int c = 0;
        int startAt=0;
        bool done = false;
        while (true)
        {
            TexPoint newPoint = NextPoint(tex, dirs, samples, fromPt);
            if (!startDone && newPoint.grade > 0)
            {
                startDone = true;
                startPoint = newPoint;
                c = 0;
            }
            if (startDone)
            {
                // 检查新点是远离已有收集到的点
                for(int i=0;i< points.Count; i++)
                {
                    if(TexPoint.GetDelta(newPoint,points[i]).sqrMagnitude<sqr)
                    {
                        startAt = i;
                        done = true;
                        break;
                    }
                }
                if (done) break;
                points.Add(newPoint);
            }

            fromPt = newPoint;
            c++;
            if (c > maxStep) break;
        }
        List<TexPoint> newPoints = new List<TexPoint>(points.Count);
        for(int i=startAt;i<points.Count;i++)
        {
            newPoints.Add(points[i]);
        }
        points = newPoints;

        bool isClockwise = points[1].pixel.y > points[points.Count - 1].pixel.y;
        if(!isClockwise)
        {
            points.Reverse();

            Debug.Log("Is Reverse:");
        }
        return points;
    }
    /// <summary>
    /// 获取下一个点
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="dirs"></param>
    /// <param name="samples"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static TexPoint NextPoint(float[,] tex, TestDirs dirs, Samples samples, TexPoint point)
    {
        samples.Reset();

        int U0 = point.pixel.x;
        int V0 = point.pixel.y;
        int inverseDir = point.inverse;

        int countDir = dirs.count;
        int depth = samples.depth;
        int wing = samples.wing;

        int width = tex.GetLength(0);
        int height = tex.GetLength(1);

        Vector2 preDir = dirs[point.dir];

        Vector2 dir;
        Vector2 dirX;
        Vector2 depthVec;
        Vector2 valWing;
        SampleDir sampleDir;

        int atU, atV;
        float dot;
        for (int d = 0; d < countDir; d++)
        {
            dir = dirs[d];
            dirX = dirs.perpendicular[d];
            sampleDir = samples[d];
            sampleDir.dir = d;
            for (int dep = 0; dep < depth; dep++)
            {
                depthVec = dir * dep;
                atU = U0 + Mathf.RoundToInt(depthVec.x);
                atV = V0 + Mathf.RoundToInt(depthVec.y);
                valWing = TestWing(tex, dirX, atU, atV, wing);
                sampleDir[dep] = valWing;
                sampleDir.pixel = new Vector2Int(atU, atV);
            }
            dot = Vector2.Dot(dir, preDir) + 1;
            sampleDir.Refresh(dot);
        }
        float grade = -1f;
        int ofDir = point.dir; ;
        for (int s = 0; s < countDir; s++)
        {
            if (s != inverseDir)
            {
                if (samples[s].grade > grade)
                {
                    grade = samples[s].grade;
                    ofDir = s;
                }
            }
        }

        TexPoint newPoint = new TexPoint(ofDir, dirs.inverse[ofDir], samples[ofDir].pixel, grade);
        return newPoint;
    }
    public static Vector2 TestWing(float[,] tex, Vector2 dir, int u0, int v0, int wing)
    {
        int width = tex.GetLength(0);
        int height = tex.GetLength(1);
        Vector2 vec;
        float color;
        float sumA = 0;
        float sumB = 0;
        int atU, atV;
        for (int w = 0; w < wing; w++)
        {
            vec = dir * w;
            atU = u0 + Mathf.RoundToInt(vec.x);
            atV = v0 + Mathf.RoundToInt(vec.y);
            if (atU >= 0 && atU < width && atV < height && atV >= 0)
            {
                color = tex[atU, atV];
                sumA += color;
            }
            atU = u0 - Mathf.RoundToInt(vec.x);
            atV = v0 - Mathf.RoundToInt(vec.y);
            if (atU >= 0 && atU < width && atV < height && atV >= 0)
            {
                color = tex[atU, atV];
                sumB += color;
            }
        }
        return new Vector2(sumA, sumB);
    }
}
#endif