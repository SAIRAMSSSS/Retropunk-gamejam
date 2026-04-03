using UnityEngine;

public class SetBoolOnAnimation : StateMachineBehaviour
{
    [SerializeField]
    string _parameterName;
    [SerializeField]
    bool _value;
    [SerializeField]
    bool m_stateMachine;
    [SerializeField]
    bool _state;

    [SerializeField]
    bool _onExit;
    [SerializeField]
    bool _onEnter;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (m_stateMachine && _onEnter)
        {
            animator.SetBool(Animator.StringToHash(_parameterName), _value);
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_state && _onEnter)
        {
            animator.SetBool(Animator.StringToHash(_parameterName), _value);
        }
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (m_stateMachine && _onExit)
        {
            animator.SetBool(Animator.StringToHash(_parameterName), _value);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_state && _onExit)
        {
            animator.SetBool(Animator.StringToHash(_parameterName), _value);
        }
    }
}
