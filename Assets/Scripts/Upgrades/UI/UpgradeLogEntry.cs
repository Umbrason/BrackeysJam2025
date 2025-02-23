using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeLogEntry : MonoBehaviour
{
    public int Count { set { text.text = value.ToString(); } }
    public IUpgrade Upgrade { set { image.sprite = value.Icon; } }

    [SerializeField] Image image;
    [SerializeField] TMP_Text text;
}