using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    #region 控件及变量

    public Text textMoney;// 金币数量
    public Text textName;// 角色名字
    public Button btnLeft;// 左选按钮
    public Button btnRight;// 右选按钮
    public Button btnStart;// 开始按钮
    public Button btnReturn;// 返回开始界面按钮

    public Button btnUnLoack;// 解锁角色按钮
    public Text textUnLock;// 解锁按钮的显示文字

    private Transform heroPos;// 角色展示的位置

    private GameObject heroObj;// 当前显示的角色
    private RoleInfo nowRoleInfo;// 当前使用的数据
    private int nowIndex;// 当前角色的索引

    #endregion

    #region  面板初始化

    public override void Init()
    {
        // 找到场景中，预设体放置的位置
        heroPos = GameObject.Find("HeroPos").transform;

        // 更新玩家的金币数量
        textMoney.text = GameDataManager.Instance.playerData.haveMoney.ToString();

        // 左选操作
        btnLeft.onClick.AddListener(()=>{
            // 索引值减小
            --nowIndex;
            if(nowIndex < 0)
                nowIndex = GameDataManager.Instance.roleInfoList.Count - 1;

            // 刷新模型
            ChangeHero();
        });

        // 右选操作
        btnRight.onClick.AddListener(()=>{
            ++nowIndex;
            if(nowIndex >= GameDataManager.Instance.roleInfoList.Count)
                nowIndex = 0;
            // 刷新模型
            ChangeHero();
        });
        // 开始游戏
        btnStart.onClick.AddListener(()=>{
            // 记录当前选择的角色
            GameDataManager.Instance.nowSelfInfo = nowRoleInfo;
            // 隐藏自己
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            // 显示场景选择界面
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
            
        });

        btnReturn.onClick.AddListener(()=>{
            // 播放摄像机旋转动画
            // 播放，摄像机右转动画，然后执行，显示游戏开始界面
            Camera.main.GetComponent<CameraAnimatorCtrl>().PlayTurnRight(()=>{
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
            // 隐藏自己
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
        });

        // 解锁按钮
        btnUnLoack.onClick.AddListener(()=>{
            // 点击解锁按钮逻辑
            // 获取数据的引用，方便简化代码
            PlayerData data = GameDataManager.Instance.playerData;
            // 判断钱够不够
            if(nowRoleInfo.lockMoney <= data.haveMoney)
            {
                // 购买逻辑
                // 解锁就要扣钱
                data.haveMoney -= nowRoleInfo.lockMoney;
                // 更新钱的显示
                textMoney.text = data.haveMoney.ToString();
                // GameDataManager.Instance.playerData = data;
                // 玩家的存档数据中，就要添加上这个角色的id
                data.haveHero.Add(nowRoleInfo.id);
                // 存储一次数据
                GameDataManager.Instance.SavePlayerData();

                // 更新解锁按钮是否显示之类的
                UpdateLock();

                // 显示提示面板，购买成功
                UIManager.Instance.ShowPanel<TipsPanel>().SetTip("你购买成功了");
            }else{
                // 如果钱不够
                // 打开提示面板，显示钱不够
                UIManager.Instance.ShowPanel<TipsPanel>().SetTip("购买失败，钱不够啊");
            }

        });

        // 更新默认显示的角色
        ChangeHero();
    }

    #endregion

    #region 更新方法

    /// <summary>
    /// 刷新角色模型，更新角色数据
    /// </summary>
    private void ChangeHero(){
        // 先删除之前的角色模型
        if(heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }

        // 获取角色的数据
        nowRoleInfo = GameDataManager.Instance.roleInfoList[nowIndex];
        // 实例化角色，记录下来，用于后续切换的时候删除
        heroObj = GameObject.Instantiate(Resources.Load<GameObject>(nowRoleInfo.res), heroPos.position, heroPos.rotation);
        // 设置角色的父对象，TODO方便后面可能，做鼠标拖动查看角色
        heroObj.transform.SetParent(heroPos, true);

        // 根据解锁相关数据，来决定是否显示解锁按钮
        UpdateLock();
    }

    /// <summary>
    /// 根据数据判断，是否显示解锁按钮
    /// </summary>
    private void UpdateLock(){

        textName.text = nowRoleInfo.tips;// 角色的描述，无论是否解锁都需要显示的

        // 如果该角色已经拥有了，就不需要解锁
        // 如果该角色解锁所需的金币数量为0，也不需要解锁
        if(nowRoleInfo.lockMoney == 0 ||  GameDataManager.Instance.playerData.haveHero.Contains(nowRoleInfo.id))
        {   
            // 当前角色已解锁，就不用显示解锁按钮了
            btnUnLoack.gameObject.SetActive(false);
            // 当前角色已解锁，即可开始游戏
            btnStart.gameObject.SetActive(true);
            return;
        }

        // 该角色需要解锁
        btnUnLoack.gameObject.SetActive(true);
        // 显示解锁角色所需金币
        textUnLock.text = "￥"+ nowRoleInfo.lockMoney;
        // 如果该角色需要解锁，就不能点击开始游戏
        btnStart.gameObject.SetActive(false);
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);

        // 隐藏面板的时候，就要删除当前模型
        Destroy(heroObj);
        heroObj = null;
    }

    #endregion
}
