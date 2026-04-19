using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesPrefab : MonoBehaviour
{

    public Text DesText;     //提示的文本内容
    public int DesID = 0;    ///用来存储当前提示的ID的
    public float DeathTime;    //死亡时间



    /// <summary>
    /// 设置一些提示的属性信息
    /// </summary>
    /// <param name="DesValue"></param>
    /// <param name="isMove"></param>
    /// <param name="DesID"></param>
    public void SetUI(string DesValue, bool isMove = true, int DesID = 0)
    {
        DesText = transform.Find("DesText").GetComponent<Text>();  //获得提示文本
        DesText.text = DesValue;   //输出提示内容

        this.DesID = DesID;   //设置提示信息的ID 默认为0

        if (isMove)
        {
            //判断当前的提示预制体是否向上移动
            StartCoroutine(UpMove());
        }
        else
        {

            StartCoroutine(HidePanel());
            ///不移动
        }


    }


    /// <summary>
    /// 向上移动的携程
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpMove()
    {
        float tempValue = 0;
        while (tempValue < 0.5f)
        {
            //如果值小于0.5的
            tempValue += 0.01f;
            transform.GetComponent<RectTransform>().position += new Vector3(0, 2, 0);  //给他的Y值 相加
            yield return new WaitForSeconds(0.01f);

        }

        //就需要关闭界面
        StartCoroutine(HidePanel());
    }


    /// <summary>
    /// 关闭面板
    /// </summary>
    /// <returns></returns>
    public IEnumerator HidePanel()
    {

        float tempValue = 0;
        yield return new WaitForSeconds(DeathTime);  //等待这个时间
        while (tempValue < 0.5f)
        {
            //如果小于的话
            tempValue += 0.01f;
            transform.GetComponent<CanvasGroup>().alpha -= 0.02f;    //透明度减速
            yield return new WaitForSeconds(0.01f);
        }
        if (DesID > 0)
        {
            //如果大于0 代表当前的这个数据可能是多个数据 
            DesManager.des.DesList.Remove(gameObject);  //在数组中移除自身
        }
        Destroy(gameObject);   //删除自身




    }
}
