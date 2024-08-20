using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] AudioSource standardVer, caveVer, conveyorVer;
    [SerializeField] AudioClip[] musicTracks;
    [SerializeField] AudioClip winSound;
    bool playing = false;

    private void Awake()
    {
        OnLevelWasLoaded(0);
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

    private void OnLevelWasLoaded(int level)
    {
        if (level <= 4)
        {
            standardVer.gameObject.SetActive(true);
            caveVer.gameObject.SetActive(false);
            conveyorVer.gameObject.SetActive(false);
        }
        else if (level > 4 && level <= 7)
        {
            standardVer.gameObject.SetActive(false);
            caveVer.gameObject.SetActive(true);
            conveyorVer.gameObject.SetActive(false);
        }
        else
        {
            standardVer.gameObject.SetActive(false);
            caveVer.gameObject.SetActive(false);
            conveyorVer.gameObject.SetActive(true);
        }
    }

    public void PlayWinSound()
    {
        GetComponent<AudioSource>().PlayOneShot(winSound);
    }

    public void DisableAllMusic()
    {
        standardVer.gameObject.SetActive(false);
        caveVer.gameObject.SetActive(false);
        conveyorVer.gameObject.SetActive(false);
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