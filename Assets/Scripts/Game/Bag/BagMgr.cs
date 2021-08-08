using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 背包管理类
/// 1.管理背包中的格子鼠标进入,离开等事件的监听
/// 2.管理背包中的装备格子的拖拽事件(3个)
/// 2.管理背包中的装备与角色装备面板中的装备交换逻辑
/// </summary>
public class BagMgr : BaseSingleton<BagMgr>
{
    //当前拖动的格子------背包中的格子
    private ItemCell nowDragItem;
    //当前鼠标进入的格子-----角色装备面板中的格子
    private ItemCell nowInItem;
    //当前拖到的格子的装备图片信息
    private Image nowDragImg;
    //是否处于拖动状态的开关
    private bool isDraging = false;

    public void Init()
    {
        EventCenter.Instance.AddEventListener<ItemCell>("ItemCellBeginDrag", OnBeginDragItemCell);
        EventCenter.Instance.AddEventListener<BaseEventData>("ItemCellDrag", OnDragItemCell);
        EventCenter.Instance.AddEventListener<ItemCell>("ItemCellEndDrag", OnEndDragItemCell);
        EventCenter.Instance.AddEventListener<ItemCell>("ItemCellEnter", OnEnterItemCell);
        EventCenter.Instance.AddEventListener<ItemCell>("ItemCellExit", OnExitItemCell);
    }

    public void ChangeEquip()
    {
        //交换装备的条件:
        //1.拖动的时背包中的装备格子,
        //2.存在进入的格子(不能是随意的位置,随意的位置可以不存在进入的格子),且进入的格子不是背包中的装备格子
        //3.拖动的装备的类型(注意不是"格子类型",拖动装备的格子类型是"背包"),与进入的装备槽的"格子类型"必须是一致的
        //4.区分进入的装备槽是否穿戴了装备:没穿则穿上,穿了则交换
        if (nowDragItem.itemType == ItemType.BAG)//拖动的是装备格子
        {
            //存在进入的格子          进入的格子不是背包中的装备格子
            if (nowInItem != null && nowInItem.itemType != ItemType.BAG)//也即是进入了角色装备界面的装备槽
            {
                //装备交换
                //拖动的装备的类型(注意不是"格子类型",拖动装备的格子类型是"背包"),与进入的装备槽的"格子类型"必须是一致的
                Item info = GameDataMgr.Instance.GetItemById(nowDragItem.itemInfo.id);
                if (info.equipType == (int)nowInItem.itemType)
                {
                    //装备槽没穿装备,则穿装备
                    if (nowInItem.itemInfo == null)
                    {
                        //装备槽穿装备
                        GameDataMgr.Instance.playerInfo.nowEquips.Add(nowDragItem.itemInfo);
                        //背包移除装备
                        GameDataMgr.Instance.playerInfo.equips.Remove(nowDragItem.itemInfo);

                    }
                    //装备槽穿了装备,交换装备
                    else
                    {
                        //交换装备
                        //装备槽放入拖动装备,拖下进入装备
                        GameDataMgr.Instance.playerInfo.nowEquips.Add(nowDragItem.itemInfo);
                        GameDataMgr.Instance.playerInfo.nowEquips.Remove(nowInItem.itemInfo);
                        //背包放入进入装备,删除拖动装备
                        GameDataMgr.Instance.playerInfo.equips.Add(nowInItem.itemInfo);
                        GameDataMgr.Instance.playerInfo.equips.Remove(nowDragItem.itemInfo);
                    }
                    //更新背包
                    UIMgr.Instance.GetPanel<UIBagPanel>("Prefabs/UI/UIBagPanel").ChangeBagType(BagType.EQUIP);
                    //更新人物
                    UIMgr.Instance.GetPanel<UIRolePanel>("Prefabs/UI/UIRolePanel").UpdateRolePanel();
                    //更新数据
                    GameDataMgr.Instance.SavePlayerData();
                }
            }
        }
        //从装备槽往外托
        else
        {
            //下装备(拖到非格子位置或者是托到了其他装备槽)
            if (nowInItem == null || nowInItem.itemType != ItemType.BAG)
            {
                GameDataMgr.Instance.playerInfo.nowEquips.Remove(nowDragItem.itemInfo);
                GameDataMgr.Instance.playerInfo.equips.Add(nowDragItem.itemInfo);
                //更新背包
                UIMgr.Instance.GetPanel<UIBagPanel>("Prefabs/UI/UIBagPanel").ChangeBagType(BagType.EQUIP);
                //更新人物
                UIMgr.Instance.GetPanel<UIRolePanel>("Prefabs/UI/UIRolePanel").UpdateRolePanel();
                //更新数据
                GameDataMgr.Instance.SavePlayerData();
            }
            //装备由装备槽拖到背包中装备格子
            else if (nowInItem != null && nowInItem.itemType == ItemType.BAG)
            {
                //判断装备槽中拖动的装备与背包装备格子中的装备类型是否一致
                Item info = GameDataMgr.Instance.GetItemById(nowInItem.itemInfo.id);
                if ((int)nowDragItem.itemType == info.equipType)
                {
                    //交换装备
                    //装备槽放入拖动装备,拖下进入装备 
                    GameDataMgr.Instance.playerInfo.nowEquips.Add(nowInItem.itemInfo);
                    GameDataMgr.Instance.playerInfo.nowEquips.Remove(nowDragItem.itemInfo);
                    //背包放入进入装备,删除拖动装备
                    GameDataMgr.Instance.playerInfo.equips.Add(nowDragItem.itemInfo);
                    GameDataMgr.Instance.playerInfo.equips.Remove(nowInItem.itemInfo);
                    //更新背包
                    UIMgr.Instance.GetPanel<UIBagPanel>("Prefabs/UI/UIBagPanel").ChangeBagType(BagType.EQUIP);
                    //更新人物
                    UIMgr.Instance.GetPanel<UIRolePanel>("Prefabs/UI/UIRolePanel").UpdateRolePanel();
                    //更新数据
                    GameDataMgr.Instance.SavePlayerData();
                }
            }
        }
    }

