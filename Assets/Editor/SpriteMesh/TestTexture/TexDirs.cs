#if UNITY_EDITOR
using UnityEngine;

public class TestDirs
{
    public Vector2[] dirs;
    public Vector2[] perpendicular;
    public int count;
    public int[] inverse;

    public Vector2 this[int index]
    {
        get
        {
            return dirs[index];
        }
    }
    public TestDirs(int countDir)
    {
        this.count = countDir;

        dirs = new Vector2[countDir];
        perpendicular = new Vector2[countDir];
        inverse = new int[countDir];
        float ang;
        float sin, cos;
        for (int d = 0; d < countDir; d++)
        {
            ang = Mathf.PI * 2f / countDir * d;
            cos = Mathf.Cos(ang);
            sin = Mathf.Sin(ang);
            dirs[d] = new Vector2(cos, sin);
            perpendicular[d] = new Vector2(sin, -cos);

            inverse[d] = (d + countDir / 2) % countDir;
        }
    }
}
#endif