using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoImage;
    [Min(0.1f)]
    [SerializeField] private float fadeSpeed = 2.0f;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private Button skipButton;
    [SerializeField] private string sceneToLoad;

    private Coroutine videoCoroutine;

    private void PlayVideoAndLoadScene()
    {
        if(videoCoroutine == null)
        {
            videoCoroutine = StartCoroutine(PlayVideoAndLoadSceneCoroutine());
        }
    }

    private void SkipVideoAndLoadScene()
    {
        skipButton.gameObject.SetActive(false);
        videoPlayer.Stop();
        videoImage.gameObject.SetActive(false);
        sceneLoader.LoadScene(sceneToLoad);
    }

    private IEnumerator PlayVideoAndLoadSceneCoroutine()
    {
        videoPlayer.Stop();
        videoPlayer.Play();

        yield return new WaitForSeconds(0.5f);

        while(videoPlayer.isPlaying)
        {
            yield return null;
        }

        skipButton.gameObject.SetActive(false);
        float delta = 0.0f;
        Color modifiedColor = videoImage.color;
        while(delta < 1.0f)
        {
            videoImage.color = modifiedColor;
            modifiedColor.a = Mathf.Lerp(1.0f, 0.0f, delta);
            delta += fadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }

        modifiedColor.a = 0.0f;
        videoImage.color = modifiedColor;
        videoImage.gameObject.SetActive(false);

        sceneLoader.LoadScene(sceneToLoad);
    }

    // Start is called before the first frame update
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
