using System.Collections.Generic;
using UnityEngine;


namespace RayGraphics.Render
{
    /// <summary>
    /// 普通渲染
    /// </summary>
    public class SameMaterialRender : IRender
    {
        private Material m_targetMaterial = null;
        private List<Mesh> m_listTargetMesh = new List<Mesh>();
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
            if (mesh != null)
            {
                m_listTargetMesh.Add(mesh);
            }
            return true;
        }
        /// <summary>
        /// 添加mesh
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public bool AddMesh(Mesh mesh)
        {
            if (mesh != null)
            {
                m_listTargetMesh.Add(mesh);
            }
            return true;
        }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <returns></returns>
        public bool Draw()
        {
            if (!m_targetMaterial || m_listTargetMesh== null)
                return false;

            foreach (Mesh mesh in m_listTargetMesh)
            {
                if (mesh != null)
                {
                    Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, m_targetMaterial, 0);
                }
            }
            return true;
        }
    }
}