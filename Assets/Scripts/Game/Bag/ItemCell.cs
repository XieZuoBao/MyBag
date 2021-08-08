using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 背包格子对象类
/// </summary>
public class ItemCell : UIBase
{
    //背包格子的信息(有装备/空格子)
    private ItemInfo _itemInfo;
    //格子类型,默认是背包格子
    public ItemType itemType = ItemType.BAG;
    public Image imgIcon;
    public Image imgBg;
    public ItemInfo itemInfo
    {
        get { return _itemInfo; }
    }
    private bool isOpenDrag = false;
    protected override void Awake()
    {
        base.Awake();
        imgIcon = GetComponentAtChildren<Image>("ImgIcon");
        imgBg = GetComponentAtChildren<Image>("ImgBg");
        imgIcon.gameObject.SetActive(false);
        //监听鼠标移入和鼠标移除的事件
        UIMgr.AddCustomEventListener(imgBg, EventTriggerType.PointerEnter, OnEnterItemCell);
        UIMgr.AddCustomEventListener(imgBg, EventTriggerType.PointerExit, OnExitItemCell);
    }

    /// <summary>
    /// 根据道具信息 初始化格子信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(ItemInfo info)
    {
        this._itemInfo = info;
        //装备槽隐藏图标
        if (info == null)
        {
            imgIcon.gameObject.SetActive(false);
            return;
        }
        imgIcon.gameObject.SetActive(true);
        //根据道具信息的数据,来更新格子对象
        Item itemData = GameDataMgr.Instance.GetItemById(info.id);
        //图标
        imgIcon.sprite = ResMgr.Instance.LoadResource<Sprite>(itemData.iconPath);
        //数量
        if (itemType == ItemType.BAG)
            GetComponentAtChildren<Text>("TxtNum").text = info.num.ToString();
        //背包格子中,装备格子添加拖拽相关的事件
        if (itemData.type == (int)BagType.EQUIP)
        {
            if (isOpenDrag)
                return;
            isOpenDrag = true;
            //开始拖拽
            UIMgr.AddCustomEventListener(imgBg, EventTriggerType.BeginDrag, OnBeginDragItemCell);
            //拖拽中
            UIMgr.AddCustomEventListener(imgBg, EventTriggerType.Drag, OnDragItemCell);
            //结束拖拽
            UIMgr.AddCustomEventListener(imgBg, EventTriggerType.EndDrag, OnEndDragItemCell);
        }
    }

    private void OnBeginDragItemCell(BaseEventData data)
    {
        //Debug.Log("begin drag");
        EventCenter.Instance.EventTrigger<ItemCell>("ItemCellBeginDrag", this);
    }

    private void OnDragItemCell(BaseEventData data)
    {
        //Debug.Log("drag");
        EventCenter.Instance.EventTrigger<BaseEventData>("ItemCellDrag", data);
    }

    private void OnEndDragItemCell(BaseEventData data)
    {
        //Debug.Log("end drag");
        EventCenter.Instance.EventTrigger<ItemCell>("ItemCellEndDrag", this);
    }

    private void OnEnterItemCell(BaseEventData data)
    {
        EventCenter.Instance.EventTrigger<ItemCell>("ItemCellEnter", this);
    }

    private void OnExitItemCell(BaseEventData data)
    {
        EventCenter.Instance.EventTrigger<ItemCell>("ItemCellExit", this);
    }
}