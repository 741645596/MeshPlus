#if UNITY_EDITOR
using UnityEngine;

/// <summary>
/// 采样器
/// </summary>
public class Samples
{
    /// <summary>
    /// 采样器方向数组
    /// </summary>
    public SampleDir[] sampleDirs;
    /// <summary>
    /// 采样方向总数
    /// </summary>
    public int countDir;
    /// <summary>
    /// 采样深度
    /// </summary>
    public int depth;
    public int wing;
    public SampleDir this[int index]
    {
        get
        {
            return sampleDirs[index];
        }
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="countDir">方向</param>
    /// <param name="depth">深度</param>
    /// <param name="wing"></param>
    public Samples(int countDir, int depth, int wing)
    {
        this.countDir = countDir;
        this.depth = depth;
        this.wing = wing;

        sampleDirs = new SampleDir[countDir];
        for (int i = 0; i < countDir; i++)
        {
            sampleDirs[i] = new SampleDir(depth, wing);
        }
    }
    /// <summary>
    /// 还原
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < countDir; i++)
        {
            sampleDirs[i].Reset();
        }
    }
}
/// <summary>
/// 方向采样器
/// </summary>
public class SampleDir
{
    public int dir;
    public Vector2Int pixel;
    public Vector2[] wingSamples;
    public Vector2 wingAcc;
    public float grade;

    public int depth;
    public int wing;

    public Vector2 this[int indexDepth]
    {
        get { return wingSamples[indexDepth]; }
        set { wingSamples[indexDepth] = value; }
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="wing"></param>
    public SampleDir(int depth, int wing)
    {
        this.depth = depth;
        this.wing = wing;

        wingSamples = new Vector2[depth];
    }
    public void Refresh(float dot)
    {
        wingAcc = Vector2.zero;
        for (int i = 0; i < depth; i++)
        {
            wingAcc += wingSamples[i];
        }
        grade = Mathf.Abs(wingAcc.x - wingAcc.y)*dot;
    }
    public void Reset()
    {
        wingAcc = Vector2.zero;
        for (int i = 0; i < depth; i++)
        {
            wingSamples[i] = Vector2.zero;
        }
    }
}
#endif