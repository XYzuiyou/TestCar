using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public static ModelManager model;

    public List<CardPrefab> CardList = new List<CardPrefab>();

    public LevelData CurrentData;
    public List<GameObject> cardPosList = new List<GameObject>();

    public List<GameObject> CardPosList
    {
        get
        {
            GameManager.gam.CleanupGameObjectList(cardPosList);
            if (cardPosList.Count <= 0)
            {
                List<GameObject> PosLIst = new List<GameObject>(BattleUIPanel.battle.CardPosList);
        PosLIst.Reverse();  //翻转数据

                PosLIst.ForEach(i => cardPosList.Add(i));
            }

            return cardPosList;
        }
    }


    public List<GameObject> DesList = new List<GameObject>();

    private void Start()
    {
        if (WorldData.NewbieState != 2)
        {
            BattleManager.battle.ExcelNewBie();
        }
    }
    /// <summary>
    /// 用来存储当前创建出来的卡牌的模型的-
    /// </summary>
    public List<GameObject> CreateCardList = new List<GameObject>();

    public int GetCardIndex(GameObject targetObject)
    {
        // 检查列表中是否包含该物体
        if (CreateCardList.Contains(targetObject))
        {
            // 返回物体在列表中的索引
            return CreateCardList.IndexOf(targetObject);
        }
        else
        {
            // 如果列表中不包含该物体，返回-1
            return -1;
        }
    }

    public void ShowDesList(int ID)
    {

        ResetDes();
        DesList[ID].gameObject.SetActive(true);
    }
    public void ResetDes()
    {
        for (int i = 0; i < DesList.Count; i++)
        {
            DesList[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 获得未知的索引
    /// </summary>
    /// <param name="targetObject"></param>
    /// <returns></returns>
    public int GetPosIndex(GameObject targetObject)
    {
        // 检查列表中是否包含该物体
        if (CardPosList.Contains(targetObject))
        {
            // 返回物体在列表中的索引
            return CardPosList.IndexOf(targetObject);
        }
        else
        {
            // 如果列表中不包含该物体，返回-1
            return -1;
        }
    }
    /// <summary>
    /// 检查卡牌的未知
    /// </summary>
    public void CheckCardPos( int Num =0)
    {
        if (BattleManager.battle.isMoveCard)
        {
            return;
        }
        if (CreateCardList.Count > 0)
        {
       

            for (int i = 0; i < CreateCardList.Count; i++)
            {
                if (i < Num)
                {
                    continue;
                }

                int CardPosID = GetCardIndex(CreateCardList[i]);   ///获得当前卡牌在数组里面的位置
                int PosID = GetPosIndex(CreateCardList[i].GetComponent<CurrentCardPrefab>().CurrentPos);  //获得当前卡牌对应的未知
                if (CardPosID == PosID)
                {
                    continue;
                }
                else
                {
                    if (BattleManager.battle.isMoveCard == false)
                    {
                        BattleManager.battle.isMoveCard = true;
                    }
                    //如果不一致
                    StartCoroutine(MoveItem(CreateCardList[i], CardPosList[CardPosID]));
                }
            } 
        }
        BattleManager.battle.isMoveCard = false;

    }

    public IEnumerator MoveItem(GameObject currentModel, GameObject pos)
    {
  

        // 移动持续时间（秒）
        float moveDuration = 0.3f;
        // 记录开始时间
        float elapsedTime = 0f;
        // 获取开始位置
        Vector3 startPos = currentModel.transform.position;
        // 获取目标位置
        Vector3 targetPos = pos.transform.position;

        // 当移动时间小于总持续时间时，持续更新位置
        while (elapsedTime < moveDuration)
        {
            // 计算插值比例（0到1之间）
            float t = elapsedTime / moveDuration;
            // 使用平滑插值使移动更自然
            t = Mathf.SmoothStep(0f, 1f, t);

            // 更新物体位置
            currentModel.transform.position = Vector3.Lerp(startPos, targetPos, t);

            // 累加已用时间
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        // 确保物体最终位置准确
        currentModel.transform.position = targetPos;
    }



    public IEnumerator MoveItemAndScale(GameObject currentModel, GameObject pos)
    {
        // 移动和缩放持续时间（秒）
        float moveDuration = 0.6f;
        // 记录开始时间
        float elapsedTime = 0f;
        // 获取开始位置和目标位置
        Vector3 startPos = currentModel.transform.position;
        Vector3 targetPos = pos.transform.position;

        // 定义缩放的起始值和目标值
        Vector3 startScale = Vector3.one * 0.5f;  // 初始缩放0.5
        Vector3 targetScale = Vector3.one;        // 目标缩放1

        // 应用初始缩放
        currentModel.transform.localScale = startScale;

        // 当移动时间小于总持续时间时，持续更新位置和缩放
        while (elapsedTime < moveDuration)
        {
            // 计算插值比例（0到1之间）
            float t = elapsedTime / moveDuration;
            // 使用平滑插值使移动和缩放更自然
            t = Mathf.SmoothStep(0f, 1f, t);

            // 更新物体位置
            currentModel.transform.position = Vector3.Lerp(startPos, targetPos, t);

            // 同步更新物体缩放
            currentModel.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            // 累加已用时间
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        // 确保物体最终状态准确
        currentModel.transform.position = targetPos;
        currentModel.transform.localScale = targetScale;
        currentModel.GetComponent<CurrentCardPrefab>().isRead = true;
    }
    private void Awake()
    {
        model = this;
    }



    public bool isRead;  //当前是否就绪

    private void Update()
    {
    }

    public void SetUI(LevelData data)
    {
        CardList.Clear();
        CollectCardPrefabsRecursively(transform);
        CurrentData = data;   //进行数据赋值
        SetCardValue();
        CheckAllCard();
        InitCurrentCard();
    }

    private void CollectCardPrefabsRecursively(Transform parentTransform)
    {
        // 检查当前物体是否有CardPrefab组件
        CardPrefab card = parentTransform.GetComponent<CardPrefab>();
        if (card != null)
        {
            CardList.Add(card);
        }

        // 递归遍历所有子物体
        foreach (Transform child in parentTransform)
        {
            CollectCardPrefabsRecursively(child);
        }
    }

    /// <summary>
    /// 初始化当前的卡牌数据
    /// </summary>
    public void InitCurrentCard()
    {
        if (BattleUIPanel.battle.CurrentReservePart.childCount > 0)
        {
            GameManager.gam.DeathChild(BattleUIPanel.battle.CurrentReservePart);
        }
        if (BattleUIPanel.battle.CurrentCardPart.childCount > 0)
        {
            GameManager.gam.DeathChild(BattleUIPanel.battle.CurrentCardPart);
        }
        CreateCardList.Clear();
        if (CurrentData.CurrentCardNum >= 1)
        {
            for (int i = 0; i < CurrentData.CurrentCardNum; i++)
            {
                GameObject CardPrefab = Instantiate<GameObject>(Resources.Load<GameObject>("CurrentCardPrefab"));
                CardPrefab.transform.SetParent(BattleUIPanel.battle.CurrentReservePart);
                CardPrefab.transform.localPosition = Vector3.zero;
                CardPrefab.transform.localScale = Vector3.one;
                CardPrefab.GetComponent<CurrentCardPrefab>().SetUI(RangeGetPokerType(),BattleManager.battle.GetCardColor(),false);
                CardPrefab.GetComponent<CurrentCardPrefab>().isRead = true;
                CreateCardList.Add(CardPrefab);
            }
        }
        if (CreateCardList.Count > 0)
        {
            for (int i = 0; i < CreateCardList.Count; i++)
            {
                CreateCardList[i].GetComponent<CurrentCardPrefab>().SetPosID(i, CardPosList[i]);
                CreateCardList[i].transform.position = CardPosList[i].transform.position;   //设置卡牌的位置
            }
        }


        ReverseHierarchyOrder();   //颠倒这些卡牌的顺序


        ///创建场景上面的卡牌的数据

        GameObject Data = Instantiate<GameObject>(Resources.Load<GameObject>("CurrentCardPrefab"));
        Data.transform.SetParent(BattleUIPanel.battle.CurrentCardPart);
        Data.transform.position = BattleUIPanel.battle.CurrentCard.transform.position;
        Data.transform.localScale = Vector3.one;

        Data.GetComponent<CurrentCardPrefab>().SetUI(BattleManager.battle.GetPokerType(CurrentData.CurrentCardData), BattleManager.battle.GetCardColor(), true);
        BattleManager.battle.CurrentCard = Data.GetComponent<CurrentCardPrefab>();
    }

    /// <summary>
    /// 随机获得卡牌的类型
    /// </summary>
    public PokerCardsType RangeGetPokerType()
    {
        PokerCardsType type = PokerCardsType.A;
        int Value = Random.Range(0,13);
        switch (Value)
        {
            case 0:
                type = PokerCardsType.A;
                break;
            case 1:
                type = PokerCardsType.To;
                break;
            case 2:
                type = PokerCardsType.Three;
                break;
            case 3:
                type = PokerCardsType.Four;
                break;
            case 4:
                type = PokerCardsType.Five;
                break;
            case 5:
                type = PokerCardsType.Six;
                break;
            case 6:
                type = PokerCardsType.Seven;
                break;
            case 7:
                type = PokerCardsType.Eight;
                break;
            case 8:
                type = PokerCardsType.Nine;
                break;
            case 9:
                type = PokerCardsType.Ten;
                break;
            case 10:
                type = PokerCardsType.J;
                break;
            case 11:
                type = PokerCardsType.Q;
                break;
            case 12:
                type = PokerCardsType.K;
                break;
        }
        return type;

    }
    public void ReverseHierarchyOrder()
    {
        // 检查列表是否有足够元素需要颠倒
        if (CreateCardList.Count <= 1)
            return;

        // 保存所有物体的原始父物体
        Transform[] originalParents = new Transform[CreateCardList.Count];
        for (int i = 0; i < CreateCardList.Count; i++)
        {
            originalParents[i] = CreateCardList[i].transform.parent;
        }

        // 颠倒顺序 - 从后往前设置兄弟索引
        for (int i = 0; i < CreateCardList.Count; i++)
        {
            // 当前物体在列表中的索引
            int currentIndex = i;
            // 颠倒后的目标索引（最后一个变成第一个）
            int targetIndex = CreateCardList.Count - 1 - i;

            // 设置物体的兄弟索引，控制在Hierarchy中的显示顺序
            // 兄弟索引越小，在Hierarchy中位置越靠上
            CreateCardList[currentIndex].transform.SetSiblingIndex(targetIndex);
        }
    }

    public IEnumerator MoveCardToCurrentPos(GameObject currentModel, GameObject targetPos, float duration)
    {

        GameManager.gam.SetToLastSibling(currentModel);  //设置层级为最后一个
        BattleManager.battle.isSelect = true;
        CreateCardList.Remove(currentModel);
        // 安全校验
        if (currentModel == null || targetPos == null)
        {
            Debug.LogError("移动目标或目标位置物体为空！");
            yield break;
        }

        // 确保时长为正数
        if (duration <= 0)
        {
            Debug.LogWarning("时长必须为正数，已自动设置为1秒");
            duration = 1f;
        }

        // 记录初始状态
        Vector3 startPos = currentModel.transform.position;
        Vector3 endPos = targetPos.transform.position;
        float startYRotate = currentModel.transform.localEulerAngles.y;
        float halfDuration = duration / 2f; // 每个阶段的时长

        // 第一阶段：移动到中途位置，旋转到-180°
        float elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            float progress = elapsedTime / halfDuration;

            // 移动到中途位置（总距离的一半）
            currentModel.transform.position = Vector3.Lerp(
                startPos,
                Vector3.Lerp(startPos, endPos, 0.5f), // 中途位置
                progress
            );

            // 旋转到-180°
            float targetLocalYRotate = Mathf.Lerp(startYRotate, -90, progress);
            currentModel.transform.localEulerAngles = new Vector3(
                currentModel.transform.localEulerAngles.x,
                targetLocalYRotate,
                currentModel.transform.localEulerAngles.z
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 强制设置到第一阶段结束的精确状态
        currentModel.transform.position = Vector3.Lerp(startPos, endPos, 0.5f);
        currentModel.transform.localEulerAngles = new Vector3(
            currentModel.transform.localEulerAngles.x,
            -180f,
            currentModel.transform.localEulerAngles.z
        );

        // 在这里执行旋转到-180°后需要做的操作

        currentModel.GetComponent<CurrentCardPrefab>().LoadCardImg();

        // 第二阶段：从中间位置移动到终点，旋转到-360°
        elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            float progress = elapsedTime / halfDuration;

            // 从中途位置移动到终点
            currentModel.transform.position = Vector3.Lerp(
                Vector3.Lerp(startPos, endPos, 0.5f), // 中途位置
                endPos,
                progress
            );

            // 从-180°旋转到-360°
            float targetLocalYRotate = Mathf.Lerp(-90, -0, progress);
            currentModel.transform.localEulerAngles = new Vector3(
                currentModel.transform.localEulerAngles.x,
                targetLocalYRotate,
                currentModel.transform.localEulerAngles.z
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 强制修正终点状态
        currentModel.transform.position = endPos;
        currentModel.transform.localEulerAngles = new Vector3(
            currentModel.transform.localEulerAngles.x,
            -360f,
            currentModel.transform.localEulerAngles.z
        );

        BattleManager.battle.isSelect = false ;
        if (BattleManager.battle.CurrentCard.gameObject != null)
        {
            Destroy(BattleManager.battle.CurrentCard.gameObject);
        }

        BattleManager.battle.CurrentCard = currentModel.GetComponent<CurrentCardPrefab>();
        BattleManager.battle.CurrentCard.CurrentValue = true;

    }


    /// <summary>
    /// 检查所有的卡牌
    /// </summary>
    public void CheckAllCard()
    {
        GameManager.gam.CleanupGameObjectList(CardList);
        if (CardList.Count > 0)
        {
            for (int i = 0; i < CardList.Count; i++)
            {
                CardList[i].CheckCard();
            }
        }
    }

    /// <summary>
    /// 设置卡牌的值
    /// </summary>
    public void SetCardValue()
    {
        string CardValue = CurrentData.CardData;
        string[] arr = CardValue.Split(',');  //分割逗号
        CardList.Reverse();   //翻转数据
        print(arr.Length);
        if (arr.Length > 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                CardList[i].SetUI(arr[i]);
            }
        }
    }




}
