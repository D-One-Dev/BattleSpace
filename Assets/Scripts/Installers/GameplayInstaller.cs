using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private Spaceship testPrefab;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private EnemySpaceship[] enemySpaceships;
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    public override void InstallBindings()
    {
        this.Container.Bind<Spaceship>()
            .WithId("TestPrefab")
            .FromInstance(testPrefab)
            .AsTransient();

        this.Container.Bind<float>()
            .WithId("TimeBetweenWaves")
            .FromInstance(timeBetweenWaves)
            .AsTransient();

        this.Container.Bind<EnemySpaceship[]>()
            .WithId("EnemySpaceships")
            .FromInstance(enemySpaceships)
            .AsTransient();

        this.Container.Bind<float>()
            .WithId("InnerRadius")
            .FromInstance(innerRadius)
            .AsTransient();

        this.Container.Bind<float>()
            .WithId("OuterRadius")
            .FromInstance(outerRadius)
            .AsTransient();

        this.Container.Bind<EnemyWaves>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        this.Container.Bind<MonoInstaller>()
            .FromInstance(this)
            .AsSingle();
    }
}