#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

public class MonotonePartition
{
    public static PolyPartSet DoPartition(List<Vector2> pts)
    {
        PolyPartSet partSet = new PolyPartSet();
        int countP = pts.Count;

        List<int> ids = new List<int>(countP);
        for(int i=0;i<countP;i++)
        {
            ids.Add(i);
        }
        partSet.AddTestPoly(ids);

        while(true)
        {
            List<int> testPoly = partSet.Next();
            if(testPoly==null)
            {
                break;
            }
            else
            {
                PolyPart part = Partition(pts, testPoly);
                if(part==null)
                {
                    partSet.AddMonotone(testPoly);
                }
                else
                {
                    partSet.AddTestPoly(part.partA);
                    partSet.AddTestPoly(part.partB);
                }
            }
        }
        return partSet;
    }
    public static PolyPart Partition(List<Vector2> pts, List<int> ids)
    {
        int countPt = pts.Count;
        int countId = ids.Count;

        int idxPa;
        int idxPb;
        int idxPc;

        Vector2 pa, pb, pc;
        Vector2 dirCA;
        Vector2 penCA;
        Vector2 vecB;
        for (int i = 0; i < countId; i++)
        {
            idxPa = ids[i];
            idxPb = ids[(i + 1) % countId] % countPt;
            idxPc = ids[(i + 2) % countId] % countPt;

            pa = pts[idxPa];
            pb = pts[idxPb];
            pc = pts[idxPc];

            if (pa.y > pb.y && pc.y > pb.y)
            {
                dirCA = pc - pa;
                penCA = new Vector2(-dirCA.y, dirCA.x);
                vecB = pb - (pa + pc) / 2f;

                if (Vector2.Dot(penCA, vecB) < 0)
                {
                    Vector2 tmp;
                    List<IdsTestPoint> tests = new List<IdsTestPoint>(countId);
                    IdsTestPoint testPt;
                    for (int b = 0; b < countId; b++)
                    {
                        tmp = pts[ids[b]];
                        if (tmp.y < pb.y)
                        {
                            testPt = new IdsTestPoint(tmp, b, (tmp - pb).sqrMagnitude);
                            tests.Add(testPt);
                        }
                    }
                    int idxDiagonal = TestDiagonal(pts, ids, tests, pb);
                    PolyPart partNew = new PolyPart(ids, (i + 1) % countId, idxDiagonal);
                    return partNew;
                }
            }
            if (pa.y < pb.y && pc.y < pb.y)
            {
                dirCA = pc - pa;
                penCA = new Vector2(-dirCA.y, dirCA.x);
                vecB = pb - (pa + pc) / 2f;

                if (Vector2.Dot(penCA, vecB) < 0)
                {
                    Vector2 tmp;
                    List<IdsTestPoint> tests = new List<IdsTestPoint>(countId);
                    IdsTestPoint testPt;
                    for (int b = 0; b < countId; b++)
                    {
                        tmp = pts[ids[b]];
                        if (tmp.y > pb.y)
                        {
                            testPt = new IdsTestPoint(tmp, b, (tmp - pb).sqrMagnitude);
                            tests.Add(testPt);
                        }
                    }
                    int idxDiagonal = TestDiagonal(pts, ids, tests, pb);

                    PolyPart partNew = new PolyPart(ids, (i + 1) % countId, idxDiagonal);
                    return partNew;
                }
            }
        }
        return null;
    }
    static int TestDiagonal(List<Vector2> pts, List<int> ids, List<IdsTestPoint> tests, Vector2 pt)
    {
        tests.Sort();

        int countP = pts.Count;
        int countId = ids.Count;
        int countT = tests.Count;

        Vector2 pa, pb, pc;
        Vector2 dirBA;
        Vector2 dirCB;
        Vector2 penBA;
        Vector2 penCB;
        Vector2 vecTest1;
        Vector2 vecTest2;

        int idxPa;
        int idxPb;
        int idxPc;
        int id;
        for (int i = 0; i < countT; i++)
        {
            id = tests[i].idx;
            idxPb = ids[id];
            idxPa = ids[(id - 1 + countId) % countId] % countP;
            idxPc = ids[(id + 1) % countId] % countP;
            //
            pa = pts[idxPa];
            pb = pts[idxPb];
            pc = pts[idxPc];

            dirBA = pb - pa;
            dirCB = pc - pb;
            penBA = new Vector2(-dirBA.y, dirBA.x);
            penCB = new Vector2(-dirCB.y, dirCB.x);

            vecTest1 = pt - (pa + pb) / 2f;
            vecTest2 = pt - (pc + pb) / 2f;
            if (Vector2.Dot(penBA, vecTest1) < 0 && Vector2.Dot(penCB, vecTest2) < 0)
            {
                return id;
            }
        }
        return -1;
    }
}
#endif
