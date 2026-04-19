using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class GameLosePanel : MonoBehaviour
{


    public Button AgainBtn;
    public Button MainBtn;

    public GameObject Panel;

    public UniversalScaleAnimation animator;
    public bool isOpen;
    public void SetUI( )
        {


        if (isOpen)
        {
            return;
        }
        isOpen = true;
        Panel = transform.Find("Panel").gameObject;
        BattleManager.battle.isDrag = false; //当前不可以拖拽 

        AgainBtn = transform.Find("Panel/AgainBtn").GetComponent<Button>();
        MainBtn = transform.Find("Panel/MainBtn").GetComponent<Button >();
        animator = GetComponent<UniversalScaleAnimation>();


        // 播放序列1：0.7→1.1→1
        animator.PlaySequence1(Panel, () =>
            {


            });
        CancelBtn();
        BindingBtn();
    }

    public void CancelBtn()
    {
        MainBtn.onClick.RemoveListener(ShowMain);
        AgainBtn.onClick.RemoveListener(AgainGame);
    }


    public void BindingBtn()
    {                                                                                       
        MainBtn.onClick.AddListener(ShowMain);
        AgainBtn.onClick.AddListener(AgainGame);
    }

    public void HidePanel()
    {
        UIManager.ui.DeathPanel(UIManager.UIType.GameLosePanel);
    }
    public void ShowMain()
    {





        //// 播放序列2：1→1.1→0.7
        animator.PlaySequence2(Panel, () =>
        {
            ///回到主界面
            HidePanel();
            BattleManager.battle.GoToMain();
            UIManager.ui.DeathPanel(UIManager.UIType.BattleUIPanel);
            UIManager.ui.DeathPanel(UIManager.UIType.GameLosePanel);

        });
    }


    /// <summary>
    /// 重新开始当前的游戏
    /// </summary>
    public void AgainGame()
    {



        // 播放序列2：1→1.1→0.7
        animator.PlaySequence2(Panel, () =>
        {

            HidePanel();
            UIManager.ui.DeathPanel(UIManager.UIType.GameLosePanel);
            UIManager.ui.DeathPanel(UIManager.UIType.BattleUIPanel);
            BattleManager.battle.AgainGame();

        });
    }


}
