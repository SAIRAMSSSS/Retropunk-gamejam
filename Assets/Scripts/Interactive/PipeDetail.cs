using UnityEngine;

public enum PipeType
{
    End,
    Movable, 
    Fixed
}

public class PipeDetail : MonoBehaviour
{
    [SerializeField]
    PipeType _pipeType;

    readonly float _rotationAngle = 90f;
    readonly float _pipeRadius = 10f;

    public bool CanMove()
    {
        return _pipeType == PipeType.Movable;
    }

    public void Rotate(int direction)
    {
        transform.Rotate(0, direction * _rotationAngle, 0, Space.Self);
        TryConnectAllPipes();
    }

    public bool TryConnectAllPipes(PipeDetail previousPipe = null)
    {
        if(_pipeType == PipeType.End)
        {
            return true;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform corner = transform.GetChild(i);
            Collider[] hits = Physics.OverlapSphere(corner.position, _pipeRadius, LayerMask.GetMask("PipeDetail"));
            bool cornerConnected = false;
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out PipeDetail pipe))
                {
                    if (pipe!=previousPipe&&!pipe.TryConnectAllPipes(this))
                    {
                        return false;
                    }
                    else
                    {
                        cornerConnected = true;
                    }
                }
            }

            if (!cornerConnected)
                return false;
        }

        return true;
    }
}
