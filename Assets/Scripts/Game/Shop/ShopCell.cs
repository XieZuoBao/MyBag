using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCell : UIBase
{
    private ShopCellInfo info;

    void Start()
    {
        GetComponentAtChildren<Button>("BtnBuy").onClick.AddListener(BuyItem);
    }

    /// <summary>
    /// 买物品
    /// </summary>
    private void BuyItem()
    {
        //金币购买
        if (info.priceType == 1 && GameDataMgr.Instance.playerInfo.coin >= info.price)
        {
            //玩家道具更新
            GameDataMgr.Instance.playerInfo.AddItem(info.itemInfo);
            //减少金币
            //GameDataMgr.Instance.playerInfo.ChangeMoney(-info.price);
            //存储数据
            //GameDataMgr.Instance.SavePlayerData();
            //换事件分发模式实现
            EventCenter.Instance.EventTrigger<int>("ChangeMoney", -info.price);

            //显示购买成功弹窗
            //UIMgr.Instance.ShowPanel<UIBuyedPanel>("Prefabs/UI/UIBuyedPanel", E_UI_LAYER.SYSTEM, (panel) =>
            //{
            //    panel.InitInfo("购买成功");
            //});

            TipMgr.Instance.ShowBuyedTip("购买成功");
        }
        //宝石购买
        else if (info.priceType == 2 && GameDataMgr.Instance.playerInfo.gem >= info.price)
        {
            //玩家道具更新
            GameDataMgr.Instance.playerInfo.AddItem(info.itemInfo);
            //减少钻石
            //GameDataMgr.Instance.playerInfo.ChangeGem(-info.price);
            //存储数据
            //GameDataMgr.Instance.SavePlayerData();
            //换事件分发模式实现
            EventCenter.Instance.EventTrigger<int>("ChangeGem", -info.price);

            //显示购买成功弹窗
            //UIMgr.Instance.ShowPanel<UIBuyedPanel>("Prefabs/UI/UIBuyedPanel", E_UI_LAYER.SYSTEM, (panel) =>
            //{
            //    panel.InitInfo("购买成功");
            //});

            TipMgr.Instance.ShowBuyedTip("购买成功");
        }
        //货币不足
        else
        {
            //显示余额不足弹窗
            //UIMgr.Instance.ShowPanel<UIBuyedPanel>("Prefabs/UI/UIBuyedPanel", E_UI_LAYER.SYSTEM, (panel) =>
            //{
            //    panel.InitInfo("货币不足");
            //});
            TipMgr.Instance.ShowBuyedTip("货币不足");
        }
    }
    /// <summary>
    /// 初始化显示信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(ShopCellInfo info)
    {
        this.info = info;
        //根据售卖的道具id,得到道具表信息
        Item item = GameDataMgr.Instance.GetItemById(info.itemInfo.id);
        //更新控件显示信息
        GetComponentAtChildren<Image>("ImgIcon").sprite = ResMgr.Instance.LoadResource<Sprite>(item.iconPath);
        GetComponentAtChildren<Text>("TxtNum").text = info.itemInfo.num.ToString();
        GetComponentAtChildren<Text>("TxtName").text = item.name;
        GetComponentAtChildren<Image>("ImgMoney").sprite = ResMgr.Instance.LoadResource<Sprite>(info.iconPath);
        GetComponentAtChildren<Text>("TxtPrice").text = info.price.ToString();
        GetComponentAtChildren<Text>("TxtTips").text = info.tips;
    }
}