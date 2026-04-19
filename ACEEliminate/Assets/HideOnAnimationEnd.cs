using UnityEngine;

/// <summary>
/// 检测动画状态播放完毕后隐藏物体
/// 适用于 Animator 动画控制器
/// </summary>
[RequireComponent(typeof(Animator))] // 自动添加 Animator 组件（可选）
public class HideOnAnimationEnd : MonoBehaviour
{
    [Header("动画配置")]
    [Tooltip("目标动画状态的名称（必须与 Animator 中状态机的状态名称完全一致）")]
    public string targetAnimationName; // 目标动画名称

    [Tooltip("是否检测动画的最后一帧（true=严格检测到最后一帧才隐藏，false=检测到动画退出即隐藏）")]
    public bool checkLastFrame = true;

    [Tooltip("动画播放完毕后延迟多久隐藏（单位：秒）")]
    public float hideDelay = 0f;

    private Animator _animator;
    private int _targetAnimHash; // 动画名称的哈希值（用于高效查找）
    private bool _isAnimationPlaying = false; // 动画是否正在播放
    private float _delayTimer = 0f; // 延迟隐藏计时器


    public bool Death;
    private void Awake()
    {
        // 获取当前物体上的 Animator 组件
        _animator = GetComponent<Animator>();

        // 验证 Animator 是否存在
        if (_animator == null)
        {
            Debug.LogError($"[{gameObject.name}] 没有找到 Animator 组件！脚本已禁用", this);
            enabled = false;
            return;
        }

        // 验证动画名称是否为空
        if (string.IsNullOrEmpty(targetAnimationName))
        {
            Debug.LogError($"[{gameObject.name}] 请在 Inspector 中设置目标动画名称！", this);
            enabled = false;
            return;
        }

        // 将动画名称转换为哈希值（提高性能）
        _targetAnimHash = Animator.StringToHash(targetAnimationName);
    }

    private void Update()
    {
        // 检测目标动画是否正在播放
        CheckAnimationPlayingStatus();

        // 如果动画正在播放，重置延迟计时器
        if (_isAnimationPlaying)
        {
            _delayTimer = 0f;
            return;
        }

        // 动画已结束，处理延迟隐藏
        if (hideDelay > 0f)
        {
            _delayTimer += Time.deltaTime;
            if (_delayTimer >= hideDelay)
            {
                HideGameObject();
            }
        }
        else
        {
            // 无延迟，立即隐藏
            HideGameObject();
        }
    }

    /// <summary>
    /// 检测目标动画的播放状态
    /// </summary>
    private void CheckAnimationPlayingStatus()
    {
        // 获取当前正在播放的动画状态信息
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0); // 0 表示 Base Layer（基础层）

        // 方式1：检测动画是否正在播放（包含过渡状态）
        bool isCurrentAnim = currentState.shortNameHash == _targetAnimHash;

        if (checkLastFrame)
        {
            // 严格检测：动画是否播放到最后一帧（normalizedTime >= 1 表示播放完毕）
            _isAnimationPlaying = isCurrentAnim && currentState.normalizedTime < 1f;
        }
        else
        {
            // 宽松检测：只要动画状态激活（包括过渡中）就算播放中
            _isAnimationPlaying = isCurrentAnim && currentState.normalizedTime < 1f && !_animator.IsInTransition(0);
        }
    }

    /// <summary>
    /// 隐藏当前物体
    /// </summary>
    private void HideGameObject()
    {
        gameObject.SetActive(false);

        if (Death)
        {
            Destroy(gameObject);
        }
    }


}