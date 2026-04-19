using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    private Animator animator;
    private float animationLength;
    private bool animationStarted = false;

    void Start()
    {
        // 获取当前物体上的Animator组件
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("没有找到Animator组件！", gameObject);
            return;
        }

        // 获取默认动画的长度
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        if (clips.Length > 0)
        {
            // 假设我们使用第一个动画片段
            animationLength = clips[0].length;
            animationStarted = true;
            // 动画结束后调用DestroySelf方法
            Invoke("DestroySelf", animationLength);
        }
        else
        {
            Debug.LogError("动画控制器中没有找到动画片段！", gameObject);
        }
    }

    void DestroySelf()
    {
        if (animationStarted)
        {
            // 销毁当前游戏对象
            Destroy(gameObject);
        }
    }
}