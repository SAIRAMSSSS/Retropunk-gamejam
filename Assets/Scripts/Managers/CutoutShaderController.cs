using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CutoutShaderController : MonoBehaviour
{
    [SerializeField]
    Transform _target;
    [SerializeField]
    float _cutoutSize = 0.15f;
    [SerializeField]
    float _falloffSize = 0.1f;
    [SerializeField]
    float _raycastRadius = 2.2f;
    [SerializeField]
    float _raycastDistOffset = 0.0f;
    [SerializeField]
    LayerMask _cutoutMask;

    MaterialPropertyBlock _materialBlock;
    HashSet<Renderer> _activeDetails = new();

    RaycastHit[] _hitBuffer = new RaycastHit[180];
    HashSet<Renderer> _currentHits = new();
    List<Renderer> _removeDetails = new();

    Vector3 _lastPlayerPosition;
    Vector3 _lastCameraPosition;
    Vector3 _lastCutoutPosition;
    float _screenRatio;

    private void Start()
    {
        _materialBlock = new MaterialPropertyBlock();
        _screenRatio = (Screen.width / Screen.height);
    }

    private void Update()
    {
        if (_target.position != _lastPlayerPosition || _lastCameraPosition != Camera.main.transform.position)
        {
            Vector2 cutoutPos = Camera.main.WorldToViewportPoint(_target.position + _target.up * 1.2f);
            cutoutPos.y /= _screenRatio;
            _lastPlayerPosition = _target.transform.position;
            _lastCutoutPosition = cutoutPos;
            _lastCameraPosition = Camera.main.transform.position;
        }
        else
        {
            return;
        }

        Vector3 direction = _target.position + _target.up * 1.2f - transform.position;

        int hitCount = Physics.SphereCastNonAlloc(transform.position, _raycastRadius, direction.normalized, _hitBuffer, direction.magnitude - _raycastDistOffset, _cutoutMask);
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

    public void SetValues(Transform newTarget, float newCutoutSize)
    {
        _target = newTarget;
        _cutoutSize = newCutoutSize;
        _falloffSize = newCutoutSize - 0.05f;
    }
}
