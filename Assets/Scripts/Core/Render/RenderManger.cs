using System.Collections.Generic;
/// <summary>
/// 渲染管理器
/// </summary>
namespace RayGraphics.Render
{
    public class RenderManger 
    {
        private  HashSet<IRender> m_ListRender = new HashSet<IRender>();
        /// <summary>
        /// 添加渲染
        /// </summary>
        /// <param name="render"></param>
        public  void AddRender(IRender render)
        {
            if (render == null)
                return;
            m_ListRender.Add(render);
        }
        /// <summary>
        /// 移除渲染
        /// </summary>
        /// <param name="render"></param>
        public  void Remover(IRender render)
        {
            if (render == null)
                return;
            m_ListRender.Remove(render);
        }
        /// <summary>
        /// 清理渲染
        /// </summary>
        public  void Clear()
        {
            m_ListRender.Clear();
        }
        /// <summary>
        /// 进行绘制
        /// </summary>
        public  void Draw()
        {
            if (m_ListRender == null || m_ListRender.Count == 0)
                return ;

            foreach (IRender render in m_ListRender)
            {
                if (render != null)
                {
                    render.Render();
                }
            }
        }
    }
}