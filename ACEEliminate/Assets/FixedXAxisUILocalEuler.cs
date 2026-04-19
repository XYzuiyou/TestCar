using UnityEngine;

public class FixedXAxisUILocalEuler : MonoBehaviour
{
    // 保存初始的本地旋转 
    private Quaternion initialLocalRotation;

    void Start()
    {
        // 在开始时保存初始的本地旋转
        initialLocalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // 每一帧更新时，重置本地旋转为初始值
        transform.rotation = initialLocalRotation;
    }
}