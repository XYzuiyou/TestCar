using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;


public class BattleManager : MonoBehaviour
{
    public static BattleManager battle;


    public bool GameLose;  //当前游戏已经失败了

    public bool isFusion;   //当前是否处于融合状态

    public bool isDrag;  //当前是否可以拖拽





    public bool isSkill;  //当前是否处于技能状态
    public bool isSkill1;
    public bool isSkill2;
    public bool isSkill3;

    public GameObject BG;



    public int CardColorID;  //当前卡牌的花色ID


    public CurrentCardPrefab CurrentCard;   //当前场景上面的卡牌数据
    public bool isSelect;    //当前是否可以选择卡牌



    /// <summary>
    /// 连击奖励ID   0是单张卡牌  1是2张卡牌 2是万能卡牌
    /// </summary>
    public int ComboRewardID=0;

    /// <summary>
    /// 扑克牌卡牌的颜色
    /// </summary>
    public List<PokerCardsColor> PokerColorList = new List<PokerCardsColor>();

    public bool isMoveCard = false;  //当前是否在移动备用卡牌
    public bool GetCardEvemt = false;  //当前处于获得卡牌事件
    public bool isMoveAnyCard=false;  //当前处于移动万能卡牌状态  


    public bool isUserAnyCard = false;   //当前处于使用万能卡牌状态

    public int AnyCardNum = 0;  //万能卡牌的数量
    public int UserCardNum = 0;  //使用备用卡牌的输了

    public int NewBieSelectCardNum = -1;  //选择的卡牌的下标
    public int NewBieState = 0;   //当前的新手教程的状态


    public float IdleTime = 0;  //待机的时间
    public bool TriggerIdle;  //当前触发待机了
    public List<GameObject> DesList = new List<GameObject>();
    private void Awake()
    {
        battle = this;
    }



    public void ClearData()
    {
        isFusion = false;  //当前不处于融合状态
        isSkill = false;
        isSkill1 = false;
        isSkill2 = false;
        isSkill3 = false;
        CardColorID = 0;
        isSelect = false;
        TriggerIdle = false;
        GameLose = false;
        isDrag = false;
        isMoveCard = false;
        GetCardEvemt = false;
        isUserAnyCard = false;
        ComboRewardID = 0;
        AnyCardNum = 0;
        UserCardNum = 0;
        NewBieState = 0;
        NewBieSelectCardNum = -1;
        PokerColorList.Clear();
        ResetComboReward();
        IdleTime = 0;  //清空待机的时间
        UIManager.ui.DeathPanel(UIManager.UIType.NewbiePanelTo);
        if (DesList.Count > 0)
        {
            for (int i = 0; i < DesList.Count; i++)
            {
                Destroy(DesList[i]);
            }
        }
        DesList.Clear();

        if (WorldData.IceState == 1)
        {
            WorldData.IceState = 0;
        }
        if (WorldData.Up_MoveState == 1)
        {
            WorldData.Up_MoveState = 0;
        }
    }


    /// <summary>
    /// 获得当前场景上面即将刷新的卡片的类型
    /// </summary>
    /// <returns></returns>
    public PokerCardsType GetCurrentPokerType()
    {
        PokerCardsType type = PokerCardsType.Any;
        List<GameObject> ModelList = GetReadCardNum();
        if (ModelList.Count > 0)
        {
            GameObject Model = ModelList[Random.Range(0, ModelList.Count)];

            PokerCardsType CurrentType = GetNextOrUpType(Model.GetComponent<CardPrefab>().cardType);

            type = CurrentType;
        }
        return type;
    }

