using UnityEngine;
using RayGraphics.Math;
using System.Collections.Generic;
/// <summary>
/// 格子纹理
/// </summary>
namespace RayGraphics.Texture
{
    public class GridTexture : ITexture
    {
        private Mesh m_targetQuadMesh = null;
        private Material m_targetMaterial = null;

        private Texture2D m_GridTexture = null;
        private byte[] m_ColorBuffer = null;
        private bool m_isDirty = false;

        public bool Init(Material gridMaterial, Mesh targetQuadMesh, Int2 gridSize)
        {
            if (!gridMaterial || !targetQuadMesh)
                return false;
            m_targetQuadMesh = targetQuadMesh;
            m_targetMaterial = gridMaterial;
            m_GridTexture = CreateTexture(gridSize.x, gridSize.y);
            gridMaterial.SetTexture("_MainTex", m_GridTexture);
            return true;
        }
        /// <summary>
        /// 创建纹理
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Texture2D CreateTexture(int width, int height)
        {
            Texture2D tex = new Texture2D(width, height, TextureFormat.RG16, false);
            tex.hideFlags = HideFlags.HideAndDontSave;
            int resolution = width * height;
            int size = resolution * 2;
            m_ColorBuffer = new byte[resolution * 2];
            for (int i = 0; i < size - 1; i += 2)
            {
                m_ColorBuffer[i] = 0;
                m_ColorBuffer[i + 1] = 128;
            }
            m_GridTexture.SetPixelData(m_ColorBuffer, 0);
            m_GridTexture.Apply();
            return tex;
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <returns></returns>
        public bool Release()
        {
            if (m_GridTexture != null)
            {
                Object.Destroy(m_GridTexture);
            }
            if (m_ColorBuffer != null)
            {
                m_ColorBuffer = null;
            }
            return true;
        }
        /// <summary>
        /// 设置像素
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="isEnable"></param>
        public void SetPixel(Short2 pos, bool isEnable)
        {
            int destIndex = pos.y * m_GridTexture.width + pos.x;
            destIndex *= 2;
            if (destIndex < m_ColorBuffer.Length)
            {
                if (isEnable == true)
                {
                    m_ColorBuffer[destIndex] = 128;
                    m_ColorBuffer[destIndex + 1] = 0;
                }
                else 
                {
                    m_ColorBuffer[destIndex] = 0;
                    m_ColorBuffer[destIndex + 1] = 0;
                }
            }
        }
        /// <summary>
        /// 设置挡格
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool SetBlock(List<Short2> listBlock)
        {
            if (listBlock == null || listBlock.Count == 0)
                return false;
            foreach (Short2 pos in listBlock)
            {
                SetPixel(pos, true);
            }
            m_isDirty = true;
            return true;
        }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <returns></returns>
        public bool Draw()
        {
            if ((!m_targetQuadMesh) || (!m_targetMaterial))
                return false;

            if (m_isDirty)
            {
                m_isDirty = false;
                m_GridTexture.SetPixelData(m_ColorBuffer, 0);
                m_GridTexture.Apply();
            }
            Graphics.DrawMesh(m_targetQuadMesh, Vector3.zero, Quaternion.identity, m_targetMaterial, 0);
            return true;
        }
    }

}
