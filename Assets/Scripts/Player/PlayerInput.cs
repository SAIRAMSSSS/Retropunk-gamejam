using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    bool _click;
    bool _doubleClick;
    bool _interact;
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

    public bool DoubleClick
    {
        get
        {
            if (_doubleClick)
            {
                _doubleClick = false;
                return !_inputLocked;
            }
            return false;
        }
    }

    public bool Interact
    {
        get
        {
            if (_interact)
            {
                _interact = false;
                return !_inputLocked;
            }
            return false;
        }
    }
    public Vector2 MousePosition => _inputLocked ? Vector2.zero : _mousePosition;

    bool _inputLocked = false;
    bool _enableInteraction;

    public void LockInput(bool enable)
    {
        _inputLocked = enable;
        _click = false;
        _doubleClick = false;
        _interact = false;
    }
    /// <summary>
    /// Enables or disables an interaction with an object and sets an interaction functions
    /// </summary>
    /// <param name="enable"></param>
    public void EnableInteraction(bool enable)
    {
        _enableInteraction = enable;
    }

    public void OnClick(InputValue inputValue)
    {
        _click = inputValue.isPressed;
    }

    public void OnDoubleClick(InputValue inputValue)
    {
        _doubleClick = true;
    }

    public void OnMousePosition(InputValue inputValue)
    {
        _mousePosition = inputValue.Get<Vector2>();
    }

    public void OnInteract(InputValue inputValue)
    {
        if (_enableInteraction)
        {
            _interact = inputValue.isPressed;
        }
    }
}