    /// <summary>
    /// 根据传递进来的卡片类型 获得他上一个类型或者下一个类型
    /// </summary>
    /// <returns></returns>
    public PokerCardsType GetNextOrUpType(PokerCardsType types)
    { 
        PokerCardsType type = PokerCardsType.Any;
        int Num = Random.Range(0,2);
        if (Num == 0)   //返回上一级的数据
        {
            switch (types)
            {
                case PokerCardsType.A:
                    type = PokerCardsType.To;
                    break;
                case PokerCardsType.To:
                    type = PokerCardsType.Three;
                    break;
                case PokerCardsType.Three:
                    type = PokerCardsType.Four;
                    break;
                case PokerCardsType.Four:
                    type = PokerCardsType.Five;
                    break;
                case PokerCardsType.Five:
                    type = PokerCardsType.Six;
                    break;
                case PokerCardsType.Six:
                    type = PokerCardsType.Seven;
                    break;
                case PokerCardsType.Seven :
                    type = PokerCardsType.Eight;
                    break;
                case PokerCardsType.Eight :
                    type = PokerCardsType.Nine ;
                    break;
                case PokerCardsType.Nine :
                    type = PokerCardsType.Ten ;
                    break;
                case PokerCardsType.Ten :
                    type = PokerCardsType.J ;
                    break;
                case PokerCardsType.J :
                    type = PokerCardsType.Q ;
                    break;
                case PokerCardsType.Q :
                    type = PokerCardsType.K ;
                    break;
                case PokerCardsType.K :
                    type = PokerCardsType.A;
                    break;
            }
        }
        else
        {
            switch (types)
            {
                case PokerCardsType.A:
                    type = PokerCardsType.K;
                    break;
                case PokerCardsType.To:
                    type = PokerCardsType.A;
                    break;
                case PokerCardsType.Three:
                    type = PokerCardsType.To;
                    break;
                case PokerCardsType.Four:
                    type = PokerCardsType.Three;
                    break;
                case PokerCardsType.Five:
                    type = PokerCardsType.Four;
                    break;
                case PokerCardsType.Six:
                    type = PokerCardsType.Five;
                    break;
                case PokerCardsType.Seven:
                    type = PokerCardsType.Six;
                    break;
                case PokerCardsType.Eight:
                    type = PokerCardsType.Seven ;
                    break;
                case PokerCardsType.Nine:
                    type = PokerCardsType.Eight;
                    break;
                case PokerCardsType.Ten:
                    type = PokerCardsType.Nine;
                    break;
                case PokerCardsType.J:
                    type = PokerCardsType.Ten;
                    break;
                case PokerCardsType.Q:
                    type = PokerCardsType.J ;
                    break;
                case PokerCardsType.K:
                    type = PokerCardsType.Q ;
                    break;
            }
        }

        return type;
    }
    /// <summary>
    /// 重置连击的奖励
    /// </summary>
    public void ResetComboReward(bool Value =false)
    {
        if (!Value)
        {
            ComboRewardID = 0;

        }
        if (BattleUIPanel.battle != null)
        {
            BattleUIPanel.battle.ResetComboData(Value );
        }
        PokerColorList.Clear();  //清空数据

    }

    /// <summary>
    /// 增加连击的次数
    /// </summary>
    public IEnumerator AddComboNum(PokerCardsColor  color)
    {

        if (WorldData.NewbieState == 0 || WorldData.NewbieState ==1)
        {
            if (WorldData.CurrentSelectLevelID == 1|| WorldData.CurrentSelectLevelID ==2)
            {
         //如果等于关卡1 就退出
                yield break;  //如果是新手教程就退出
            }
        }
        PokerColorList.Add(color);
        if (BattleUIPanel.battle != null)
        {
            BattleUIPanel.battle.IconList[PokerColorList.Count - 1].enabled = true;
            BattleUIPanel.battle.IconList[PokerColorList.Count - 1].sprite = Resources.Load<Sprite>(ColorGetIconPath(color));
        }
        yield return new WaitForSeconds(0.2f);
        if (PokerColorList.Count >= 4)
        {
            PokerColorList.Clear();  //清空数据
            //准备获得奖励
            BattleUIPanel.battle.PlayTextEffect();
            MusicManager.music.PlayMusic(8);

            switch (ComboRewardID)
            {
                case 0:
                  StartCoroutine(AddReserveCard(1));
                    break;
                case 1:
                    StartCoroutine(AddReserveCard(2));


                    break;
                case 2:
                    if (isMoveAnyCard == false)
                    {
                        if (!isUserAnyCard)
                        {
                            StartCoroutine(AddAnyCard()); //增加万能卡牌
                        }                    }
                        break;
            }

            ComboRewardID++;
            if (ComboRewardID >= 3)
            {
                ComboRewardID = 0;
            }
            ResetComboReward(true);

            switch (ComboRewardID)
            {
                case 0:
                   BattleUIPanel.battle. RewardIcon.sprite = Resources.Load<Sprite>("Reward1");
                    break;
                case 1:
                    BattleUIPanel.battle.RewardIcon.sprite = Resources.Load<Sprite>("Reward2");
                    break;
                case 2:
                    BattleUIPanel.battle.RewardIcon.sprite = Resources.Load<Sprite>("Reward3");
                    break;
            }
        }
    }





    /// <summary>
    /// 根据扑克牌的颜色或者对应图标的地址
    /// </summary>
    /// <returns></returns>
    public string ColorGetIconPath(PokerCardsColor color)
    {
        string Path = "Card1";
        switch (color)
        {
            case PokerCardsColor.Hearts:
                 Path = "Card1";

                break;
            case PokerCardsColor.Diamonds:
                Path = "Card2";

                break;
            case PokerCardsColor.Clubs:
                Path = "Card4";

                break;
            case PokerCardsColor.Spades:
                Path = "Card3";

                break;
        }
        return Path;
    }




