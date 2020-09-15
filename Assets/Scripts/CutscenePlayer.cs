using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutscenePlayer : MonoBehaviour
{
    public OpenScene sceneOpener;
    public VideoPlayer videoPlayer;
    private WaitUntil canPlayVid;
    public Animator anim;
    public int index;
    private WaitForSeconds pause;
    // Start is called before the first frame update
    void Start()
    {
        canPlayVid = new WaitUntil(() => sceneOpener.sceneOpening);
        StartCoroutine("PlayVideo");
        pause = new WaitForSeconds(1.5f);
    }

    IEnumerator PlayVideo()
    {
        yield return canPlayVid;
        videoPlayer.Play();
        yield return pause;
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        anim.SetTrigger("EndScene");
        yield return pause;
        SceneManager.LoadScene(index);
    }
}
