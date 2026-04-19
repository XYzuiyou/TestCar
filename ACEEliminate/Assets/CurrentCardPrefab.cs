using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentCardPrefab : MonoBehaviour,IPointerDownHandler
{
    public PokerCardsType CurrentType;
    public PokerCardsColor CurrentColor;
    public Image CardImg;   //卡牌图片

    public int PosID;  //当前的位置的ID
    public GameObject CurrentPos;

    public bool CurrentValue;

    public bool isRead;  //当前是否已经准备就绪可以被点击
    /// <summary>
    /// 设置参数
    /// </summary>
    public void SetUI(PokerCardsType type,PokerCardsColor color,bool CurrentValue)
    {
        CardImg = GetComponent<Image>();
        this.CurrentValue = CurrentValue;
        CurrentType = type;
        CurrentColor = color;
        InitData();
    }



    public void LoadDes()
    {
        GameObject Model = Instantiate<GameObject>(Resources.Load<GameObject>("Des"));
        Model.transform.SetParent(transform);
        Model.transform.localPosition = Vector3.zero;
        Model.transform.localScale = Vector3.one;
        BattleManager.battle.DesList.Add(Model);
        Model.transform.parent = transform.parent;
        Model.GetComponent<RectTransform>().SetSiblingIndex(Model.transform.parent.childCount - 1);

    }
    /// <summary>
    /// 当前是万能卡牌
    /// </summary>
    public void SetUI(PokerCardsType type)
    {
        CardImg = GetComponent<Image>();

        CurrentValue = false;
        CurrentType = type;
        InitData();
    }
    public void InitData()
    {
        if (CurrentValue)
        {
            LoadCardImg();
         
        }
        else
        {
            InitBG();
        }
    }
    /// <summary>
    /// 初始化背景图片
    /// </summary>
    public void InitBG()
    {

        if (CurrentType != PokerCardsType.Any)  //如果不等于万能卡牌的话
        {
            if (WorldData.CurrentUserGroundID == 1)
            {
                CardImg.sprite = Resources.Load<Sprite>("pk/pk_back_01");
            }
            else if (WorldData.CurrentUserGroundID == 2)
            {
                CardImg.sprite = Resources.Load<Sprite>("pk/pk_back_02");

            }
            else if (WorldData.CurrentUserGroundID == 3)
            {
                CardImg.sprite = Resources.Load<Sprite>("pk/pk_back_03");

            }
        }
        else
        { 
                CardImg.sprite = Resources.Load<Sprite>("AnyIcon");

        }
    }
    /// <summary>
    /// 加载新的卡牌图片
    /// </summary>
    public void LoadCardImg()
    {
        string Path = "";

        switch (CurrentColor)
        {
            case PokerCardsColor.Hearts:
                Path = "h_x_";
                break;
            case PokerCardsColor.Diamonds:
                Path = "h_f_";
                break;
            case PokerCardsColor.Clubs:
                Path = "d_m_";
                break;
            case PokerCardsColor.Spades:
                Path = "d_t_";
                break;
        }

        switch (CurrentType)
        {
            case PokerCardsType.A:
                Path += "14";
                break;
            case PokerCardsType.To:
                Path += "2";
                break;
            case PokerCardsType.Three:
                Path += "3";
                break;
            case PokerCardsType.Four:
                Path += "4";
                break;
            case PokerCardsType.Five:
                Path += "5";
                break;
            case PokerCardsType.Six:
                Path += "6";
                break;
            case PokerCardsType.Seven:
                Path += "7";
                break;
            case PokerCardsType.Eight:
                Path += "8";
                break;
            case PokerCardsType.Nine:
                Path += "9";
                break;
            case PokerCardsType.Ten:
                Path += "10";
                break;
            case PokerCardsType.J:
                Path += "11";
                break;
            case PokerCardsType.Q:
                Path += "12";
                break;
            case PokerCardsType.K:
                Path += "13";
                break;
        }
        CardImg.sprite = Resources.Load<Sprite>("pk/" + Path);
    }
    public void SetPosID(int ID,GameObject Pos)
    {
        PosID = ID;
        CurrentPos = Pos;
    }
    /// <summary>
    /// 当前的卡牌点击事件   
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CurrentValue)
        {
            if (isRead)
            {
                if (!BattleManager.battle.isSelect)
                {
                    if (!BattleManager.battle.isMoveCard)
                    {
                        if (!BattleManager.battle.GetCardEvemt)  //如果当前不处于获得卡牌事件 
                        {
                            //如果当前不处于选择卡牌状态
                            if (ModelManager.model.GetCardIndex(this.gameObject) == 0)
                            {
                                if (WorldData.NewbieState == 1)
                                {
                                    if (WorldData.CurrentSelectLevelID == 2)
                                    {
                                        if (BattleManager.battle.NewBieState == 0)
                                        {
                                            return;
                                        }else 
                                        if (BattleManager.battle.NewBieState == 1)
                                        {
                                            transform.Find("Des").gameObject.SetActive(false);
                                            CurrentType = PokerCardsType.Six;
                                            UIManager.ui.DeathPanel(UIManager.UIType.NewbiePanelTo);
                                            BattleManager.battle.NewBieState = 2;
                                        }else 
                                        if (BattleManager.battle.NewBieState == 2)
                                        {
                                            return;
                                        }
                                    }
                                }
                                BattleManager.battle.UserCardNum++;
                                if (BattleManager.battle.UserCardNum >= 5)
                                {
                                    BattleManager.battle.UserCardNum = 0;

                                    PokerCardsType type = BattleManager.battle.GetCurrentPokerType();
                                    if (type != PokerCardsType.Any)
                                    {
                                        CurrentType =type  ;
                                    } 
}

                                MusicManager.music.PlayMusic(3);

                                //如果下标等于0 点当前是第一个卡牌数据
                                BattleManager.battle.ResetComboReward(true);   //重置当前连击的奖励
                                StartCoroutine(ModelManager.model.MoveCardToCurrentPos(this.gameObject, BattleUIPanel.battle.CurrentCard.gameObject, 0.4f));
                                ModelManager.model.CheckCardPos();   //移动卡牌的位置
                                BattleManager.battle.PlayDownEvemt();  //当前玩家点击了

                            }
                        }
                    }
                }
            }
        }
    }
}
