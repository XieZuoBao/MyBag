using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包面板
/// </summary>
public class UIBagPanel : UIBase
{
    //ScrollView组件下的Content节点
    private Transform content;
    //页签下的背包数据内容
    private List<ItemCell> list = new List<ItemCell>();
    private void Start()
    {
        GetComponentAtChildren<Button>("BtnClose").onClick.AddListener(() =>
        {
            UIMgr.Instance.HidePanel("Prefabs/UI/UIBagPanel");
        });

        GetComponentAtChildren<Toggle>("TogItem").onValueChanged.AddListener(OnToggleValueChanged);
        GetComponentAtChildren<Toggle>("TogIEquip").onValueChanged.AddListener(OnToggleValueChanged);
        GetComponentAtChildren<Toggle>("TogMoney").onValueChanged.AddListener(OnToggleValueChanged);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        //默认显示第一页
        ChangeBagType(BagType.ITEM);
    }

    /// <summary>
    /// Toggle状态发生改变的回调
    /// </summary>
    /// <param name="value"></param>
    private void OnToggleValueChanged(bool value)
    {
        if (GetComponentAtChildren<Toggle>("TogItem").isOn)
        {
            ChangeBagType(BagType.ITEM);
        }
        else if (GetComponentAtChildren<Toggle>("TogIEquip").isOn)
        {
            ChangeBagType(BagType.EQUIP);
        }
        else if (GetComponentAtChildren<Toggle>("TogMoney").isOn)
        {
            ChangeBagType(BagType.MONEY);
        }
    }

    /// <summary>
    /// 根据用户选择的Toogle选项,显示对应页下的背包信息
    /// </summary>
    /// <param name="type"></param>
    public void ChangeBagType(BagType type)
    {
        //默认状态
        List<ItemInfo> tempInfo = GameDataMgr.Instance.playerInfo.items;
        switch (type)
        {
            case BagType.EQUIP:
                tempInfo = GameDataMgr.Instance.playerInfo.equips;
                break;
            case BagType.MONEY:
                tempInfo = GameDataMgr.Instance.playerInfo.moneys;
                break;
        }

        //更新背包显示的内容
        //删除之前的格子信息
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }
        list.Clear();
        //更新显示新的数据
        for (int i = 0; i < tempInfo.Count; i++)
        {
            ItemCell cell = ResMgr.Instance.LoadResource<GameObject>("Prefabs/UI/ItemCell").GetComponent<ItemCell>();
            //设置父对象
            content = GetComponentAtChildren<ContentSizeFitter>("Content").transform;
            cell.transform.SetParent(content);
            cell.transform.localScale = Vector3.one;
            //初始化数据
            cell.InitInfo(tempInfo[i]);
            //把数据存到list中
            list.Add(cell);
        }
    }
}