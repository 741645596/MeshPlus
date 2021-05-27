#if UNITY_EDITOR
using System;
using UnityEngine;

[Serializable]
public class TexPoint
{
    public int dir;
    public int inverse;

    public Vector2Int pixel;
    public float grade;
    /// <summary>
    /// 位置
    /// </summary>
    public Vector2 Pos
    {
        get { return new Vector2(pixel.x, pixel.y); }
    }
    /// <summary>
    /// uv
    /// </summary>
    /// <param name="maxX"></param>
    /// <param name="maxY"></param>
    /// <returns></returns>
    public Vector2 UV(int maxX, int maxY)
    {
        return new Vector2((float)pixel.x / maxX, (float)(pixel.y) / maxY);
    }
    public TexPoint Clone
    {
        get { return new TexPoint(dir, inverse, pixel, grade); }
    }

    public TexPoint(int dir,int inverse,Vector2Int pixel,float grade)
    {
        this.dir = dir;
        this.inverse = inverse;
        this.pixel = pixel;
        this.grade = grade;
    }
    public bool IsDirty(int maxX,int maxY)
    {
        if (pixel.x < 0 || pixel.x > maxX||pixel.y<0||pixel.y>maxX) return true;
        return false;
    }
    public void Clamp(int maxX,int maxY)
    {
        pixel.x = Mathf.Clamp(pixel.x, 0, maxX);
        pixel.y = Mathf.Clamp(pixel.y, 0, maxY);
    }
    public void Offset(Vector2 delta)
    {
        pixel.x = pixel.x + (int)delta.x;
        pixel.y = pixel.y + (int)delta.y;
    }
    /// <summary>
    /// 计算diff
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static Vector2 GetDelta(TexPoint p1,TexPoint p2)
    {
        return new Vector2(p1.pixel.x - p2.pixel.x, p1.pixel.y - p2.pixel.y);
    }
}
#endif