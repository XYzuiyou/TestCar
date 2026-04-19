using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPrefab : MonoBehaviour
{

    public Image IDIcon;   //排名图标
    public Image BG;
    public Text NumText;
    public Text DesText;
    public Text RoleName;
    public Text RoleNum;
    public Image Icon;

    public bool Value;
    public RankItem CurrentItem;

    public void SetUI(RankItem item)
    {
        CurrentItem = item;
        BG = transform.Find("BG").GetComponent<Image>();
        IDIcon = BG.transform.Find("IDIcon").GetComponent<Image>();
        NumText = BG.transform.Find("NumText").GetComponent<Text>();
        DesText = BG.transform.Find("DesText").GetComponent<Text>();
        RoleName = BG.transform.Find("RoleName").GetComponent<Text>();
        RoleNum = BG.transform.Find("RoleNum").GetComponent<Text>(); ;
        Icon = BG.transform.Find("RoleIconBG/Icon").GetComponent<Image>();



        UpdateUI();

    }

    public void InitData()
    {
    }
    private void Update()
    {
        if (Value == false)
        {
         
        }
        print(transform.localScale + "|||| " + transform.GetComponent<RectTransform>().sizeDelta.x + "||||" + transform.GetComponent<RectTransform>().sizeDelta.y

      );    
    }
    public void UpdateUI()
    {

        Debug.Log("开始赋值排行榜数据");
        if (CurrentItem.rank == 0)
        {
            ShowUIIcon(2);
            DesText.text = "未上榜";
            BG.sprite = Resources.Load<Sprite>("Four");

        }
        else
        if (CurrentItem.rank == 1)
        {
            ShowUIIcon(1);
            IDIcon.sprite = Resources.Load<Sprite>("金牌");
            BG.sprite = Resources.Load<Sprite>("One");
        }
        else
        if (CurrentItem.rank == 2)
        {
            ShowUIIcon(1);
            IDIcon.sprite = Resources.Load<Sprite>("银牌");
            BG.sprite = Resources.Load<Sprite>("To");

        }
        else
        if (CurrentItem.rank == 3)
        {
            ShowUIIcon(1);
            IDIcon.sprite = Resources.Load<Sprite>("铜牌");
            BG.sprite = Resources.Load<Sprite>("Three");

        }
        else
        {
            ShowUIIcon(3);
            BG.sprite = Resources.Load<Sprite>("Four");
            NumText.text = CurrentItem.rank.ToString();  //输出当前的排名 
        }


        RoleName.text = CurrentItem.nickName;

            Debug.Log("设置名称成功");




            RankingManager.Instance.LoadAvatar(CurrentItem.avatarUrl, Icon);
            Debug.Log("设置图片成功");
        
        RoleNum.text = CurrentItem.maxScore.ToString();
        Debug.Log("设置数量成功");
    }




    public void ShowUIIcon(int StateID)
    {

        if (StateID == 1)
        {
            IDIcon.gameObject.SetActive(true);
            DesText.gameObject.SetActive(false);
            NumText.gameObject.SetActive(false);
        }
        else if (StateID == 2)
        {
            IDIcon.gameObject.SetActive(false);
            DesText.gameObject.SetActive(true);
            NumText.gameObject.SetActive(false);
        }
        if (StateID == 3)
        {
            IDIcon.gameObject.SetActive(false);
            DesText.gameObject.SetActive(false);
            NumText.gameObject.SetActive(true);
        }
    }
    public void BindingBtn()
    {

    }

    public void CancelBtn()
    {

    }
}
