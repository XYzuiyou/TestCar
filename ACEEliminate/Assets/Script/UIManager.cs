

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIManager : MonoBehaviour
{
    public static UIManager ui;


    public Transform UIPart;     //UI的父级
    public Transform ToPart;
    public Transform DesPart;   //提示UI的父级
    public Transform DesPartGrid;   //提示UI数组的父级
    /// <summary>
    /// 字典用来存储创建出来的UI的
    /// </summary>
    public Dictionary<string, GameObject> UIObject = new Dictionary<string, GameObject>();


    /// <summary>
    /// 创建出来的UI临时的
    /// </summary>
    public Dictionary<string, GameObject> TempUIObject = new Dictionary<string, GameObject>();



    public void Awake()
    {
        ui = this;
    }
    private void Start()
    {



    }






    public GameObject GetUIPanel(UIType type)
    {
        if (UIObject.ContainsKey(type.ToString()))    //检查字典里面是否有这个面板
        {
            GameObject Panel = UIObject[type.ToString()];   //获得面板
            return Panel;
        }

        return null;
    }
    public GameObject GetTempUIPanel(UIType type)
    {
        if (TempUIObject.ContainsKey(type.ToString()))    //检查字典里面是否有这个面板
        {
            GameObject Panel = TempUIObject[type.ToString()];   //获得面板
            return Panel;
        }
   
        return null;
    }


    /// <summary>
    /// 把面板取消或者开启交互
    /// </summary>
    public void HideAndOpenInteractionPanel(UIType type, bool value)
    {

        GameObject Panel = GetUIPanel(type);
        if (Panel != null)
        {
            Panel.GetComponent<CanvasGroup>().interactable = value;
            Panel.GetComponent<CanvasGroup>().blocksRaycasts = value;
        }

    }


    /// <summary>
    /// 创建UI面板
    /// </summary>
    /// <returns></returns>
    public GameObject CreateUIPanel(UIType type)
    {
        //如果没有这个面板
        GameObject CreatePanel = Instantiate(Resources.Load<GameObject>("UI/Panel/" + type.ToString()));   //加载面板预制体 并创建他
        Transform tempPart = null;
        tempPart = UIPart;
        CreatePanel.transform.SetParent(tempPart);
        CreatePanel.transform.localPosition = Vector3.zero;
        RectTransform transform = CreatePanel.GetComponent<RectTransform>();

        transform.offsetMin = new Vector2(0, 0);
        transform.offsetMax = new Vector2(0, 0);


         return CreatePanel;
    }
    public GameObject CreateAndAddUIPanel(UIType type,bool Value =false)
    {
        if (TempUIObject.ContainsKey(type.ToString()))
        {
            return TempUIObject[type.ToString()];
        }
        //如果没有这个面板
        GameObject CreatePanel = Instantiate(Resources.Load<GameObject>("UI/Panel/" + type.ToString()));   //加载面板预制体 并创建他
        Transform tempPart = null;
        if (Value == false)
        {
            tempPart = UIPart;
        }
        else
        {

            tempPart = ToPart;
        }
            CreatePanel.transform.SetParent(tempPart);
        CreatePanel.transform.localPosition = Vector3.zero;
        RectTransform transform = CreatePanel.GetComponent<RectTransform>();

        transform.offsetMin = new Vector2(0, 0);
        transform.offsetMax = new Vector2(0, 0);

        TempUIObject.Add(type.ToString(), CreatePanel);   //把面板添加进入字典里面
        CreatePanel.transform.localScale = Vector3.one;
        CreatePanel.transform.localEulerAngles = Vector3.zero;
        return CreatePanel;
    }

    
    
    /// <summary>
    /// 删除UI面板
    /// </summary>
    /// <param name="type"></param>
    public void DeathPanel(UIType type)
    {
        if (TempUIObject.ContainsKey(type.ToString()))
        {

            GameObject Panel = TempUIObject[type.ToString()];
            TempUIObject.Remove(type.ToString());
            Destroy(Panel);

        }
    }

    /// <summary>
    /// 显示UI面板
    /// </summary>
    public GameObject ShowUIPanel(UIType type, ShowType showType = ShowType.None)
    {

        if (UIObject.ContainsKey(type.ToString()))    //检查字典里面是否有这个面板
        {


            GameObject Panel = UIObject[type.ToString()];   //获得面板
            Panel.GetComponent<CanvasGroup>().alpha = 1;
            Panel.GetComponent<CanvasGroup>().interactable = true;  //可以交互了
            Panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            ExecuteState(Panel, showType);
            return Panel;
        }
        //如果没有这个面板
        GameObject CreatePanel = Instantiate(Resources.Load<GameObject>("UI/Panel/" + type.ToString()));   //加载面板预制体 并创建他
        Transform tempPart = null;
        tempPart = UIPart;
        CreatePanel.transform.SetParent(tempPart);
        CreatePanel.transform.localPosition = Vector3.zero;
        RectTransform transform = CreatePanel.GetComponent<RectTransform>();
        CreatePanel.GetComponent<CanvasGroup>().alpha = 1;
        CreatePanel.GetComponent<CanvasGroup>().interactable = true;  //可以交互了
        CreatePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.offsetMin = new Vector2(0, 0);
        transform.offsetMax = new Vector2(0, 0);

        //     CreatePanel.transform.localScale = Vector3.one;
        UIObject.Add(type.ToString(), CreatePanel);   //把面板添加进入字典里面
        ExecuteState(CreatePanel, showType);
        return CreatePanel;
    }




    /// <summary>
    /// 执行打开的状态
    /// </summary>
    /// <param name="type"></param>
    public void ExecuteState(GameObject Panel, ShowType type)
    {
        switch (type)
        {
            case ShowType.None:
                break;
            case ShowType.ZoomIn:

                Panel.transform.localScale = Vector3.zero;
                StartCoroutine(ZoomInAndZoomOut(Panel, Vector3.one));
                break;
            case ShowType.NarrowDown:

                break;
        }
    }



    /// <summary>
    /// 放大或者缩小携程
    /// </summary>
    /// <returns></returns>

    public IEnumerator ZoomInAndZoomOut(GameObject panel, Vector3 targetLocal, float duration = 0.3f)
    {
        // 获取Panel的Transform组件
        Transform panelTransform = panel.transform;

        // 记录初始缩放值
        Vector3 initialScale = panelTransform.localScale;

        // 计算每帧需要变化的缩放值
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            // 计算当前缩放值
            panelTransform.localScale = Vector3.Lerp(initialScale, targetLocal, elapsedTime / duration);

            // 等待一帧
            yield return null;

            // 增加已过去的时间
            elapsedTime += Time.deltaTime;
        }

        // 确保最终缩放值达到目标值
        panelTransform.localScale = targetLocal;
    }


    /// <summary>
    /// UI界面显示的类型枚举
    /// </summary>
    public enum ShowType
    {
        None,
        ZoomIn,                //放大
        NarrowDown,        //缩小


    }
    /// <summary>
    /// 删除面板
    /// </summary>

    public bool DestoryPanel(UIType type)
    {

        if (UIObject.ContainsKey(type.ToString()))    //检查字典里面是否有这个面板
        {
            if (type == UIType.MainMenuPanel)
            {

                RankManager.rank.HideWXMouseBtn();
            }
            GameObject Panel = UIObject[type.ToString()];   //获得面板
            Destroy(Panel);  //删除面板

            return true;
        }
        return false;  //删除失败
    }




    /// <summary>
    /// 关闭面板
    /// </summary>
    public GameObject HidePanel(UIType type)
    {

        if (UIObject.ContainsKey(type.ToString()))    //检查字典里面是否有这个面板
        {
            GameObject Panel = UIObject[type.ToString()];   //获得面板
            Panel.GetComponent<CanvasGroup>().alpha = 0;
            Panel.GetComponent<CanvasGroup>().interactable = false;  //不可以交互了
            Panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            return Panel;
        }

        //提示错误
        return null;
    }

    /// <summary>
    /// 关闭UI面板
    /// </summary>
    public void HideUIPanel(UIType type)
    {

        if (UIObject.ContainsKey(type.ToString()))    //检查字典里面是否有这个面板
        {
            GameObject Panel = UIObject[type.ToString()];   //获得面板
         //   Panel.GetComponent<UIPanel>().HidePanel();
        }


    }


    /// <summary>
    /// UI面板的类型
    /// </summary>
    public enum UIType
    {
        None,
        LoadPanel,
        BattleUIPanel,
        MainMenuPanel,
        RankingPanel,
        SettingPanel,
        LevelSelectionPanel,
GameLosePanel,
        GameWinPanel,
        NewbiePanelTo,
        PlayGameDesPanel,
        ShopPanel,
        SkillPanel,
    }


    /// <summary>
    /// 关闭所有的UI面板
    /// </summary>
    public void HideAllPanel()
    {
        if (UIObject.Count > 0)
        {
            foreach (KeyValuePair<string, GameObject> pair in UIObject)
            {
                pair.Value.GetComponent<CanvasGroup>().alpha = 0;
                pair.Value.GetComponent<CanvasGroup>().interactable = false;  //不可以交互了
                pair.Value.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }


}
