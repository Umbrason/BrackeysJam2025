using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public UnityEvent OnBeforeLoading;
    public UnityEvent<float> OnLoading;

    private Coroutine loadingCoroutine = null;
    public void LoadScene(string sceneName)
    {
        if(loadingCoroutine == null)
        {
            loadingCoroutine = StartCoroutine(LoadSceneAysnc(sceneName));
        }
    }

    private IEnumerator LoadSceneAysnc(string sceneName)
    {
        var task = SceneManager.LoadSceneAsync(sceneName);

        OnBeforeLoading?.Invoke();

        while(!task.isDone)
        {
            OnLoading?.Invoke(task.progress);
            yield return null;
        }

        loadingCoroutine = null;
    }
}
