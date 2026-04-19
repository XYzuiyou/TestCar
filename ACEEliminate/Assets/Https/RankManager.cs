using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class RankManager : MonoBehaviour
{
    public static RankManager rank;
    private void Awake()
    {
        rank = this;
    }

    public WXUserInfoButton myUserInfoButton;
    public bool isGet;  //当前是否已经获取一次了

    /// <summary>
    /// 获得用户的头像和名称
    /// </summary>
    public void getAcataAndNickName(Image img)
    {
        if (!isGet)
        {
            WX.GetSetting(new GetSettingOption()
            {
                success = (result) =>
                {
                    if (result.authSetting.ContainsKey("scope.userInfo") && result.authSetting["scope.userInfo"])
                    {
                        WX.GetUserInfo(new GetUserInfoOption()
                        {
                            success = (res) =>
                            {
                                Debug.Log("用户信息获取成功" + res.userInfo.nickName + "  " + res.userInfo.avatarUrl);
                                isGet = true;  //获取一次了
                                SetUserNameAndUrl(res.userInfo.nickName, res.userInfo.avatarUrl);
                            }
                        });
                    }
                    else
                    {
                        // 如果当前没有授权就手动获取
                        RectTransform rectTransform = img.rectTransform;
                        Vector3[] corners = new Vector3[4];
                        rectTransform.GetWorldCorners(corners);

                        Canvas canvas = img.GetComponentInParent<Canvas>();
                        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

                        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
                        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);

                        int x = (int)bottomLeft.x;
                        int y = (int)(Screen.height - topRight.y);
                        int width = (int)(topRight.x - bottomLeft.x);
                        int height = (int)(topRight.y - bottomLeft.y);

                        myUserInfoButton = WX.CreateUserInfoButton(
                            x, y, width, height,
                            "zh_CN",  // 语言设置为中文
                            true      // 通常需要凭证
                        );

                        myUserInfoButton.OnTap((WXUserInfoResponse userInfo) =>
                        {
                            if (userInfo.errMsg.Contains("ok"))
                            {
                                Debug.Log("用户点击了同意");
                                Debug.Log("当前授权了");
                                Debug.Log(userInfo.userInfo.nickName + userInfo.userInfo.avatarUrl);
                                SetUserNameAndUrl(userInfo.userInfo.nickName, userInfo.userInfo.avatarUrl);
                                isGet = true;
                                HideWXMouseBtn();
                                MainMenuPanel.main.ShowRankingPanel();
                            }
                            else
                            {
                                Debug.Log("用户点击了拒绝");
                                HideWXMouseBtn();
                                MainMenuPanel.main.ShowRankingPanel();
                                SetUserNameAndUrl("匿名用户", GetRandeAvatar());
                            }
                        });
                    }
                    Debug.Log("调用成功");
                },
                fail = (error) =>
                {
                    Debug.Log("调用失败");
                },
                complete = (result) =>
                {
                    Debug.Log("调用完成");
                },
                withSubscriptions = false
            });
        }
        else
        {
            HideWXMouseBtn();
        }
    }

    public void HideWXMouseBtn()
    {
        // 核心：先判断按钮是否存在（不为null），再执行销毁
        if (myUserInfoButton != null)
        {
            try
            {
                // 1. 先隐藏按钮（避免销毁前仍显示在界面上）
                myUserInfoButton.Hide();

                // 2. 调用微信SDK的销毁方法（释放底层资源）
                myUserInfoButton.Destroy();

                // 3. 将引用置为null（关键：避免后续调用时误判为非空）
                myUserInfoButton = null;

                Debug.Log("用户信息按钮已成功销毁");
            }
            catch (Exception e)
            {
                Debug.LogError($"销毁按钮时发生异常：{e.Message}");
                // 即使出错，仍强制将引用置为null，避免下次调用报错
                myUserInfoButton = null;
            }
        }
        else
        {
            Debug.Log("用户信息按钮已不存在，无需重复销毁");
        }
    }

    public void SetUserNameAndUrl(string currentUserName, string currentUserAvatar, bool isSend = true)
    {
        WorldData.currentUserName = currentUserName;
        WorldData.currentUserAvatar = currentUserAvatar;

        WXHttpManager.Instance.SaveUserInfo(WXHttpManager.Instance.UserID, WorldData.currentUserName, WorldData.currentUserAvatar);
    }

    /// <summary>
    /// 获得随机的头像
    /// </summary>
    /// <returns></returns>
    public  string GetRandeAvatar()
    {
        int Num = UnityEngine.Random.Range(0, 5);
        string Path = "";
        switch (Num)
        {
            case 0:
                Path = "1";
                break;
            case 1:
                Path = "2";
                break;
            case 2:
                Path = "3";
                break;
            case 3:
                Path = "4";
                break;
            case 4:
                Path = "5";
                break;
        }

        return Path;

    }
}