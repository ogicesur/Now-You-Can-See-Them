using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public AudioClip collisionSound;
    private AudioSource audioSource;

    void Start()
    {
        // 获取 Audio Source 组件
        audioSource = GetComponent<AudioSource>();

        // 播放背景音乐
        PlayBackgroundMusic();
    }

    void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayCollisionSound()
    {
        if (collisionSound != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }
    }
}
