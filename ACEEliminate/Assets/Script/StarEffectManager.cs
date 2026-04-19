using UnityEngine;

public class StarEffectManager : MonoBehaviour
{
    public GameObject StarPrefab;
    public float moveSpeed = 5f; // 移动速度
    public float moveDistance = 10f; // 移动距离
    public float rotationSpeed = 60f; // 旋转速度（度/秒）
    public float scaleEndDistancePercent = 0.9f; // 缩放结束时的距离百分比

    public void CreateStars(int num,Transform Part,Vector3 Pos)
    {
        for (int i = 0; i < num; i++)
        {
            // 创建星星并设置父级
            GameObject star = Instantiate(StarPrefab);
            if (Part != null)
            {
                star.transform.SetParent(Part, false);
            }

            // 设置星星初始位置（相对于父级）
            star.transform.localPosition = Pos;

            // 生成随机方向（绕Z轴旋转）
            float randomAngle = Random.Range(0f, 360f);
            Vector3 direction = Quaternion.Euler(0, 0, randomAngle) * Vector3.up;

            // 获取子物体Star
            Transform starTransform = star.transform.Find("Star");
            if (starTransform == null)
            {
                Debug.LogWarning("未找到子物体Star");
                starTransform = star.transform;
            }

            // 设置X轴角度为45度
            starTransform.localRotation = Quaternion.Euler(45f, 0f, 0f);

            // 计算缩放结束距离和总移动时间
            float scaleEndDistance = moveDistance * scaleEndDistancePercent;
            float totalMoveTime = moveDistance / moveSpeed;

            // 添加移动、缩放和旋转协程
            StartCoroutine(MoveStar(star, direction, totalMoveTime));
            StartCoroutine(ScaleStar(star, scaleEndDistance, totalMoveTime));
            StartCoroutine(RotateStar(starTransform, totalMoveTime));
        }
    }

    private System.Collections.IEnumerator MoveStar(GameObject star, Vector3 direction, float totalTime)
    {
        Vector3 startPos = star.transform.localPosition;
        float timer = 0f;

        while (timer < totalTime && star != null)
        {
            float progress = timer / totalTime;
            star.transform.localPosition = startPos + direction * moveDistance * progress;
            timer += Time.deltaTime;
            yield return null;
        }

        if (star != null) Destroy(star);
    }

    private System.Collections.IEnumerator ScaleStar(GameObject star, float endDistance, float totalTime)
    {
        Vector3 startPos = star.transform.localPosition;
        float startScale = 1f;
        float endScale = 0f;
        float scaleEndTime = (endDistance / moveDistance) * totalTime;
        float timer = 0f;

        while (timer < totalTime && star != null)
        {
            float scaleProgress = Mathf.Min(timer / scaleEndTime, 1f);
            star.transform.localScale = Vector3.Lerp(
                Vector3.one * startScale,
                Vector3.one * endScale,
                scaleProgress
            );

            if (timer >= scaleEndTime)
            {
                star.transform.localScale = Vector3.one * endScale;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private System.Collections.IEnumerator RotateStar(Transform starTransform, float totalTime)
    {
        float startRotation = 0f;
        float targetRotation = 120f;
        float timer = 0f;

        while (timer < totalTime && starTransform != null && starTransform.gameObject != null)
        {
            float progress = timer / totalTime;
            float currentRotation = Mathf.Lerp(startRotation, targetRotation, progress);
            starTransform.localRotation = Quaternion.Euler(45f, 0f, currentRotation);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}