using UnityEngine;
using UnityEngine.InputSystem;

public class ClawInput : MonoBehaviour
{
    bool _grab;
    bool _rotateClockwise;
    bool _rotateConuterclockwise;
    bool _exitGame;
    Vector2 _move;

    public bool Grab
    {
        get
        {
            if (_grab)
            {
                _grab = false;
                return !_inputLocked;
            }
            return false;
        }
    }

    public bool RotateClockwise
    {
        get
        {
            if (_rotateClockwise)
            {
                _rotateClockwise = false;
                return !_inputLocked;
            }
            return false;
        }
    }

    public bool RotateCounterclockwise
    {
        get
        {
            if (_rotateConuterclockwise)
            {
                _rotateConuterclockwise = false;
                return !_inputLocked;
            }
            return false;
        }
    }

    public bool ExitGame
    {
        get
        {
            if (_exitGame)
            {
                _exitGame = false;
                return !_inputLocked;
            }
            return false;
        }
    }

    public Vector2 Move => _inputLocked ? Vector2.zero : _move;

    bool _inputLocked = true;

    public void LockInput(bool enable)
    {
        _inputLocked = enable;
    }

    public void OnGrab(InputValue inputValue)
    {
        _grab = inputValue.isPressed;
    }

    public void OnRotateClockwise(InputValue inputValue)
    {
        _rotateClockwise = true;
    }

    public void OnRotateCounterclockwise(InputValue inputValue)
    {
        _rotateConuterclockwise = inputValue.isPressed;
    }

    public void OnMove(InputValue inputValue)
    {
        _move = inputValue.Get<Vector2>();
    }

    public void OnExitGame(InputValue inputValue)
    {
        _exitGame = inputValue.isPressed;
    }
}
