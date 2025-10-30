using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpriteDragHandler : MonoBehaviour
{
    [Header("Limites de Movimento")]
    public bool usarLimites = false;
    public Vector2 limiteMin = new Vector2(-10, -5);
    public Vector2 limiteMax = new Vector2(10, 5);

    [Header("Controle por Scroll do Mouse")]
    public bool usarScroll = false;
    public enum EixoScroll { Nenhum, Horizontal, Vertical }
    public EixoScroll eixoScroll = EixoScroll.Vertical;
    [Tooltip("Força de movimento ao usar o scroll do mouse.")]
    public float forcaScroll = 1f;

    private bool arrastando = false;
    private Vector3 offset;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;

        if (GetComponent<Collider2D>() == null)
            Debug.LogWarning("SpriteDragHandler requer um Collider2D para detectar o clique.");
    }

    void Update()
    {
        // Movimento por scroll
        if (usarScroll)
        {
            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > 0.001f)
            {
                Vector3 pos = transform.position;

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

                transform.position = pos;
            }
        }

        // Arrastar com mouse
        if (arrastando)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            Vector3 novaPos = mousePos + offset;

            if (usarLimites)
                novaPos = AplicarLimites(novaPos);

            transform.position = novaPos;
        }
    }

    void OnMouseDown()
    {
        arrastando = true;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        offset = transform.position - mousePos;
    }

    void OnMouseUp()
    {
        arrastando = false;
    }

    private Vector3 AplicarLimites(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, limiteMin.x, limiteMax.x);
        pos.y = Mathf.Clamp(pos.y, limiteMin.y, limiteMax.y);
        return pos;
    }

    private void OnDrawGizmosSelected()
    {
        if (usarLimites)
        {
            Gizmos.color = Color.green;
            Vector3 centro = new Vector3(
                (limiteMin.x + limiteMax.x) / 2,
                (limiteMin.y + limiteMax.y) / 2,
                0
            );
            Vector3 tamanho = new Vector3(
                Mathf.Abs(limiteMax.x - limiteMin.x),
                Mathf.Abs(limiteMax.y - limiteMin.y),
                0
            );
            Gizmos.DrawWireCube(centro, tamanho);
        }
    }
}
