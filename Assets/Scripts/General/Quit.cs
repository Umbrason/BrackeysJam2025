using EasyButtons;
using UnityEngine;

public class Quit : MonoBehaviour
{
    [Button]
    public void ApplicationQuit() => Application.Quit();
}
