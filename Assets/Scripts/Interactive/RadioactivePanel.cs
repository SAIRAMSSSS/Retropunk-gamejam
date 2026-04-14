using UnityEngine;

[RequireComponent (typeof(Collider))]
public class RadioactivePanel : MonoBehaviour
{
    [SerializeField]
    Transform _startPoint;

    AudioSource _audioPlayer;

    private void Start()
    {
        _audioPlayer = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //teleports the player to the start point when they step on the panel
        if (other.CompareTag("Player"))
        {
            _audioPlayer.Play();
            other.transform.rotation = _startPoint.rotation;
        }
    }
}
