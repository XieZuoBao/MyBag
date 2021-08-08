using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPanel : UIBase
{
    private void Start()
    {
        //背包按钮
        GetComponentAtChildren<Button>("BtnBag").onClick.AddListener(() =>
        {
            UIMgr.Instance.ShowPanel<UIBagPanel>("Prefabs/UI/UIBagPanel");
            UIMgr.Instance.ShowPanel<UIRolePanel>("Prefabs/UI/UIRolePanel");
        });
        //商店按钮
        GetComponentAtChildren<Button>("BtnShop").onClick.AddListener(() =>
        {
            UIMgr.Instance.ShowPanel<UIShopPanel>("Prefabs/UI/UIShopPanel");
        });
        //加钱按钮(测试)
        GetComponentAtChildren<Button>("BtnAddCoin").onClick.AddListener(() =>
        {
            EventCenter.Instance.EventTrigger<int>("ChangeMoney", 1000);
        });
        //加钻石按钮(测试)
        GetComponentAtChildren<Button>("BtnAddGem").onClick.AddListener(() =>
        {
            EventCenter.Instance.EventTrigger<int>("ChangeGem", 1000);
        });
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        //更新显示玩家信息
        GetComponentAtChildren<Text>("TxtName").text = GameDataMgr.Instance.playerInfo.name;
        GetComponentAtChildren<Text>("TxtLv").text = GameDataMgr.Instance.playerInfo.lv.ToString();
        GetComponentAtChildren<Text>("TxtCoin").text = GameDataMgr.Instance.playerInfo.coin.ToString();
        GetComponentAtChildren<Text>("TxtGem").text = GameDataMgr.Instance.playerInfo.gem.ToString();
        GetComponentAtChildren<Text>("TxtPower").text = GameDataMgr.Instance.playerInfo.power.ToString();
        //监听货币改变事件
        EventCenter.Instance.AddEventListener<int>("ChangeMoney", UpdatePanelMoney);
        EventCenter.Instance.AddEventListener<int>("ChangeGem", UpdatePanelMoney);
    }

    public override void HidePanel()
    {
        base.HidePanel();
        EventCenter.Instance.RemoveEventListener<int>("ChangeMoney", UpdatePanelMoney);
        EventCenter.Instance.RemoveEventListener<int>("ChangeGem", UpdatePanelMoney);
    }

    /// <summary>
    /// 货币改变事件的监听回调
    /// </summary>
    /// <param name="count"></param>
    private void UpdatePanelMoney(int count)
    {
        GetComponentAtChildren<Text>("TxtCoin").text = GameDataMgr.Instance.playerInfo.coin.ToString();
        GetComponentAtChildren<Text>("TxtGem").text = GameDataMgr.Instance.playerInfo.gem.ToString();
    }
}