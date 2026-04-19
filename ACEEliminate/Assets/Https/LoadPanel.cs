using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour
{

    public Text SliderValueText;
    public Image SliderValue;
    public Text SliderDesText;

    public bool isInitData;   //当前初始化数据是否完成
    public bool isLoadData = true;   //是否加载完成本地数据   
    public bool isCreateScene;        //当前创建场景是否完成

    public GameObject DeathPanel;   //要被删除的面板

    public static LoadPanel load;
    public bool isOpen;
    private void Awake()
    {
        load = this;
    }

    public void SetUI(GameObject Panel =null)
    {
        if (isOpen)
        {

            return;
        }
        isOpen = true;

        SliderValue = transform.Find("BG/LoadSliderBG/LoadSliderValue").GetComponent<Image>();
        SliderValueText = transform.Find("BG/LoadSliderBG/SliderValueText").GetComponent<Text>();
        SliderDesText = transform.Find("BG/SliderDesText").GetComponent<Text>();
        SliderValue.fillAmount = 0;
        if (Panel != null)
        {
            DeathPanel = Panel;
        }
        StartCoroutine(LoadingValue());

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

    }

    public void BindingBtn()
    {

    }

    public void CancelBtn()
    {

    }



    /// <summary>
    /// 开始加载进度条
    /// </summary>
    /// <returns></returns>


    /// <summary>
    /// 开始加载进度条
    /// </summary>
    /// <returns></returns>

    public IEnumerator LoadingValue()
    {
        int Value = 0;

        while (Value < 10)
        {
            SliderValueText.text = Value + "%";
            SliderValue.fillAmount += 0.01f;
            SliderDesText.text = "加载资源中";
            Value++;
            yield return new WaitForSeconds(0.01f);
        }
        GetUserID.Instance.GetID();   //获得当前的服务器的ID
        while (!GetUserID.Instance.isGetID)  //如果当前没有获得服务器ID的话就会一致等待
        {
            yield return new WaitForSeconds(0.01f);
        }


        while (Value < 30)
        {
            SliderValueText.text = Value + "%";
            SliderValue.fillAmount += 0.01f;
            SliderDesText.text = "加载资源中";
            Value++;
            yield return new WaitForSeconds(0.01f);
        }

        GameDataManager.gameData.LoadGameData();  //开始加载游戏数据


        yield return new WaitForSeconds(0.5f);
        SliderDesText.text = "初始化游戏数据中";
        yield return InitManager.init.InitData();                  //等待初始化数据


        while (!isInitData)
        {
            yield return new WaitForSeconds(0.01f);
        }
        while (Value < 80)
        {
            SliderValueText.text = Value + "%";
            SliderValue.fillAmount += 0.01f;
            SliderDesText.text = "准备加载本地数据";
            Value++;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.1f);
        SliderDesText.text = "加载本地数据";


        while (Value < 90)
        {
            SliderValueText.text = Value + "%";
            SliderValue.fillAmount += 0.01f;
            SliderDesText.text = "加载场景模型";
            Value++;
            yield return new WaitForSeconds(0.01f);
        }



        while (Value < 100)
        {
            SliderValueText.text = Value + "%";
            SliderValue.fillAmount += 0.01f;
            SliderDesText.text = "资源即将加载完成";
            Value++;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);

        ///关闭加载界面  显示 菜单界面

        if (DeathPanel != null)
        {
            Destroy(DeathPanel);
        }
        //关于加载进度条界面

        GameManager.gam.InitData();   //在初始化 游戏数据

    }

}
