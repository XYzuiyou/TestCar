using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class WorldData
{

    public static SystemData SystemData;

    public static int ChangeMusicState=1;  //0是停止 1是播放

    public static string currentUserName= "匿名用户";
    public static string currentUserAvatar = "https://mmbiz.qpic.cn/mmbiz/icTdbqWNOwNRna42FI242Lcia07jQodd2FJGIYQfG0LAJGFxM4FbnQP6yfMxBgJ0F3YRqJCJ1aPAK2dQagdusBZg/0";

    public static int CurrentSelectLevelID;  //当前玩家选择的关卡ID

    public static  int NewbieState = 0;   //当前是否处于新手教程状态



    public static int IceState = 0;  
    public static int Up_MoveState = 0;


    /// <summary>
    /// 当前选择的地板ID
    /// </summary>
    public static int CurrentUserGroundID = 1;
   
    //当前选择的背景的ID
    public static int CurrentUserBGID = 1;

    public static int CurrentLevelNum;  //当前通关的关卡数量
    public static  string FinishedTime = "";   //完成时间
    public static int FinishedState = 0;  //当前是否通关了




    public static Dictionary<int, int> LevelState = new Dictionary<int, int>() {
        { 1,1}//, {2,1}, {3,1}, {4,1}, {5,1},
        //{6,1}, {7,1}, {8,1}, {9,0 }, {10,1},
        //{11,1}, {12,1}, {13,1}, {14,1}, {15,1},
        //{16,1}, {17,1}, {18,1}, {19,1}, {20,1}, {21,1}, {22,1}, {23,1}, {24,1}, {25,1}, {26,1}, {27,1}, {28,1}, {29,1}, {30,1}, {31,1}, {32,1}, {33,1},
        //{34,1}, {35,1}, {36,1}, {37,1}, {38,1}, {39,1}, {40,1},{41,1},{42,1},{43,1},{44,1},{45,1},{46,1},{47,1},{48,1},{49,1},{50,1},{51,1},
        //{52,1},{53,1},{54,1},{55,1},{56,1},{57,1},{58,1},{59,1},{60,1},{61,1},{62,1},{63,1},{64,1},{65,1},{66,1},{67,1},{68,1},{69,1},{70,1},{71,1},
        //{72,1},{73,1},{74,1},{75,1},{76,1},{77,1},{78,1},{79,1},{80,1},{81,1},{82,1},{83,1},{84,1},{85,1},{86,1},{87,1},{88,1},{89,1},{90,1},{91,1},
        //{92,1},{93,1},{94,1},{95,1},{96,1},{97,1},{98,1},{99,1},{100,1},
    };
    public static Dictionary<int, int> BGState = new Dictionary<int, int>()
    {
        { 1,1},{ 2,1},{ 3,0},
    }; public static Dictionary<int, int> GroundState = new Dictionary<int, int>()
    {
        { 1,1},{ 2,1},{ 3,0},
    };

    // ======================
    // 转 JSON（上传到服务器）
    // ======================
    public static string ToMongoDBJson()
    {
        WorldDataData data = new WorldDataData();

        // 把你的数据赋值给可序列化结构
        data.CurrentLevelNum = CurrentLevelNum;
        data.FinishedState = FinishedState;
        data.FinishedTime = FinishedTime;
        data.NewbieState = NewbieState;
        data.CurrentUserBGID = CurrentUserBGID;
        data.CurrentUserGroundID = CurrentUserGroundID;
        data.IceState = IceState;
        data.Up_MoveState = Up_MoveState;
        data.LevelState = new List<LevelPair>(LevelState.Count);
        foreach (var pair in LevelState)
        {
            data.LevelState.Add(new LevelPair { key = pair.Key, value = pair.Value });
        }
        data.BGState = new List<LevelPair>(BGState.Count);
        foreach (var pair in BGState)
        {
            data.BGState.Add(new LevelPair { key = pair.Key, value = pair.Value });
        }
        data.GroundState = new List<LevelPair>(GroundState.Count);
        foreach (var pair in GroundState)
        {
            data.GroundState.Add(new LevelPair { key = pair.Key, value = pair.Value });
        }
        return JsonUtility.ToJson(data);
    }

    // ======================
    // ✅ 从服务器读取数据 → 赋值回游戏
    // ======================
    public static void LoadData(WorldDataData data)
    {
        if (data == null)
        {
            Debug.LogWarning("⚠️ 服务器无数据，使用默认数据");
            InitDefaultData();
            return;
        }

        // 服务器数据 → 游戏全局数据
        CurrentLevelNum = data.CurrentLevelNum;
        FinishedState = data.FinishedState;
        FinishedTime = data.FinishedTime;

        NewbieState = data.NewbieState;
        CurrentUserBGID = data.CurrentUserBGID;
        CurrentUserGroundID = data.CurrentUserGroundID;
        IceState = data.IceState;
        Up_MoveState = data.Up_MoveState;


        // 还原字典
        LevelState.Clear();
        foreach (var pair in data.LevelState)
        {
            LevelState[pair.key] = pair.value;
        }
        BGState.Clear();
        foreach (var pair in data.BGState)
        {
            BGState[pair.key] = pair.value;
        }
        GroundState.Clear();
        foreach (var pair in data.GroundState)
        {
            GroundState[pair.key] = pair.value;
        }

        Debug.Log("<color=cyan>✅ WorldData 已从服务器加载完成！</color>");
    }

    // ======================
    // 初始化默认数据
    // ======================
    public static void InitDefaultData()
    {
        CurrentLevelNum = 0;
        FinishedState = 0;
        FinishedTime = "";
        NewbieState = 0;
        CurrentUserBGID = 1;
        CurrentUserGroundID = 1;
        IceState = 0;
        Up_MoveState = 0;

        LevelState.Clear();
        BGState.Clear();
        GroundState.Clear();
        for (int i = 0; i <50; i++)
        {
            LevelState.Add(i, 0);
        }
        for (int i = 0; i < 4; i++)
        {
            BGState.Add(i, 0);
        }
        for (int i = 0; i < 4; i++)
        {
            GroundState.Add(i, 0);
        }

    }
}

// ======================
// 【固定结构】用于服务器存储
// ======================
[System.Serializable]
public class WorldDataData
{
    public int CurrentLevelNum = 0;
    public int FinishedState = 0;
    public string  FinishedTime = "";
    public int NewbieState = 0;
    public int CurrentUserBGID = 0;
    public int CurrentUserGroundID = 0;
    public int IceState = 0;
    public int Up_MoveState = 0;
    public List<LevelPair> LevelState = new List<LevelPair>() { new LevelPair { key = 1, value = 1 } };
    public List<LevelPair> BGState = new List<LevelPair>() { new LevelPair { key = 1, value = 1 } };
    public List<LevelPair> GroundState = new List<LevelPair>() { new LevelPair { key = 1, value = 1 } };
}

[System.Serializable]
public class LevelPair
{
    public int key;
    public int value;

}