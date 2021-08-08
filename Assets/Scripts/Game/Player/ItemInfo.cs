using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家拥有的道具基础信息
/// </summary>
[System.Serializable]
public class ItemInfo
{
    /// <summary>
    /// 道具id
    /// </summary>
    public int id;
    /// <summary>
    /// 道具数量
    /// </summary>
    public int num;

    public ItemInfo()
    {

    }
}