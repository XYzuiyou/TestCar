using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGPrefab : MonoBehaviour
{
    public Image BG;
    public Button UserBtn;
    public Text Des;
    public GameObject InUseBtn;

    public int SkinID;  //当前的皮肤的ID
    public int BGID;  //页签ID

    public bool isHave;  //当前是否存在
    /// <summary>
    /// 进行使用皮肤
    /// </summary>
    public void UserSkin()
    {
        if (isHave)
        {
            if (BGID == 1)
            {
                ShopPanel.shop.ResetBGList(); 
                WorldData.CurrentUserBGID = SkinID;
                UserBtn.gameObject.SetActive(false);
                InUseBtn.gameObject.SetActive(true);
                Des.gameObject.SetActive(false);

            }
            else if (BGID == 2)
            {
                ShopPanel.shop.ResetGroundList();
                WorldData.CurrentUserGroundID = SkinID;
                UserBtn.gameObject.SetActive(false);
                InUseBtn.gameObject.SetActive(true);
                    Des.gameObject.SetActive(false);

            }

            GameDataManager.gameData.SaveGameData();
        }
    }


    /// <summary>
    /// 开始重置数据
    /// </summary>
    public void ResetData()
    {
        if (isHave)
        {
            InUseBtn.gameObject.SetActive(false);
            UserBtn.gameObject.SetActive(true);
        }
    }
    public void InitData()
    {
        UserBtn.onClick.RemoveAllListeners();
        UserBtn.onClick.AddListener(UserSkin);
     //   SelectDes = transform.Find("SelectDes").gameObject;
        InUseBtn = transform.Find("InUseBtn").gameObject;
        Des = transform.Find("Des").GetComponent<Text>();
        if (BGID == 1)
        {
            //1是背景
            bool Vlaue = BattleManager.battle.IDGetBGState(SkinID);
            isHave = Vlaue;
            if (Vlaue)
            {
                //如果存在
                if (WorldData.CurrentUserBGID == SkinID)
                {
                    //如果一致
                    InUseBtn.gameObject.SetActive(true);
                    UserBtn.gameObject.SetActive(false);

                    Des.gameObject.SetActive(false);
                }
                else
                {
                    //不一致
                    InUseBtn.gameObject.SetActive(false );
                    UserBtn.gameObject.SetActive(true);
                    Des.gameObject.SetActive(false);

                }
            }
            else
            { 
                InUseBtn.gameObject.SetActive(false);
                UserBtn.gameObject.SetActive(false);
                Des.gameObject.SetActive(true);

                Des.text = GetDes(BGID,SkinID);
            }
        }
        else if (BGID == 2)
        {
            //2是 圆环皮肤
            bool Vlaue = BattleManager.battle.IDGetGroundState(SkinID);
            isHave = Vlaue;

            if (Vlaue)
            {
                //如果存在
                if (WorldData.CurrentUserGroundID == SkinID)
                {
                    //如果一致
                    InUseBtn.gameObject.SetActive(true);
                    UserBtn.gameObject.SetActive(false);
                    Des.gameObject.SetActive(false);

                }
                else
                {
                    //不一致
                    InUseBtn.gameObject.SetActive(false);
                    UserBtn.gameObject.SetActive(true);
                    Des.gameObject.SetActive(false);

                }
            }
            else
            {
                InUseBtn.gameObject.SetActive(false);
                UserBtn.gameObject.SetActive(false);
                Des.gameObject.SetActive(true);

                Des.text = GetDes(BGID, SkinID);
            }
        }
    }

    /// <summary>
    /// 根据页签ID和皮肤ID获得提示
    /// </summary>
    /// <param name="BGID"></param>
    /// <param name="SkinID"></param>
    /// <returns></returns>
    public string GetDes(int BGID, int SkinID)
    {

        string Des = "";
        if (BGID == 1)
        {
            //背景
            if (SkinID == 2)
            {
                Des = "通关10关解锁";
            }
            else if (SkinID == 3)
            { 
                Des = "通关30关解锁";

            }
        }
        else if (BGID == 2)
        {
            if (SkinID == 2)
            {
                Des = "通关20关解锁";
            }
            else if (SkinID == 3)
            {
                Des = "通关40关解锁";

            }
        }

        return Des;
    }

}
