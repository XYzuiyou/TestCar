using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;

public class EvemtManager : MonoBehaviour
{
    public static EvemtManager evemt;

    private void Awake()
    {
        evemt = this;
    }

    /// <summary>
    /// 分享事件
    /// </summary>
    public void ShareEvemt()
    {

        RankManager.rank.HideWXMouseBtn();

        ShareAppMessageOption samo = new ShareAppMessageOption();
        int Num = Random.Range(0, 3);
        if (Num == 0)
        {
            samo.imageUrl = "https://mmocgame.qpic.cn/wechatgame/HwKSzQicD5rmvibTlPsEFFpm4NA4L8ImcxO1qF8EdtjkjTUCp8FjTRvLiaG6aBp2tGt/0";
            samo.imageUrlId = "1e4AsKARSWyQ25ELzx1fmw==";
        }
        else if (Num == 1)
        {
            samo.imageUrl = "https://mmocgame.qpic.cn/wechatgame/QiafRTQn7J4oicibdJYCcOa1T8ztWbImxmpxFvHaA9w381uEZNXmuwxHYMt3JYCeQ6P/0";
            samo.imageUrlId = "mhSGbHv1TUil/NZVJOOUBQ==";
        }
        else if (Num == 2)
        {
            samo.imageUrl = "https://mmocgame.qpic.cn/wechatgame/icrURWwSBY9zu6pY5iaySM0eMdFcQTYvZribGpmSpibm7DdkX5p2gSG305ps1icQWO8sI/0";
            samo.imageUrlId = "4GzLUd8pRWyOiCzw1x3T4w==";
        }

        samo.title = "抽牌匹配数字消除,一起来玩吧";
        WX.ShareAppMessage(samo);
    }

}
