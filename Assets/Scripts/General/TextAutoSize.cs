using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class TextAutoSize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private Vector2 padding = new Vector2(10f, 10f);
    [SerializeField] private bool adjustWidth = true;
    [SerializeField] private bool adjustHeight = true;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (targetText == null)
            targetText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateSize();
    }

    private void OnValidate()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        UpdateSize();
    }

    private void UpdateSize()
    {
        if (targetText == null) return;

        // Força o TMP a atualizar seu layout antes de pegar o tamanho preferido
        targetText.ForceMeshUpdate();

        Vector2 preferredSize = targetText.GetPreferredValues();

        Vector2 newSize = rectTransform.sizeDelta;

        if (adjustWidth)
            newSize.x = preferredSize.x + padding.x;

        if (adjustHeight)
            newSize.y = preferredSize.y + padding.y;

        rectTransform.sizeDelta = newSize;
    }
}
