using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    CinemachineCamera m_playerCamera;

    [Inject]
    PlayerController _player;

    void Awake()
    {
        //m_playerCamera = GetComponentInChildren<CinemachineCamera>();
        //m_playerCamera.Follow = _player.transform;
    }
}
