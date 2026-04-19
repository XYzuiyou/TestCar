using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPanel : MonoBehaviour
{
    public int ID;
    [SerializeField] private int targetFrameRate = 30;

    public bool isNewbieState;

    public int IDValue;
    void Start()
    {
        // 禁用垂直同步，允许自由设置帧率
        QualitySettings.vSyncCount = 0;
        // 设置目标帧率
        Application.targetFrameRate = targetFrameRate;
        Debug.Log($"帧率已设置为: {targetFrameRate} FPS");

        if (GameManager.gam.isTest == false)
        {
            Screen.SetResolution(750, 1624, true);

            GameDataManager.gameData.InitData();
        }
        else
        {


            InventoryManager.inventory.InitData();  //初始化系统数据
            WorldData.CurrentSelectLevelID = IDValue;  //设置当前玩家选择的关卡的ID
            BattleManager.battle.PlayGame(IDValue);

            //     GameManager.gam.InitData();
        }
    }








    // 可选：允许在运行时动态修改帧率
    public void SetTargetFrameRate(int newFrameRate)
    {
        targetFrameRate = newFrameRate;
        Application.targetFrameRate = targetFrameRate;
        Debug.Log($"帧率已更新为: {targetFrameRate} FPS");
    }
}
