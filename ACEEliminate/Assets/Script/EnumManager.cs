using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnumManager : MonoBehaviour
{

}


/// <summary>
/// 扑克牌花色
/// </summary>
public enum PokerCardsColor
{
    Hearts,  //红桃
    Diamonds,  //方块
    Clubs,  //梅花
    Spades,  //黑桃

}


/// <summary>
/// 卡牌的方向类型
/// </summary>
public enum CardDireType
{
    Positive,   //正面
    Reverse,  //反面
}

/// <summary>
/// 扑克牌类型
/// </summary>
public enum PokerCardsType
{
A,
    To,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    J,
    Q,
    K,
    Any,

}


/// <summary>
/// 卡牌的效果类型
/// </summary>
public enum CardEffectType
{ 
    None, //普通
    Ice,  //冰冻
    Up,  //
    Down,
}