    /// <summary>
    /// 开始拖动
    /// </summary>
    /// <param name="itemCell"></param>
    private void OnBeginDragItemCell(ItemCell itemCell)
    {
        //隐藏提示面板
        UIMgr.Instance.HidePanel("Prefabs/UI/TipsPanel");
        isDraging = true;
        //记录当前拖动的格子
        nowDragItem = itemCell;
        //创建拖动的装备对应的图片nowDragImg的信息
        PoolMgr.Instance.GetGameObjectByPool("Prefabs/UI/DragIcon", (obj) =>
        {
            nowDragImg = obj.GetComponent<Image>();
            nowDragImg.sprite = itemCell.imgIcon.sprite;
            //设置父对象和相对大小,坐标
            //将图片设置在显示UI的Canvas下
            nowDragImg.transform.SetParent(UIMgr.Instance.canvasTrans);
            nowDragImg.transform.localScale = Vector3.one;
        });
    }

    /// <summary>
    /// 拖动中
    /// </summary>
    /// <param name="data"></param>
    private void OnDragItemCell(BaseEventData data)
    {
        //更新nowDragImg的位置信息
        if (nowDragImg == null)
            return;
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIMgr.Instance.canvasTrans,//nowDragImg的父对象
            (data as PointerEventData).position,//鼠标位置
            (data as PointerEventData).pressEventCamera, //ui相机
            out localPos);//返回nowDragImg相对父对象的坐标信息
        nowDragImg.transform.localPosition = localPos;
        //异步加载图片需要时间,如果在图片加载完成前就已经停止拖动,直接将创建的图片放回缓存池
        if (!isDraging)
        {
            PoolMgr.Instance.RecoverGameObjectToPools("Prefabs/UI/DragIcon", nowDragImg.gameObject);
            nowDragImg = null;
        }
    }

    /// <summary>
    /// 拖动结束
    /// </summary>
    /// <param name="itemCell"></param>
    private void OnEndDragItemCell(ItemCell itemCell)
    {
        isDraging = false;
        //交换装备
        ChangeEquip();
        //结束拖动时清空当前拖动的格子
        nowDragItem = null;
        nowInItem = null;
        if (nowDragImg == null)
            return;
        //移除nowDragImg 
        PoolMgr.Instance.RecoverGameObjectToPools("Prefabs/UI/DragIcon", nowDragImg.gameObject);
        nowDragImg = null;
    }

    /// <summary>
    /// 鼠标进入
    /// </summary>
    /// <param name="itemCell"></param>
    private void OnEnterItemCell(ItemCell itemCell)
    {
        //Debug.Log("in");
        if (isDraging)
        {
            //记录当前进入的格子
            nowInItem = itemCell;
            return;
        }

        //如果进入的是空格子,不显示提示面板
        if (itemCell.itemInfo == null)
            return;

        UIMgr.Instance.ShowPanel<TipsPanel>("Prefabs/UI/TipsPanel", E_UI_LAYER.TOP, (panel) =>
        {
            //异步加载完面板后的逻辑
            //更新提示信息
            panel.InitInfo(itemCell.itemInfo);
            //设置提示面板显示位置(提示面板的锚点指向格子里面的Icon中心-----格子自身的锚点是左上角)
            panel.transform.position = itemCell.imgBg.transform.position;

            //如果面板异步加载结束,发现已经开始拖动,直接隐藏Tips面板
            if (isDraging)
                UIMgr.Instance.HidePanel("Prefabs/UI/TipsPanel");
        });
    }

    /// <summary>
    /// 鼠标离开
    /// </summary>
    /// <param name="itemCell"></param>
    private void OnExitItemCell(ItemCell itemCell)
    {
        //Debug.Log("exit");
        if (isDraging)
        {
            //拖动中离开了进入的(装备槽) 格子,清空记录
            nowInItem = null;
            return;
        }
        //如果进入的是空格子,不需要隐藏提示面板(因为提示面板没有打开)
        if (itemCell.itemInfo == null)
            return;

        UIMgr.Instance.HidePanel("Prefabs/UI/TipsPanel");
    }
}