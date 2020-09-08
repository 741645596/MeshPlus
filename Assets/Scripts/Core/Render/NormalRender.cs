using UnityEngine;
using System.Collections.Generic;
namespace RayGraphics.Render
{
    /// <summary>
    /// 普通渲染
    /// </summary>
    public class NormalRender : IRender
    {
        private Material m_targetMaterial = null;
        private Mesh m_targetMesh = null;
        /// <summary>
        /// 绑定mat，mesh
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public bool Init(Material mat, Mesh mesh)
        {
            m_targetMaterial = mat;
            m_targetMesh = mesh;
            return true;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="meshMaterial"></param>
        /// <returns></returns>
        public bool SetMaterial(Material mat)
        {
            m_targetMaterial = mat;
            return true;
        }
        /// <summary>
        /// 设置mesh
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public bool SetMesh(Mesh mesh)
        {
            m_targetMesh = mesh;
            return true;
        }
        /// <summary>
        /// 添加mesh
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public bool AddMesh(Mesh mesh)
        {
            m_targetMesh = mesh;
            return true;
        }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <returns></returns>
        public bool Render()
        {
            if (!m_targetMaterial || !m_targetMesh)
                return false;

            Graphics.DrawMesh(m_targetMesh, Matrix4x4.identity, m_targetMaterial, 0);
            return true;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="block"></param>
        public void UpdateInstancedData(Matrix4x4 matrix, MaterialPropertyBlock block)
        {

        }
        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateInstancedData(List<Matrix4x4> matrixArray, MaterialPropertyBlock block)
        {

        }
    }
}