    /// <summary>
    /// 增加相反的卡牌
    /// </summary>
    /// <param name="Num"></param>
    public IEnumerator AddReserveCard(int Num)
    {

        int PosID=0;  //下标位置
        for (int i = 0; i < Num; i++)
        {

            GameObject Data = Instantiate<GameObject>(Resources.Load<GameObject>("CurrentCardPrefab"));
            Data.transform.SetParent(BattleUIPanel.battle.CurrentReservePart);
            Data.transform.position = BattleUIPanel.battle.ComboPos.transform.position;   //在连击奖励位置生成
            Data.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Data.GetComponent<CurrentCardPrefab>().SetUI(ModelManager.model .RangeGetPokerType(), BattleManager.battle.GetCardColor(), false );
            if (PosID == 1)
            {
                
                    SetToPenultimatePosition(Data );
            }
            StartCoroutine(ModelManager.model.MoveItemAndScale(Data, ModelManager.model.CardPosList[PosID]));
            ModelManager.model.CreateCardList.Insert(PosID,Data);
  
            PosID++;
        }
        yield return new WaitForSeconds(0.61f);

        ModelManager.model.CheckCardPos();
    }
    /// <summary>
    /// 将指定物体设置到其同级层级中的倒数第二个位置
    /// </summary>
    /// <param name="target">要调整位置的物体</param>
    /// <returns>是否成功设置位置</returns>
    public  bool SetToPenultimatePosition( GameObject target,int PosIndex=2)
    {
        // 验证目标物体是否有效
        if (target == null)
        {
            Debug.LogError("目标物体不能为空！");
            return false;
        }

        Transform targetTransform = target.transform;
        Transform parent = targetTransform.parent;

        // 处理没有父物体的情况（根物体）
        if (parent == null)
        {
            // 根物体的父级是场景根节点
            Transform root = targetTransform.root;
            int siblingCount = root.childCount;

            return SetSiblingIndexSafely(targetTransform, siblingCount, root, PosIndex);
        }
        else
        {
            // 有父物体的情况
            int siblingCount = parent.childCount;

            return SetSiblingIndexSafely(targetTransform, siblingCount, parent, PosIndex);
        }
    }
    private  bool SetSiblingIndexSafely(Transform target, int siblingCount, Transform parent,int PosIndex)
    {
        // 计算目标索引
        int targetIndex;

        // 根据子物体数量确定合适的索引
        if (siblingCount <= 1)
        {
            // 当只有0或1个同级物体时，只能放在第一个位置
            targetIndex = 0;
            Debug.LogWarning("同级物体数量不足，已将物体设置到第一个位置");
        }
        else
        {
            // 正常情况：倒数第二个位置 = 总数量 - 2
            targetIndex = siblingCount - PosIndex;
        }

        // 执行设置
        target.SetSiblingIndex(targetIndex);
        return true;
    }

    /// <summary>
    /// 增加一张万能卡牌
    /// </summary>
    public IEnumerator AddAnyCard()
    {
        isMoveAnyCard = true;  
        GameObject Data = Instantiate<GameObject>(Resources.Load<GameObject>("CurrentCardPrefab"));
        Data.transform.SetParent(BattleUIPanel.battle.CurrentReservePart);
        Data.transform.position = BattleUIPanel.battle.ComboPos.transform.position;   //在连击奖励位置生成
        Data.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Data.GetComponent<CurrentCardPrefab>().SetUI(PokerCardsType.Any); //当前的类型是万能卡牌

        StartCoroutine(ModelManager.model.MoveItemAndScale(Data,BattleUIPanel.battle.AnyCard.gameObject));
        yield return new WaitForSeconds(0.61f);
        isMoveAnyCard = false;
        AnyCardNum++;  //数量增加1
        Destroy(Data);
        BattleUIPanel.battle.UpdateAnyCardNum();  //更新数量
    }
    /// <summary>
    /// 开始游戏事件
    /// </summary>
    /// <param name="ID"></param>
    public void PlayGame(int ID)
    {
       
        ClearData();

   
        LevelManager.level.PlayGame(ID);
    }


    /// <summary>
    /// 分享事件
    /// </summary>
    public void ShareEvemt()
    {
        EvemtManager.evemt.ShareEvemt();
    }


    private void Update()
    {
        if (LevelManager.level.isPlay)
        {

            if (WorldData.NewbieState == 0 || WorldData.NewbieState == 1)
            {
                return;
            }
            //如果当前的游戏开始了
            IdleTime += Time.deltaTime;  //时间开始增加
            if (IdleTime >= 15)
            {
                if (!TriggerIdle)
                {
                    //如果15秒钟玩家没有操作的话
                    TriggerIdle = true;
                    ExcuteGameDes();
                }
            }
        }
    }


    /// <summary>
    /// 清空待机时间的
    /// </summary>
    public void ClearIdleTime()
    {
        IdleTime = 0;
        TriggerIdle = false;
        if (DesList.Count > 0)
        {
            for (int i = 0; i < DesList.Count; i++)
            {
                Destroy(DesList[i]);
            }
        }
        DesList.Clear();
    }
    
