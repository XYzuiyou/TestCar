using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class GameManager : MonoBehaviour
{
    public static GameManager gam;

    public bool isTest;

    public bool isStartBomb;  //是否启动炸弹
    private void Awake()
    {
        gam = this;
    }



    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        GameObject MainPanel = UIManager.ui.CreateAndAddUIPanel(UIManager.UIType.MainMenuPanel);
        MainPanel.GetComponent<MainMenuPanel>().SetUI();

        UIManager.ui.DeathPanel(UIManager.UIType.LoadPanel);
    }
    public void TriggerVibration()
    {

        print("手机震动");
        VibrateShortOption vs = new VibrateShortOption();
        vs.type = "heavy";
        WX.VibrateShort(vs);

    }


    public void CleanupGameObjectList(List<GameObject> list)
    {
        // 检查列表是否为null或为空
        if (list == null || list.Count == 0)
        {
            Debug.Log("列表为空或未初始化，无需清理。");
            return;
        }

        // 从后向前遍历列表，移除空引用或已销毁的游戏对象
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null || !list[i])
            {
                list.RemoveAt(i);
                Debug.Log($"已移除索引 {i} 处的空引用。");
            }
        }
    }
    public void CleanupGameObjectList(List<CardPrefab> list)
    {
        // 检查列表是否为null或为空
        if (list == null || list.Count == 0)
        {
            Debug.Log("列表为空或未初始化，无需清理。");
            return;
        }

        // 从后向前遍历列表，移除空引用或已销毁的游戏对象
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null || !list[i])
            {
                list.RemoveAt(i);
                Debug.Log($"已移除索引 {i} 处的空引用。");
            }
        }
    }

    /// <summary>
    /// 把物体的层级设置在最后一个
    /// </summary>
    /// <param name="currentModel"></param>
    public void SetToLastSibling(GameObject currentModel)
    {
        if (currentModel == null)
        {
            Debug.LogError("当前物体为空，无法调整层级位置！");
            return;
        }

        // 将物体设置为父物体子列表中的最后一个
        currentModel.transform.SetAsLastSibling();


    }

    /// <summary>
    /// 清除一个物体下面的所有子集
    /// </summary>
    /// <param name="part"></param>
    public void DeathChild(Transform part)
    {
        for (int i = part.childCount - 1; i >= 0; i--)
        {

            Destroy(part.GetChild(i).gameObject);
        }
    }



    /// <summary>
    /// 根据关卡的ID获得关卡的数据信息
    /// </summary>
    /// <returns></returns>
    public LevelData LevelIDGetLevelData(int ID)
    {
        LevelData data = new LevelData();

        if (WorldData.SystemData.LevelData.Count > 0)
        {
            for (int i = 0; i < WorldData.SystemData.LevelData.Count; i++)
            {
                if (WorldData.SystemData.LevelData[i].LevelID == ID)
                {
                    return WorldData.SystemData.LevelData[i];
                }
            }
        }

        return data;
    }

    public void SetUIRectTransform(GameObject Panel, float Value, bool xORy = true)
    {
        Vector2 sizeDelta = Panel.GetComponent<RectTransform>().sizeDelta;
        // 设置新的高度
        if (xORy)
        {
            sizeDelta.x = Value;
        }
        else
        {
            sizeDelta.y = Value;
        }

        // 应用新的大小Delta
        Panel.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }

    // 跳字效果的协程方法 
    public IEnumerator TextJumpCoroutine(int mTargetValue, Text m_JumpText)
    {
        float mUpdateTime = 0.7f; // 跳字总时间 
        float mUpdateDeltaTime = 0.03f; // 每次更新的时间间隔 
        int mLastValue = 0; // 上一次显示的数值 
        int mCurValue = 0; // 当前显示的数值 
        bool useRangeNum = true; // 是否使用随机数 

        // 如果当前显示值等于目标值，则直接返回 
        if (mLastValue == mTargetValue)
            yield break;

        float elapsedTime = 0f; // 已经经过的时间 

        // 在指定时间内逐步更新数值 
        while (elapsedTime < mUpdateTime)
        {
            if (mTargetValue - mLastValue > 0)
            {
                if (useRangeNum)
                {
                    // 计算随机范围参数 
                    int rangeParameter = (int)Mathf.Pow(10, (mTargetValue - mLastValue).ToString().Length);
                    // 计算当前显示值，使用随机数 
                    mCurValue = (mTargetValue / rangeParameter) * rangeParameter + UnityEngine.Random.Range(rangeParameter / 10, rangeParameter);
                }
                else
                {
                    // 计算当前显示值，不使用随机数 
                    mCurValue = mLastValue + (int)((mTargetValue - mLastValue) / mUpdateTime * elapsedTime);
                }
            }
            else
            {
                // 如果偏移值小于等于0，直接设置当前值为目标值 
                mCurValue = mTargetValue;
            }

            // 更新文本显示 
            m_JumpText.text = mCurValue.ToString();

            // 增加已经过的时间 
            elapsedTime += mUpdateDeltaTime;

            // 等待下一帧 
            yield return null;
        }

        // 结束时设置最终值 
        m_JumpText.text = mTargetValue.ToString();
        mLastValue = mCurValue = mTargetValue;
    }
}
