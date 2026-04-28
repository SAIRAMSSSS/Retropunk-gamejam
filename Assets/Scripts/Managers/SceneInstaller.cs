using Zenject;

public class SceneInstaller : MonoInstaller<SceneInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<InteractionUIManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<PlayerController>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<PlayerInput>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<GameManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<AudioManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<LevelManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<UIManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<DialogueManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<CutoutShaderController>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<CutsceneManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<PlayerSpawner>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}
