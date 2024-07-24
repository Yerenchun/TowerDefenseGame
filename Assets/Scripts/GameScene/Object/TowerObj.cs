using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObj : MonoBehaviour
{
    #region 基础数据

    // 炮台头部 用于旋转 指向目标
    public Transform head;
    // 开火点 用于释放攻击特效的位置
    public Transform gunPoint;
    // 炮台头部旋转速度 可以写死，
    public float headRotateSpeed = 5f;

    // 炮台关联的数据
    private TowerInfo info;

    // 当前要攻击的目标
    private MonsterObj targetObj;
    // 当前要攻击的目标们
    private List<MonsterObj> targetObjs;

    // 用于计时的 用来判断攻击间隔时间
    private float lastAttackTimer = 0f;
    // 用于记录怪物的位置，方便获得向量
    private Vector3 monsterPos;

    #endregion

    // 测试
    // private void Start() {
    //     // 初始化炮台数据
    //     InitInfo(GameDataManager.Instance.towerInfoList[6]);
    // }

    /// <summary>
    /// 初始化炮台相关数据
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(TowerInfo info)
    {
        this.info = info;
    }

    #region 攻击逻辑

    private void Update() {

        // 单体攻击逻辑
        if(info.type == 1){
            // 没有目标，或者目标已经死亡，或者目标超出攻击范围,就要去查找目标
            if(targetObj == null || 
                targetObj.isDead || 
                Vector3.Distance(transform.position, targetObj.transform.position) > info.atkRange){
                    targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position, info.atkRange);
            }

            if (targetObj == null)
                return;

            // 得到怪物位置，偏移y的目的是希望，炮台是水平转动的
            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;
            // 炮台旋转看向目标
            head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(monsterPos - head.position), headRotateSpeed * Time.deltaTime);

            // 因为炮台是在转动的，只有当炮台指向目标时，才进行攻击
            // 夹角小于5°，才进行攻击
            if (Quaternion.Angle(head.rotation, Quaternion.LookRotation(monsterPos - head.position)) < 5f &&
                Time.time - lastAttackTimer > info.offestTime) 
            { 
                // 已经满足条件，直接让怪物受伤
                targetObj.TakeDamage(info.atk);
                
                // 播放音效
                GameDataManager.Instance.PlaySound("Music/Tower");
                // 创建开火特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.hitEff), gunPoint.position, gunPoint.rotation);

                Destroy(effObj, 0.2f); // 0.2秒后销毁特效

                // 记录这次开火时间
                lastAttackTimer = Time.time;
            }

        }
        // 群体攻击逻辑
        else{
            targetObjs = GameLevelMgr.Instance.FindMonsters(this.transform.position, info.atkRange);
            // 满足条件即攻击
            if(targetObjs.Count > 0 && Time.time - lastAttackTimer > info.offestTime){
                // TODO播放音效，暂时没有合适的

                // 创建开火特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.hitEff), this.transform.position + Vector3.up, this.transform.rotation);

                Destroy(effObj, 0.3f); // 0.3秒后销毁特效

                // 满足条件直接让所有怪物受伤
                for(int i = 0; i < targetObjs.Count; i++){
                    targetObjs[i].TakeDamage(info.atk);
                }
                lastAttackTimer = Time.time;
            }
        }
    }
    #endregion
}
