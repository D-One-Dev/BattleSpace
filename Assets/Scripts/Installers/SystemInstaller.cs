using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Zenject;

public class SystemInstaller : MonoInstaller
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private Button[] spaceshipsButtons;
    [SerializeField] private Spaceship[] spaceships;
    [SerializeField] private SpaceshipSelector spaceshipsSelector;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private GameObject planeSelectScreen;
    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private LayerMask playerShipsLayer;
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

        this.Container.BindInterfacesAndSelfTo<GlobalGameState>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        this.Container.Bind<PlayerMoney>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        this.Container.Bind<Button[]>()
            .WithId("SpaceshipsButtons")
            .FromInstance(spaceshipsButtons)
            .AsTransient();

        this.Container.Bind<Spaceship[]>()
            .WithId("Spaceships")
            .FromInstance(spaceships)
            .AsTransient();

        this.Container.Bind<SpaceshipSelector>()
            .FromInstance(spaceshipsSelector)
            .AsSingle();

        this.Container.Bind<TMP_Text>()
            .WithId("MoneyText")
            .FromInstance(moneyText)
            .AsTransient();

        this.Container.Bind<UIController>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        this.Container.Bind<GameObject>()
            .WithId("PlaneSelectScreen")
            .FromInstance(planeSelectScreen)
            .AsTransient();

        this.Container.Bind<GameObject>()
            .WithId("GameplayScreen")
            .FromInstance(gameplayScreen)
            .AsTransient();

        this.Container.Bind<NavMeshSurface>()
            .FromInstance(navMeshSurface)
            .AsSingle();

        this.Container.Bind<LayerMask>()
            .WithId("PlayerShipsLayer")
            .FromInstance(playerShipsLayer)
            .AsTransient();
    }
}