using TMPro;
using UnityEngine;

public class InteractionUIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _interactionCommandText;

    Canvas _interactionCanvas;

    float _offsetAboveObject = 1.5f;
    bool _show;

    Transform _obj;

    void Start()
    {
        _interactionCanvas = GetComponent<Canvas>();
        _interactionCanvas.worldCamera = Camera.main;
    }

    void Update()
    {
        if (_show)
        {
            transform.position = _obj.position + _offsetAboveObject * Vector3.up;
            _interactionCanvas.transform.LookAt(Camera.main.transform);
            _interactionCanvas.transform.Rotate(0, 180, 0);
        }
    }
    /// <summary>
    /// Shows the given text
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="text"></param>
    public void Show(Transform obj, string text)
    {
        _show = true;
        _interactionCommandText.gameObject.SetActive(_show);
        _obj = obj;
        _interactionCommandText.text = text;
    }
    /// <summary>
    /// Hides the text
    /// </summary>
    public void Hide()
    {
        _show = false;
        _interactionCommandText.gameObject.SetActive(_show);
        _obj = null;
    }
}
