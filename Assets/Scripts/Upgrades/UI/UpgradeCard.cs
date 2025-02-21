using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(NonDrawingGraphic))]
public class UpgradeCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    IUpgrade m_displayedUpgrade;
    public IUpgrade DisplayedUpgrade
    {
        get => m_displayedUpgrade;
        set
        {
            m_displayedUpgrade = value;
            icon.sprite = m_displayedUpgrade.Icon;
            title.text = m_displayedUpgrade.Name;
            description.text = m_displayedUpgrade.Description;
        }
    }
    public event Action<UpgradeCard> OnClicked;
    public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke(this);

    void OnEnable()
    {
        scaleSpring.Position = 0;
        transform.GetChild(0).gameObject.SetActive(true);
    }
    void OnDisable() => transform.GetChild(0).gameObject.SetActive(false);

    #region Cosmetics
    [Header("Visual Settings")]
    [SerializeField] Spring.Config scaleSpringConfig = new(20, .6f);
    BaseSpring scaleSpring;
    [SerializeField] Spring.Config rotationSpringConfig = new(20, .6f);
    RotationVector3Spring rotationSpring;

    [SerializeField] Vector2 RotationAmplitude;

    Cached<RectTransform> cached_rectTransform;
    RectTransform rectTransform => cached_rectTransform[this];

    Cached<Canvas> cached_Canvas = new(Cached<Canvas>.GetOption.Parent);
    Canvas canvas => cached_Canvas[this];

    Cached<RectTransform> cached_canvasRectTransform;
    RectTransform canvasRectTransform => cached_canvasRectTransform[canvas.rootCanvas];

    void Awake()
    {
        rotationSpring = new(rotationSpringConfig)
        {
            Position = new(0, 90),
            RestingPos = new(0, 0)
        };
        rotationSpring.OnSpringUpdated += OnRotationUpdated;

        scaleSpring = new(scaleSpringConfig)
        {
            RestingPos = 1f,
            Position = 1f
        };
        scaleSpring.OnSpringUpdated += OnScaleUpdated;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Shake()
    {
        rotationSpring.Velocity = new(0, 0, 200);
    }

    public bool Disabled { get; set; } = true;
    public void Hide()
    {
        rotationSpring.RestingPos = new(0, 90);
        scaleSpring.AngularFrequency = 15;
        scaleSpring.DampingRatio = .9f;
        scaleSpring.RestingPos = 0;
        Disabled = true;
    }

    private void OnScaleUpdated(float scale)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).localScale = Vector3.one * scale;
    }

    private void OnRotationUpdated(Vector3 rotation)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    const float HoveredScale = 1.2f;
    const float PressedScale = .9f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Disabled) return;
        scaleSpring.RestingPos = HoveredScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Disabled) return;
        scaleSpring.RestingPos = 1;
        rotationSpring.RestingPos = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Disabled) return;
        scaleSpring.RestingPos = PressedScale;
        scaleSpring.Velocity = (scaleSpring.RestingPos - scaleSpring.Position) * 20f; //speed up downscale motion a bit
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Disabled) return;
        if (scaleSpring.RestingPos < 1) //only set back to hovered scale when the pointer is still on the card
            scaleSpring.RestingPos = HoveredScale;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (Disabled) return;
        var localPos = ScreenToRectLocalPosition(eventData.position);
        rotationSpring.RestingPos = (Vector2.one * .5f - new Vector2(1 - localPos.y, localPos.x)) * RotationAmplitude;
    }

    private Vector3[] _worldCorners = new Vector3[4];
    private Vector2 ScreenToRectLocalPosition(Vector2 screenPosition)
    {
        var normalizedEventPosition = screenPosition / canvas.pixelRect.size;

        rectTransform.GetWorldCorners(_worldCorners);
        var rectMinWS = (Vector2)canvas.transform.InverseTransformDirection(_worldCorners[0]);
        var rectMaxWS = (Vector2)canvas.transform.InverseTransformDirection(_worldCorners[2]);

        canvasRectTransform.GetWorldCorners(_worldCorners);
        var canvasMinWS = (Vector2)canvas.transform.InverseTransformDirection(_worldCorners[0]);
        var canvasMaxWS = (Vector2)canvas.transform.InverseTransformDirection(_worldCorners[2]);

        var canvasWSScale = canvasMaxWS - canvasMinWS;
        var selfWSScale = rectMaxWS - rectMinWS;
        var rectMinNormalized = (rectMinWS - canvasMinWS) / canvasWSScale;

        var normalizedLocalPosition = (normalizedEventPosition - rectMinNormalized) / selfWSScale * canvasWSScale;
        return normalizedLocalPosition;
    }

    void Update()
    {
        scaleSpring.Step(Time.unscaledDeltaTime);
        rotationSpring.Step(Time.unscaledDeltaTime);
    }
    #endregion
}