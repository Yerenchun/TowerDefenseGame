using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Toggle isPlayBGM;// 音乐多选框
    public Toggle isPlaySE;// 音效多选框
    public Slider sliderBGM;// 音乐滑动条
    public Slider sliderSE;// 音效滑动条
    public Button btnExit;// 退出按钮

    #region 注册控件事件监听

    public override void Init()
    {
        // 多选框勾选是否改变的事件的监听
        isPlayBGM.onValueChanged.AddListener((isOn)=>{
            // 控制实际的是否播放
            BKMusic.Instance.SetIsPlayBGM(isOn);
            // 存储到临时数据
            GameDataManager.Instance.musicData.isPlayBGM = isOn;
        });
        isPlaySE.onValueChanged.AddListener((isOn)=>{
            // 存储到临时数据
            GameDataManager.Instance.musicData.isPlaySE = isOn;
        });

        // 滑动条的值改变的事件的监听
        sliderBGM.onValueChanged.AddListener((value)=>{
            // 控制实际的音量
            BKMusic.Instance.SetBGMValue(value);
            // 存储到临时数据
            GameDataManager.Instance.musicData.BGMValue = value;
        });
        sliderSE.onValueChanged.AddListener((value)=>{
            // 存储到临时数据
            GameDataManager.Instance.musicData.SEValue = value;
        });
        // 关闭按钮点击事件监听
        btnExit.onClick.AddListener(()=>{
            UIManager.Instance.HidePanel<SettingPanel>();

            // 隐藏面板时，要将音乐音效相关数据存储起来
            GameDataManager.Instance.SaveMusicData();
        });
    }

    #endregion

    #region 重写显示隐藏

    /// <summary>
    /// 显示自己的时候，更新面板
    /// </summary>
    public override void ShowMe()
    {
        base.ShowMe();
        // 要根据数据管理器中的临时数据，更新面板
        MusicData data = GameDataManager.Instance.musicData;
        isPlayBGM.isOn = data.isPlayBGM;
        isPlaySE.isOn = data.isPlaySE;
        sliderBGM.value = data.BGMValue;
        sliderSE.value = data.SEValue;
    }
    #endregion
}
