using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource player { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        // 获取音频组件
        this.player = GetComponent<AudioSource>();
    }

    // 播放音效
    public void PlaySound(string name)
    {
        var clip = Resources.Load<AudioClip>(name);
        player.PlayOneShot(clip);
    }

    // 停止播放 
    public void StopSound()
    {
        player.Stop();
    }
}
