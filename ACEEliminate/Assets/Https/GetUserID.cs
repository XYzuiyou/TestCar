using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WeChatWASM;
using static LoadPanel;

public class GetUserID : MonoBehaviour
{
    public static GetUserID Instance;

    public bool isGetID;  //当前是否获得了ID


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        WX.cloud.Init();  //初始化
    }
    public void GetID()
    {

        GetWXOpenID();
    }
    public void GetWXOpenID()
    {


        LoginOption info = new LoginOption();
        info.complete = (aa) => { /*登录完成处理,成功失败都会调*/ };
        info.fail = (aa) => { /*登录失败处理*/ };
        info.success = (aa) =>
        {
            //登录成功处理
            Debug.Log("__OnLogin success登陆成功!查看Code：" + aa.code);
            StartCoroutine(SendCodeToServer(aa.code));
            //登录成功...这完成后，跳到下一步，《二、查看授权》
        };
        WX.Login(info);
#if UNITY_EDITOR
        isGetID = true;  //当前获得了ID

#endif
    }

    /// <summary>
    /// 发送 code 到自己服务器
    /// </summary>
    IEnumerator SendCodeToServer(string code)
    {
        // ✅ 在这里把 code 和 GameID 一起传给服务器
        string url = $"{WXHttpManager.Instance.GetHttpRequestHost()}/api/game/GetWXOpenID?code={code}&gameID={WXHttpManager.Instance.GameID}";
        print("发送的请求OPENID的链接" + url);
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.certificateHandler = new WXHttpManager.BypassCert();
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("服务器返回：" + req.downloadHandler.text);
                var result = JsonUtility.FromJson<OpenIDResult>(req.downloadHandler.text);

                Debug.Log("✅ 最终获取到 OpenID：" + result.openid);
                WXHttpManager.Instance.UserID = result.openid;
                isGetID = true;
            }
            else
            {
                Debug.LogError("获取OpenID失败：" + req.error);
                isGetID = true;
            }
        }
    }

    [Serializable]
    public class OpenIDResult
    {
        public int code;
        public string openid;
    }
}

