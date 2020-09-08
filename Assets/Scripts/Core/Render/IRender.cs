using System.Collections.Generic;
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
        /// 渲染
        /// </summary>
        /// <returns></returns>
        bool Render();

        /// <summary>
        /// 更新数据
        /// </summary>
        void UpdateInstancedData(Matrix4x4 matrix, MaterialPropertyBlock block);
        /// <summary>
        /// 更新数据
        /// </summary>
        void UpdateInstancedData(List<Matrix4x4> matrixArray, MaterialPropertyBlock block);
    }
}
