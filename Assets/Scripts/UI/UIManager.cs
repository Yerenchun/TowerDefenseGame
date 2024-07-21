using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    // 单例模式
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;
    // 一开始，就得到Canvas对象
    private Transform canvasTrans;
    // 面板字典 键是面板的名字，值，使用里氏替换原则
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    /// <summary>
    /// 管理器初始化
    /// </summary>
    private UIManager(){
        // 生成Canvas对象
        GameObject canvasObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvasObj.transform;
        // 保证过场景，不移除Canvas
        // 保证场景中，只有一个Canvas
        GameObject.DontDestroyOnLoad(canvasObj);
    }

    #region 重要方法

    /// <summary>
    /// 显示面板
    /// 返回面板对象方便获取调用
    /// T泛型的作用，就是得到类的名字
    /// </summary>
    /// <typeparam name="T">面板基类</typeparam>
    /// <returns>返回面板对象</returns>
    public T ShowPanel<T>() where T : BasePanel
    {
        // T泛型的作用，就是得到类的名字
        // 获取类名的名字，通过类名来加载对应的面板，前提是面板的名字和类名必须相同
        string panelName = typeof(T).Name;
        // 如果字典中，已经有了，就直接返回
        if(panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        
        // 创建出来的应该是游戏物体
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        // 设置父对象
        panelObj.transform.SetParent(canvasTrans, false);

        // 得到对应面板的脚本
        T panel = panelObj.GetComponent<T>();
        // 将创建出来的面板，添加到字典中，方便后面获取
        panelDic.Add(panelName, panel);
        // 调用面板的显示方法
        panel.ShowMe();

        return panel;
    }

    /// <summary>
    /// 隐藏面板
    /// 传入一个是否需要淡出的bool值
    /// </summary>
    /// <param name="isFade">是否需要淡出</param>
    /// <typeparam name="T">需要隐藏的面板</typeparam>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        // 获取类名，通过类名获取面板
        string panelName = typeof (T).Name;
        if( panelDic.ContainsKey(panelName))
        {
            // 需要淡出
            if(isFade)
            {
                // 调用隐藏自己的方法，并且传入一个lambda表达式的匿名方法
                panelDic[panelName].HideMe(()=>{
                    // 删除面板
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    // 从字典中移除该面板
                    panelDic.Remove(panelName);
                });
            }
            // 不需要淡出，就直接删除
            else{
                // 删除面板
                GameObject.Destroy(panelDic[panelName].gameObject);
                // 从字典中移除该面板
                panelDic.Remove(panelName);
            }
        }
    }

    /// <summary>
    /// 从字典中获取面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if(panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        return null;
    }
    
    #endregion
}
