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
    [SerializeField]
    GameObject _interactiveCanvasPrefab;
    [SerializeField]
    GameObject _audioManagerPrefab;

    public override void InstallBindings()
    {
        //creates objects that will move between scenes
        Container.Bind<InteractionUIManager>()
        .FromComponentInNewPrefab(_interactiveCanvasPrefab)
        .AsSingle()
        .NonLazy();

        var player = Container.InstantiatePrefab(_playerPrefab);
        Container.Bind<PlayerController>()
            .FromComponentOn(player)
            .AsSingle();

        Container.Bind<PlayerInput>()
            .FromComponentOn(player)
            .AsSingle();

        Container.Bind<GameManager>()
            .FromComponentInNewPrefab(_gameManagerPrefab)
            .WithGameObjectName("GameManager")
            .UnderTransformGroup("Managers")
            .AsSingle()
            .NonLazy();      

        Container.Bind<AudioManager>()
            .FromComponentInNewPrefab(_audioManagerPrefab)
            .WithGameObjectName("AudioManager")
            .UnderTransformGroup("Managers")
            .AsSingle()
            .NonLazy();
        
        Container.Bind<SceneTransitionManager>()
            .FromComponentInNewPrefab(_transitionPrefab)
            .WithGameObjectName("TransitionManager")
            .UnderTransformGroup("Managers")
            .AsSingle()
            .NonLazy();

        Container.Bind<CameraController>()
        .FromComponentInNewPrefab(_cameraPrefab)
        .AsSingle()
        .NonLazy();

        var canvas = Container.InstantiatePrefab(_UIManagerPrefab);

        Container.Bind<UIManager>()
            .FromComponentOn(canvas)
            .AsSingle();

        Container.Bind<DialogueManager>()
            .FromComponentOn(canvas)
            .AsSingle();

    }
}
