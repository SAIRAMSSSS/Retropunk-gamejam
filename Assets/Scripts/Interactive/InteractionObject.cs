using System;
using UnityEngine;
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

    public int Prioroty => _priority;
    public string InteractionText => _interactionText;
    public string InteractionName => _interactionName;
    public float InteractionOffset => _interactonTextOffset;

    protected Action _interaction;
    protected bool _canInteract = true;

    [Inject]
    protected PlayerController _player;

    protected virtual void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public abstract bool CanInteract();

    public void PerformActions()
    {
        _interaction.Invoke();
    }

    public void DisableInteraction()
    {
        _canInteract = false;
    }
}
