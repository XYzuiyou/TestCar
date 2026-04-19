using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static MusicManager music;

    public List<AudioClip> ClipList = new List<AudioClip>();
    public Transform Pos;


    public AudioSource BGMusic;
    private void Awake()
    {
        music = this;
    }

    public void PlayMusic(int MusicID)
    {
        GameObject Prefab = Instantiate<GameObject>(Resources.Load<GameObject>("MusicPrefab"), Pos);


        Prefab.GetComponent<MusicPrefab>().SetUI(ClipList[MusicID]);
    }
    public void PlayMusic()
    {
        BGMusic.volume = 0.5f;
    }

    public void StopMusic()
    {
        BGMusic.volume = 0;

    }

}
