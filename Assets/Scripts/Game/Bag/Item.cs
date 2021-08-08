using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 与道具表转换过来的json文件配置表信息对应
/// </summary>
[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public string iconPath;
    public int type;
    public int equipType;
    public int price;
    public string tips;
}