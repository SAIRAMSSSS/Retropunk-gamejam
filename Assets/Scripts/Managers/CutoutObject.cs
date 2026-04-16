using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    LayerMask _cutoutMask;
    [Inject]
    PlayerController _player;

    MaterialPropertyBlock _materialBlock;
    HashSet<Renderer> _activeDetails = new();

    RaycastHit[] _hitBuffer = new RaycastHit[180];
    HashSet<Renderer> _currentHits = new();
    List<Renderer> _removeDetails = new();

    readonly float _cutoutSize = 0.15f;
    readonly float _falloffSize = 0.1f;
    readonly float _raycastRadius = 2.2f;

    Vector3 _lastPlayerPosition;
    Vector3 _lastCutoutPosition;
    float _screenRatio;

    private void Start()
    {
        _materialBlock = new MaterialPropertyBlock();
        _screenRatio = (Screen.width / Screen.height);
    }

    private void Update()
    {
        if (_player.transform.position != _lastPlayerPosition)
        {
            Vector2 cutoutPos = Camera.main.WorldToViewportPoint(_player.transform.position);
            cutoutPos.y /= _screenRatio;
            _lastPlayerPosition = _player.transform.position;
            _lastCutoutPosition = cutoutPos;
        }
        else
        {
            return;
        }

        Vector3 direction = _player.transform.position + _player.transform.up - transform.position;

        int hitCount = Physics.SphereCastNonAlloc(transform.position, _raycastRadius, direction.normalized, _hitBuffer, direction.magnitude, _cutoutMask);
        Debug.Log($"Hit count: {hitCount}");
        _currentHits.Clear();
        for (int i = 0; i < hitCount; i++)
        {
            if (_hitBuffer[i].distance < direction.magnitude - 0.5f)
            {
                _currentHits.Add(_hitBuffer[i].transform.GetComponent<Renderer>());
            }
        }
        //removes details that are not covering now
        _removeDetails.Clear();
        foreach (var detail in _activeDetails)
        {
            if (!_currentHits.Contains(detail))
            {
                _removeDetails.Add(detail);
            }
        }

        foreach (var detail in _removeDetails)
        {
            ResetCutout(detail);
            _activeDetails.Remove(detail);
        }
        foreach (var detail in _currentHits)
        {
            if (!_activeDetails.Contains(detail))
            {
                _activeDetails.Add(detail);
            }

            _materialBlock.SetVector("_cutoutPosition", _lastCutoutPosition);
            _materialBlock.SetFloat("_cutoutSize", _cutoutSize);
            _materialBlock.SetFloat("_falloffSize", _falloffSize);
            detail.SetPropertyBlock(_materialBlock);

        }

        Debug.Log($"  Active cuts: {_activeDetails.Count}");
    }

    void ResetCutout(Renderer wallDetail)
    {
        _materialBlock.SetFloat("_cutoutSize", 0f);
        _materialBlock.SetFloat("_falloffSize", 0f);
        wallDetail.SetPropertyBlock(_materialBlock);
    }
}
