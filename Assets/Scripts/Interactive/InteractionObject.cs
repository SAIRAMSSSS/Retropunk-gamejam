using UnityEngine;
using UnityEngine.Events;
using Zenject;

public abstract class InteractionObject: MonoBehaviour
{
    [SerializeField]
    int _priority;
    [SerializeField]
    protected string _interactionText;
    [SerializeField]
    protected string _interactionName;
    [SerializeField]
    protected UnityEvent _interactions;

    public int Prioroty => _priority;
    public string InteractionText => _interactionText;
    public string InteractionName => _interactionName;

    [Inject]
    protected PlayerController _player;

    protected virtual void Start()
    {
        _player =GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public abstract bool CanInteract();

    public void PerformActions()
    {
        _interactions.Invoke();

    }
}
