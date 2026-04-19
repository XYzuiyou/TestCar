using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextPrefab : MonoBehaviour
{

    public Text ScoreText;

    private void Start()
    {
       /// SetUI("+5x2kd",Color.red);
    }

    /// <summary>
    /// 设置数值效果
    /// </summary>
    public void SetUI(string Value,Color color)
    {
        ScoreText = transform.Find("ScoreText").GetComponent<Text>();
        ScoreText.text = Value;
        ScoreText.color = color;
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = transform.position;
        StartCoroutine(ScaleAndMoveSequence());
    }

    public float scaleDuration = 0.1f; // 缩放持续时间
    public float moveDistance = 1f; // Y轴移动距离
    public float fadeStartPosition = 0.8f; // 开始淡出的Y位置

    private CanvasGroup canvasGroup;
    private Vector3 initialPosition;



    private IEnumerator ScaleAndMoveSequence()
    {
        // 缩放阶段：从0到1.05
        yield return StartCoroutine(ScaleObject(0f, 1.05f, scaleDuration * 0.5f));

        // 缩放阶段：从1.05到1
        yield return StartCoroutine(ScaleObject(1.05f, 1f, scaleDuration * 0.5f));

        // 移动阶段：向上移动并淡出
        yield return StartCoroutine(MoveAndFade());
    }

    private IEnumerator ScaleObject(float startScale, float endScale, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float scale = Mathf.Lerp(startScale, endScale, elapsedTime / duration);
            transform.localScale = Vector3.one * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one * endScale;
    }

    private IEnumerator MoveAndFade()
    {
        float startY = initialPosition.y;
        float endY = initialPosition.y + moveDistance;
        float elapsedTime = 0f;
        float totalDuration = 0.4f; // 移动总时间，可根据需要调整

        while (elapsedTime < totalDuration)
        {
            float yPos = Mathf.Lerp(startY, endY, elapsedTime / totalDuration);
            transform.position = new Vector3(initialPosition.x, yPos, initialPosition.z);

            // 计算当前Y位置对应的透明度
            if (yPos >= startY + fadeStartPosition)
            {
                float fadeProgress = (yPos - (startY + fadeStartPosition)) / (endY - (startY + fadeStartPosition));
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeProgress);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终位置和透明度
        transform.position = new Vector3(initialPosition.x, endY, initialPosition.z);
        canvasGroup.alpha = 0f;
        yield return new WaitForSeconds(0.05f);

        Destroy(gameObject);
    }
}
