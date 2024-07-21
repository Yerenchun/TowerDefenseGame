using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;// 整体控制UI组件
    private bool isShow;// 面板是否在显示
    private float alphaSpeed = 10;// 淡入淡出的速度
    // 淡出面板后，需要执行的委托函数
    private UnityAction hideCallBack;

    protected virtual void Awake() {
        // 获取CanvasGroup组件
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 面板初始化方法，用于子面板，注册控件的事件
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 显示面板方法
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// 隐藏面板
    /// 传入回调函数，在隐藏面板后，就立即执行
    /// </summary>
    /// <param name="callBack">回调函数</param>
    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 0;
        hideCallBack = callBack;
    }

    void Update()
    {   
        // 当处于显示状态时，如果透明度，不到1，就会不停地加到1，到1后，就停止变化
        // 面板淡入
        if(isShow && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        // 面板淡出
        else if(!isShow)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha <= 0){
                canvasGroup.alpha = 0;
                // 启动回调函数
                hideCallBack?.Invoke();
            }
        }
    }
}
