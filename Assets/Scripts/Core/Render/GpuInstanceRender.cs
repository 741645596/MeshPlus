using UnityEngine;
using System.Collections.Generic;


namespace RayGraphics.Render
{
    public class GpuInstanceRender : IRender
    {
        private Material m_targetMaterial = null;
        private Mesh m_targetMesh = null;
        /// <summary>
        /// instance 数据
        /// </summary>
        private MaterialPropertyBlock m_propertyBlock = null;
        /// <summary>
        /// transform相关信息
        /// </summary>
        private List<Matrix4x4> m_matrixArray = null;
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
            return true;
        }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <returns></returns>
        public bool Render()
        {
            if (m_matrixArray == null || m_propertyBlock == null)
                return false;
            Graphics.DrawMeshInstanced(m_targetMesh,
            0,
            m_targetMaterial,
            m_matrixArray,
            m_propertyBlock);
            return true;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateInstancedData(List<Matrix4x4> matrixArray, MaterialPropertyBlock block)
        {
            m_matrixArray = matrixArray;
            m_propertyBlock = block;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="block"></param>
        public void UpdateInstancedData(Matrix4x4 matrix, MaterialPropertyBlock block)
        {
            
        }
    }
}
