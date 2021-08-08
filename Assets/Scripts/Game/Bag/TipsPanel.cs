using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 道具装备详细信息面板
/// </summary>
public class TipsPanel : UIBase
{
    /// <summary>
    /// 根据道具信息 初始化Tips面板信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(ItemInfo info)
    {
        //根据道具信息的数据,来更新格子对象
        Item itemData = GameDataMgr.Instance.GetItemById(info.id);
        //图标
        GetComponentAtChildren<Image>("ImgIcon").sprite = ResMgr.Instance.LoadResource<Sprite>(itemData.iconPath);
        //数量
        GetComponentAtChildren<Text>("TxtNum").text = "数量: " + info.num.ToString();
        //名字
        GetComponentAtChildren<Text>("TxtName").text = itemData.name;
        //描述
        GetComponentAtChildren<Text>("TxtTips").text = itemData.tips;
    }
}