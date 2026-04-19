using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class MainMenuPanel : MonoBehaviour
{

    public Button PlayBtn;
    public Button RankingsBtn;
    public Button ShareBtn;
    public Button ShopBtn;

    public static MainMenuPanel main;
    public void SetUI()
    {
             PlayBtn = transform.Find("BG/PlayBtn").GetComponent<Button>();
      RankingsBtn = transform.Find("BG/RankingsBtn").GetComponent<Button>();
        ShareBtn = transform.Find("BG/ShareBtn").GetComponent<Button>();
        ShopBtn = transform.Find("BG/ShopPanel").GetComponent<Button>();
        InitData();
        UpdateUI();

        CancelBtn();

        BindingBtn();
    }

    public void InitData()
    {
        if (WorldData.BGState.ContainsKey(1))
        {
            WorldData.BGState[1] = 1;
        }
        if (WorldData.GroundState.ContainsKey(1))
        {
            WorldData.GroundState[1] = 1;
        }

        if (WorldData.CurrentUserGroundID == 0)
        {
            WorldData.CurrentUserGroundID = 1;
        }
        if (WorldData.CurrentUserBGID == 0)
        {
            WorldData.CurrentUserBGID = 1;
        }
    }


    public void TesetValue()
    {
        for (int i = 0; i < 101; i++)
        {
            if (WorldData.LevelState.ContainsKey(i))
            {
                WorldData.LevelState[i] = 1;

            }
            else
            {
                WorldData.LevelState.Add(i,1);
            }
        }
    }
    public void UpdateUI()
    {
        RankManager.rank.getAcataAndNickName(RankingsBtn.GetComponent<Image>());
    }

    public void BindingBtn()
    {
        PlayBtn.onClick.AddListener(ShowPlayGame);
        RankingsBtn.onClick.AddListener(ShowRankingPanel);
        ShareBtn.onClick.AddListener(ShareEvemt);
        ShopBtn.onClick.AddListener(ShowShopPanel);
    }

    public void CancelBtn()
    {
        PlayBtn.onClick.RemoveAllListeners();
        RankingsBtn.onClick.RemoveAllListeners();
        ShareBtn.onClick.RemoveAllListeners();
        ShopBtn.onClick.RemoveAllListeners();
    }
    public void Textssss()
    {

        WorldData.CurrentLevelNum = 40;
        if (WorldData.CurrentLevelNum >= 40)
        {
            if (WorldData.FinishedState == 0)
            {
                WorldData.FinishedState = 1;
                WorldData.FinishedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }




    public void ShowPlayGame()
    {
        //开始游戏
        GameObject Panel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.LevelSelectionPanel);
        Panel.GetComponent<LevelSelectionPanel>().SetUI();
        RankManager.rank.HideWXMouseBtn();

    }


    public void ShowRankingPanel()
    {
        ///显示排行榜
        GameObject Panel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.RankingPanel);
        Panel . GetComponent<RankingPanel>().SetUI();


    }


    /// <summary>
    /// 显示商店界面
    /// </summary>
    public void ShowShopPanel ()
    {
        ///显示排行榜
        GameObject Panel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.ShopPanel);
        Panel.GetComponent<ShopPanel>().SetUI();


    }


    /// <summary>
    /// 分享事件
    /// </summary>
    public void ShareEvemt()
    {
        EvemtManager.evemt.ShareEvemt();

    }


}

     

