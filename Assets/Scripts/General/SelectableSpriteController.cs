using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableSpriteController : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Sprite clicked: " + gameObject.name);
    }
}
