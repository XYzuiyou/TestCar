using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitManager : MonoBehaviour
{

    public static InitManager init;
    private void Awake()
    {
        init = this;
    }

    private void Start()
    {
      //  StartCoroutine(InitData());
    }

    public IEnumerator InitData()
    {
        ///初始化数据

        InventoryManager.inventory.InitData();  //初始化系统数据
        yield return new WaitForSeconds(0.1f);  //等待3秒
        LoadPanel.load.isInitData = true;  //当前初始化数据完成了
     
    }
}
