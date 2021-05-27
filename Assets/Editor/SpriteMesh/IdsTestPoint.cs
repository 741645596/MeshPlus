#if UNITY_EDITOR
using UnityEngine;

[System.Serializable]
public class IdsTestPoint : System.IComparable
{
    public Vector2 pt;
    public int idx;
    public float dist;

    public IdsTestPoint(Vector2 pt, int idx, float dist)
    {
        this.pt = pt;
        this.idx = idx;
        this.dist = dist;
    }

    public int CompareTo(object obj)
    {
        IdsTestPoint b = obj as IdsTestPoint;
        if (this.dist > b.dist) return 1;
        return -1;
    }
}
#endif