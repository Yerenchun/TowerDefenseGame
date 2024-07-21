using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimatorCtrl : MonoBehaviour
{
    private Animator animator;
    // 用于记录，动画播放完成之后，要做的事
    UnityAction overAction;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    #region 动画控制

    /// <summary>
    /// 提供给外部，启动，向左旋转动画
    /// </summary>
    /// <param name="callBack">动画播放结束后，要执行的委托</param>
    public void PlayTurnLeft(UnityAction callBack){
        overAction = callBack;
        animator.SetTrigger("TurnLeft");
    }

    /// <summary>
    /// 提供给外部，启动向右旋转动画
    /// </summary>
    /// <param name="callBack">动画播放结束后，要执行的委托</param>
    public void PlayTurnRight(UnityAction callBack){
        overAction = callBack;
        animator.SetTrigger("TurnRight");
    }

    /// <summary>
    /// 两个旋转动画播放完后，都会调用这个方法
    /// </summary>
    public void PlayOver(){
        // 执行委托
        overAction?.Invoke();
        overAction = null;
    }
    #endregion
}
