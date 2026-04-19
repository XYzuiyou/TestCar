using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardPrefab : MonoBehaviour, IPointerDownHandler
{

    public bool isDown;  //判断当前是否可以被点击
    public bool Excute;   //当前处于翻面状态
    public bool isSelectRead;   //当前是否被选择
    public PokerCardsType cardType;
    public PokerCardsColor cardColor;   //当前卡牌的花色
    public CardDireType CurrentDireType;   //当前卡牌的方向类型  


    public int CardID;   //当前的卡牌的ID

    public Image CardImg;   //卡牌图片

    public GameObject Des;
    /// <summary>
    /// 当前身上的卡牌
    /// </summary>
    public List<GameObject> UpCardList = new List<GameObject>();

    public bool isHide;  //当前是否处于消失状态


    public CardEffectType EffectType;
    public int IceNum;

    public GameObject Model;
    public void SetUI(string Value)
    {
        CardImg = GetComponent<Image>();
        InitBG();

        cardType = BattleManager.battle.GetPokerType(Value);
        cardColor = BattleManager.battle.GetCardColor();

        switch (EffectType)
        {
            case CardEffectType.Ice:
                GameObject Ice = Instantiate<GameObject>(Resources.Load<GameObject>("Ice"));
                Ice.GetComponent<Image>().sprite = Resources.Load<Sprite>("pk_back_bd_00");
                Ice.transform.SetParent(transform);
                Ice.transform.localPosition = Vector3.zero;
                FillParent(Ice.GetComponent<RectTransform>());
                Model = Ice;
                IceNum = 3;
                break;

            case CardEffectType.Up:
                GameObject Up_Move = Instantiate<GameObject>(Resources.Load<GameObject>("Up_Move"));
                Model = Up_Move;
                Up_Move.transform.SetParent(transform);
                Up_Move.transform.localPosition = new Vector3(-39.8f, -52f, 0);
                Up_Move.transform.localEulerAngles = new Vector3(0, 0, 180);
                break;
            case CardEffectType.Down:
                GameObject Down_Move = Instantiate<GameObject>(Resources.Load<GameObject>("Down_Move"));
                Model = Down_Move;
                Down_Move.transform.SetParent(transform);
                Down_Move.transform.localPosition = new Vector3(39.8f, 52, 0);

                break;
        }

    }
    void FillParent(RectTransform rect)
    {
        if (rect == null) return;

        // 1. 设置锚点铺满父对象（Min(0,0)，Max(1,1)）
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        // 2. 设置四个边距为0
        rect.offsetMin = Vector2.zero; // Left=0, Bottom=0
        rect.offsetMax = Vector2.zero; // Right=0, Top=0（offsetMax是负数表示向内偏移，0即无偏移）

        // 可选：设置支点为中心（默认就是(0.5,0.5)，无需额外设置）
        // rect.pivot = new Vector2(0.5f, 0.5f);
    }
    public void LoadDes()
    {
        GameObject Model = Instantiate<GameObject>(Resources.Load<GameObject>("Des"));
        Model.transform.SetParent(transform);
        Model.transform.localPosition = Vector3.zero;
        Model.transform.localScale = Vector3.one;
        Des = Model;
        BattleManager.battle.DesList.Add(Model);
        Model.transform.parent = transform.parent;
        Model.GetComponent<RectTransform>().SetSiblingIndex(Model.transform.parent.childCount-1);
    }


    /// <summary>
    /// 初始化背景图片
    /// </summary>
    public void InitBG()
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

    /// <summary>
    /// 检查当前的卡牌头上是否还有卡牌
    /// </summary>
    public void CheckCard()
    {
        if (!Excute)
        {
            GameManager.gam.CleanupGameObjectList(UpCardList);
            //当前没有执行翻面逻辑的话
            if (UpCardList.Count > 0)
            {
                for (int i = 0; i < UpCardList.Count; i++)
                {
                    if (!UpCardList[i].GetComponent<CardPrefab>().isSelectRead)
                    {
                        return;
                    }
                }
                StartCoroutine(RotateItem(gameObject));
                return;
            }
            else if (UpCardList.Count <= 0)
            {
                StartCoroutine(RotateItem(gameObject));

                return;
            }

        }
    }

    private float rotateDuration = 0.4f; // 默认每个阶段1.5秒
    /// <summary>
    /// 旋转物体
    /// </summary>
    /// <returns></returns>
    public IEnumerator RotateItem(GameObject Model)
    {
        Excute = true;
        // 1. 记录初始旋转（保留X、Z轴，只改Y轴）
        Quaternion startRotation = Model.transform.localRotation;
        Vector3 startEuler = startRotation.eulerAngles;


        // -------------------------- 第一阶段：旋转到Y=-90度 --------------------------
        Vector3 target1Euler = new Vector3(startEuler.x, -90f, startEuler.z); // 目标角度
        float timePassed1 = 0f; // 已消耗时间

        while (timePassed1 < rotateDuration)
        {
            // 计算当前时间在总时长中的占比（0~1）
            float t = Mathf.InverseLerp(0f, rotateDuration, timePassed1);
            // 基于占比平滑插值Y轴角度（LerpAngle自动处理角度绕转问题）
            float currentY = Mathf.LerpAngle(startEuler.y, target1Euler.y, t);
            // 应用旋转
            Model.transform.localRotation = Quaternion.Euler(startEuler.x, currentY, startEuler.z);

            // 累积时间，等待下一帧（确保帧率无关）
            timePassed1 += Time.deltaTime;
            yield return null;
        }
        // 强制设置到目标角度（避免浮点误差导致未对齐）
        Model.transform.localRotation = Quaternion.Euler(target1Euler);

        LoadCardImg();
        yield return null;
        // -------------------------- 第二阶段：旋转回Y=0度 --------------------------
        Vector3 target2Euler = new Vector3(startEuler.x, 0f, startEuler.z); // 最终目标角度
        float timePassed2 = 0f; // 已消耗时间

        while (timePassed2 < rotateDuration)
        {
            // 同上：计算时间占比 → 插值角度 → 应用旋转
            float t = Mathf.InverseLerp(0f, rotateDuration, timePassed2);
            float currentY = Mathf.LerpAngle(target1Euler.y, target2Euler.y, t);
            Model.transform.localRotation = Quaternion.Euler(startEuler.x, currentY, startEuler.z);

            timePassed2 += Time.deltaTime;
            yield return null;
        }
        // 强制对齐最终角度
        Model.transform.localRotation = Quaternion.Euler(target2Euler);
        isDown = true;
    }

    /// <summary>
    /// 加载新的卡牌图片
    /// </summary>
    public void LoadCardImg()
    {
        string Path = "";

        switch (cardColor)
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

        switch (cardType)
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
        print("地址" + Path);
        CardImg.sprite = Resources.Load<Sprite>("pk/" + Path);
    }


    /// <summary>
    /// 获得卡牌的地址
    /// </summary>
    public string  GetCardImg(PokerCardsColor cardColor , PokerCardsType cardType)
    {
        string Path = "";

        switch (cardColor)
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

        switch (cardType)
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
return Path;
    }

    /// <summary>
    /// 被点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDown)
        {
            if (BattleManager.battle.isSelect == false)
            {
                if (!isHide)
                {

                    if (EffectType != CardEffectType.Ice)
                    {

                        //如果当前可以被点击了
                        BattleManager.battle.SelectCard(transform.gameObject);
                    }
                    else
                    {
                        StartShake();
                    }
                }
            }
        }

    }
    

    /// <summary>
    /// 更改当前的leix
    /// </summary>
    public void ChangeType()
    {
        if (EffectType == CardEffectType.Up )
        {
            GameObject CardIcon = Instantiate<GameObject>(Resources.Load<GameObject>("CardIcon"));
            CardIcon.transform.SetParent(transform);
            FillParent(CardIcon.GetComponent<RectTransform>());
            PokerCardsType type = GetNextAndUpCardType(cardType,false );
            CardIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("pk/" + GetCardImg(cardColor, type)) ;

            cardType = type;
           StartCoroutine(ChangeImageColor(CardIcon.GetComponent<Image>()));
        }

        else if (EffectType == CardEffectType.Down)
        { 
            PokerCardsType type = GetNextAndUpCardType(cardType,true );
            GameObject CardIcon = Instantiate<GameObject>(Resources.Load<GameObject>("CardIcon"));
            CardIcon.transform.SetParent(transform);
            FillParent(CardIcon.GetComponent<RectTransform>());
            CardIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("pk/" + GetCardImg(cardColor, type));

            cardType = type;
            StartCoroutine(ChangeImageColor(CardIcon.GetComponent<Image>()));

        }
    }

    /// <summary>
    /// 协程：Image透明度从0渐变到1
    /// </summary>
    /// <param name="img">需要渐变的Image</param>
    public IEnumerator ChangeImageColor(Image img)
    {
        // 1. 初始化：设置Image初始颜色（透明度为0，保留原RGB值）
        Color targetColor = img.color; // 获取Image当前的RGB颜色
        targetColor.a = 0f; // 初始透明度设为0（完全透明）
        img.color = targetColor;

        // 2. 渐变过程：帧更新透明度
        float elapsedTime = 0f; // 已流逝的时间

        while (elapsedTime < 0.2f)
        {
            // 计算当前透明度（0→1的插值）：使用Mathf.Lerp实现线性插值
            // elapsedTime / fadeDuration → 0到1的比例（时间进度）
            float currentAlpha = Mathf.Lerp(0f, 1f, elapsedTime / 0.2f);

            // 更新Image颜色（保留RGB，只改alpha）
            targetColor.a = currentAlpha;
            img.color = targetColor;

            // 等待一帧（让Unity渲染更新，实现平滑过渡）
            yield return null;

            // 累加已流逝的时间（与帧率无关，确保不同设备渐变速度一致）
            elapsedTime += Time.deltaTime;
        }

        // 3. 渐变结束：确保透明度最终为1（避免因浮点数精度问题未达到1）
        targetColor.a = 1f;
        img.color = targetColor;

        LoadCardImg();

        Destroy(img.gameObject);
    }


    /// <summary>
    /// 获得下一个或者上一个卡牌的类型
    /// </summary>
    /// <returns></returns> true是上一个  false是下一个
    public PokerCardsType GetNextAndUpCardType(PokerCardsType type,bool Up)
    {
        PokerCardsType CurrentType=PokerCardsType.A;

        switch (type)
        { 
        case PokerCardsType.A :
                if (Up)
                {
                    CurrentType = PokerCardsType.K;
                }
                else
                { 
                    CurrentType = PokerCardsType.To;
                }
                break;
            case PokerCardsType.To:
                if (Up)
                {
                    CurrentType = PokerCardsType.A;
                }
                else
                {
                    CurrentType = PokerCardsType.Three;
                }
                break;
            case PokerCardsType.Three:
                if (Up)
                {
                    CurrentType = PokerCardsType.To;
                }
                else
                {
                    CurrentType = PokerCardsType.Four;
                }
                break;
            case PokerCardsType.Four:
                if (Up)
                {
                    CurrentType = PokerCardsType.Three;
                }
                else
                {
                    CurrentType = PokerCardsType.Five;
                }
                break;
            case PokerCardsType.Five:
                if (Up)
                {
                    CurrentType = PokerCardsType.Four;
                }
                else
                {
                    CurrentType = PokerCardsType.Six;
                }
                break;
            case PokerCardsType.Six:
                if (Up)
                {
                    CurrentType = PokerCardsType.Five;
                }
                else
                {
                    CurrentType = PokerCardsType.Seven ;
                }
                break;
            case PokerCardsType.Seven :
                if (Up)
                {
                    CurrentType = PokerCardsType.Six;
                }
                else
                {
                    CurrentType = PokerCardsType.Eight;
                }
                break;
            case PokerCardsType.Eight:
                if (Up)
                {
                    CurrentType = PokerCardsType.Seven;
                }
                else
                {
                    CurrentType = PokerCardsType.Nine ;
                }
                break;
            case PokerCardsType.Nine :
                if (Up)
                {
                    CurrentType = PokerCardsType.Eight ;
                }
                else
                {
                    CurrentType = PokerCardsType.Ten ;
                }
                break;
            case PokerCardsType.Ten :
                if (Up)
                {
                    CurrentType = PokerCardsType.Nine;
                }
                else
                {
                    CurrentType = PokerCardsType.J;
                }
                break;
            case PokerCardsType.J:
                if (Up)
                {
                    CurrentType = PokerCardsType.Ten;
                }
                else
                {
                    CurrentType = PokerCardsType.Q;
                }
                break;
            case PokerCardsType.Q:
                if (Up)
                {
                    CurrentType = PokerCardsType.J;
                }
                else
                {
                    CurrentType = PokerCardsType.K;
                }
                break;
            case PokerCardsType.K:
                if (Up)
                {
                    CurrentType = PokerCardsType.Q;
                }
                else
                {
                    CurrentType = PokerCardsType.A;
                }
                break;
        }

        return CurrentType;
    }

    public void ReduceIceNum()
    {
        //减少冰块
        if (EffectType == CardEffectType.Ice)
        {
            IceNum -= 1;
            switch (IceNum)
            {
                case 3:
                    Model.GetComponent<Image>().sprite = Resources.Load<Sprite>("pk_back_bd_00");
                    break;
                case 2:
                    Model.GetComponent<Image>().sprite = Resources.Load<Sprite>("pk_back_bd_03");

                    break;
                case 1:
                    Model.GetComponent<Image>().sprite = Resources.Load<Sprite>("pk_back_bd_06");

                    break;
                case 0:
                    Destroy(Model);
                    EffectType = CardEffectType.None;
                    break;
            }
            MusicManager.music.PlayMusic(7);

            GameObject IceAnim        = Instantiate<GameObject>(Resources.Load<GameObject>("IceAnim"));
            IceAnim.transform.SetParent(transform);
            IceAnim.transform.localPosition = Vector3.zero;

        }

    }

    #region 抖动逻辑

    private RectTransform rectTransform;
    private Quaternion originalRotation; // 初始旋转状态
    private bool isShaking = false; // 是否正在摇晃
    private Coroutine currentShakeCoroutine; // 当前协程引用

    [Tooltip("开始和结束阶段的平滑过渡时间（秒）")]
    public float smoothTransitionTime = 0.1f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originalRotation = rectTransform.localRotation;
        }
        else
        {
            Debug.LogError("UI元素上没有RectTransform组件！");
        }
    }

    /// <summary>
    /// 开始固定轨迹的左右摇晃，包含平滑的开始和结束过渡
    /// </summary>
    /// <param name="stepDuration">每段旋转的持续时间（秒）</param>
    public void StartShake(float stepDuration = 0.15f)
    {
        if (rectTransform == null || isShaking)
            return;

        currentShakeCoroutine = StartCoroutine(ShakePatternCoroutine(stepDuration));
    }

    /// <summary>
    /// 立即停止摇晃并平滑恢复初始旋转
    /// </summary>
    public void StopShakeImmediately()
    {
        if (currentShakeCoroutine != null)
        {
            StopCoroutine(currentShakeCoroutine);
        }

        // 平滑过渡到初始旋转后再结束
        StartCoroutine(SmoothStopCoroutine());
    }

    /// <summary>
    /// 摇晃主协程：平滑开始 → 固定轨迹 → 平滑结束
    /// </summary>
    private IEnumerator ShakePatternCoroutine(float stepDuration)
    {
        isShaking = true;
        Quaternion startRot = originalRotation;
        MusicManager.music.PlayMusic(1);  //播放选择错误印象


        // 1. 平滑开始：从初始角度缓慢加速到第一个目标角度（15°）
        Quaternion firstTarget = Quaternion.Euler(
            originalRotation.eulerAngles.x,
            originalRotation.eulerAngles.y,
            15f
        );
        yield return StartCoroutine(SmoothRotate(startRot, firstTarget, smoothTransitionTime, true));

        // 2. 固定轨迹摇晃：15°→-15°→15°→-15°（快速过渡）
        float[] rotationSteps = { -15f, 15f, -15f };
        foreach (float targetZ in rotationSteps)
        {
            Quaternion targetRot = Quaternion.Euler(
                originalRotation.eulerAngles.x,
                originalRotation.eulerAngles.y,
                targetZ
            );
            yield return StartCoroutine(SmoothRotate(rectTransform.localRotation, targetRot, stepDuration, false));
        }

        // 3. 平滑结束：从最后角度（-15°）缓慢减速回到初始位置
        yield return StartCoroutine(SmoothRotate(rectTransform.localRotation, originalRotation, smoothTransitionTime, true));

        // 完成所有动画
        isShaking = false;
        currentShakeCoroutine = null;
    }

    /// <summary>
    /// 平滑旋转到目标角度的子协程
    /// </summary>
    /// <param name="start">开始旋转</param>
    /// <param name="target">目标旋转</param>
    /// <param name="duration">持续时间</param>
    /// <param name="useSmoothCurve">是否使用平滑曲线（用于开始和结束阶段）</param>
    private IEnumerator SmoothRotate(Quaternion start, Quaternion target, float duration, bool useSmoothCurve)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // 开始和结束阶段使用缓入缓出曲线，中间阶段使用匀速曲线
            if (useSmoothCurve)
            {
                // 缓入缓出：开始慢→中间快→结束慢
                t = Mathf.SmoothStep(0f, 1f, t);
            }
            else
            {
                // 匀速过渡（保持节奏感）
                t = Mathf.Lerp(0f, 1f, t);
            }

            rectTransform.localRotation = Quaternion.Lerp(start, target, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.localRotation = target;
    }

    /// <summary>
    /// 平滑停止的协程（从当前角度缓慢回到初始位置）
    /// </summary>
    private IEnumerator SmoothStopCoroutine()
    {
        Quaternion currentRot = rectTransform.localRotation;
        yield return StartCoroutine(SmoothRotate(currentRot, originalRotation, smoothTransitionTime, true));

        isShaking = false;
        currentShakeCoroutine = null;
    }
    #endregion
}
