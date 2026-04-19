using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathManager
{
    public static float ChangeAngle(float angle)
    {
        // 使用模运算将角度转换到 -360 到 360 度的范围内
        angle = angle % 360;

        // 如果角度为负数，则加上 360 度将其转换为正数
        if (angle < 0)
        {
            angle += 360;
        }

        return angle;
    }

    // 分类方法：根据输入数值返回true或者false
    public static  bool GetClassifiedOutput(int number)
    {
        // 处理0的情况
        if (number == 0)
            return true;

        // 处理奇数的情况
        if (number % 2 != 0)
            return true;

        // 处理偶数的情况
        return false;
    }
}
