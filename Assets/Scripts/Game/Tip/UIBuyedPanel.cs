using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyedPanel : UIBase
{

    private void Start()
    {
        GetComponentAtChildren<Button>("BtnSure").onClick.AddListener(() =>
        {
            UIMgr.Instance.HidePanel("Prefabs/UI/UIBuyedPanel");
        });
    }

    public void InitInfo(string info)
    {
        GetComponentAtChildren<Text>("TxtInfo").text = info;
    }
}