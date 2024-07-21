using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnExit;

    #region 注册各控件事件监听
    public override void Init()
    {
        // 开始按钮点击事件监听
        btnStart.onClick.AddListener(()=>{

            // 播放，摄像机左转动画，然后执行，显示选角面板
            Camera.main.GetComponent<CameraAnimatorCtrl>().PlayTurnLeft(()=>{
                // 显示选角面板
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });

            // 隐藏自己
            UIManager.Instance.HidePanel<BeginPanel>();
        });

        // 
        btnSetting.onClick.AddListener(()=>{
            // 打开设置面板
            UIManager.Instance.ShowPanel<SettingPanel>();
        });

        btnAbout.onClick.AddListener(()=>{
            // TODO 打开开发者介绍面板
            // 隐藏自己
            UIManager.Instance.HidePanel<BeginPanel>();
        });

        btnExit.onClick.AddListener(()=>{
            Application.Quit();
        });
    }
    #endregion
}
