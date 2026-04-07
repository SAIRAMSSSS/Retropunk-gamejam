using UnityEngine;

public class PipeDetail : MonoBehaviour
{
    [SerializeField]
    bool _canMove;

    readonly float _rotationAngle = 90f;
    readonly float _pipeRadius = 10f;

    bool _connected;

    public bool CanMove()
    {
        return _canMove;
    }

    public void Rotate(int direction)
    {
        transform.Rotate(0, direction * _rotationAngle, 0, Space.Self);
        TryConnectPipe();
    }

    public void TryConnectPipe()
    {
        if (!_connected)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform corner = transform.GetChild(i);
                Collider[] hits = Physics.OverlapSphere(corner.position, _pipeRadius, LayerMask.GetMask("InteractiveObject"));
                bool cornerConnected = false;
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent(out PipeDetail pipe))
                    {
                        pipe.TryConnectPipe();
                        cornerConnected = true;
                        break;
                    }
                }

                if (!cornerConnected)
                    return;
            }
            _connected = true;
        }
    }
}
