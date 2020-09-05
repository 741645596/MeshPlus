using UnityEngine;

namespace RayGraphics.Render
{
    public interface IRender
    {
        /// <summary>
        /// 设置材质
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        bool SetMaterial(Material mat);
        /// <summary>
        /// 设置mesh
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        bool SetMesh(Mesh mesh);
        /// <summary>
        /// 添加mesh
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        bool AddMesh(Mesh mesh);
        /// <summary>
        /// 绘制
        /// </summary>
        /// <returns></returns>
        bool Draw();
    }
}
