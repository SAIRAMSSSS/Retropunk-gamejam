using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerInput _input;
    NavMeshAgent _navAgent;
    Animator _animator;

    void Start()
    {
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //moves player
        if (_input.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(_input.MousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                _navAgent.SetDestination(hit.point);
            }
        }
    }
}
