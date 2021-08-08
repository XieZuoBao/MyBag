using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRolePanel : UIBase
{
    private ItemCell itemHead;
    private ItemCell itemNeck;
    private ItemCell itemWeapon;
    private ItemCell itemClothes;
    private ItemCell itemTrousers;
    private ItemCell itemShoes;

    protected override void Awake()
    {
        base.Awake();
        itemHead = transform.Find("ItemHead").GetComponent<ItemCell>();
        itemNeck = transform.Find("ItemNeck").GetComponent<ItemCell>();
        itemWeapon = transform.Find("ItemWeapon").GetComponent<ItemCell>();
        itemClothes = transform.Find("ItemClothes").GetComponent<ItemCell>();
        itemTrousers = transform.Find("ItemTrousers").GetComponent<ItemCell>();
        itemShoes = transform.Find("ItemShoes").GetComponent<ItemCell>();
        itemHead.itemType = ItemType.HEAD;
        itemNeck.itemType = ItemType.NECK;
        itemWeapon.itemType = ItemType.WEAPON;
        itemClothes.itemType = ItemType.CLOTHES;
        itemTrousers.itemType = ItemType.TROUSERS;
        itemShoes.itemType = ItemType.SHOES;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        UpdateRolePanel();
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);

        switch (btnName)
        {
            case "BtnClose":
                UIMgr.Instance.HidePanel("Prefabs/UI/UIRolePanel");
                break;
        }
    }

    public void UpdateRolePanel()
    {
        //得到玩家身上穿戴的装备信息
        List<ItemInfo> nowEquips = GameDataMgr.Instance.playerInfo.nowEquips;
        Item itemInfo;
        //每次更新前置空装备槽后再加载显示
        itemHead.InitInfo(null);
        itemNeck.InitInfo(null);
        itemWeapon.InitInfo(null);
        itemClothes.InitInfo(null);
        itemTrousers.InitInfo(null);
        itemShoes.InitInfo(null);
        for (int i = 0; i < nowEquips.Count; i++)
        {
            itemInfo = GameDataMgr.Instance.GetItemById(nowEquips[i].id);
            //根据装备类型更新装备格子
            switch (itemInfo.equipType)
            {
                case (int)ItemType.HEAD:
                    itemHead.InitInfo(nowEquips[i]);
                    break;
                case (int)ItemType.NECK:
                    itemNeck.InitInfo(nowEquips[i]);
                    break;
                case (int)ItemType.WEAPON:
                    itemWeapon.InitInfo(nowEquips[i]);
                    break;
                case (int)ItemType.CLOTHES:
                    itemClothes.InitInfo(nowEquips[i]);
                    break;
                case (int)ItemType.TROUSERS:
                    itemTrousers.InitInfo(nowEquips[i]);
                    break;
                case (int)ItemType.SHOES:
                    itemShoes.InitInfo(nowEquips[i]);
                    break;
            }//switch_end
        }//for_end
        GameDataMgr.Instance.SavePlayerData();
    }
}