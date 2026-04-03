using TMPro;
using UnityEngine;

public class InteractionUIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _interactiondText;

    Canvas _interactionCanvas;

    bool _show;
    Transform _obj;

    float _offset;

    void Start()
    {
        _interactionCanvas = GetComponent<Canvas>();
        _interactionCanvas.worldCamera = Camera.main;
    }

    void Update()
    {
        if (_show)
        {
            transform.position = _obj.position + _offset*Vector3.up;
            Vector3 direction = Camera.main.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
            transform.Rotate(0, 180, 0);
        }
    }
    /// <summary>
    /// Shows the given text
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="text"></param>
    public void Show(Transform obj, string text, float offset)
    {
        _show = true;
        Color color = _interactiondText.color;
        color.a = 1;
        _interactiondText.color = color;
        _obj = obj;
        _interactiondText.text = text;
        _offset = offset;
    }
    /// <summary>
    /// Hides the text
    /// </summary>
    public void Hide()
    {
        _show = false;
        Color color = _interactiondText.color;
        color.a = 0;
        _interactiondText.color = color; _obj = null;
    }
}
