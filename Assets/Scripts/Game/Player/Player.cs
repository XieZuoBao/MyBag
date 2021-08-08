using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家基础信息
/// </summary>
public class Player
{
    public string name;
    public int lv;
    public int coin;
    public int gem;
    public int power;
    public List<ItemInfo> items;
    public List<ItemInfo> equips;
    public List<ItemInfo> moneys;

    //玩家当前穿上的装备
    public List<ItemInfo> nowEquips;

    public Player()
    {
        name = "李逍遥";
        lv = 1;
        coin = 9999;
        gem = 0;
        power = 50;
        items = new List<ItemInfo>() { new ItemInfo() { id = 3, num = 10 } };
        equips = new List<ItemInfo>() { new ItemInfo() { id = 1, num = 1 } };
        moneys = new List<ItemInfo>();

        nowEquips = new List<ItemInfo>();
    }

    /// <summary>
    /// 给玩家添加物品
    /// </summary>
    /// <param name="info"></param>
    public void AddItem(ItemInfo info)
    {
        Item item = GameDataMgr.Instance.GetItemById(info.id);
        switch (item.type)
        {
            //道具
            case (int)BagType.ITEM:
                items.Add(info);
                break;
            //装备
            case (int)BagType.EQUIP:
                equips.Add(info);
                break;
            //货币
            case (int)BagType.MONEY:
                moneys.Add(info);
                break;
        }
    }

    /// <summary>
    /// 更新金币数量
    /// </summary>
    /// <param name="count"></param>
    public void ChangeMoney(int count)
    {
        //余额不够
        if (count < 0 && this.coin < count)
            return;
        this.coin += count;
    }

    /// <summary>
    /// 更新钻石数量
    /// </summary>
    /// <param name="count"></param>
    public void ChangeGem(int count)
    {
        //余额不足
        if (count < 0 && this.gem < count)
            return;
        this.gem += count;
    }
}