using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    #region UI控件

    public Button btnExit;
    public Image imgHp;
    public Text txtHp;
    public Text txtMoney;
    public Text txtCount;
    public float hpWith = 690f;// 记录血条初始宽度
    private int money = 0;// 玩家获取的金币
    // 下方造塔组合控件的父对象，用来控制显隐
    public Transform botTrans;
    // 管理3个复合控件
    public List<TowerBtn> towerBtns = new List<TowerBtn>();

    #endregion

    #region 初始化

    public override void Init()
    {
        
        hpWith = imgHp.rectTransform.rect.width;
        btnExit.onClick.AddListener(()=>{
            // 因为过场景不会移除，所以需要隐藏当前面板
            UIManager.Instance.HidePanel<GamePanel>();
            // TODO 返回到游戏开始场景
            SceneManager.LoadScene("BeginScene");
        });

        // 没有到达造塔点，应该隐藏造塔的按钮
        botTrans.gameObject.SetActive(false);

        // 更改鼠标锁定的状态
        Cursor.lockState = CursorLockMode.Confined;
    }

    #endregion

    #region 更新面板各参数

    /// <summary>
    /// 更新安全区域的血量
    /// </summary>
    /// <param name="hp">当前血量</param>
    /// <param name="maxHp">最大血量</param>
    public void UpdateTowerHp(int hp, int maxHp)
    {
        txtHp.text = hp + "/" + maxHp;
        // 设置血条的长度
        imgHp.rectTransform.sizeDelta = new Vector2(hpWith * (float)hp / maxHp, imgHp.rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// 更新当前的金币
    /// </summary>
    /// <param name="money"></param>
    public void UpdateMoney(int money) { 
        this.money = money; 
        txtMoney.text = money.ToString(); 
    }

    /// <summary>
    /// 更新当前的怪物波次
    /// </summary>
    /// <param name="count"></param>
    /// <param name="maxCount"></param>
    public void UpdateWaveNum(int count, int maxCount) { 
        txtCount.text = count + "/" + maxCount; 
    }

    // TODO 造塔点的更新

    #endregion


}
