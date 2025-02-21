using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PauseHandler : MonoBehaviour
{
    public GameObject pauseScreen;
    public float cooldown = 1f;

    public UnityEvent onPaused;
    public UnityEvent onResume;

    private Guid _currentGuid;
    private bool _disableInputs;
    private bool _isPaused;

    private void Start()
    {
        pauseScreen.SetActive(false);
    }

    public void Toggle()
    {
        if (_disableInputs)
            return;
        
        if (_isPaused)
            DoResume();
        else 
            DoPause();

        StartCoroutine(Cooldown());
        
        IEnumerator Cooldown()
        {
            _disableInputs = true;
            yield return new WaitForSecondsRealtime(cooldown);
            _disableInputs = false;
        }
    }

    public void DoPause()
    {
        _currentGuid = Pause.Request();
        _isPaused = true;
        pauseScreen.SetActive(true);
        onPaused?.Invoke();
    }

    public void DoResume()
    {
        Pause.Return(_currentGuid);
        _isPaused = false;
        pauseScreen.SetActive(false);
        onResume?.Invoke();
    }
}
