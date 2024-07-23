using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    #region 控件及私有变量

    public Button btnStart;
    public Button btnRight;
    public Button btnLeft;
    public Button btnBack;
    public Image imageScene;
    public Text textInfo;
    private SceneInfo nowSceneInfo;
    private int nowIndex = 0;

    #endregion

    #region 初始化

    public override void Init()
    {
        btnStart.onClick.AddListener(()=>{
            // 隐藏当前面板
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            // 加载游戏场景开始游戏
            // 保险起见，使用异步加载
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            // 异步加载完成再调用，就不会报空了
            ao.completed += (AsyncOperation obj) => {
                // 关卡管理进行初始化
                GameLevelMgr.Instance.InitInfo(nowSceneInfo);
            };
        });

        btnRight.onClick.AddListener(()=>{
            // 加载下一张场景图片
            ++nowIndex;
            if(nowIndex >= GameDataManager.Instance.sceneInfoList.Count)
                nowIndex = 0;
            ChangeScene();
        });

        btnLeft.onClick.AddListener(()=>{
            // 加载上一张场景图片
            --nowIndex;
            if(nowIndex < 0)
                nowIndex = GameDataManager.Instance.sceneInfoList.Count - 1;
            ChangeScene();
        });

        btnBack.onClick.AddListener(()=>{
            // 隐藏当前面板
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            // 显示选角面板
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });

        // 显示默认场景
        ChangeScene();
    }

    #endregion

    #region 更新场景信息

    /// <summary>
    /// 刷新场景图片和场景描述
    /// </summary>
    private void ChangeScene()
    {
        // 先将之前的图片置空
        if(imageScene.sprite != null)
        {
            imageScene.sprite = null;
        }

        // 获取当前场景的数据
        nowSceneInfo = GameDataManager.Instance.sceneInfoList[nowIndex];
        
        // 加载场景图片
        imageScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);

        // 加载场景的相关描述
        textInfo.text = "名称：\n" + nowSceneInfo.name  + "\n"+ "描述：\n" + nowSceneInfo.tips;
    }

    #endregion
}
