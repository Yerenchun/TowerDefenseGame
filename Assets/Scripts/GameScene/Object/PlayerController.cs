using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class    PlayerController : MonoBehaviour
{
    private Animator playerAnimator;

    // 1.玩家属性初始化
    // 2.移动变化 动作变化
    // 3.攻击动作的不同处理
    // 4.钱变化的逻辑

    // 玩家的攻击力
    private int atk;
    // 玩家拥有的钱
    public int money;
    // 旋转速度
    public float rotateSpeed = 25f;
    // 持枪对象才有的开火点
    public Transform firePoint;
    

    private void Awake() {
        playerAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// 初始化玩家基础属性
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="money"></param>
    public void InitPlayerInfo(int atk, int money)
    {
        this.atk = atk;
        this.money = money;
        // 更新界面上钱的数量
        ChangePanelMoney();
    }

    // 移动动作变化
    private void Update() {

        // 监听输入
        playerAnimator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        playerAnimator.SetFloat("VSpeed", Input.GetAxis("Vertical"));

        // 翻滚
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            playerAnimator.SetTrigger("Roll");
        }

        // 攻击
        if(Input.GetMouseButtonDown(0)) {
            playerAnimator.SetTrigger("Fire");

            // TODO创建子弹特效

        }

        // 是否下蹲
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            playerAnimator.SetLayerWeight(1, 1);
        }else if(Input.GetKeyUp(KeyCode.LeftControl)){
            playerAnimator.SetLayerWeight(1, 0);
        }

        // 角色旋转通过鼠标控制
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed);
    }

    #region 攻击事件

    // 3.攻击动作的不同处理

    /// <summary>
    /// 专门用于处理刀武器攻击动作的伤害检测事件
    /// </summary>
    public void KnifeEvent()
    {
        // 对怪物层进行范围伤害检测
        Collider[] colliders =  Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up,
                                     1f, 1 << LayerMask.NameToLayer("Monster"));
        // 播放刺刀攻击音效
        GameDataManager.Instance.PlaySound("Music/Knife");
        // 对怪物造成伤害

        for (int i = 0; i < colliders.Length; i++)
        {
            // 对怪物造成伤害
            MonsterObj monster =  colliders[i].GetComponent<MonsterObj>();
            if(monster != null)
            {
                // 只让一个对象受伤
                monster.TakeDamage(this.atk);
                break;
            }
        }
    }

    /// <summary>
    /// 专门用于处理枪武器攻击动作的伤害检测事件
    /// </summary>
    public void ShootEvent()
    {
        // 进行射线检测
        // 获取相交的多个物体
        RaycastHit[] hits= Physics.RaycastAll(firePoint.position, firePoint.forward, 100f, 
                1 << LayerMask.NameToLayer("Monster"));
        // 播放开枪攻击音效
        GameDataManager.Instance.PlaySound("Music/Gun");

        // 对怪物造成伤害
        for(int i = 0; i < hits.Length; i++)
        {
            MonsterObj monster = hits[i].collider.GetComponent<MonsterObj>();
            if(monster != null)
            {

                // 打击特效的创建
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataManager.Instance.nowSelfInfo.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1f);

                // 只让一个对象受伤
                monster.TakeDamage(this.atk);
                break;
            }
        }
    }

    #endregion

    #region 其他

    // 4.钱变化的逻辑
    public void ChangePanelMoney() { 
        // 更新UI上的钱
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }
    
    /// <summary>
    /// 击杀怪物获得金钱
    /// </summary>
    /// <param name="money"></param>
    public void AddMoney(int money)
    {
        // 加钱
        this.money += money;
        ChangePanelMoney();
    }

    #endregion

}
