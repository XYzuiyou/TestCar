using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPanel : MonoBehaviour
{
    public Button QuitBtn;
    public Button BlackBG;

    public Transform RankingPart;
    public GameObject Panel;

    public UniversalScaleAnimation animator;

    public RankingPrefab CurrentRankingPrefab;

    public List<RankItem> RankingItemList = new List<RankItem>();

    public GameObject LoadDes;

    public Transform Part;
    public void SetUI()
    {

        RankingItemList.Clear();
        Panel = transform.Find("Panel").gameObject;
        Part = Panel.transform.Find("Part");
        if (Part.childCount > 0)
        {
            GameManager.gam.DeathChild(Part);
        }
        GameObject RankingPrefab = Instantiate<GameObject>(Resources.Load<GameObject>(LoadAssetManager.load.LoadSprite(LoadType.UIItem) + "RankingPrefab"));

        RankingPrefab.transform.SetParent(Part);
        RankingPrefab.transform.localPosition = Vector3.zero;
        RankingPrefab.transform.localEulerAngles = Vector3.zero;

        RankingPrefab.transform.localScale = Vector3.one;
        CurrentRankingPrefab = RankingPrefab.GetComponent<RankingPrefab>();
        QuitBtn = transform.Find("Panel/QuitBtn").GetComponent<Button>();
            BlackBG = transform.Find("BlackBG").GetComponent<Button>();
        LoadDes = transform.Find("Panel/LoadDes").gameObject;
        RankingPart = transform.Find("Panel/Mask/Part");
        animator = GetComponent<UniversalScaleAnimation>();
        animator.PlaySequence1(Panel, () => {

        });

        LoadDes.gameObject.SetActive(true);
        WXHttpManager.Instance.RequestRankList(WXHttpManager.Instance.UserID);
        Invoke("HideLoadDes", 4);

        InitData();

        CancelBtn();

        BindingBtn();
    }


    public void HideLoadDes()
    {
        LoadDes.gameObject.SetActive(false);

    }
    public void InitData()
    {

        WXHttpManager.Instance.OnRankListLoadeds += OnRankListReceived;
    }

    public void OnDestroy()
    {
        WXHttpManager.Instance.OnRankListLoadeds -= OnRankListReceived;
    }

    // ==========================
    // 排行榜数据回来时自动调用
    // ==========================
    void OnRankListReceived(List<RankItem> rankingList)
    {
        Debug.Log("✅ 我收到了排行榜！总数：" + rankingList.Count);
        CancelInvoke("HideLoadDes");
        HideLoadDes();
        if (RankingPart.transform.childCount > 0)
        {
            GameManager.gam.DeathChild(RankingPart);
        }

        if (rankingList != null)
        {
            // 成功获取数据，可在此处处理（如打印、额外逻辑）
            Debug.Log($"排行榜数据获取成功，共{rankingList.Count}条数据");

            rankingList.ForEach(i => RankingItemList.Add(i));

            // 示例：遍历数据
            for (int i = 0; i < rankingList.Count; i++)
            {

                print("遍历排行榜数据++=" + i);
                CreateItem(rankingList[i]);
            }
            GameManager.gam.SetUIRectTransform(RankingPart.gameObject, rankingList.Count * 109, false);

            LoadCurrentRank();
        }
        else
        {
            // 数据获取失败，可显示提示


            GameManager.gam.SetUIRectTransform(RankingPart.gameObject, 109, false);
            LoadCurrentRank(true);

            Debug.LogError("排行榜数据长度为" + rankingList.Count);
        }
    }


    /// <summary>
    /// 加载自身的排名
    /// </summary>
    public void LoadCurrentRank(bool Value=false )
    {
        if (RankingItemList.Count > 0)
        {
            for (int i = 0; i < RankingItemList.Count; i++)
            {

                if (RankingItemList[i].isCurrentUser)
                {
                    if (RankingItemList[i].maxScore <= 0)
                    {
                        RankingItemList[i].rank = 0;  //如果积分小于0 那么就设置成未上榜
                    }

                    //如果当前是用户数据的话
                    CurrentRankingPrefab.SetUI(RankingItemList[i]);
                    Debug.Log("数据在排行榜里面");
                    return;
                }
        
            }
        }
        
                    Debug.Log("数据不存在排行榜里面");


        RankItem item = new RankItem();
        item.rank = 0;
        item.isCurrentUser = false; 
        item.maxScore = WorldData.CurrentLevelNum;
        item.nickName = WorldData.currentUserName;
        item.avatarUrl = WorldData.currentUserAvatar;
        CurrentRankingPrefab.SetUI(item);
        if (Value)
        {
            item.rank = 1;
            CreateItem(item);
        }



    }

    public void CreateItem(RankItem item)
    {
        if (item.maxScore <= 0)
        {
            return;
        }


        GameObject RankingPrefab = Instantiate<GameObject>(Resources.Load<GameObject>(LoadAssetManager.load.LoadSprite(LoadType.UIItem) + "RankingPrefab"));

        RankingPrefab.transform.SetParent(RankingPart);
        RankingPrefab.transform.localPosition = Vector3.zero;
        RankingPrefab.transform.localScale = Vector3.one;
        RankingPrefab.transform.localEulerAngles = Vector3.zero;

        RankingPrefab.GetComponent<RankingPrefab>().SetUI(item);


    }


    public void BindingBtn()
    {
        QuitBtn.onClick.AddListener(HidePanel);
        BlackBG.onClick.AddListener(HidePanel);
    }

    public void CancelBtn()
    {
        QuitBtn.onClick.RemoveListener(HidePanel);
        BlackBG.onClick.RemoveListener(HidePanel);

    }
    public void HidePanel()
    {

        // 播放序列2：1→1.1→0.7
        animator.PlaySequence2(Panel, () => {
            UIManager.ui.DeathPanel(UIManager.UIType.RankingPanel);

        });
  

    }
}
