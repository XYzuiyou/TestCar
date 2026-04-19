using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // 相机的Transform组件
    private Transform cameraTransform;
    // 原始位置
    private Vector3 originalPosition;
    // 抖动持续时间
    private float shakeDuration = 0f;
    // 抖动强度
    private float shakeIntensity = 0.7f;
    // 衰减系数
    private float dampingSpeed = 1.0f;
                                                                                                 
    public static ScreenShake screen;
    void Awake()
    {

        screen = this;
        // 如果脚本没有附加到主相机上，尝试获取主相机
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }
    }

    void OnEnable()
    {
        // 记录相机的原始位置
        originalPosition = cameraTransform.localPosition;
    }

    void Update()
    {
        // 处理抖动效果
        if (shakeDuration > 0)
        {
            // 生成随机位置偏移
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeIntensity;
            // 减少抖动持续时间
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // 抖动结束后恢复到原始位置
            shakeDuration = 0f;
            cameraTransform.localPosition = originalPosition;
        }
    }

    // 公共方法：触发屏幕抖动效果
    public void TriggerShake(float duration = 0.5f, float intensity = 0.7f, float damping = 1.0f)
    {
        // 记录原始位置
        originalPosition = cameraTransform.localPosition;
        // 设置抖动参数
        shakeDuration = duration;
        shakeIntensity = intensity;
        dampingSpeed = damping;
    }
}