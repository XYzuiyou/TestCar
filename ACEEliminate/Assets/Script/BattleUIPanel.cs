    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIPanel : MonoBehaviour
{
    public Button SettingBtn;
    public Button Skill1;
    public GameObject Skill1_Des;
    public Button Skill2;
    public Button Skill3;
    public static BattleUIPanel battle;
    public GameObject BG;


    public Image GameBG;  //游戏背景
    public GameObject ComboPos;
    public Transform ScenePart;
    public Button VideoBtn;
    public Button QuitBtn;

    public Image RewardIcon;  //连击奖励
    public Image Icon1;
    public Image Icon2;
    public Image Icon3;
    public Image Icon4;
    public List<Image> IconList = new List<Image>();

    public List<GameObject> CardPosList = new List<GameObject>();
    public Image CurrentCard;
    public Button AnyCard;
    public Text Num;

    public Transform CurrentCardPart;    //当前的卡牌的父级
    public Transform CurrentReservePart;   ///当前备用卡牌的父级
    public Transform CreateCardPos;   //获得卡牌5张的生成位置
    public Transform SelectCardPart;   //当前选择的卡牌移动的的父级 


    public Text LevelNum;

    public GameObject Anim1;
    public GameObject Anim2;
    public GameObject Anim3;

    public GameObject Skill1Effect;
    public GameObject Skill2Effect;

    public GameObject Skill3Effect;

    public Text DesText;

    public Image Pos;

    public GameObject DesPanel;
    public GameObject DesIcon1;
    public GameObject DesIcon2;
    private void Awake()
    {
        battle = this;
    }
    public void SetUI()
    {
        SettingBtn = transform.Find("BG/SettingBtn").GetComponent<Button>();
        LevelNum = transform.Find("LevelNum").GetComponent<Text>();

        Pos = transform.Find("Pos").GetComponent<Image>();
        CurrentCardPart = transform.Find("BG/CurrentCardPart");
        CreateCardPos = transform.Find("BG/CreateCardPos");
        SelectCardPart = transform.Find("BG/SelectCardPart");
        CurrentReservePart = transform.Find("BG/CurrentReservePart");
        Skill1 = transform.Find("BG/Skill1").GetComponent<Button>();
        Skill1_Des = Skill1.transform.Find("Des").gameObject;
        Skill1_Des.gameObject.SetActive(false);
        Skill2 = transform.Find("BG/Skill2").GetComponent<Button>();
        Skill3 = transform.Find("BG/Skill3").GetComponent<Button>();

    

        BG = transform.Find("BG").gameObject;
     
        QuitBtn = BG.transform.Find("QuitBtn").GetComponent<Button>();


           GameBG = transform.Find("GameBG").GetComponent<Image>();  //游戏背景
        ComboPos = BG.transform.Find("ComboPos").gameObject;
        ScenePart = transform.Find("ScenePart");
      VideoBtn = BG. transform.Find("VideoBtn").GetComponent<Button>();
      QuitBtn = BG.transform.Find("QuitBtn").GetComponent<Button>();

      RewardIcon = BG.transform.Find("ComboBG/RewardIcon").GetComponent<Image>();  //连击奖励
      Icon1 =  transform.Find("BG/ComboBG/Icon1").GetComponent<Image>();
      Icon2 = transform.Find("BG/ComboBG/Icon2").GetComponent<Image>();
      Icon3 = transform.Find("BG/ComboBG/Icon3").GetComponent<Image>();
      Icon4 = transform.Find("BG/ComboBG/Icon4").GetComponent<Image>();
        IconList.Add(Icon1);
        IconList.Add(Icon2);
        IconList.Add(Icon3);
        IconList.Add(Icon4);


        CurrentCard = BG.transform.Find("CurrentCard").GetComponent<Image>();
      AnyCard = BG.transform.Find("AnyCard").GetComponent<Button >();
      Num = AnyCard. transform.Find("NumBG/Num").GetComponent<Text>();

        Anim1 =transform.Find("TextAnimPart/Anim1").gameObject;
        Anim2 =transform.Find("TextAnimPart/Anim2").gameObject;
        Anim3 =transform.Find("TextAnimPart/Anim3").gameObject;
      Skill1Effect = transform.Find("EffectPart/Skill1").gameObject;
        Skill2Effect = transform.Find("EffectPart/Skill2").gameObject;
       Skill3Effect = transform.Find("EffectPart/Skill3").gameObject;

        DesText = transform.Find("DesPanel/Panel/DesText").GetComponent<Text>();
        DesPanel = transform.Find("DesPanel").gameObject;
        DesPanel.gameObject.SetActive(false);

        DesIcon1 = DesPanel.transform.Find("Icon1").gameObject;
        DesIcon2 = DesPanel.transform.Find("Icon2").gameObject;
        InitData();
        UpdateUI();

        CancelBtn();

        BindingBtn();
    }

    public void InitData()
    {

        HideSkillDes();
        UpdateAnyCardNum();

        if (WorldData.IceState == 0)
        {
            if (WorldData.CurrentSelectLevelID == 30)
            {
                DesPanel.gameObject.SetActive(true);
                DesText.text = "冻结的卡牌无法被选中\n执行三个步骤后即可解锁";
                WorldData.IceState = 1;
                DesIcon1.gameObject.SetActive(true);
                DesIcon2.gameObject.SetActive(false );
            }
        }
        if (WorldData.Up_MoveState == 0)
        {
            if (WorldData.CurrentSelectLevelID == 15)
            {
                DesPanel.gameObject.SetActive(true);

                DesText.text = "带有箭头的卡牌会在\n执行步骤后更改卡牌的点数";
                WorldData.Up_MoveState = 1;
                DesIcon1.gameObject.SetActive(false);
                DesIcon2.gameObject.SetActive(true);
            }
        }
    }


    public void UpdateUI()
    {
        InitBG();
        LevelNum.text = "第"+WorldData.CurrentSelectLevelID + "关";
    }
    public void InitBG()
    {
        if (WorldData.CurrentUserBGID == 1)
        {
            GameBG.sprite = Resources.Load<Sprite>("BG1");
        }
        else if (WorldData.CurrentUserBGID == 2)
        {
            GameBG.sprite = Resources.Load<Sprite>("BG2");

        }
        else if (WorldData.CurrentUserBGID == 3)
        {
            GameBG.sprite = Resources.Load<Sprite>("BG3");

        }
    }

    public void BindingBtn()
    {
        SettingBtn.onClick.AddListener(ShowSettingPanel);
        Skill1.onClick.AddListener(delegate { UserSkill(1); });
        Skill2.onClick.AddListener(delegate { UserSkill(2); });
        Skill3.onClick.AddListener(delegate { UserSkill(3); });
        QuitBtn.onClick.AddListener(HideSkillDes);
        AnyCard.onClick.AddListener(ExcuteAnyCard);
        VideoBtn.onClick.AddListener(VideoGetFiveCard);
    }

    public void CancelBtn()
    {
        SettingBtn.onClick.RemoveAllListeners();
        Skill1.onClick.RemoveAllListeners();
        Skill2.onClick.RemoveAllListeners();
        Skill3.onClick.RemoveAllListeners();
        QuitBtn.onClick.RemoveAllListeners();
        AnyCard.onClick.RemoveAllListeners();
        VideoBtn.onClick.RemoveAllListeners();

    }


    /// <summary>
    /// 更新万能卡牌的数量
    /// </summary>
    public void UpdateAnyCardNum()
    {
        Num.text = "x"+BattleManager.battle.AnyCardNum.ToString();

    }


    /// <summary>
    ///重置连击数据
    /// </summary>
    public void ResetComboData(bool Value)
    {
        if (Value == false)
        {
            RewardIcon.sprite = Resources.Load<Sprite>("Reward1");
        }
            for (int i = 0; i < IconList.Count; i++)
        {
            IconList[i].enabled = false;
        }
    }
    public void ShowSettingPanel()
    {
        GameObject Panel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.SettingPanel,true);
        Panel.GetComponent<SettingPanel>().SetUI();
    }

    public void GoToMain()
    {

        if (BattleManager.battle.GameLose)
        {
            return;
        }
        BattleManager.battle.GoToMain();
    }

    public void VideoEvemt()
    {

        DesManager.des.CreateDes("调用广告跳关事件");

    }

    public void AgainGame()
    {

        if (BattleManager.battle.GameLose)
        {
            return;
        }

        BattleManager.battle.AgainGame();
    }



    public void UpdateSkillDes()
    {

        if (ModelManager.model.CreateCardList.Count <= 0)
        {
            if (WorldData.CurrentSelectLevelID == 1 || WorldData.CurrentSelectLevelID == 2)
            {
                return;
            }
            //如果没有卡牌了
            List<GameObject> ModelList = BattleManager.battle.GetReadCardNum();
            if (ModelList.Count > 0)
            {
                for (int i = 0; i < ModelList.Count; i++)
                {
                    if (BattleManager.battle.CheckPokerTypeAdjacent(BattleManager.battle.CurrentCard.CurrentType, ModelList[i].GetComponent<CardPrefab>().cardType))
                    {
                        //如果符合条件的话就退出
                        if (Skill1_Des.gameObject.activeInHierarchy == true)
                        {
                            Skill1_Des.gameObject.SetActive(false);
                        }
                        return;
                    }

                }

                //如果都不符合条件
                if (Skill1_Des.gameObject.activeInHierarchy == false)
                {
                    Skill1_Des.gameObject.SetActive(true);
                }

            }
            else
            {
                if (Skill1_Des.gameObject.activeInHierarchy == false)
                {
                    Skill1_Des.gameObject.SetActive(true);
                }
            }

        }
        else
        {
            if (Skill1_Des.gameObject.activeInHierarchy == true)
            {
                Skill1_Des.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 检查当前的游戏是否胜利
    /// </summary>
    public void CheckGameEnd()
    {
    //    CheckGameLose();
        CheckGameWin();

   

    }

    public void CheckGameLose()
    {

     
        //否则游戏失败
        BattleManager.battle.DelayShowLosePanel();
    }

    public void CheckGameWin()
    {
        GameManager.gam.CleanupGameObjectList(ModelManager.model.CardList);

        if (ModelManager.model.CardList.Count <= 0)
        {
            //代表游戏胜利
            BattleManager.battle.DelayShowWinPanel();
        }


    }
    public void UserSkill(int ID)
    {
        if (WorldData.NewbieState == 0|| WorldData.NewbieState == 1)
        {
            return;
        }
        if (BattleManager.battle.isSkill)
        {
            DesManager.des.CreateDes("请等待其他技能结束");
            return;
        }


        GameObject Panel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.SkillPanel,true);
        Panel.GetComponent<SkillPanel>().SetUI(ID);
   
    }


    /// <summary>
    /// 执行万能卡牌
    /// </summary>
    public void ExcuteAnyCard()
    {

        ///如果当前不处于移动备用卡牌的状态下
        if (!BattleManager.battle.isMoveCard)
        {
            if (!BattleManager.battle.isMoveAnyCard)  //如果当前不处于获得万能卡牌的状态下
            {

                ///当前的卡牌不能是万能卡牌
                if (BattleManager.battle.CurrentCard.GetComponent<CurrentCardPrefab>().CurrentType != PokerCardsType.Any)
                {
                    ///当前没有在执行使用万能卡牌状态
                    if (BattleManager.battle.isUserAnyCard == false)
                    {
                        if (BattleManager.battle.AnyCardNum > 0)
                        {
                            BattleManager.battle.isUserAnyCard = true;
                            MusicManager.music.PlayMusic(3);

                            BattleManager.battle.AnyCardNum -= 1;
                            UpdateAnyCardNum();
                            GameObject Data = Instantiate<GameObject>(Resources.Load<GameObject>("CurrentCardPrefab"));
                            Data.transform.SetParent(BattleUIPanel.battle.CurrentCardPart);
                            Data.transform.position = BattleUIPanel.battle.AnyCard.transform.position;   //在连击奖励位置生成
                            Data.transform.localScale = Vector3.one;
                            Data.GetComponent<CurrentCardPrefab>().SetUI(PokerCardsType.Any );

                            BattleUIPanel.battle.AnyCard.transform.Find("Des").gameObject.SetActive(false );
                            BattleManager.battle.NewBieState = 4;
                            StartCoroutine(MoveItem(Data,BattleUIPanel.battle.CurrentCard.gameObject));
                        }
                    }
                }
            }
        }
    }



    public IEnumerator MoveItem(GameObject CurrentModel, GameObject Target)
    {
        // 检查参数是否有效
        if (CurrentModel == null || Target == null)
        {
            Debug.LogError("移动对象或目标对象为空！");
            yield break;
        }

        // 移动总时间（可根据需要调整）
        float moveDuration = 0.3f;
        float elapsedTime = 0f;

        // 记录起始位置
        Vector3 startPosition = CurrentModel.transform.position;
        // 目标位置
        Vector3 targetPosition = Target.transform.position;

        // 逐步移动对象
        while (elapsedTime < moveDuration)
        {
            // 计算插值比例（0到1之间）
            float t = elapsedTime / moveDuration;
            // 可以使用Mathf.SmoothStep实现更平滑的缓动效果
            t = Mathf.SmoothStep(0f, 1f, t);

            // 计算当前位置并赋值
            CurrentModel.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // 累加已用时间
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        // 确保最终位置准确
        CurrentModel.transform.position = targetPosition;
        if (BattleManager.battle.CurrentCard.gameObject != null)
        {
            Destroy(BattleManager.battle.CurrentCard.gameObject);
        }
        BattleManager.battle.CurrentCard = CurrentModel.GetComponent<CurrentCardPrefab>();

        BattleManager.battle.isUserAnyCard = false ;

    }


    public void HideSkillDes()
    {
        BG.gameObject.SetActive(true);
        BattleManager.battle.isSkill1 = false;
        BattleManager.battle.isSkill2 = false;


    }



    /// <summary>
    /// 视频广告活获得5张卡牌
    /// </summary>
    public void VideoGetFiveCard()
    {
        if (WorldData.NewbieState == 0 || WorldData.NewbieState == 1)
        {
            return;
        }
        ///当前没有在使用卡牌
        if (!BattleManager.battle.isMoveCard)
        {

            //当前没有在获得卡牌
            if (!BattleManager.battle.GetCardEvemt)
            {
                if (ModelManager.model.CreateCardList.Count <= 15)
                {
                    //调用广告事件
                    VideoFiveCardRewardEvemt();
                }
                else
                {
                    DesManager.des.CreateDes("当前卡牌特别多了，请先使用");
                    
                }
            }
        }
    }



    /// <summary>
    /// 看广告获得5张卡牌的事件
    /// </summary>
    public void VideoFiveCardRewardEvemt()
    {
        StartCoroutine(BattleManager.battle.GetNumCard(5));


    }

    public int IDValue = 0;
    public void PlayTextEffect( )
    {
        if (IDValue >= 3)
        {
            IDValue = 0;
        }

        switch (IDValue)
        {
            case 0:
                Anim1.gameObject.SetActive(true);
                Anim1.GetComponent<Animator>().Play(1,0,0);
                break;
            case 1:
                Anim2.gameObject.SetActive(true);
                Anim2.GetComponent<Animator>().Play(1, 0, 0);
                break;
            case 2:
                Anim3.gameObject.SetActive(true);
                Anim3.GetComponent<Animator>().Play(1, 0, 0);
                break;
        }

        IDValue++;
    }

    public void PlaySkillEffect(int ID)
    {

        switch (ID)
        {
            case 1:
                Skill1Effect  .gameObject.SetActive(true);
                Skill1Effect.GetComponent<Animator>().Play(1, 0, 0);
                break;
            case 2:
                Skill2Effect.gameObject.SetActive(true);
                Skill2Effect.GetComponent<Animator>().Play(1, 0, 0);
                break;
            case 3:
                Skill3Effect.gameObject.SetActive(true);
                Skill3Effect.GetComponent<Animator>().Play(1, 0, 0);
                break;
        }

    }
}