    /// <summary>
    /// 执行游戏的提示
    /// </summary>
    public void ExcuteGameDes()
    {
        PokerCardsType type = CurrentCard.GetComponent<CurrentCardPrefab>().CurrentType;



        if (ModelManager.model.CardList.Count > 0)
        {
            for (int i = 0; i < ModelManager.model.CardList.Count; i++)
            {

                if (ModelManager.model.CardList[i].GetComponent<CardPrefab>().Excute)
                {

                    if (ModelManager.model.CardList[i].GetComponent<CardPrefab>().EffectType != CardEffectType.Ice)
                    {
                        if (CheckPokerTypeAdjacent(CurrentCard.CurrentType, ModelManager.model.CardList[i].GetComponent<CardPrefab>().cardType))
                        {
                            //如果其中一张卡牌的类型可以接上当前卡牌的类型的话
                            ///退出
                            ///
                            CardPrefab card = ModelManager.model.CardList[i].GetComponent<CardPrefab>();
                            card.LoadDes();
                            return;
                        }
                    }
                }
                }
        }

        if (ModelManager.model.CreateCardList.Count > 0)
        {
            ModelManager.model.CreateCardList[0].GetComponent<CurrentCardPrefab>().LoadDes();
        }
    }
    /// <summary>
    /// 玩家点击事件
    /// </summary>
    public void PlayDownEvemt(bool Value =true)
    {
        ClearIdleTime();

        if (Value)
        {
            //玩家执行了一个操作

            for (int i = 0; i < ModelManager.model.CardList.Count; i++)
            {
                if (ModelManager.model.CardList[i].GetComponent<CardPrefab>().EffectType == CardEffectType.Ice)
                {
                    ModelManager.model.CardList[i].GetComponent<CardPrefab>().ReduceIceNum();
                }
                if (ModelManager.model.CardList[i].GetComponent<CardPrefab>().EffectType == CardEffectType.Up|| ModelManager.model.CardList[i].GetComponent<CardPrefab>().EffectType == CardEffectType.Down)
                {
          ModelManager.model.CardList[i].GetComponent<CardPrefab>().ChangeType();
                }
            }
        }
    }


    public PokerCardsType GetPokerType(string Value)
    {
        PokerCardsType type = PokerCardsType.A;
        switch (Value)
        {
            case "A":
                type = PokerCardsType.A;
                break;
            case "2":
                type = PokerCardsType.To;
                break;
            case "3":
                type = PokerCardsType.Three;
                break;
            case "4":
                type = PokerCardsType.Four;
                break;
            case "5":
                type = PokerCardsType.Five;
                break;
            case "6":
                type = PokerCardsType.Six;
                break;
            case "7":
                type = PokerCardsType.Seven;
                break;
            case "8":
                type = PokerCardsType.Eight;
                break;
            case "9":
                type = PokerCardsType.Nine;
                break;
            case "10":
                type = PokerCardsType.Ten;
                break;
            case "J":
                type = PokerCardsType.J;
                break;
            case "Q":
                type = PokerCardsType.Q;
                break;
            case "K":
                type = PokerCardsType.K;
                break;
        }
        return type;
    }

