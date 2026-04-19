using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesManager : MonoBehaviour
{
    public static DesManager des;

    public List<GameObject> DesList;   //创建出来的消息数组

    private void Awake()
    {
        des = this;
    }
    public void CreateDes(string Value = "游戏错误")
    {
        ///加载消息的预制体
        GameObject des = Instantiate(Resources.Load<GameObject>(LoadAssetManager.load.LoadSprite(LoadType.UIItem) + "DesPrefab"));
        des.transform.SetParent(UIManager.ui.DesPart);  //设置父级
        des.transform.localPosition = Vector3.zero;   //位置清零
        des.GetComponent<DesPrefab>().SetUI(Value);
    }


    //

    /// <summary>
    /// /创建提示队列
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="DesID"></param>
    public void CreateDesList(string Value, int DesID)
    {
        if (DesList.Count > 0)
        {
            //如果大于0 代表当前拥有提示数据
            for (int i = 0; i < DesList.Count; i++)
            {
                if (DesList[i].GetComponent<DesPrefab>().DesID == DesID)
                {
                    //如果其中一个消息的 ID 等于传递进来新的消息的ID的话  就代表  这个消息已经存在了
                    return;
                }
            }


            GameObject des = Instantiate(Resources.Load<GameObject>(LoadAssetManager.load.LoadSprite(LoadType.UIItem) + "DesPrefab"));
            des.transform.SetParent(UIManager.ui.DesPartGrid);  //设置父级
            des.transform.localPosition = Vector3.zero;

            des.GetComponent<DesPrefab>().SetUI(Value, false, DesID);             //设置提示的数据

            DesList.Add(des);  //把消息添加进入数组里面


        }
    }

    /// <summary>
    /// 创建多个重复的消息
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="DesID"></param>
    public void CreateDesList(string Value)
    {


            GameObject des = Instantiate(Resources.Load<GameObject>(LoadAssetManager.load.LoadSprite(LoadType.UIItem) + "DesPrefab"));
            des.transform.SetParent(UIManager.ui.DesPartGrid);  //设置父级
            des.transform.localPosition = Vector3.zero;

            des.GetComponent<DesPrefab>().SetUI(Value, false);             //设置提示的数据

            DesList.Add(des);  //把消息添加进入数组里面


    }
}
