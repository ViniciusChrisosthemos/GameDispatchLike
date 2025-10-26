using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Limites de Movimento (opcional)")]
    public bool usarLimites = false;
    public Rect limites = new Rect(0, 0, Screen.width, Screen.height);

    [Header("Controle por Scroll do Mouse")]
    public bool usarScroll = false;
    public enum EixoScroll { Nenhum, Horizontal, Vertical }
    public EixoScroll eixoScroll = EixoScroll.Vertical;
    [Tooltip("Força de movimento ao usar o scroll do mouse.")]
    public float forcaScroll = 10f;

    private RectTransform rectTransform;
    private Canvas canvas;

    // Posição clicada dentro do elemento
    private Vector2 clickOffset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
            Debug.LogError("UIDragHandler precisa estar dentro de um Canvas!");
    }

    void Update()
    {
        if (!usarScroll) return;

        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.001f)
        {
            Vector2 pos = rectTransform.anchoredPosition;

            switch (eixoScroll)
            {
                case EixoScroll.Horizontal:
                    pos.x += scroll * forcaScroll;
                    break;
                case EixoScroll.Vertical:
                    pos.y += scroll * forcaScroll;
                    break;
            }

            if (usarLimites)
                pos = AplicarLimites(pos);

            rectTransform.anchoredPosition = pos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out clickOffset
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint))
        {
            Vector2 newPos = localPoint - clickOffset;

            if (usarLimites)
                newPos = AplicarLimites(newPos);

            rectTransform.anchoredPosition = newPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Pode adicionar efeitos visuais aqui ao soltar
    }

    private Vector2 AplicarLimites(Vector2 pos)
    {
        Vector2 size = rectTransform.sizeDelta * 0.5f;

        float minX = limites.xMin + size.x;
        float maxX = limites.xMax - size.x;
        float minY = limites.yMin + size.y;
        float maxY = limites.yMax - size.y;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        return pos;
    }

    private void OnDrawGizmosSelected()
    {
        if (usarLimites)
        {
            Gizmos.color = Color.green;
            Vector3 pos = new Vector3(limites.center.x, limites.center.y, 0);
            Vector3 size = new Vector3(limites.width, limites.height, 0);
            Gizmos.DrawWireCube(pos, size);
        }
    }
}
