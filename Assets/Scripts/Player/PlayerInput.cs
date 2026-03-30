using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    bool _click;
    Vector2 _mousePosition;

    public bool Click
    {
        get
        {
            if (_click)
            {
                _click = false;
                return !_inputLocked;
            }
            return false;
        }
    }
    public Vector2 MousePosition => _inputLocked ? Vector2.zero : _mousePosition;

    bool _inputLocked = false;

    public void LockInput(bool enable)
    {
        _inputLocked = enable;
    }

    public void OnClick(InputValue inputValue)
    {
        _click = inputValue.isPressed;
    }

    public void OnMousePosition(InputValue inputValue)
    {
        _mousePosition = inputValue.Get<Vector2>();
    }

}
