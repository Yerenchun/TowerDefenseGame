using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text textInfo;
    public Text textMoney;
    public Button btnSubmit;


    public override void Init()
    {
        btnSubmit.onClick.AddListener(()=>{
            // 隐藏面板
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.HidePanel<GameOverPanel>();

            // 清空当前关卡的数据
            GameLevelMgr.Instance.ClearData();

            // 返回开始场景
            SceneManager.LoadScene("BeginScene");
        });
    }

    // 提供给外部的方法，用于设置显示文字内容，即游戏是胜利还是失败
    public void SetInfo(int money, bool isWin) {
        if(isWin)
        {
            textInfo.text = "失败\n" + "获得失败奖励"; 
        }else{
            textInfo.text = "胜利\n" + "获得胜利奖励";
        }
        textMoney.text = "￥" + money;

        // 根据获取奖励，存储到存档中
        GameDataManager.Instance.playerData.haveMoney += money;
        GameDataManager.Instance.SavePlayerData();

    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 解除鼠标锁定
        Cursor.lockState = CursorLockMode.None;
    }


}
