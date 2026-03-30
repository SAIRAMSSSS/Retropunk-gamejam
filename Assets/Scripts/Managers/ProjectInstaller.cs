using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    [SerializeField]
    GameObject _playerPrefab;
    [SerializeField]
    GameObject _transitionPrefab;
    [SerializeField]
    GameObject _gameManagerPrefab;
    [SerializeField]
    GameObject _UIManagerPrefab;
    [SerializeField]
    GameObject _cameraPrefab;

    public override void InstallBindings()
    {
        //creates objects that will move between scenes
        Container.Bind<PlayerController>()
            .FromComponentInNewPrefab(_playerPrefab)
            .AsSingle()
            .NonLazy();

        Container.Bind<SceneTransitionManager>()
            .FromComponentInNewPrefab(_transitionPrefab)
            .WithGameObjectName("SceneTransitionManager")
            .UnderTransformGroup("Managers")
            .AsSingle()
            .NonLazy();

        Container.Bind<GameManager>()
            .FromComponentInNewPrefab(_gameManagerPrefab)
            .WithGameObjectName("GameManager")
            .UnderTransformGroup("Managers")
            .AsSingle()
            .NonLazy();

        Container.Bind<UIManager>()
            .FromComponentInNewPrefab(_UIManagerPrefab)
            .AsSingle()
            .NonLazy();

        Container.Bind<CameraController>()
            .FromComponentInNewPrefab(_cameraPrefab)
            .AsSingle()
            .NonLazy();
    }
}
