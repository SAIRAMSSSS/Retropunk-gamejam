using DG.Tweening;
using System;
using Unity.Cinemachine;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField]
    CinemachineCamera _clawCamera;
    [SerializeField]
    Transform _holdPoint;
    [SerializeField]
    ConsoleObject _console;
    [Header("Constraints")]
    [SerializeField]
    Vector2 _upperCorner;
    [SerializeField]
    Vector2 _lowerCorner;

    ClawInput _input;
    Animator _clawAnimator;
    Animator _stretchAnimator;
    SFXController _SFXPlayer;

    readonly int _hashGrab = Animator.StringToHash("Grab");
    readonly int _hashRelease = Animator.StringToHash("Release");

    readonly float _grabDistance = 8f;
    readonly float _placeY = 10f;
    readonly float _moveSpeed = 5f;
    readonly float _verticalMoveTime = 1.5f;

    bool _moving = false;
    bool _goingDown;
    bool _grabbing;
    bool _isHolding;
    float _verticalPosition;

    PipeDetail _chosenPipe;
    PipeDetail _grabedPipe;

    void Start()
    {
        _input = GetComponent<ClawInput>();
        _SFXPlayer = GetComponent<SFXController>();
        _clawAnimator = transform.GetChild(0).GetComponent<Animator>();
        _stretchAnimator = transform.GetChild(1).GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        HighlightPipe();

        if (_input.Grab && !_moving && (!_isHolding && _chosenPipe != null || _isHolding && _chosenPipe == null))
        {
            _goingDown = true;
            _grabbing = true;
            _SFXPlayer.PlaySound("Reaching");
            _verticalPosition = transform.position.y - _grabDistance;
            //lowe

        }

        if (!_grabbing && !_moving && !_isHolding && _chosenPipe.CanMove() && _input.RotateClockwise)
        {
            _chosenPipe.Rotate(1);
            _SFXPlayer.PlaySound("Rotate");
        }

        if (!_grabbing && !_moving && !_isHolding && _chosenPipe.CanMove() && _input.RotateCounterclockwise)
        {
            _chosenPipe.Rotate(-1);
            _SFXPlayer.PlaySound("Rotate");
        }
    }

    public void VerticalMovement()
    {
        transform.DOMoveY(_verticalPosition, _verticalMoveTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _SFXPlayer.StopSound();
                if (_goingDown)
                {
                    _goingDown = false;
                    _verticalPosition = transform.position.y + _grabDistance;
                    if (_isHolding)
                    {
                        //_animator.SetTrigger(_hashRelease);
                    }
                    else
                    {
                        //_animator.SetTrigger(_hashGrab);
                    }
                }
                else
                {
                    _grabbing = false;
                    if (!_isHolding && _grabedPipe.TryConnectAllPipes())
                    {
                        _console.CompletePuzzle(LevelNames.WaterSupplyRoom);
                        ExitGame();
                    }
                }
            });
    }
    /// <summary>
    /// Moves the claw
    /// </summary>
    void Move()
    {
        float moveToX = Mathf.Clamp(transform.position.x + _input.Move.x, _lowerCorner.x, _upperCorner.x);
        float moveToZ = Mathf.Clamp(transform.position.y + _input.Move.y, _lowerCorner.y, _upperCorner.y);
        transform.position += new Vector3(moveToX, transform.position.y, moveToZ) * Time.deltaTime;
        bool move = !Mathf.Approximately(transform.position.z, moveToZ);
        if (move && !_moving)
        {
            _SFXPlayer.PlaySound("Movement");
        }
        _moving = move;
    }
    /// <summary>
    /// Highlights a pipe under the claw
    /// </summary>
    void HighlightPipe()
    {
        if (Physics.Raycast(transform.position - transform.up * 2f, -transform.up, out RaycastHit hit, 100f, LayerMask.GetMask("InteractiveObject")))
        {
            PipeDetail newPipe = hit.collider.gameObject.GetComponent<PipeDetail>();
            if (_chosenPipe != newPipe)
            {
                _chosenPipe.GetComponent<Outline>().enabled = false;
                _chosenPipe = newPipe;
                _chosenPipe.GetComponent<Outline>().enabled = true;
            }
        }
    }
    /// <summary>
    /// Shifts camera to the claw game view
    /// </summary>
    public void StartClawGame()
    {
        _input.LockInput(false);
        _clawCamera.Priority = 10;
    }
    /// <summary>
    /// Shifts camera to the player
    /// </summary>
    public void ExitGame()
    {
        _input.LockInput(true);
        _clawCamera.Priority = 0;
    }
    /// <summary>
    /// Connects a pipe to the claw
    /// </summary>
    public void GrabPipe()
    {
        _grabedPipe = _chosenPipe;
        _isHolding = true;
        _grabedPipe.transform.SetParent(_holdPoint);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        _verticalPosition = transform.position.y + _grabDistance;
        _SFXPlayer.PlaySound("Reaching");
    }
    /// <summary>
    /// Dissconnects a pipe from the claw
    /// </summary>
    public void ReleasePipe()
    {
        _isHolding = true;
        _grabedPipe.transform.SetParent(null);
        Vector3 pos = transform.position;
        pos.y = _placeY;
        _grabedPipe.transform.position = pos;
        _verticalPosition = transform.position.y + _grabDistance;
        _SFXPlayer.PlaySound("Reaching");
    }
}
