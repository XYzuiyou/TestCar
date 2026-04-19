using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPrefab : MonoBehaviour
{
    private AudioSource audioSource;

    public void SetUI(AudioClip Music)
    {
        // 获取音频源组件
        audioSource = GetComponent<AudioSource>();
       
        // 确保有音频源组件
        if (audioSource == null)
        {
            Debug.LogError("未找到AudioSource组件，将销毁物体");
            Destroy(gameObject);
            return;
        }
        audioSource.clip = Music;
        audioSource.Play();  //播放音频

        // 如果音频不循环，则在播放完毕后销毁
        if (!audioSource.loop)
        {
            Destroy(gameObject, audioSource.clip.length);
        }
        else
        {
            Debug.LogWarning("音频设置为循环播放，不会自动销毁物体");
        }
    }
}
