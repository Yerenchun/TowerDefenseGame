using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr
{
    // 单例模式
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance { get { return instance; } }
    #region 关卡数据

    // 当前角色的控制脚本
    public PlayerController player;

    // 记录所有刷怪点
    private List<MonsterBirthPlace> bornPoints = new List<MonsterBirthPlace>();
    // 记录当前还有多少波怪物
    private int nowWaveNum = 0;
    // 记录一共有多少波怪物
    private int allWaveNum = 0;

    // 记录当前场景中的怪物数量
    // private int nowMonsterNum = 0;
    // 记录当前场景上的怪物的列表
    private List<MonsterObj> monsterList = new List<MonsterObj>();


    #endregion

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
        if(monsterList.Count> 0)
            return false;
        Debug.Log("胜利");
        return true;
    }

    // 记录场景上怪物的数量
    // 创建怪物的时候就+1，怪物死亡就-1
    // public void ChangeNowMonsterNum(int num){
    //     nowMonsterNum += num;
    // }

    #region 怪物列表

    /// <summary>
    /// 每创建一只怪物就向列表中添加怪物
    /// </summary>
    /// <param name="monster"></param>
    public void AddMonsterToList(MonsterObj monster) { 
        monsterList.Add(monster); 
    }

    // 移除该怪物
    public void RemoveMonsterFromList(MonsterObj monster) { 
        monsterList.Remove(monster); 
    }

    /// <summary>
    /// 遍历查找符合距离的单个怪物
    /// </summary>
    /// <param name="pos">炮塔的位置</param>
    /// <param name="Range">炮塔的攻击范围</param>
    /// <returns></returns>
    public MonsterObj FindMonster(Vector3 pos, float Range)
    {
        // 在怪物列表中，找到满足距离条件的怪物 返回出去 用于塔攻击
        for (int i = 0; i < monsterList.Count; i++)
        {
            // 并且是没有死亡的怪物
            if ( !monsterList[i].isDead && Vector3.Distance(monsterList[i].transform.position, pos) <= Range)
                return monsterList[i];
        }
        return null;
    }

    /// <summary>
    /// 遍历查找符合距离的怪物列表
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="Range"></param>
    /// <returns>怪物列表</returns>
    public List<MonsterObj> FindMonsters(Vector3 pos, float Range) 
    {
        // 在怪物列表中，找到满足距离条件的所有怪物 返回出去 用于塔攻击
        return monsterList.FindAll(m => 
            !m.isDead && Vector3.Distance(m.transform.position, pos) <= Range
        );

        // 两段代码作用相同
        // List<MonsterObj> monsters = new List<MonsterObj>();
        // for (int i = 0; i < monsterList.Count; i++)
        // {
        //     // 并且是没有死亡的怪物
        //     if (!monsterList[i].isDead && Vector3.Distance(monsterList[i].transform.position, pos) <= Range)
        //         monsters.Add(monsterList[i]);
        // }
        // return monsters;
    }


    #endregion


    #endregion

    #region 清除数据
    /// <summary>
    /// 因为是单例模式，切换场景不会被移除，所以要手动清除数据
    /// 避免，下一次进入关卡仍然有上一次的数据
    /// </summary>
    public void ClearData() { 
        bornPoints.Clear();
        monsterList.Clear();
        nowWaveNum = allWaveNum = 0;
        player = null;
    }
    #endregion

}
