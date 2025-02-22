using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowHealthVisual : MonoBehaviour
{
    [SerializeField] private Image lowHealthImage;
    
    [Range(0.0f, 1.0f)]
    [SerializeField] private float minAlpha = 0.1f;
    
    [Range(0.0f, 1.0f)]
    [SerializeField] private float maxAlpha = 1.0f;

    [Min(1.0f)]
    [SerializeField] private float transitionSpeed = 1.0f;
    [SerializeField] private bool shouldShowEffect = false;
    
    private bool shouldUnfade = false;
    private float currentAlpha = 0.0f;
    public bool ShouldShowEffect { get => shouldShowEffect; set => shouldShowEffect = value; }
    private void Start()
    {
        shouldUnfade = true;
    }

    private void Update()
    {
        if(!shouldShowEffect)
        {
            lowHealthImage.enabled = false;
            return;
        }

        lowHealthImage.enabled = true;

        if (shouldUnfade)
        {
            currentAlpha += transitionSpeed * Time.deltaTime;
            if(currentAlpha >= 1.0f)
            {
                currentAlpha = 1.0f;
                shouldUnfade = false;
            }
        }
        else
        {
            currentAlpha -= transitionSpeed * Time.deltaTime;
            if (currentAlpha <= 0.0f)
            {
                currentAlpha = 0.0f;
                shouldUnfade = true;
            }
        }

        Color modifiedColor = lowHealthImage.color;
        modifiedColor.a = currentAlpha;

        lowHealthImage.color = modifiedColor;
    }

}
