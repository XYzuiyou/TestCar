using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{

    public Image SkillIcon;
    public Button CancelBtn;
    public Button VideoBtn;
    public GameObject Panel;
    public UniversalScaleAnimation animator;
    public int CurrentSkillID;  //当前的技能的ID
    public void SetUI(int ID)
    {
        CurrentSkillID = ID;
        animator = GetComponent<UniversalScaleAnimation>();
        Panel = transform.Find("Panel").gameObject;
        SkillIcon =Panel. transform.Find("SkillIcon").GetComponent<Image>();
        CancelBtn = Panel. transform.Find("CancelBtn").GetComponent<Button>();
        VideoBtn = Panel. transform.Find("VideoBtn").GetComponent<Button >();

        // 播放序列1：0.7→1.1→1
        animator.PlaySequence1(Panel, () =>
        {
            CancelBtn.onClick.AddListener(HidePanel);
            VideoBtn.onClick.AddListener(UserSkill);
        });


        switch (ID)
        {
            case 1:
                SkillIcon.sprite = Resources.Load<Sprite>("Skill1Icon");
                break;

            case 2:
                SkillIcon.sprite = Resources.Load<Sprite>("Skill2Icon");
                break;
            case 3:
                SkillIcon.sprite = Resources.Load<Sprite>("Skill3Icon");
                break;
        }

    }
    public void UserSkill()
    {

        if (CurrentSkillID == 1)
        {
            ///当前没有在使用卡牌
            if (!BattleManager.battle.isMoveCard)
            {

                //当前没有在获得卡牌
                if (!BattleManager.battle.GetCardEvemt)
                {
                    if (ModelManager.model.CreateCardList.Count <= 15)
                    {
                        RewardEvemt();

                    }
                    else
                    {
                        DesManager.des.CreateDes("当前卡牌数量太多了");
                    }
                    //触发视频
                }

            }
        }
        else if (CurrentSkillID == 2)
        {

            if (BattleManager.battle.GetReadCardNum().Count > 0)
            {
                RewardEvemt();

            }
            else
            {
                DesManager.des.CreateDes("你点击的太快了，休息一下吧");
            }
                }
        else if (CurrentSkillID == 3)
        {
            if (BattleManager.battle.isMoveAnyCard == false)
            {
                if (!BattleManager.battle.isUserAnyCard)
                {
                    RewardEvemt();
                }
            }
        }
    }


    /// <summary>
    /// 奖励
    /// </summary>
    public void RewardEvemt()
    { 
        HidePanel();
        switch (CurrentSkillID)
        {
            case 1:
                BattleManager.battle.ExcuteSkil1();

                break;
            case 2:
                BattleManager.battle.ExcuteSkill2();
                break;
            case 3:
                BattleManager.battle.ExcuteSkill3();

                break;
        }

    }

    public void HidePanel()
    {
        animator.PlaySequence2(Panel, () =>
        {
            UIManager.ui.DeathPanel(UIManager.UIType.SkillPanel);
        });
    }

}
