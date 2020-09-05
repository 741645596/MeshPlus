using UnityEngine;
using RayGraphics.Math;

namespace RayGraphics.Texture
{
    /// <summary>
    /// 自定义纹理接口
    /// </summary>
    public interface ITexture
    {
        /// <summary>
        /// 释放
        /// </summary>
        /// <returns></returns>
        bool Release();
        /// <summary>
        /// 创建纹理
        /// </summary>
        /// <returns></returns>
        Texture2D CreateTexture(int width, int height);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="isEnable"></param>
        void SetPixel(Short2 pos, bool isEnable);
    }
}

