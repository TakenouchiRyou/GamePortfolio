using UnityEngine;
using UnityEngine.EventSystems;

public class Plug : MonoBehaviour, IPointerClickHandler
{
    public string colorId;
    public bool isLeft;
    [HideInInspector] public bool isConnected = false;

    private WiringManager manager;
    private Vector3 defaultScale;

    void Start()
    {
        manager = FindObjectOfType<WiringManager>();
        defaultScale = transform.localScale;
        GetComponent<SpriteRenderer>().color = manager.GetColorFromId(colorId);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isConnected) return;
        manager.OnPlugClicked(this);
    }

    public void SetSelected(bool on)
    {
        transform.localScale = on ? defaultScale * 1.25f : defaultScale;
    }
}