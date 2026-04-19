using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewbiePanelTo : MonoBehaviour
{

    public GameObject OneLight;
    public Text DesText1;
    public Text DesText2;
    public Text DesText3;
    public List<GameObject> ItemList = new List<GameObject>();

    public void SetUI( )
    {
        DesText1 = transform.Find("DesText1").GetComponent<Text>();
        DesText2 = transform.Find("DesText2").GetComponent<Text>();
        DesText3 = transform.Find("DesText3").GetComponent<Text>();

        ItemList.Add(DesText1.gameObject);
        ItemList.Add(DesText2.gameObject);
        ItemList.Add(DesText3.gameObject);
        ResetItem();


    }
    public void ShowDes(int ID)
    {
        ResetItem();
        ItemList[ID].gameObject.SetActive(true);
    }

    public void ResetItem()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].gameObject.SetActive(false);
        }
                }
}
