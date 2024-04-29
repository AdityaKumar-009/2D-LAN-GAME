using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audio;

    public AudioClip walk;
    public AudioClip pistol;

    public AudioClip bloc;
    public AudioClip reload;

    public void playwalk()
    {
        audio.PlayOneShot(walk);
    }

    public void playpistol()
    {
        audio.PlayOneShot(pistol);
    }

    public void playbloc()
    {
        audio.PlayOneShot(bloc);
    }

    public void playreload()
    {
        audio.PlayOneShot(reload);
    }

}
