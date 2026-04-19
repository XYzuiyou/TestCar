using LitJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventory;  //单例
                                               //



    private void Awake()
    {
        inventory = this;

       
    }
    public void InitData()
    {
       ParseSystemData();


    }



/// <summary>
/// 用来解析当前系统的数据
/// </summary>
public void ParseSystemData()
{
    try
    {
        // 加载JSON文本资源
        TextAsset itemText = Resources.Load<TextAsset>("LevelData");

        // 检查资源是否成功加载
        if (itemText == null)
        {
            Debug.LogError("无法加载LevelData资源，请检查文件是否存在于Resources文件夹中");
            return;
        }

        string itemJson = itemText.text;

        // 检查JSON内容是否为空
        if (string.IsNullOrEmpty(itemJson))
        {
            Debug.LogError("LevelData资源内容为空");
            return;
        }

        // 使用LitJSON解析JSON
        JsonData jsonData = JsonMapper.ToObject(itemJson);

        // 检查解析结果
        if (jsonData == null || !jsonData.IsObject)
        {
            Debug.LogError("JSON解析失败，无效的JSON格式");
            return;
        }

        // 创建并填充UserData实例
        SystemData userData = new SystemData();

        // 检查并解析LevelData列表
        if (jsonData.ContainsKey("LevelData") && jsonData["LevelData"].IsArray)
        {
            JsonData levelDataArray = jsonData["LevelData"];

            for (int i = 0; i < levelDataArray.Count; i++)
            {
                JsonData levelItem = levelDataArray[i];

                // 确保每个item是有效的对象
                if (levelItem.IsObject)
                {
                    LevelData num = new LevelData
                    {
                        LevelID = (int)levelItem["LevelID"],
                        CardData = (string)levelItem["CardData"],
                        CurrentCardNum = (int)levelItem["CurrentCardNum"],
                        CurrentCardData = (string)levelItem["CurrentCardData"],
                        CurrentCardList = (string)levelItem["CurrentCardList"],
                    };

                    userData.LevelData.Add(num);
                }
            }
        }
        else
        {
            Debug.LogWarning("LevelData列表不存在或格式错误");
        }

        // 赋值给全局数据
        WorldData.SystemData = userData;
        Debug.Log("系统数据解析成功");
    }
    catch (Exception e)
    {
        Debug.LogError($"解析系统数据时出错: {e.Message}");
        Debug.LogException(e);
    }
}
// 添加包装类，用于处理根对象
[System.Serializable]
    public class SystemDataWrapper
    {
        public SystemData SystemData;
    }

    /// <summary>
    /// 解析系统数据的实体
    /// </summary>

}
/// <summary>
///关卡数据
/// </summary>
public class LevelData
    {
    public int LevelID;
    public string CardData;
    public int CurrentCardNum;
    public string CurrentCardData;
    public string CurrentCardList;


}

public class SystemData
{


    public List<LevelData> LevelData = new List<LevelData>();




}