    /// <summary>
    /// 获得卡牌的花色
    /// </summary>
    /// <returns></returns>
    public PokerCardsColor GetCardColor()
    {
        PokerCardsColor color = new PokerCardsColor();
        switch (CardColorID)
        {
            case 0:
                color = PokerCardsColor.Hearts;  //红桃
                break;
            case 1:
                color = PokerCardsColor.Spades;  //红桃
                break;
            case 2:
                color = PokerCardsColor.Diamonds;  //红桃
                break;
            case 3:
                color = PokerCardsColor.Clubs;  //红桃
                break;

        }
        CardColorID++;
        if (CardColorID >= 4)
        {
            CardColorID = 0;
        }

        return color;
    }
    /// <summary>
    /// 显示游戏失败
    /// </summary>
    public void ShowGameLose()
    {

        GameObject Panel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.GameLosePanel,true);
        Panel.GetComponent<GameLosePanel>().SetUI();
    }

    public void DelayShowLosePanel()
    {
        GameEnd();

        Invoke("ShowGameLose",0.5f);
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown("t"))
        {
            GameEnd();
            ShowGameWin();
        }
    }
    /// <summary>
    /// 延迟显示游戏胜利界面
    /// </summary>
    public void DelayShowWinPanel()
    {
        GameEnd();

        Invoke("ShowGameWin", 0.5f);
    }
    public void ShowGameWin()
    {

        GameObject Panel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.GameWinPanel,true);
        Panel.GetComponent<GameWinPanel>().SetUI();
    }

    /// <summary>
    /// 游戏结束事件
    /// </summary>
    public void GameEnd()
    {
        LevelManager.level.isPlay = false;
        BattleManager.battle.isSkill = false;
        BattleManager.battle.isSkill1 = false;
        BattleManager.battle.isSkill2 = false;
        BattleManager.battle.isSkill3 = false;
    }

    /// <summary>
    /// 重新开始当前的游戏关卡
    /// </summary>
    public void AgainGame()
    {
        PlayGame(WorldData.CurrentSelectLevelID);
    }


    public void GoToMain()
    {
        ClearData();
        UIManager.ui.DeathPanel(UIManager.UIType.BattleUIPanel);
    GameObject Panel=    UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.MainMenuPanel);
        Panel.GetComponent<MainMenuPanel>().SetUI();
   

    }


    /// <summary>
    /// 选择卡牌
    /// </summary>
    public void SelectCard(GameObject Model)
    {
        if (CurrentCard.gameObject != null)
        {
            if (CheckPokerTypeAdjacent(CurrentCard.CurrentType, Model.GetComponent<CardPrefab>().cardType))
            {

                if (Model.GetComponent<CardPrefab>().EffectType == CardEffectType.Down || Model.GetComponent<CardPrefab>().EffectType == CardEffectType.Up)
                {
                    Model.GetComponent<CardPrefab>().EffectType = CardEffectType.None;
                }
                ExcelNewBie();
                MusicManager.music.PlayMusic(3);

                isSelect = true;
                Model.GetComponent<CardPrefab>().StopShakeImmediately();  //停止抖动
                Model.transform.SetParent(BattleUIPanel.battle.SelectCardPart);
                StartCoroutine(AddComboNum(Model.GetComponent<CardPrefab>().cardColor));
                Model.GetComponent<CardPrefab>().isSelectRead = true;  //当前处于选择状态
                StartCoroutine(MoveCardPos(Model, BattleUIPanel.battle.CurrentCard.gameObject, 0.4f));
                ModelManager.model.CheckAllCard();
                BattleManager.battle.PlayDownEvemt();  //当前玩家点击了
            }
            else
            {
                //如果不符合
                Model.GetComponent<CardPrefab>().StartShake();
            }
        }
    }



    /// <summary>
    /// 执行新手教程的逻辑
    /// </summary>
    public void ExcelNewBie()
    {
        if (WorldData.NewbieState == 0)
        {
            if (WorldData.CurrentSelectLevelID == 1)
            {
                NewBieSelectCardNum++;  //卡牌数量增加1

                //如果等于1
                //开始计算
                if (NewBieSelectCardNum < ModelManager.model.DesList.Count)
                {
                    ModelManager.model.ShowDesList(NewBieSelectCardNum);
                }
                else
                {
                    ModelManager.model.ResetDes();
                }
            }
        }
        if (WorldData.NewbieState == 1)
        {
            if (WorldData.CurrentSelectLevelID == 2)
            {
                NewBieSelectCardNum++;  //卡牌数量增加1

                if (NewBieSelectCardNum >= 3 && NewBieSelectCardNum < 4)
                {

                    GameObject NewbiePanelTo = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.NewbiePanelTo, true);
                    NewbiePanelTo.GetComponent<NewbiePanelTo>().SetUI();
                    NewbiePanelTo.GetComponent<NewbiePanelTo>().ShowDes(1);
                    BattleManager.battle.NewBieState = 1;  //状态为1
                    if (ModelManager.model.CreateCardList.Count > 0)
                    {
                        for (int i = 0; i < ModelManager.model.CreateCardList.Count; i++)
                        {
                            if (ModelManager.model.GetCardIndex(ModelManager.model.CreateCardList[i]) == 0)
                            {
                                //如果是第一个
                                ModelManager.model.CreateCardList[i].transform.Find("Des").gameObject.SetActive(true);
                                break;
                            }
                        }

                    }
                }
            }
                    if (NewBieSelectCardNum >= 9 && NewBieSelectCardNum < 10)
                    {

                        GameObject NewbiePanelTo = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.NewbiePanelTo, true);
                        NewbiePanelTo.GetComponent<NewbiePanelTo>().SetUI();
                        NewbiePanelTo.GetComponent<NewbiePanelTo>().ShowDes(2);
                        BattleManager.battle.NewBieState = 3;  //状态为1
                AnyCardNum++;
                BattleUIPanel.battle.UpdateAnyCardNum();
                BattleUIPanel.battle.AnyCard.transform.Find("Des").gameObject.SetActive(true);
                    }
        }
    }

    /// <summary>
    /// 移动卡片到目标位置，并伴随缩放和旋转动画
    /// </summary>
    /// <param name="currentModel">要移动的卡片</param>
    /// <param name="targetPos">目标位置对象</param>
    /// <param name="duration">动画总时长（秒）</param>
    /// <returns>协程迭代器</returns>
    public IEnumerator MoveCardPos(GameObject currentModel, GameObject targetPos, float duration)
    {
        // 记录初始状态
        Vector3 startPos = currentModel.transform.position;
        Vector3 endPos = targetPos.transform.position;
        Vector3 startScale = currentModel.transform.localScale;
        Quaternion startRot = currentModel.transform.rotation;

        // 定义旋转目标值
        Quaternion midRot = Quaternion.Euler(0, 0, -180);
        Quaternion endRot = Quaternion.Euler(0, 0, -360);

   

        float elapsedTime = 0f;

        // 动画循环
        while (elapsedTime < duration)
        {
            // 计算总进度（0到1）
            float t = elapsedTime / duration;

            // 移动位置
            currentModel.transform.position = Vector3.Lerp(startPos, endPos, t);

            if (t <= 0.5f)
            {
                // 前半段动画（0-50%）
                float halfT = t / 0.5f; // 前半段内的进度（0到1）
                currentModel.transform.rotation = Quaternion.Lerp(startRot, midRot, halfT);
            }
            else
            {
                // 后半段动画（50-100%）
                float halfT = (t - 0.5f) / 0.5f; // 后半段内的进度（0到1）
                currentModel.transform.rotation = Quaternion.Lerp(midRot, endRot, halfT);
            }

            // 等待一帧
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 强制设置最终状态，确保精确到位
        currentModel.transform.position = endPos;
        currentModel.transform.localScale = startScale;
        currentModel.transform.rotation = endRot;

        isSelect = false;

        if (CurrentCard.gameObject != null)
        {
            Destroy(CurrentCard.gameObject);
        }
      

        GameObject CardPrefab = Instantiate<GameObject>(Resources.Load<GameObject>("CurrentCardPrefab"));
        CardPrefab.transform.SetParent(BattleUIPanel.battle.CurrentCardPart);
        CardPrefab.transform.position = BattleUIPanel.battle.CurrentCard.transform.position;
        CardPrefab.GetComponent<CurrentCardPrefab>().SetUI(currentModel.GetComponent<CardPrefab>().cardType , currentModel.GetComponent<CardPrefab>().cardColor, true);
        CurrentCard = CardPrefab.GetComponent<CurrentCardPrefab>();
        CurrentCard.transform.localScale = startScale; //设置大小一致
        Destroy(currentModel.gameObject);

    }

    /// <summary>
    /// 检查卡牌是否是相邻的
    /// </summary>
    /// <returns></returns>
    public bool CheckPokerTypeAdjacent(PokerCardsType CurrentType, PokerCardsType NextType)
    {
        if (CurrentType == PokerCardsType.Any)
        {
            return true;
        }
        if (NextType == PokerCardsType.Any)
        {
            return true;
        }
        switch (CurrentType)
        {
            case PokerCardsType.A:
                if (NextType == PokerCardsType.K)
                {
                    return true;
                }
                if (NextType == PokerCardsType.To)
                {
                    return true;
                }
                break;
            case PokerCardsType.To:
                if (NextType == PokerCardsType.A)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Three)
                {
                    return true;
                }
                break;
            case PokerCardsType.Three:
                if (NextType == PokerCardsType.To)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Four)
                {
                    return true;
                }
                break;
            case PokerCardsType.Four:
                if (NextType == PokerCardsType.Three)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Five)
                {
                    return true;
                }
                break;
            case PokerCardsType.Five:
                if (NextType == PokerCardsType.Four)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Six)
                {
                    return true;
                }
                break;
            case PokerCardsType.Six:
                if (NextType == PokerCardsType.Five)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Seven)
                {
                    return true;
                }
                break;
            case PokerCardsType.Seven:
                if (NextType == PokerCardsType.Six)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Eight)
                {
                    return true;
                }
                break;
            case PokerCardsType.Eight:
                if (NextType == PokerCardsType.Nine)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Seven)
                {
                    return true;
                }
                break;
            case PokerCardsType.Nine:
                if (NextType == PokerCardsType.Ten)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Eight)
                {
                    return true;
                }
                break;
            case PokerCardsType.Ten:
                if (NextType == PokerCardsType.Nine)
                {
                    return true;
                }
                if (NextType == PokerCardsType.J)
                {
                    return true;
                }
                break;
            case PokerCardsType.J:
                if (NextType == PokerCardsType.Ten)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Q)
                {
                    return true;
                }
                break;
            case PokerCardsType.Q:
                if (NextType == PokerCardsType.K)
                {
                    return true;
                }
                if (NextType == PokerCardsType.J)
                {
                    return true;
                }
                break;
            case PokerCardsType.K:
                if (NextType == PokerCardsType.A)
                {
                    return true;
                }
                if (NextType == PokerCardsType.Q)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    /// <summary>
    /// 开始下一个关卡
    /// </summary>
    public void NextGame()
    {
        PlayGame(WorldData.CurrentSelectLevelID);
    }

    public void ExcuteSkil1()
    {
        StartCoroutine(StartSkill1());
    }
    public IEnumerator StartSkill1()
    {

        isSkill1 = true;
        isSkill = true;
        BattleUIPanel.battle.PlaySkillEffect(1);  //执行了1号技能特效

        yield return new WaitForSeconds(0.2f);
        MusicManager.music.PlayMusic(4);
        int Num = Random.Range(1,7);  //随机1-6之间的数据
        BattleUIPanel.battle.Skill1Effect.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon" + Num);
        yield return new WaitForSeconds(1.3f);
        BattleUIPanel.battle.Skill1Effect.GetComponent<Animator>().enabled = false;
        BattleUIPanel.battle.Skill1Effect.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dice" + Num);
        yield return new WaitForSeconds(1);
        BattleUIPanel.battle.Skill1Effect.gameObject.SetActive(false);

        BattleUIPanel.battle.Skill1Effect.GetComponent<Animator>().enabled = true;
        yield return StartCoroutine( GetNumCard(Num));
       isSkill1 = false ;
        isSkill = false ;
        print("技能1");

    }

    public void ExcuteSkill3()
    {
        StartCoroutine(StartSkill3());
    }
    public IEnumerator StartSkill3()
    {

        BattleManager.battle.isSkill3 = true;
        BattleManager.battle.isSkill = true;
        yield return new WaitForSeconds(0.2f);
        BattleUIPanel.battle.PlaySkillEffect(3);
        MusicManager.music.PlayMusic(6);

        yield return new WaitForSeconds(1.2f);

        yield return StartCoroutine(GetAnyCard());
        isSkill3 = false;
        isSkill = false;
    }

    /// <summary>
    /// 增加一个万能卡牌
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetAnyCard()
    {
        isMoveAnyCard = true;
        GameObject Data = Instantiate<GameObject>(Resources.Load<GameObject>("CurrentCardPrefab"));
        Data.transform.SetParent(BattleUIPanel.battle.CurrentReservePart);
        Data.transform.position = BattleUIPanel.battle.CreateCardPos.transform.position;   //在连击奖励位置生成
        Data.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Data.GetComponent<CurrentCardPrefab>().SetUI(PokerCardsType.Any); //当前的类型是万能卡牌

        StartCoroutine(ModelManager.model.MoveItemAndScale(Data, BattleUIPanel.battle.AnyCard.gameObject));
        yield return new WaitForSeconds(0.61f);
        isMoveAnyCard = false;
        AnyCardNum++;  //数量增加1
        Destroy(Data);
        BattleUIPanel.battle.UpdateAnyCardNum();  //更新数量
    }

    public void ExcuteSkill2()
    {

        BattleManager.battle.PlayDownEvemt(false);  //当前玩家点击了
        BattleUIPanel.battle.PlaySkillEffect(2);  //执行了2号技能特效
        MusicManager.music.PlayMusic(5);
        StartCoroutine(StartSkill2());
    }
    /// <summary>
    /// 协程：2D物体在0.2秒内移动到世界坐标(0,0)
    /// </summary>
    /// <param name="targetTransform">要移动的物体的Transform</param>
    public IEnumerator MoveToZeroCoroutine(Transform targetTransform)
    {
        if (targetTransform == null)
        {
            Debug.LogError("目标物体的Transform为空，无法移动！");
            yield break;
        }

        // 1. 记录初始位置（物体当前位置）
        Vector3 startPos = targetTransform.position;
        // 2. 目标位置（世界坐标0,0，Z轴保持初始值，避免2D物体Z轴偏移）
        Vector3 targetPos = new Vector3(BattleUIPanel.battle.Pos.transform.position.x, BattleUIPanel.battle.Pos.transform.position.y, startPos.z);

        float elapsedTime = 0f; // 已流逝的时间

        // 3. 移动过程：分帧更新位置
        while (elapsedTime < 0.3f)
        {
            // 计算移动进度（0→1，与帧率无关）
            float progress = elapsedTime / 0.3f;

            // 线性插值计算当前位置（匀速移动）
            targetTransform.position = Vector3.Lerp(startPos, targetPos, progress);

            // 等待下一帧（让Unity渲染，实现平滑移动）
            yield return null;

            // 累加已流逝时间
            elapsedTime += Time.deltaTime;
        }

        // 4. 强制收尾：确保最终位置准确为(0,0)（解决浮点数精度误差）
        targetTransform.position = targetPos;

        Debug.Log("物体已移动到(0,0)位置！");
    }
    public IEnumerator StartSkill2()
    {
        isSkill = true; //当前处于执行技能状态
        isSkill2 = true;

        yield return new WaitForSeconds(0.9f);
        List<GameObject> ModelList = GetReadCardNum();

        if (ModelList.Count > 0)
        {
            for (int i = 0; i < ModelList.Count; i++)
            {
                StartCoroutine(MoveToZeroCoroutine(ModelList[i].GetComponent<Transform>()));
            }
        }yield return new WaitForSeconds(0.3f);

        if (ModelList.Count > 0)
        {
            for (int i = 0; i < ModelList.Count; i++)
            {
                StartCoroutine(HideItem(ModelList[i]));
            }
        }
        yield return new WaitForSeconds(0.31f);
        isSkill = false;
        isSkill2 = false;
        ModelManager.model.CheckAllCard();
    }

    public IEnumerator HideItem(GameObject Model)
    {


        // 缩放动画的持续时间（可根据需要调整）
        float scaleDuration = 0.2f;
        // 记录初始缩放值和目标缩放值
        Vector3 startScale = Model.transform.localScale; // 当前缩放
        Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f); // 目标缩放

        float elapsedTime = 0f;

        // 执行缩放动画
        while (elapsedTime < scaleDuration)
        {
            // 计算插值比例（0到1）
            float t = elapsedTime / scaleDuration;
            // 使用平滑插值使缩放更自然
            t = Mathf.SmoothStep(0f, 1f, t);

            // 应用缩放
            Model.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            // 累加时间并等待下一帧
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终缩放准确
        Model.transform.localScale = targetScale;

        // 缩放完成后删除物体
        Destroy(Model);
    }

    /// <summary>
    /// 获得当前准备就绪的卡牌数组
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetReadCardNum()
    {

        List<GameObject> ModelList = new List<GameObject>();
        if (ModelManager.model != null)
        {
            GameManager.gam.CleanupGameObjectList(ModelManager.model.CardList);
            if (ModelManager.model.CardList.Count > 0)
            {
                for (int i = 0; i < ModelManager.model.CardList.Count; i++)
                {
                    if (ModelManager.model.CardList[i].isDown)
                    {
                        if (!ModelManager.model.CardList[i].isSelectRead)
                        {
                            if (ModelManager.model.CardList[i].EffectType != CardEffectType.Ice)
                            {
                                //当前没有被选择
                                ModelList.Add(ModelManager.model.CardList[i].gameObject);
                            }
                        }
                    }
                }
            }
        }

        return ModelList;
    }


    /// <summary>
    /// 获得任意数量的卡牌
    /// </summary>
    public IEnumerator GetNumCard(int Num)
    {
        int PosID = 0;  //下标位置
        BattleManager.battle.GetCardEvemt = true;
        List<GameObject> CardList = new List<GameObject>();
        List<GameObject> PosList = new List<GameObject>();
        for (int i = 0; i < Num; i++)
        {

            GameObject Data = Instantiate<GameObject>(Resources.Load<GameObject>("CurrentCardPrefab"));
            Data.transform.SetParent(BattleUIPanel.battle.CurrentReservePart);
            Data.transform.position = BattleUIPanel.battle.CreateCardPos.transform.position;   //在连击奖励位置生成
            Data.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Data.GetComponent<CurrentCardPrefab>().SetUI(ModelManager.model.RangeGetPokerType(), BattleManager.battle.GetCardColor(), false);
            BattleManager.battle.SetToPenultimatePosition(Data, i + 1);
            ModelManager.model.CreateCardList.Insert(PosID, Data);
            CardList.Add(Data);
            PosList.Add(ModelManager.model.CardPosList[PosID]);

            PosID++;

        }
        ModelManager.model.CheckCardPos(Num);

        yield return new WaitForSeconds(0.3f);
        for (int i = CardList.Count - 1; i >= 0; i--)
        {
            StartCoroutine(ModelManager.model.MoveItemAndScale(CardList[i], PosList[i]));
            yield return new WaitForSeconds(0.1f);
        }

        BattleManager.battle.GetCardEvemt = false ;

    }

    /// <summary>
    /// ID获得当前的背景的状态
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public bool IDGetBGState(int ID)
    {
        if (WorldData.BGState.ContainsKey(ID))
        {
            if (WorldData.BGState[ID] == 1)
            {
                return true;
            }
            else
            {

                return false;
            }
        }

        WorldData.BGState.Add(ID,0);
        return false;

    }
    public void IDSetBGState(int ID,int Value)
    {
        if (WorldData.BGState.ContainsKey(ID))
        {
            WorldData.BGState[ID] = Value;
            return;
        }

        WorldData.BGState.Add(ID, Value);

    }

    //=============
    public bool IDGetGroundState(int ID)
    {
        if (WorldData.GroundState.ContainsKey(ID))
        {
            if (WorldData.GroundState[ID] == 1)
            {
                return true;
            }
            else
            {

                return false;
            }
        }

        WorldData.GroundState.Add(ID, 0);
        return false;

    }
    public void IDSetGroundStateState(int ID, int Value)
    {

        if (WorldData.GroundState.ContainsKey(ID))
        {
            WorldData.GroundState[ID] = Value;
            return;
        }

        WorldData.GroundState.Add(ID, Value);

    }
}