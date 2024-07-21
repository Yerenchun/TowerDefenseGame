using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class GameDataManager
{
    private static GameDataManager instance = new GameDataManager();
    public static GameDataManager Instance => instance;

    #region 临时数据

    // 临时中间数据
    public MusicData musicData;
    // 记录选择的角色数据，用于之后在游戏场景中创建
    public RoleInfo nowSelfInfo;
    // 角色数据列表
    public List<RoleInfo> roleInfoList;
    // 玩家相关数据，即存档
    public PlayerData playerData;
    // 场景数据列表
    public List<SceneInfo> sceneInfoList;
    // 当前选择的场景
    public SceneInfo nowSceneInfo;

    // 怪物数据列表
    public List<MonsterInfo> monsterInfoList;



    #endregion

    #region 初始化

    // 各种数据初始化
    private GameDataManager(){
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        // 从Json文件中，加载角色数据
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        // 初始化存档
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        // 从Json文件中，加载场景数据
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        // 从Json文件中，加载怪物数据
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
    }
    #endregion

    #region 存储各种数据方法

    /// <summary>
    /// 存储音乐音效相关的数据
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    /// <summary>
    /// 存档
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }


    #endregion

}
