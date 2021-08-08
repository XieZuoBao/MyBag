using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对应从本地json数据中解析出来的逐条数据
/// </summary>
[System.Serializable]
public class ShopCellInfo
{
    public int id;
    public ItemInfo itemInfo;
    public int priceType;
    public int price;
    public string tips;
    public string iconPath;
}