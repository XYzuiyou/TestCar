using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScaleAnimation : MonoBehaviour
{
    [Header("缩放参数")]
    [SerializeField] private float shrinkScale = 0.7f;
    [SerializeField] private float overshootScale = 1.1f;
    [SerializeField] private float animationDuration = 0.5f; // 总动画时长

    [Header("动画曲线")]
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Button button;
    private Vector3 originalScale;
    private bool isAnimating = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;

        if (button != null)
        {
            // 注册指针按下事件
            EventTrigger trigger = GetEventTrigger();
            AddEventTrigger(trigger, EventTriggerType.PointerDown, OnPointerDown);
        }
        else
        {
            Debug.LogError("Button component not found!", gameObject);
        }
    }

    private EventTrigger GetEventTrigger()
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();
        return trigger;
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType type, System.Action callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((data) => { callback?.Invoke(); });
        trigger.triggers.Add(entry);
    }

    private void OnPointerDown()
    {
        if (isAnimating) return;

                MusicManager.music.PlayMusic(0);

        isAnimating = true;
        StartCoroutine(AnimateButton());
    }

    private IEnumerator AnimateButton()
    {
        float elapsedTime = 0f;

        // 完整的三重缩放动画: 缩小→放大→恢复
        while (elapsedTime < animationDuration)
        {
            float progress = Mathf.Clamp01(elapsedTime / animationDuration);
            float scaledProgress = scaleCurve.Evaluate(progress);

            // 0-0.3: 缩小到0.7
            // 0.3-0.6: 放大到1.1
            // 0.6-1.0: 恢复到1.0
            Vector3 targetScale;

            if (progress < 0.3f)
            {
                // 缩小阶段
                float t = scaledProgress / 0.3f;
                targetScale = Vector3.Lerp(originalScale, originalScale * shrinkScale, t);
            }
            else if (progress < 0.6f)
            {
                // 过度放大阶段
                float t = (scaledProgress - 0.3f) / 0.3f;
                targetScale = Vector3.Lerp(originalScale * shrinkScale, originalScale * overshootScale, t);
            }
            else
            {
                // 恢复阶段
                float t = (scaledProgress - 0.6f) / 0.4f;
                targetScale = Vector3.Lerp(originalScale * overshootScale, originalScale, t);
            }

            transform.localScale = targetScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态
        transform.localScale = originalScale;
        isAnimating = false;
    }

    private void OnDestroy()
    {
        // 清理事件监听
    }
}