using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    // 单例模式
    private static BKMusic instance;
    public static BKMusic Instance => instance;

    private AudioSource audioSource;// 音源组件

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // 初始化，设置数据
        MusicData data = GameDataManager.Instance.musicData;
        //
        audioSource.volume = data.BGMValue;
        // 如果开启音乐，就是不静音
        audioSource.mute = !data.isPlayBGM;
    }

    #region 提供给外部设置

    /// <summary>
    /// 提供给外部调用，设置是否开启BGM
    /// </summary>
    /// <param name="isPlayBGM">是否开启BGM</param>
    public void SetIsPlayBGM(bool isPlayBGM)
    {
        audioSource.mute = !isPlayBGM;
    }

    /// <summary>
    /// 提供外部调用
    /// </summary>
    /// <param name="value">音量大小</param>
    public void SetBGMValue(float value)
    {
        audioSource.volume = value;
    }
    #endregion
}
