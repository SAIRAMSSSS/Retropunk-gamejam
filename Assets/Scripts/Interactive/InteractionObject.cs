using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public abstract class InteractionObject : MonoBehaviour
{
    [SerializeField]
    int _priority;
    [SerializeField]
    protected string _interactionText;
    [SerializeField]
    protected string _interactionName;
    [SerializeField]
    protected float _interactonTextOffset;
    [SerializeField]
    protected UnityEvent _interaction;

    public int Prioroty => _priority;
    public string InteractionText => _interactionText;
    public string InteractionName => _interactionName;
    public float InteractionOffset => _interactonTextOffset;

    protected bool _canInteract = true;

    [Inject]
    protected PlayerController _player;

    public virtual bool CanInteract()
    {
        return _canInteract;
    }

    public void PerformActions()
    {
        _interaction.Invoke();
    }

    public void DisableInteraction()
    {
        _canInteract = false;
    }
}
