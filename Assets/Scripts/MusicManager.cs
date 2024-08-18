using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] AudioClip[] musicTracks;
    bool playing = false;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
            //StartCoroutine(PlayRandomMusic());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*IEnumerator PlayRandomMusic()
    {
        while (true)
        {
            int mus = Random.Range(0, musicTracks.Length);
            GetComponent<AudioSource>().PlayOneShot(musicTracks[mus]);
            print(musicTracks[mus].length);
            yield return new WaitUntil(() => GetComponent<AudioSource>().isPlaying == false);
        }
    }*/
}