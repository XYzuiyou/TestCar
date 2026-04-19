using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionPanel : MonoBehaviour
{

    public Button ReturnBtn;

    public Transform LevelPart;
    public Button LeftBtn;
    public Button RightBtn;
    public Transform Part;
    public List<LevelPrefab> LevelList = new List<LevelPrefab>();



    public int CurrentIndex;  //当前的下标
    public void SetUI()
    {

        ReturnBtn = transform.Find("BG/ReturnBtn").GetComponent<Button>();
        LevelPart = transform.Find("BG/LevelPart");

              LeftBtn = transform.Find("BG/LeftBtn").GetComponent<Button>();
        RightBtn = transform.Find("BG/RightBtn").GetComponent<Button>();
        Part = transform.Find("Part");


        InitData();
        UpdateUI();

        CancelBtn();

        BindingBtn();
    }

    public void InitData()
    {
        //int Num = LevelStateGetIndex();
        //CurrentIndex = Num;
        //LoadLevelPrefab(CurrentIndex);
        LoadLevelPrefab(0);
        UpdateLeftRightBtnUI();
    }

    public void UpdateUI()
    {
    }

    public void BindingBtn()
    {
        ReturnBtn.onClick.AddListener(HidePanel);
        LeftBtn.onClick.AddListener(UpLevel);
        RightBtn.onClick.AddListener(NextLevel);
    }

    public void CancelBtn()
    {
        ReturnBtn.onClick.RemoveListener(HidePanel);
        LeftBtn.onClick.RemoveListener(UpLevel);
        RightBtn.onClick.RemoveListener(NextLevel);

    }
    public void LoadLevelIcon()
    {
        switch (CurrentIndex)
        {
      
        }
    }
    public void LoadLevelPrefab(int Num=0)
    {
        int MinNum = 0;
        switch (Num)
        {
            case 0:
                MinNum = 0;
                break;
            case 1:
                MinNum = 12;
                break;
            case 2:
                MinNum = 24;
                break;
            case 3:
                MinNum = 36;
                break;
            case 4:
                MinNum = 48;
                break;

        }
        if (LevelList.Count > 0)
        {
            for (int i = 0; i < LevelList.Count; i++)
            {
                LevelList[i].GetComponent<LevelPrefab>().SetUI(MinNum+i, this);
                if ((MinNum + i) >= 50)
                {
                    LevelList[i].gameObject.SetActive(false);
                }
                else
                {
                    LevelList[i].gameObject.SetActive(true);
                }
            }
        }


        print(WorldData.LevelState.Count);
        if (WorldData.LevelState.Count > 0)
        {

            foreach (KeyValuePair<int, int> pair in WorldData.LevelState)
            {
                if (pair.Value == 0)
                {
                    //代表当前没有通关
                    if (pair.Key == 1)
                    {
                        //如果是第一关
                        SetLevelState(1, true);  //激活第一关
                    }
                    else 
                    {

                        if (CheckLevelState(pair.Key))  //检查当前的没有 通过的关卡的上一个关卡是否通过 true就是通过了
                         {
                            //那么就激活当前的关卡
                            SetLevelState(pair.Key, true);  //激活第一关

                        }
                    }
                }

            }
        }
        else
        {
            WorldData.LevelState.Add(1,0);
            SetLevelState(1, true);  //激活第一关

        }
    }



    /// <summary>
    /// 根据当前最大的通关数值 获得当前的页签
    /// </summary>
    /// <returns></returns>
    public int LevelStateGetIndex()
    {
int Num = GetNewLevelID();

        if (Num >= 1 && Num <= 10)
        {
            return 0;
        }
        else if (Num >= 11 && Num <= 20)
        {
            return 1;
        }
        else if (Num >= 21 && Num <= 30)
        {
            return 2;
        }
        else if (Num >= 31 && Num <= 40)
        {
            return 3;
        }
        return 0;
    }


    /// <summary>
    /// 获得最新关卡的ID
    /// </summary>
    /// <returns></returns>
    public int GetNewLevelID()
    {
        foreach (KeyValuePair<int, int> pair in WorldData.LevelState)
        {
            int Key = pair.Key;
            int Value = pair.Value;
            if (Value == 0)
            {
                //如果等于0代表当前没有解说关卡
                if (Key == 1)
                {
                    //如果是关卡1
                    return 1;
                }
                else if (Key >= 2)
                {
                    //如果大于等于
                    return Key;
                }
            }
        }

        return 1;
        }
    public void UpLevel()
    {
        CurrentIndex--;
        if (CurrentIndex <= 0)
        {
            CurrentIndex = 0;
        }
        UpdateLeftRightBtnUI();
        LoadLevelPrefab(CurrentIndex);
    }

    public void NextLevel()
    {
        
        CurrentIndex++;
        if (CurrentIndex >= 5)
        {
            CurrentIndex = 4;
        }

        UpdateLeftRightBtnUI();
        LoadLevelPrefab(CurrentIndex);

    }

    public void UpdateLeftRightBtnUI()
    {
        if (CurrentIndex <= 0)

        {

            RightBtn.gameObject.SetActive(true);
            LeftBtn.gameObject.SetActive(false );

        }
        else if (CurrentIndex >= 1 && CurrentIndex <4)
        {
            RightBtn.gameObject.SetActive(true);
            LeftBtn.gameObject.SetActive(true);
        }
        else if (CurrentIndex >= 4)
        {
            RightBtn.gameObject.SetActive(false);
            LeftBtn.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 检查上一个关卡的状态
    /// </summary>
    public bool CheckLevelState(int LevelID)
    { 
        int CurrentLevelID =LevelID- 1; //关卡减少1
        if (CurrentLevelID >= 1)
        {
   


            if (WorldData.LevelState.ContainsKey(CurrentLevelID))

            {
                ///必须是激活的
                if (WorldData.LevelState[CurrentLevelID] == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        
        return false;
    }


    //设置关卡的状态
    public void SetLevelState(int LevelID,bool Value)
    {

        if (LevelList.Count > 0)
        {
            for (int i = 0; i < LevelList.Count; i++)
            {
                if (LevelList[i].CurrentLevelID == LevelID)
                {
                    if (Value)
                    {
                        LevelList[i].Unlock();
                    }
                    else
                    { 
                        LevelList[i].NoUnlocked();

                    }
                }
            }
        }
    }

    public void HidePanel()
    {
       GameObject Panel = UIManager.ui.GetTempUIPanel(UIManager.UIType.MainMenuPanel);

        Panel.GetComponent<MainMenuPanel>().SetUI();
        UIManager.ui.DeathPanel(UIManager.UIType.LevelSelectionPanel);
    }



    /// <summary>
    /// 开始游戏事件
    /// </summary>
    public void PlayGame(int ID)
    {

        UIManager.ui.DeathPanel(UIManager.UIType.LevelSelectionPanel);
        UIManager.ui.DeathPanel(UIManager.UIType.MainMenuPanel);
        WorldData.CurrentSelectLevelID = ID;  //设置当前玩家选择的关卡的ID
        BattleManager.battle.PlayGame(ID);

    }
}
