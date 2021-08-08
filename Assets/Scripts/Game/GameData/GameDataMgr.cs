using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class GameDataMgr : BaseSingleton<GameDataMgr>
{
    //键值对[道具(Item)的id,道具(Item)]
    private Dictionary<int, Item> itemInfos = new Dictionary<int, Item>();
    public List<ShopCellInfo> shopInfos;
    //玩家信息存储路径
    private static string playerInfo_Url = Application.persistentDataPath + "/PlayerInfo.txt";
    //玩家数据信息
    public Player playerInfo;

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Init()
    {
        //加载背包本地json文件,并对文件数据进行解析
        string info = ResMgr.Instance.LoadResource<TextAsset>("JsonData/ItemInfo").text;
        Items items = JsonUtility.FromJson<Items>(info);
        for (int i = 0; i < items.info.Count; i++)
        {
            itemInfos.Add(items.info[i].id, items.info[i]);
        }

        //初始化玩家信息
        if (File.Exists(playerInfo_Url))
        {
            //读取玩家信息数据
            byte[] bytes = File.ReadAllBytes(playerInfo_Url);
            string json = Encoding.UTF8.GetString(bytes);
            //解析玩家数据
            playerInfo = JsonUtility.FromJson<Player>(json);
        }
        else
        {
            //没有玩家数据时,初始化一个默认数据
            playerInfo = new Player();
            //将玩家数据转换为json数据,并保存到本地
            //string json = JsonUtility.ToJson(player);
            //File.WriteAllBytes(playerInfo_Url, Encoding.UTF8.GetBytes(json));
            SavePlayerData();
        }

        //加载商店本地json文件,并对文件数据进行解析
        string shopInfo = ResMgr.Instance.LoadResource<TextAsset>("JsonData/ShopInfo").text;
        Shops shopsInfo = JsonUtility.FromJson<Shops>(shopInfo);
        shopInfos = shopsInfo.info;

        //监听事件
        EventCenter.Instance.AddEventListener<int>("ChangeMoney", ChangeMoney);
        EventCenter.Instance.AddEventListener<int>("ChangeGem", ChangeGem);
    }

    /// <summary>
    /// 金币改变事件的回调
    /// </summary>
    /// <param name="count"></param>
    void ChangeMoney(int count)
    {
        //更新金币
        playerInfo.ChangeMoney(count);
        //存储数据
        SavePlayerData();
    }

    /// <summary>
    /// 钻石改变事件的回调
    /// </summary>
    /// <param name="count"></param>
    void ChangeGem(int count)
    {
        //更新钻石
        playerInfo.ChangeGem(count);
        //存储数据
        SavePlayerData();
    }

    /// <summary>
    /// 根据道具(Item)Id得到道具
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item GetItemById(int id)
    {
        if (itemInfos.ContainsKey(id))
            return itemInfos[id];
        return null;
    }

    /// <summary>
    /// 将玩家数据转换为json数据,并保存到本地
    /// </summary>
    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerInfo);
        File.WriteAllBytes(playerInfo_Url, Encoding.UTF8.GetBytes(json));
    }
}