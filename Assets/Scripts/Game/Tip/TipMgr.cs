/*
 * 
 *      Title:
 * 
 *             
 *      Description: 
 *           
 *              
 ***/
using System.Collections.Generic;
using UnityEngine;

public class TipMgr : BaseSingleton<TipMgr>
{
    /// <summary>
    /// 显示购买提示面板
    /// </summary>
    /// <param name="info"></param>
    public void ShowBuyedTip(string info)
    {
        UIMgr.Instance.ShowPanel<UIBuyedPanel>("Prefabs/UI/UIBuyedPanel", E_UI_LAYER.SYSTEM, (panel) =>
        {
            panel.InitInfo(info);
        });
    }
}