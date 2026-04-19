using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{



    public LevelData CurrentData;

    public static LevelManager level;

    public bool isPlay;  //当前是否可以开始游戏了


    private void Awake()
    {
        level = this;
    }


    public float  CheckTime = 0;  //当前检测的时间
    public float CheckSkillDesTime = 0;  //检测技能提示时间
    private void Start()
    {
    }
    public void ClearData()
    {
    }
    public void PlayGame(int LevelID)
    {

        GameObject Panel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.BattleUIPanel, true);
        Panel.GetComponent<BattleUIPanel>().SetUI();
        LevelData lv = GameManager.gam.LevelIDGetLevelData(LevelID);
        CurrentData = lv;
        Camera.main.orthographicSize = 35;
        string ModelPath = "Model" + LevelID;
        GameObject ModelPrefab = Instantiate<GameObject>(Resources.Load<GameObject>("Model/" + ModelPath));
        if (BattleUIPanel.battle.ScenePart.childCount > 0)
        {
            GameManager.gam.DeathChild(BattleUIPanel.battle.ScenePart);
        }
        ModelPrefab.transform.SetParent(BattleUIPanel.battle.ScenePart);
        ModelPrefab.transform.localPosition = new Vector3(0, 0,0);
        ModelPrefab.GetComponent<ModelManager>().SetUI(CurrentData);
        ModelPrefab.transform.localScale = Vector3.one;

        isPlay = true;

        if (WorldData.NewbieState == 0)
        {
            if (WorldData.CurrentSelectLevelID == 1)
            {
                GameObject NewbiePanelTo = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.NewbiePanelTo, true);
                NewbiePanelTo.GetComponent<NewbiePanelTo>().SetUI();
                NewbiePanelTo.GetComponent<NewbiePanelTo>().ShowDes(0);
            }
        }

        InitData();
    }


    /// <summary>
    /// 进行初始化数据
    /// </summary>
    public void InitData()
    {
        ClearData();
      

    }
 


    public void Update()
    {
        if (Input.GetKeyDown("q"))
        {
          
        }

        if (isPlay)
        {
            CheckTime += Time.deltaTime;  //时间开始自增
            CheckSkillDesTime += Time.deltaTime;
            if (CheckTime >= 0.3f)
            {
                CheckTime = 0;

                if (BattleUIPanel.battle != null)
                {
                    BattleUIPanel.battle.CheckGameEnd();
                }
            }

            if (CheckSkillDesTime >= 2)
            {
                CheckSkillDesTime = 0;
                if (BattleUIPanel.battle != null)
                {
                    BattleUIPanel.battle.UpdateSkillDes();
                }
            }
        }

    }
 



    ///缩小模型
    public IEnumerator ShrinkModel(GameObject model)
    {
        if (model == null)
        {
            Debug.LogError("模型为空，无法执行缩放操作");
            yield break;
        }

        // 记录初始缩放值
        Vector3 originalScale = model.transform.localScale;
        // 目标缩放值
        Vector3 targetScale = new Vector3(0.3f, 0.3f, 0.3f);
        // 缩放动画持续时间（可根据需要调整）
        float duration = 0.3f;
        float elapsedTime = 0f;

        // 平滑缩放过程
        while (elapsedTime < duration)
        {
            // 计算当前插值比例（0到1之间）
            float t = elapsedTime / duration;
            // 使用Lerp进行线性插值，也可以用Mathf.SmoothStep实现更平滑的效果
            model.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);

            // 累加时间并等待下一帧
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终缩放精确设置为目标值
        model.transform.localScale = targetScale;

        // 等待一小段时间展示最终状态（可选）
        yield return new WaitForSeconds(0.1f);
        // 删除模型
        Destroy(model);
    }


    public float MoveSpeed= 0.15f;
    public IEnumerator MoveItem(GameObject ItemA, GameObject ItemB)
    {
        // 检查物体是否存在
        if (ItemA == null || ItemB == null)
        {
            Debug.LogError("移动的物体不能为空！");
            yield break;
        }

        // 记录起始位置和目标位置
        Vector3 startPos = ItemA.transform.position;
        Vector3 targetPos = ItemB.transform.position;

        float elapsedTime = 0f;

        // 持续移动直到达到目标
        while (elapsedTime < MoveSpeed)
        {
            // 计算移动进度（0到1之间）
            float t = elapsedTime / MoveSpeed;



            // 计算当前位置并移动
            ItemA.transform.position = Vector3.Lerp(startPos, targetPos, t);

            // 累加时间并等待下一帧
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终位置准确
        ItemA.transform.position = targetPos;
        yield return StartCoroutine(ScaleCoroutine(ItemA));
    }

    /// <summary>
    /// 缩放动画协程
    /// </summary>
    private IEnumerator ScaleCoroutine(GameObject Model)
    {
        float halfDuration = 0.15f / 2; // 一半的时间用于放大，一半用于缩小
        float elapsedTime = 0f;

        // 动画前半段：放大到1.1倍
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            // 计算0到1之间的插值比例
            float t = elapsedTime / halfDuration;
            // 线性插值计算缩放因子
            float scaleFactor = Mathf.Lerp(1f, 1.1f, t);
            Model. transform.localScale = new Vector3(1, 1, 1) * scaleFactor;
            yield return null;
        }

        // 确保准确到达最大缩放
        Model.transform.localScale = new Vector3(1, 1, 1) * 1.1f;

        // 重置计时器，准备后半段动画
        elapsedTime = 0f;

        // 动画后半段：缩小回原大小
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / halfDuration;
            float scaleFactor = Mathf.Lerp(1.1f, 1f, t);
            Model.transform.localScale = new Vector3(1, 1, 1) * scaleFactor;
            yield return null;
        }

        // 确保准确回到原始大小
        Model.transform.localScale = new Vector3(1,1,1);
    }


    /// <summary>   0是3个纯色  1是6个纯色，2是2个不同颜色，3是4个村色，4是2个村色 ， 5是3个村色1个不同色、
    /// 6是3个纯色2个其他颜色  7是2个不同色和3个同色   8是2个村色1个颜色3个村色    9是2个村色4个村色   10是1个色 2个村色  2个村色    11是3个不同颜色
    /// 开始随机获得当前圆环的数量的ID
    /// </summary>
    /// 0是随机
    public List<int> GetCircleNumID()
    {
        List<int> ID = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            int Value = UnityEngine.Random.Range(0, 12);

            ID.Add(Value);
        }

        return ID;

    }



}
[SerializeField]
/// <summary>
/// 卡牌数据
/// </summary>
public class CardData
{
    public PokerCardsType type;
    public PokerCardsColor color;
}
