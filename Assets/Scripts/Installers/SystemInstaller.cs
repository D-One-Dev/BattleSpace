using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

public class SystemInstaller : MonoInstaller
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    public override void InstallBindings()
    {
        this.Container.Bind<ARRaycastManager>()
            .FromInstance(raycastManager)
            .AsSingle();
        this.Container.Bind<ARPlaneManager>()
            .FromInstance(planeManager)
            .AsSingle();
        this.Container.Bind<Controls>()
            .FromNew()
            .AsTransient();

        this.Container.Bind<ObjectPlacer>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        this.Container.BindInterfacesAndSelfTo<GloballGameState>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
}