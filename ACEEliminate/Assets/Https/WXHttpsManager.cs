using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WXHttpManager : MonoBehaviour
{
    public static WXHttpManager Instance;


    public  string UserID;     //当前的用户ID
    public  const int gameID = 6;   //当前的游戏ID  其中1是哑铃大师 2是停车游戏
    public int PlatTpyeID = 1;   //平台ID  1是微信  2是抖音


    public int GameID {
        get
        {
        return    gameID;
        }
    }

    [Header("服务器访问模式")]
    [Tooltip("勾选 = 使用IP访问 | 取消勾选 = 使用域名访问")]
    public bool useIPAddress = false;

    // 域名访问（自动 443 端口）
    private string serverHost = "game.manyouxingqiu.com";

    // IP 访问（必须加端口！！）
    private string serverIP = "47.103.5.161";
    private string serverPort = "443";



    /// <summary>
    /// 获得排行榜数据的委托事件
    /// </summary>
    /// <param name="rankList"></param>
    public delegate void OnRankListLoaded(List<RankItem> rankList);
    public event OnRankListLoaded OnRankListLoadeds;

    private void Awake()
    {
        Instance = this;

    }

    private IEnumerator Start()
    {
        yield return null;

    }

    //===================== 自动拼接地址 + 端口 =====================


    public string GetHttpRequestHost()
    {
        if (useIPAddress)
        {
            return $"https://{serverIP}:{serverPort}";
        }
        else
        {
            return $"https://{serverHost}";
        }
    }


    IEnumerator POST(string url, string json)
    {
        using (UnityWebRequest req = new UnityWebRequest(url, "POST"))
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(b);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            // 👆 上面全部保留
            // 👇 这行彻底删掉！！！
            // req.certificateHandler = new BypassCert();

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("<color=green>【✅ 数据发送成功】</color>");
                Debug.Log("==================== 发送的数据 ====================");
                Debug.Log(json);
                Debug.Log("=====================================================");
                ShowTip("数据存储成功！");
            }
            else
            {
                Debug.LogError("【❌ 发送失败】" + req.error);
                ShowTip("发送失败！");
            }
        }
    }
    // ==========================================================
    // ✅【根据 OpenID + GameID 读取玩家数据，并判断是否存在数据】
    // ==========================================================
    public void LoadPlayerDataFromServer(string openID, int gameID)
    {
string url = $"{GetHttpRequestHost()}/api/game/GetPlayerData?openID={openID}&GameID={gameID}&PlatTpyeID={PlatTpyeID}";
        Debug.Log("======================================");
        Debug.Log("【读取玩家数据】" + url);
        StartCoroutine(LoadPlayerDataCoroutine(url));
    }
    IEnumerator LoadPlayerDataCoroutine(string url)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            // 👇 这行已经删掉！！！
            // req.certificateHandler = new BypassCert();

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text.Trim();
                Debug.Log("<color=green>【✅ 数据读取成功】</color>");
                Debug.Log("==================== 服务器返回数据 ====================");
                Debug.Log(json);
                Debug.Log("=========================================================");

                if (string.IsNullOrEmpty(json) || json == "{}" || json == "[]")
                {
                    Debug.LogWarning("<color=yellow>【⚠️ 未找到该玩家的存档数据】</color>");
                    ShowTip("未找到存档数据，使用新数据！并且保存新数据");
                    WorldData.InitDefaultData();
                    GameDataManager.gameData.SaveGameData();
                    yield break;
                }

                try
                {
                    WorldDataData data = JsonUtility.FromJson<WorldDataData>(json);
                    WorldData.LoadData(data);
                    ShowTip("数据已成功加载到游戏！");
                }
                catch (Exception e)
                {
                    Debug.LogError("【❌ 数据解析失败】" + e.Message);
                }
            }
            else
            {
                Debug.LogError("【❌ 读取数据失败】" + req.error);
                ShowTip("读取数据失败！");
            }
        }
    }
    // ==========================
    // 保存 用户名 + 头像（独立存储，不影响游戏数据）
    // ==========================
    public void SaveUserInfo(string openID, string userName, string userAvatar)
    {
        var data = new UserInfoData
        {
            openid = openID,
            GameID = this.GameID,
            userName = userName,
            userAvatar = userAvatar,
            PlatTpyeID = this.PlatTpyeID
        };

        string url = $"{GetHttpRequestHost()}/api/game/SaveUserInfo";
        string json = JsonUtility.ToJson(data);
        StartCoroutine(POST(url, json));
    }

    [Serializable]
    public class UserInfoData
    {
        public string openid;
        public int GameID;
        public string userName;
        public string userAvatar;
        public int PlatTpyeID;
    }

    //=============请求排行榜数据


    // ==========================
    // 请求排行榜数据
    // ==========================
    public void RequestRankList(string openID)
    {
        // 注意：现在是 RankController，地址变了
        string url = $"{GetHttpRequestHost()}/api/Rank/GetEndlessRankList?openid={openID}&gameID={GameID}&PlatTpyeID={PlatTpyeID}";


        print("请求的排行的游戏ID" + GameID);
        StartCoroutine(RankListCoroutine(url));
    }

    IEnumerator RankListCoroutine(string url)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
           // req.certificateHandler = new BypassCert();
            yield return req.SendWebRequest();
                
            if (req.result == UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text;
                Debug.Log("✅ 排行榜数据：\n" + json);

                try
                {
                    // ===================== 这里我帮你改好了 =====================
                    // 【唯一正确的 Unity 解析数组方式】
                    RankListWrapper wrapper = JsonUtility.FromJson<RankListWrapper>("{\"list\":" + json + "}");
                    List<RankItem> rankList = wrapper.list;
                    // ==========================================================

                    // 数据回来 → 触发回调
                    OnRankListLoadeds?.Invoke(rankList);
                }
                catch (Exception e)
                {
                    Debug.LogError("❌ 解析失败：" + e.Message);
                }
            }
            else
            {
                Debug.LogError("❌ 排行榜请求失败：" + req.error);
            }
        }
    }

    // 必须加这个辅助类！！！
    [System.Serializable]
    public class RankListWrapper
    {
        public List<RankItem> list;
    }


    //=========================== 存储数据到服务器 ===========================
    public void SendSaveDataRequest(SaveDataRequest data)
    {
        string url = $"{GetHttpRequestHost()}/api/game/SavePlayerData";
        string json = JsonUtility.ToJson(data);
        StartCoroutine(POST(url, json));
    }

    void ShowTip(string msg)
    {
        Debug.Log($"<color=yellow>【提示】</color><color=green>{msg}</color>");
    }

    public class BypassCert : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] data) => true;
    }

}

//
// 上传数据结构
[Serializable]
public class SaveDataRequest
{
    public string openid;
    public string gameData;
    public int GameID;
    public int PlatTpyeID;
}


[System.Serializable]
public class RankItem
{
    public int rank;
    public string openid;
    public int maxScore;
    public string nickName;
    public string avatarUrl;
    public bool isCurrentUser;
}

