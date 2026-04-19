using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPrefab : MonoBehaviour
{


    public Text LevelNum;
    public int CurrentLevelID;  //当前关卡ID

    public Button Icon;
    public bool isNotUnlocked;  //true\可以游玩  false不可以游玩
    public LevelSelectionPanel level;
    public void SetUI(int ID,LevelSelectionPanel le)
    {
        CurrentLevelID = ID+1;
        level = le;
        LevelNum = transform.Find("Icon/LevelNum").GetComponent<Text>();
        Icon = transform.Find("Icon").GetComponent<Button>();
        InitData();
        UpdateUI();

        CancelBtn();

        BindingBtn();
    }

    public void InitData()
    {
    }

    public void UpdateUI()
    {
        if (WorldData.LevelState.Count > 0)
        {

            if (WorldData.LevelState.ContainsKey(CurrentLevelID))
            {

                if (WorldData.LevelState[CurrentLevelID] == 1)
                {
                    //如果等于1代表当前解锁了
                    Unlock();
                }
                else
                {
                    NoUnlocked();
                }
            }
            else
            {
                NoUnlocked();
            }




            }
        else
        {
  
                NoUnlocked();
   
        }
    }
    public void NoUnlocked()
    {
        isNotUnlocked = false;
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("2");
        LevelNum.color = Color.black;
        LevelNum.text = CurrentLevelID.ToString();

        // LevelNum.gameObject.SetActive(false);
    }

    public void Unlock()
    {
        isNotUnlocked = true;

        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("1");
        //      LevelNum.gameObject.SetActive(true);
        LevelNum.color = Color.white;
        LevelNum.text = CurrentLevelID.ToString();
    }


    public void BindingBtn()
    {
        Icon.onClick.AddListener(SelectLevel);
    }

    public void CancelBtn()
    {
        Icon.onClick.RemoveListener(SelectLevel);

    }



    /// <summary>
    /// /选择当前的关卡
    /// </summary>
    public void SelectLevel()
    {


        if (isNotUnlocked)
        {
            level.PlayGame(CurrentLevelID);
        }
        else
        {
            DesManager.des.CreateDes("关卡未解锁");
        }
    }
}
