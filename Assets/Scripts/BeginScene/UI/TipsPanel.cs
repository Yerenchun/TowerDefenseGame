using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : BasePanel
{
    public Button btnSubmit;
    public Text textTip;

    public override void Init()
    {
        btnSubmit.onClick.AddListener(()=>{
            UIManager.Instance.HidePanel<TipsPanel>();
        });
    }

    /// <summary>
    /// 设置提示内容
    /// </summary>
    /// <param name="tip"></param>
    public void SetTip(string tip)
    {
        textTip.text = tip;
    }

}
