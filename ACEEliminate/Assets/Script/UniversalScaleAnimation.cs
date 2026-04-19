using UnityEngine;
using System.Collections;

public class UniversalScaleAnimation : MonoBehaviour
{
    [Header("动画参数")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    /// <summary>
    /// 执行指定对象的缩放动画序列：0.3 → 1.1 → 1
    /// </summary>
    /// <param name="targetObject">要缩放的游戏对象</param>
    /// <param name="callback">动画完成后的回调</param>
    public void PlaySequence1(GameObject targetObject, System.Action callback = null)
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is null!");
            return;
        }

        StartCoroutine(AnimateScaleSequence(targetObject,
            new Vector3(0.3f, 0.3f, 0.3f),
            new Vector3(1.1f, 1.1f, 1.1f),
            Vector3.one,
            () => {
                callback?.Invoke();
            }));
    }

    /// <summary>
    /// 执行指定对象的缩放动画序列：1 → 1.1 → 0.3
    /// </summary>
    /// <param name="targetObject">要缩放的游戏对象</param>
    /// <param name="callback">动画完成后的回调</param>
    public void PlaySequence2(GameObject targetObject, System.Action callback = null)
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is null!");
            return;
        }

        StartCoroutine(AnimateScaleSequence(targetObject,
            Vector3.one,
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(0.3f, 0.3f, 0.3f),
            () => {
                callback?.Invoke();
            }));
    }

    /// <summary>
    /// 执行通用的三段式缩放动画
    /// </summary>
    private IEnumerator AnimateScaleSequence(GameObject target,
        Vector3 startScale,
        Vector3 middleScale,
        Vector3 endScale,
        System.Action onComplete = null)
    {
        Vector3 originalScale = target.transform.localScale;
        float elapsedTime = 0f;

        // 记录原始父对象（用于恢复）
        Transform originalParent = target.transform.parent;

        while (elapsedTime < animationDuration)
        {
            float progress = Mathf.Clamp01(elapsedTime / animationDuration);
            float scaledProgress = scaleCurve.Evaluate(progress);

            Vector3 targetScale;
            if (progress < 0.5f)
            {
                // 前半段：start → middle
                float t = scaledProgress * 2;
                targetScale = Vector3.Lerp(startScale, middleScale, t);
            }
            else
            {
                // 后半段：middle → end
                float t = (scaledProgress - 0.5f) * 2;
                targetScale = Vector3.Lerp(middleScale, endScale, t);
            }

            target.transform.localScale = targetScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态
        target.transform.localScale = endScale;
        onComplete?.Invoke();
    }
}