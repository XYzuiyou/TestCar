using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinPanel : MonoBehaviour
{
    public Button NextBtn;
    public Button MainBtn;
    public GameObject Panel;
    public UniversalScaleAnimation animator;

    public Button ShareBtn;

    public bool isOpen;

    /// <summary>
    /// true代表当前游戏通关了
    /// false代表当前是演示关卡通关了
    /// </summary>
    /// <param name="Value"></param>
    public void SetUI()
    {

        if (isOpen)
        {
            return;
        }
        isOpen = true;
        Panel = transform.Find("Panel").gameObject;
        BattleManager.battle.isDrag = false; //当前不可以拖拽 
        NextBtn = transform.Find("Panel/NextBtn").GetComponent<Button>();
        MainBtn = transform.Find("Panel/MainBtn").GetComponent<Button>();
        ShareBtn = transform.Find("Panel/ShareBtn").GetComponent<Button>();
        animator = GetComponent<UniversalScaleAnimation>();
        MusicManager.music.PlayMusic(2);
        // 播放序列1：0.7→1.1→1
        animator.PlaySequence1(Panel, () =>
        {

        });


        InitData();
        UpdateUI();

        CancelBtn();

        BindingBtn();
    }

    public void InitData()
    {

        if (WorldData.CurrentSelectLevelID >= WorldData.CurrentLevelNum)
        {
            WorldData.CurrentLevelNum = WorldData.CurrentSelectLevelID;
            if (WorldData.CurrentLevelNum >= 50)
            {
                if (WorldData.FinishedState == 0)
                {
                    WorldData.FinishedState = 1;
                    WorldData.FinishedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        if (WorldData.CurrentLevelNum == 10)
        {
            BattleManager.battle.IDSetBGState(2,1);
        }
        else if (WorldData.CurrentLevelNum == 20)
        {
            BattleManager.battle.IDSetGroundStateState(2, 1);

        }
        else if (WorldData.CurrentLevelNum == 30)
        {
            BattleManager.battle.IDSetBGState(3, 1);

        }
        else if (WorldData.CurrentLevelNum == 40)
        {
            BattleManager.battle.IDSetGroundStateState(3, 1);

        }

        GameDataManager.gameData.SaveGameData();// 此时才会将修改同步到云端

    }

    public void UpdateUI()
    {
    

        if (WorldData.CurrentSelectLevelID >= 50)
        {
            //如果是最后一关了
            MainBtn.gameObject.SetActive(true);
            NextBtn.gameObject.SetActive(false);

        }
        else
        {
            MainBtn.gameObject.SetActive(false );
            NextBtn.gameObject.SetActive(true);
        }
        GameWinEvemt(WorldData.CurrentSelectLevelID);
    }
    public void GameWinEvemt(int ID)
    {
        if (WorldData.LevelState.ContainsKey(WorldData.CurrentSelectLevelID))
        {
            //如果存在
            WorldData.LevelState[WorldData.CurrentSelectLevelID] = 1;  //当前的关卡被激活了
        }
        else
        {
            WorldData.LevelState.Add(WorldData.CurrentSelectLevelID, 1);
        }

        if (WorldData.CurrentSelectLevelID >= WorldData.CurrentLevelNum)
        {
            WorldData.CurrentLevelNum = WorldData.CurrentSelectLevelID;
            if (WorldData.CurrentLevelNum >= 50)
            {
                if (WorldData.FinishedState == 0)
                {
                    WorldData.FinishedState = 1;
                    WorldData.FinishedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        if (WorldData.CurrentSelectLevelID == 20)
        {
            BattleManager.battle.IDSetBGState(3,1);
        }
        else if (WorldData.CurrentSelectLevelID == 40)
        {
            BattleManager.battle.IDSetGroundStateState(2, 1);
        }
        else if (WorldData.CurrentSelectLevelID == 60)
        {
            BattleManager.battle.IDSetBGState(2, 1);
        }else 
        if (WorldData.CurrentSelectLevelID == 80)
        {
            BattleManager.battle.IDSetGroundStateState(3, 1);
        }

        if (WorldData.NewbieState == 0)
        {
            if (WorldData.CurrentSelectLevelID == 1)
            {
                WorldData.NewbieState = 1;
            }
        }

        if (WorldData.NewbieState == 1)
        {
            if (WorldData.CurrentSelectLevelID ==2)
            {
                WorldData.NewbieState = 2;
            }
        }
        if (WorldData.IceState == 1)
        {
            WorldData.IceState = 2;
        }
        if (WorldData.Up_MoveState == 1)
        {
            WorldData.Up_MoveState = 2;
        }
        GameDataManager.gameData.SaveGameData();

    }

    public void BindingBtn()
    {
        ShareBtn.onClick.AddListener(BattleManager.battle.ShareEvemt);
        MainBtn.onClick.AddListener(GoToMain);
        NextBtn.onClick.AddListener(NextLevel);
    }

    public void CancelBtn()
    {
        MainBtn.onClick.RemoveListener(GoToMain);
        ShareBtn.onClick.RemoveAllListeners();
     NextBtn.onClick.RemoveListener(NextLevel);
    }


    /// <summary>
    /// 下一关
    /// </summary>
    public void NextLevel()
    {

        animator.PlaySequence2(Panel, () =>
        {
            UIManager.ui.DeathPanel(UIManager.UIType.GameWinPanel);

            UIManager.ui.DeathPanel(UIManager.UIType.BattleUIPanel);


            WorldData.CurrentSelectLevelID += 1;  //当前关卡等级增加1
            BattleManager.battle.NextGame();
        });

    }



    public void HidePanel()
    { 
            UIManager.ui.DeathPanel(UIManager.UIType.GameWinPanel);

    }
    /// <summary>
    /// 回到主界面
    /// </summary>
    public void GoToMain()
    {



        //// 播放序列2：1→1.1→0.7
        animator.PlaySequence2(Panel, () =>
        {
            UIManager.ui.DeathPanel(UIManager.UIType.GameWinPanel);
            UIManager.ui.DeathPanel(UIManager.UIType.BattleUIPanel);
            BattleManager.battle.GoToMain();


        });
    }
}
