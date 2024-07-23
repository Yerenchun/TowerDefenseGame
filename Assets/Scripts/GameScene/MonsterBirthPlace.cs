using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBirthPlace : MonoBehaviour
{
    #region 基础变量

    // 怪物有多少波
    public int maxMonsterWave;
    // 每波怪物有多少只
    public int monsterCountPerWave;
    private int nowMonsterCount;
    // 怪物ID 多个怪物，用于随机创建
    public List<int> monsterIDs;
    // 记录当前波创建什么怪物
    private int nowMonsterID;

    // 单只怪物创建间隔时间
    public float monsterBirthInterval;
    // 每波怪物创建间隔时间
    public float monsterWaveInterval;
    // 第一波怪物创建的间隔时间
    public float firstWaveInterval;

    #endregion

    void Start()
    {
        // 开启延迟函数，创建第一波怪物
        Invoke("CreateWave", firstWaveInterval);

        // 将当前刷怪点，添加到关卡管理中
        GameLevelMgr.Instance.AddBornPoint(this);
        // 更新最大波数，加上当前刷怪点的最大波数
        GameLevelMgr.Instance.UpdateMaxNum(maxMonsterWave);
    }

    #region 刷怪

    /// <summary>
    /// 开始创建第一波怪物
    /// </summary>
    private void CreateWave()
    {
        // 得到当前波怪物的ID
        nowMonsterID = monsterIDs[Random.Range(0, monsterIDs.Count)];

        // 当前波怪物的数量
        nowMonsterCount = monsterCountPerWave;

        // 创建怪物
        CreateMonster();

        // 减少波次
        --maxMonsterWave;
        // 更新UI界面当前波数
        // 通知减少了一波怪物
        GameLevelMgr.Instance.ChangeNowWaveNum(1);
    }

    /// <summary>
    /// 创建怪物
    /// </summary>
    private void CreateMonster()
    {
        // 获取怪物数据
        MonsterInfo info = GameDataManager.Instance.monsterInfoList[nowMonsterID - 1];

        // 创建怪物预设体
        GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>(info.modelRes), this.transform.position, Quaternion.identity);
        // 手动添加怪物脚本
        MonsterObj monster = obj.AddComponent<MonsterObj>();
        monster.InitInfo(info);

        // 当前波怪物剩余数量 -1
        --nowMonsterCount;
        // 告诉关卡管理器，场景当中的怪物数量+1
        GameLevelMgr.Instance.ChangeNowMonsterNum(1);

        if(nowMonsterCount == 0)
        {
            // 递归延迟创建下一波怪物
            if(maxMonsterWave > 0){
                // 延迟创建下一波怪物
                Invoke("CreateWave", monsterWaveInterval);
            }
        }else{
            // 递归延迟创建下一只怪物
            Invoke("CreateMonster", monsterBirthInterval);
        }
    }

    /// <summary>
    /// 检测怪物是否刷完了
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return nowMonsterCount == 0 && maxMonsterWave == 0;
    }
    #endregion
}
