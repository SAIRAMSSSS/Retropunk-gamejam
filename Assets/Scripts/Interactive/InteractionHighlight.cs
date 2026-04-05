using UnityEngine;
using Zenject;

public class InteractionHighlight : MonoBehaviour
{
    [Inject]
    PlayerInput _input;

    GameObject _currentObj;
    Outline _outline;

    void Start()
    {
        //_input = GameObject.Find("Player").GetComponent<PlayerInput>();
    }

    private void Update()
    {
        //detects whether the cursor has touched an interactive object
        Ray ray = Camera.main.ScreenPointToRay(_input.MousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("InteractiveObject")))
        {
            GameObject hitObj = hit.collider.gameObject;
            InteractionObject interactive = hitObj.GetComponent<InteractionObject>();

            //if the cursor touched a new object - outlines it and is interactive now
            if (_currentObj != hitObj && interactive.CanInteract() && hitObj.TryGetComponent(out _outline))
            {
                //clears the previous highlight
                ClearHighlight();
                _currentObj = hitObj;
                _outline.enabled = true;
            }
        }
        else
        {
            //if there's no interactive object under the cursor - clears the previous highlight
            ClearHighlight();
        }
    }

    void ClearHighlight()
    {
        if (_currentObj != null)
        {
            _outline.enabled = false;
            _outline = null;
            _currentObj = null;
        }
    }
}
