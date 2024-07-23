using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr
{
    // 单例模式
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance { get { return instance; } }
    // 当前角色的控制脚本
    public PlayerController player;

    // 记录所有刷怪点
    private List<MonsterBirthPlace> bornPoints = new List<MonsterBirthPlace>();
    // 记录当前还有多少波怪物
    private int nowWaveNum = 0;
    // 记录一共有多少波怪物
    private int allWaveNum = 0;

    // 记录当前场景中的怪物数量
    private int nowMonsterNum = 0;

    private GameLevelMgr() { }

    #region 初始化


    // 1.是切换到游戏场景时，我们需要动态的创建玩家
    public void InitInfo(SceneInfo info) {
        // 显示游戏界面
        UIManager.Instance.ShowPanel<GamePanel>();

        RoleInfo playerInfo = GameDataManager.Instance.nowSelfInfo;
        Transform heroPos = GameObject.Find("HeroBronPos").transform;
        // 创建玩家
        // 1.加载资源
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(playerInfo.res), heroPos.position, heroPos.rotation);
        
        // 给主摄像机设置目标玩家
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);
        player = heroObj.GetComponent<PlayerController>();
        // 初始化基础属性，设置UI界面的金币
        player.InitPlayerInfo(playerInfo.atk, info.money);

        // 初始化保护区域的血量
        MainTower.Instance.UpdateHp(info.towerHp, info.towerHp);
    }


    #endregion

    #region 检测胜利条件

    // 2.我们需要通过游戏管理器，来判断游戏是否胜利
    // 要知道 场景中 是否还有怪物没有出 以及 场景中 是否有还没有死亡的怪物

    // 用于记录刷怪点的方法
    public void AddBornPoint(MonsterBirthPlace point) { 
        bornPoints.Add(point); 
    }

    /// <summary>
    /// 更新一共有多少波怪物，在刷怪点初始时调用
    /// </summary>
    public void UpdateMaxNum(int num){
        allWaveNum += num;
        nowWaveNum = allWaveNum;
        // 刷新游戏面板的怪物波数
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, allWaveNum);
    }

    /// <summary>
    /// 更新界面的剩余波数，在刷怪点刷完一波怪物的时候调用
    /// </summary>
    /// <param name="num"></param>
    public void ChangeNowWaveNum(int num) { 
        nowWaveNum -= num; 
        // 更新界面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, allWaveNum);
    }


    /// <summary>
    /// 检测是否胜利
    /// </summary>
    /// <returns></returns>
    public bool CheckAllOver() {
        // 判断刷怪点是否还有剩余怪物
        for(int i = 0; i < bornPoints.Count; i++){
            if(!bornPoints[i].CheckOver())
                return false;
        }
        // 是不是可以通过nowWaveNum也可以判断是否还有剩余怪物需要刷

        // 判断当前场景中是否还有怪物
        if(nowMonsterNum > 0)
            return false;
        Debug.Log("胜利");
        return true;
    }

    // 记录场景上怪物的数量
    // 创建怪物的时候就+1，怪物死亡就-1
    public void ChangeNowMonsterNum(int num){
        nowMonsterNum += num;
    }


    #endregion
}
