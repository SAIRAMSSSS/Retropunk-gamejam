using UnityEngine;
using Zenject;

public class TeleportToScene : MonoBehaviour
{
    [SerializeField]
    string _sceneName;

    [Inject]
    SceneTransitionManager _transitionManager;

    void Start()
    {
        _transitionManager = GameObject.Find("SceneTransitionManager").GetComponent<SceneTransitionManager>();
    }
    /// <summary>
    /// Moves the player to the scene with a given name
    /// </summary>
    public void GoToScene()
    {
        _transitionManager.NewScene(_sceneName);
    }
}
