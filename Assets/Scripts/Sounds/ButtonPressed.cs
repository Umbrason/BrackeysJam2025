using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    public bool toggleThisGetAllButtons; // please im too lazy to add buttons
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

    public void GetAllButtons()
    {
        buttons = Resources.FindObjectsOfTypeAll<Button>();
    }

    private void OnValidate()
    {
        if (toggleThisGetAllButtons)
        {
            GetAllButtons();
            toggleThisGetAllButtons = false;
        }
    }
}