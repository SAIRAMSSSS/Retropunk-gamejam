using UnityEngine;

[RequireComponent (typeof(Collider))]
public class RadioactivePanel : MonoBehaviour
{
    [SerializeField]
    Transform _startPoint;

    private void OnTriggerEnter(Collider other)
    {
        //teleports the player to the start point when they step on the panel
        if (other.CompareTag("Player"))
        {
            other.transform.rotation = _startPoint.rotation;
        }
    }
}
