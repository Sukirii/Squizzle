using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Music;
    public AudioSource Sounds;

    public AudioClip[] musicClips;
    public AudioClip[] soundClips;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (Music.isPlaying)
            return;

        Music.clip = musicClips[0];
        Music.Play();
    }

    public void PlaySound(int _index, float _volume = 1f)
    {
        Sounds.PlayOneShot(soundClips[_index], _volume);
    }
}