using UnityEngine;
using UnityEngine.UI;

public class BarsUI : MonoBehaviour
{
    private Image loadingImage;

    public void SetAmount(float amount)
    {
        loadingImage.fillAmount = Mathf.Clamp01(amount);
    }
    
    private void Awake()
    {
        loadingImage = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        loadingImage.type = Image.Type.Filled;
        loadingImage.fillMethod = Image.FillMethod.Horizontal;
        loadingImage.fillOrigin = (int)Image.OriginHorizontal.Left;
    }
}
