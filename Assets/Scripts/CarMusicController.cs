using UnityEngine;

public class CarMusicController : MonoBehaviour
{
    [SerializeField] AudioClip[] Musics;
    [SerializeField] AudioSource AudioSourceObj;

    bool musicPlayed;
    public void PlayMusic()
    {
        if (musicPlayed) return;

        musicPlayed = true;
        int randomMusic = Random.Range(0, Musics.Length);
        AudioClip audioClip = Musics[randomMusic];
        AudioSourceObj.clip = audioClip;
        AudioSourceObj.Play();
    }

    public void StopMusic()
    {
        musicPlayed = false;
        AudioSourceObj.Stop();
    }
}
