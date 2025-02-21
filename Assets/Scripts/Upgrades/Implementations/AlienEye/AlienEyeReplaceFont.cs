using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlienEyeReplaceFont : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset replacementFont;
    private TMP_FontAsset originalFont;
    Cached<TMP_Text> cached_Text;
    TMP_Text Text => cached_Text[this];
    void OnEnable()
    {
        originalFont = Text.font;
        AlienEye.ActiveChanged += UpdateReplaceFont;
        UpdateReplaceFont(AlienEye.IsActive);
    }

    void OnDisable()
    {
        AlienEye.ActiveChanged -= UpdateReplaceFont;
    }

    void UpdateReplaceFont(bool doReplace)
    {
        Text.font = doReplace ? replacementFont : originalFont;
    }
}
