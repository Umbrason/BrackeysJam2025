using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private Button skipButton;
    [SerializeField] private string sceneToLoad;

    private Coroutine videoCoroutine;

    private void PlayVideoAndLoadScene()
    {
        videoCoroutine ??= StartCoroutine(PlayVideoAndLoadSceneCoroutine());
    }

    private void SkipVideoAndLoadScene()
    {
        skipButton.gameObject.SetActive(false);
        videoPlayer.Stop();
        sceneLoader.LoadScene(sceneToLoad);
        videoCoroutine = null;
    }

    private IEnumerator PlayVideoAndLoadSceneCoroutine()
    {
        videoPlayer.Stop();
        videoPlayer.Play();
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        skipButton.gameObject.SetActive(true);
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        skipButton.gameObject.SetActive(false);
        sceneLoader.LoadScene(sceneToLoad);
        videoCoroutine = null;
    }

    void Start()
    {
        PlayVideoAndLoadScene();
        skipButton.onClick.AddListener(SkipVideoAndLoadScene);
    }

    private void OnDestroy()
    {
        skipButton.onClick.RemoveListener(SkipVideoAndLoadScene);
    }
}
