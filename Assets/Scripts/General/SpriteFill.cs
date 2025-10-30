using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFill : MonoBehaviour
{
    public enum FillType { Horizontal, Vertical, Radial360 }

    [Header("Tipo de preenchimento")]
    public FillType tipo = FillType.Horizontal;

    [Range(0f, 1f)]
    [Tooltip("Quantidade de preenchimento (0 = vazio, 1 = cheio)")]
    public float fillAmount = 1f;

    [Header("Direção e Inversão")]
    public bool invertido = false;

    [Header("Configurações Radiais")]
    [Tooltip("Rotação inicial em graus (para modo radial)")]
    public float anguloInicial = 90f;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock mpb;
    private Vector2 originalSize;
    private Vector3 originalScale;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
        SalvarEstadoOriginal();
        AtualizarFill();
    }

    void OnValidate()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (mpb == null)
            mpb = new MaterialPropertyBlock();

        AtualizarFill();
    }

    void Start()
    {
        SalvarEstadoOriginal();
        AtualizarFill();
    }

    void Update()
    {
        AtualizarFill();
    }

    private void SalvarEstadoOriginal()
    {
        if (spriteRenderer.sprite != null)
        {
            originalSize = spriteRenderer.sprite.bounds.size;
            originalScale = transform.localScale;
        }
    }

    private void AtualizarFill()
    {
        if (spriteRenderer.sprite == null)
            return;

        float amount = Mathf.Clamp01(fillAmount);

        if (tipo == FillType.Horizontal || tipo == FillType.Vertical)
        {
            AtualizarEscala(amount);
            spriteRenderer.sharedMaterial = null; // garante que não usa o shader radial
        }
        else if (tipo == FillType.Radial360)
        {
            AtualizarRadial(amount);
        }
    }

    private void AtualizarEscala(float amount)
    {
        Vector3 newScale = originalScale;

        if (tipo == FillType.Horizontal)
        {
            newScale.x = originalScale.x * amount;
            float desloc = originalSize.x * (1 - amount) * 0.5f * originalScale.x;
            transform.localPosition = new Vector3(invertido ? desloc : -desloc, 0, 0);
        }
        else if (tipo == FillType.Vertical)
        {
            newScale.y = originalScale.y * amount;
            float desloc = originalSize.y * (1 - amount) * 0.5f * originalScale.y;
            transform.localPosition = new Vector3(0, invertido ? -desloc : desloc, 0);
        }

        transform.localScale = newScale;
    }

    private void AtualizarRadial(float amount)
    {
        spriteRenderer.GetPropertyBlock(mpb);

        mpb.SetFloat("_FillAmount", invertido ? 1f - amount : amount);
        mpb.SetFloat("_StartAngle", anguloInicial * Mathf.Deg2Rad);

        spriteRenderer.SetPropertyBlock(mpb);

        // Garante que o material com o shader radial está sendo usado
        if (spriteRenderer.sharedMaterial == null || spriteRenderer.sharedMaterial.shader.name != "Custom/SpriteRadialFill")
        {
            Shader shader = Shader.Find("Custom/SpriteRadialFill");
            if (shader != null)
                spriteRenderer.sharedMaterial = new Material(shader);
            else
                Debug.LogWarning("Shader 'Custom/SpriteRadialFill' não encontrado! Importe o shader abaixo.");
        }
    }

    public Color Color { get => spriteRenderer.color; set => spriteRenderer.color = value; }
    public Sprite Sprite { get => spriteRenderer.sprite; set => spriteRenderer.sprite = value; }
}
