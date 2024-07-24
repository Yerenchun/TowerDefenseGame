using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class MonsterObj : MonoBehaviour
{
    // 怪物初始化
    // 出生过后再移动
    // 移动-寻路组件
    // 受伤
    // 死亡

    #region 基础变量

    // 动画相关
    private Animator animator;
    // 位移相关 寻路组件
    private NavMeshAgent agent;
    // 一些不变的基础数据
    private MonsterInfo monsterInfo;

    // 当前血量
    private int hp;
    // 怪物是否死亡
    // 公开方便玩家攻击时调用
    public bool isDead = false;

    // 攻击冷却计时
    private float frontTime = 0;

    #endregion


    #region 初始化

    private void Awake() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // 测试
        // int index = Random.Range(0, GameDataManager.Instance.monsterInfoList.Count);
        // monsterInfo = GameDataManager.Instance.monsterInfoList[index];
        // InitInfo(monsterInfo);
    }

    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        // 动画状态机的加载
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animatorRes);
        // 设置血量
        hp = info.hp;

        // 寻路组件的速度初始化
        // 速度和加速度都是一个速度
        agent.speed = agent.acceleration = info.moveSpeed;
        agent.angularSpeed = info.roundSpeed;
    }

    #endregion


    #region 出生后移动

    public void BornEvent()
    {
        // 出生动画播放完后，开始移动
        // 设置代理的目的地
        agent.SetDestination(MainTower.Instance.transform.position);

        // 这个必须要设置不然就挺不下来
        // 设置距离目的地停止的距离
        agent.stoppingDistance = 5f;

        // 播放移动动画
        animator.SetBool("IsRun", true);
    }

    #endregion


    #region 检测停止播放攻击动画

    private void FixedUpdate() {

        if(isDead)
            return;

        // 根据速度来播放动画
        animator.SetBool("IsRun", agent.velocity != Vector3.zero);

        frontTime += Time.fixedDeltaTime;

        // 检测是否到达目的地
        float dictance = Vector3.Distance(this.transform.position, MainTower.Instance.transform.position);
        // agent.ramainingDistance没有Vector3.Distance准确
        if (dictance <= agent.stoppingDistance && frontTime >= monsterInfo.atkInterval)
        {
            // 到达目的地
            // 播放攻击动画
            animator.SetTrigger("Attack");
            // 重置cd
            frontTime = 0;
        }
    }

    // 攻击事件，伤害检测
    public void AtkEvent()
    {
        // 范围检测进行伤害判断
        // 尽管只需要检测一个东西，但是范围检测返回的就是数组
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1,
                        1 << LayerMask.NameToLayer("MainTower"));

        // 播放怪物攻击音效
        GameDataManager.Instance.PlaySound("Music/Eat");
        
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject == MainTower.Instance.gameObject)
            {
                MainTower.Instance.GetWound(monsterInfo.atk);
            }
        }
    }


    #endregion

    #region 受伤死亡

    // 受伤
    public void TakeDamage(int damage)
    {
        hp -= damage;
        animator.SetTrigger("Wound");
        // 播放音效
        GameDataManager.Instance.PlaySound("Music/Wound");

        if (hp <= 0)
        {
            // 死亡
            Die();
        }
    }

    // 死亡
    public void Die()
    {
        isDead = true;
        // 停止寻路
        agent.isStopped = true;
        // 播放动画
        animator.SetBool("IsDead", isDead);
        // 播放死亡音效
        GameDataManager.Instance.PlaySound("Music/dead");

        // TODO加钱
        // 通过关卡管理类，来管理游戏中的对象，通过它来让玩家加钱
    }

    // 死亡后事件
    public void DeadEvent()
    {
        // 死亡动画播放完后，移除该对象
        // 之后使用关卡管理器来处理
        // 怪物死亡当前场景上，记录的怪物就-1
        GameLevelMgr.Instance.RemoveMonsterFromList(this);
        Destroy(this.gameObject);

        // 检测游戏是否胜利
        if(GameLevelMgr.Instance.CheckAllOver())
        {
            // 游戏胜利
            // 将获得的钱，全部给玩家
            UIManager.Instance.ShowPanel<GameOverPanel>().SetInfo(GameLevelMgr.Instance.player.money, true);
        }
    }

    #endregion
}
