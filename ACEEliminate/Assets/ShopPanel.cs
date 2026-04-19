using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{


    public GameObject BGPart;
    public GameObject GroundPart;
    public Sprite BGImg;
    public Sprite GroundImg;

    public Image TitleBG;
    public GameObject BG1;
    public GameObject BG2;

    public int CurrentBGID;  //当前打开界面的ID  1是格子 2是背景
    public Button QuitBtn;
    public List<BGPrefab> BGList = new List<BGPrefab>();
    public List<BGPrefab> GroundList = new List<BGPrefab>();

    public Button Btn1;
    public Button Btn2;

    public static ShopPanel shop;

    private void Awake()
    {
        shop = this;
    }
    public void SetUI() {

        QuitBtn.onClick.AddListener(HidePanel);
        Btn1.onClick.AddListener(delegate { ShowBGORGround(1); });
        Btn2.onClick.AddListener(delegate { ShowBGORGround(2); });
        ShowBGORGround(1);
    }

    public void ShowBGORGround(int ID)
    {
        if (ID == 1)
        {
            BGPart.gameObject.SetActive(true);
            GroundPart.gameObject.SetActive(false );
            TitleBG.sprite =GroundImg ;
            BG1.gameObject.SetActive(true);
            BG2.gameObject.SetActive(false);
            InitBGList();
        }
        else if (ID == 2)
        {
            GroundPart.gameObject.SetActive(true);
            BGPart.gameObject.SetActive(false);
            TitleBG.sprite = BGImg;
            BG1.gameObject.SetActive(false );
            BG2.gameObject.SetActive(true);
            InitGroundList();
        }
        CurrentBGID = ID;
    }


    public void InitBGList()
    {
        if (BGList.Count > 0)
        {
            for (int i = 0; i < BGList.Count; i++)
            {
                BGList[i].InitData();
            }
        }
    }
    public void InitGroundList()
    {
        if (GroundList.Count > 0)
        {
            for (int i = 0; i < GroundList.Count; i++)
            {
                GroundList[i].InitData();
            }
        }
    }
    public void ResetBGList()
    {
        if (BGList.Count > 0)
        {
            for (int i = 0; i < BGList.Count; i++)
            {
                BGList[i].ResetData();

            }
        }
    }
    public void ResetGroundList()
    {
        if (GroundList.Count > 0)
        {
            for (int i = 0; i < GroundList.Count; i++)
            {
                GroundList[i].ResetData();
            }
        }
    }

    public void HidePanel()
    {
        UIManager.ui.DeathPanel(UIManager.UIType.ShopPanel);
    }
}
