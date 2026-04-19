using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using WeChatWASM;




public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;


    [Header("默认设置")]
    [SerializeField] private Sprite defaultAvatar;
    private Dictionary<string, Sprite> avatarCache = new Dictionary<string, Sprite>();

    private void Awake()
    {

        Instance = this;

    }

    public bool CheckURL(string url, Image targetImage)
    {

        if (url == "1" || url == "2" || url == "3" || url == "4" || url == "5")
        {
            targetImage.sprite = Resources.Load<Sprite>("AvatarIcon/" + url);
            return true;
        }
        return false;
    }

    // 加载头像（带缓存）
    public void LoadAvatar(string url, Image targetImage)
    {


        if (CheckURL(url, targetImage))
        {
            return;
        }
        Debug.Log("准备下载头像");
        if (targetImage == null) return;
        if (string.IsNullOrEmpty(url))
        {
            targetImage.sprite = defaultAvatar;
            return;
        }
        Debug.Log("准备查看缓存");
        // 检查缓存
        if (avatarCache.TryGetValue(url, out Sprite cachedSprite))
        {
            targetImage.sprite = cachedSprite;
            return;
        }
        Debug.Log("开始下载" + "当前的链接" + url + "当前的图片是否为空" + targetImage.name);
        // 下载头像
        StartCoroutine(DownloadAvatar(url, targetImage));
    }

    private IEnumerator DownloadAvatar(string url, Image targetImage)
    {

        UnityWebRequest request = new UnityWebRequest();
        request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error" + request.error);
            targetImage.sprite = defaultAvatar;


        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);


            Sprite avatarSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));


            targetImage.sprite = avatarSprite;
        }


    }


}