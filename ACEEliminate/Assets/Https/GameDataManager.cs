using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager gameData;

    private void Awake()
    {
        gameData = this;
    }

    public void InitData()
    {
        // 获取登录管理器引用



        Debug.Log("开始账户登陆");

        GameDataManager.gameData.OnLoginComplete();



    }

    // 登录完成回调
    public void OnLoginComplete()
    {


        // 显示玩家数据示例
        ShowPlayerData();
    }


    // 显示玩家数据
    void ShowPlayerData()
    {


        GameObject panel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.LoadPanel);
        panel.transform.localScale = Vector3.one;
        panel.GetComponent<LoadPanel>().SetUI();

    }
    public void LoadGameData()
    {
        WXHttpManager.Instance.LoadPlayerDataFromServer(WXHttpManager.Instance.UserID, WXHttpManager.Instance.GameID);
    }
    public void SaveGameData()
    {
        // 1. 获取微信OpenID
        string openID = WXHttpManager.Instance.UserID;  //测试用的ID

        if (string.IsNullOrEmpty(openID))
        {
            Debug.LogError("OpenID 不存在，无法上传存档！");
            return;
        }

        // 2. 获取WorldData转好的JSON
        string gameDataJson = WorldData.ToMongoDBJson();

        // 3. 组装要发送的完整数据（OpenID + 游戏数据）
        SaveDataRequest data = new SaveDataRequest();
        data.openid = openID;
        data.gameData = gameDataJson;
        data.GameID = WXHttpManager.Instance.GameID;
        data.PlatTpyeID = WXHttpManager.Instance.PlatTpyeID;
        // 4. 发送到服务器
        WXHttpManager.Instance.SendSaveDataRequest(data);

        Debug.Log("✅ 开始上传存档数据...");
    }
}
