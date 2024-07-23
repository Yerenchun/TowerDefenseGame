using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTower : MonoBehaviour
{
    // 血量相关
    private int hp;
    private int maxHp;
    // 是否死亡
    private bool isDead;

    // 提供一个全局访问点
    public static MainTower Instance{get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    // 更新血量
    public void UpdateHp(int hp, int maxHp)
    {
        this.hp = hp;
        this.maxHp = maxHp;

        // 更新UI界面血量的显示
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(hp, maxHp);
    }

    // 自己受到伤害
    public void GetWound(int damage)
    {
        // 如果保护区域已经被破坏
        // 就直接GmaeOver
        if(isDead)
            return;

        hp -= damage;
        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
            // 游戏失败结束
            // 显示游戏结束界面
            // 得到玩家获得的钱
            // 失败只获得一半的钱
            int money = (int)(GameLevelMgr.Instance.player.money * 0.5f);
            UIManager.Instance.ShowPanel<GameOverPanel>().SetInfo(money, false);
        }

        // 更新UI界面血量的显示
        UpdateHp(hp, maxHp);
    }
    /// <summary>
    /// 可以确保没有任何对象继续引用该单例，
    /// 从而允许垃圾回收器回收单例对象占用的内存
    /// 避免内存泄漏
    /// </summary>
    private void OnDestroy() {
        Instance = null;
    }

}
