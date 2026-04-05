using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller<SceneInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerSpawner>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<CutsceneManager>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}
