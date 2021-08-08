using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopPanel : UIBase
{
    //ScrollView组件下的Content节点
    private Transform content;
    void Start()
    {
        GetComponentAtChildren<Button>("BtnClose").onClick.AddListener(() =>
        {
            UIMgr.Instance.HidePanel("Prefabs/UI/UIShopPanel");
        });
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        //初始化商店面板售卖信息
        //根据解析出的本地商店json数据,初始化显示商店面板
        for (int i = 0; i < GameDataMgr.Instance.shopInfos.Count; i++)
        {
            ShopCell cell = ResMgr.Instance.LoadResource<GameObject>("Prefabs/UI/ShopCell").GetComponent<ShopCell>();
            //设置父对象,相对大小
            content = GetComponentAtChildren<ContentSizeFitter>("Content").transform;
            cell.transform.SetParent(content);
            cell.transform.localScale = Vector3.one;
            //显示面板信息
            cell.InitInfo(GameDataMgr.Instance.shopInfos[i]);
        }
    }
}