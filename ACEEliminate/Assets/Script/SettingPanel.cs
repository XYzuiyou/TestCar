using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{

    public Button AgainBtn;
    public Button MainBtn;
    public Button ShareBtn;
    public Button ContinueTheGameBtn;
    public Button QuitBtn;



    public Button MusicBtn;


    public GameObject Panel;
    public UniversalScaleAnimation animator;
    public void SetUI()
    {

        if (LevelManager.level.isPlay)
        {
            BattleManager.battle.isDrag = false; //当前不可以拖拽 
        }
        Panel = transform.Find("Panel").gameObject;
              AgainBtn = transform.Find("Panel/AgainBtn").GetComponent<Button>();
      MainBtn = transform.Find("Panel/MainBtn").GetComponent<Button>();
        ShareBtn = transform.Find("Panel/ShareBtn").GetComponent<Button>();
        ContinueTheGameBtn = transform.Find("Panel/ContinueTheGameBtn").GetComponent<Button>();
      QuitBtn = transform.Find("Panel/QuitBtn").GetComponent<Button>();
        MusicBtn = transform.Find("Panel/MusicBtn").GetComponent<Button>();

        animator = GetComponent<UniversalScaleAnimation>();

        // 播放序列1：0.7→1.1→1
        animator.PlaySequence1(Panel, () => {
     
        });




        InitData();
        UpdateUI();

        CancelBtn();

        BindingBtn();
    }

    public void InitData()
    {
        UpdateMusicBtnUI();
       
    }

    public void UpdateUI()
    {

    }

    public void ShowMain()
    {
        ///回到主界面
        // 播放序列2：1→1.1→0.7
        animator.PlaySequence2(Panel, () => {
            UIManager.ui.DeathPanel(UIManager.UIType.SettingPanel);
           GameDataManager.gameData.SaveGameData(); // 此时才会将修改同步到云端
            BattleManager.battle.GoToMain();

        });
    }


    /// <summary>
    /// 重新开始当前的游戏
    /// </summary>
    public void AgainGame()
    {
        // 播放序列2：1→1.1→0.7
        animator.PlaySequence2(Panel, () => {
            UIManager.ui.DeathPanel(UIManager.UIType.SettingPanel);
            BattleManager.battle.AgainGame();
        });

    }


    public void BindingBtn()
    {
        QuitBtn.onClick.AddListener(HidePanel);
        ContinueTheGameBtn.onClick.AddListener(HidePanel);
        MainBtn.onClick.AddListener(ShowMain);
        AgainBtn.onClick.AddListener(AgainGame);
        ShareBtn.onClick.AddListener(BattleManager.battle.ShareEvemt);
MusicBtn.onClick.AddListener(ChangeMusic);

    }

    public void ChangeMusic()
    {
        if (WorldData.ChangeMusicState == 0)
        {
            WorldData.ChangeMusicState = 1;
        }
        else
        if (WorldData.ChangeMusicState == 1)
        {
            WorldData.ChangeMusicState = 0;
        }
        UpdateMusicBtnUI();
    }
    public void UpdateMusicBtnUI()
    {
        if (WorldData.ChangeMusicState == 0)
        {
            MusicBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Blue");
            MusicBtn.GetComponentInChildren<Text>().text = "音乐开启";
            MusicManager.music.StopMusic();
        }
        else
    if (WorldData.ChangeMusicState == 1)
        {
            MusicBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Green");
            MusicBtn.GetComponentInChildren<Text>().text = "音乐关闭";
            MusicManager.music.PlayMusic();

        }
    }

    public void CancelBtn()
    {
        QuitBtn.onClick.RemoveListener(HidePanel);
        ContinueTheGameBtn.onClick.RemoveListener(HidePanel);


        MainBtn.onClick.RemoveListener(ShowMain);
        AgainBtn.onClick.RemoveListener(AgainGame);

        ShareBtn.onClick.RemoveAllListeners();
        MusicBtn.onClick.RemoveAllListeners();
    }




    public void HidePanel()
    {
   //     Time.timeScale = 1;  

        // 播放序列2：1→1.1→0.7
        animator.PlaySequence2(Panel, () => {
            UIManager.ui.DeathPanel(UIManager.UIType.SettingPanel);
            if (LevelManager.level.isPlay)
            {
                BattleManager.battle.isDrag = true; //当前可以拖拽 
            }
        });

    }
}
