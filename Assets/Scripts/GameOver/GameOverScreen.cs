using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    Guid guid;
    void OnEnable()
    {
        guid = Pause.Request();
    }

    void OnDisable()
    {
        Pause.Return(guid);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
