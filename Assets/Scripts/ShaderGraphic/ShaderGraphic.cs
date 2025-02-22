using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasRenderer))]
public class ShaderGraphic : MaskableGraphic
{
    [SerializeField] private Shader shader;
    [SerializeField] private new Texture2D mainTexture;

    protected override void UpdateGeometry()
    {
        base.UpdateGeometry();
        UpdateShaderProperties();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        UpdateShaderProperties();
    }

    private void UpdateShaderProperties()
    {
        if (m_Material == null || m_Material.shader != shader)
        {
            m_Material = new Material(shader) { };
            m_Material.SetTexture("_Texture", mainTexture);
            canvasRenderer.materialCount = 1;
            canvasRenderer.SetMaterial(m_Material, 0);
        }

        // Calculate pixel size of the rectangle
        if (canvas)
        {
            var canvasSize = canvas.GetComponent<RectTransform>().rect.size;
            Vector2 rectSize = rectTransform.rect.size / canvasSize * canvas.pixelRect.size;
            m_Material.SetVector("_PixelRect", new Vector4(rectSize.x, rectSize.y, 1f / rectSize.x, 1f / rectSize.y));
        }
        var random = (float)new System.Random(this.GetInstanceID()).NextDouble();
        m_Material.SetFloat("_Random", random);
    }

    void Update()
    {
        var t = Time.unscaledTime;
        Shader.SetGlobalVector("_uTime", new(t / 20, t, t * 2, t * 3));
    }
}