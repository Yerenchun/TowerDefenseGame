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
            // TODO 游戏结束
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
