using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    public AudioClipGroup clipGroup;
    public AudioSource audioSource;
    public Button[] buttons;

    private void OnEnable()
    {
        foreach (Button button in buttons) 
            button.onClick.AddListener(PlayButtonSound);
    }

    private void PlayButtonSound()
    {
        if (clipGroup.TryGetRandom(out var clip))
            audioSource.PlayOneShot(clip);
    }

    [EasyButtons.Button]
    public void GetAllButtons()
    {
        buttons = Resources.FindObjectsOfTypeAll<Button>();
    }

}