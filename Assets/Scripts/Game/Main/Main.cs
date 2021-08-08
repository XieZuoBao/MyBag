using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        GameDataMgr.Instance.Init();
        BagMgr.Instance.Init();

        UIMgr.Instance.ShowPanel<UIMainPanel>("Prefabs/UI/UIMainPanel", E_UI_LAYER.BOTTOM);
    }
}