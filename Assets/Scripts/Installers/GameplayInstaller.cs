using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private Spaceship testPrefab;
    public override void InstallBindings()
    {
        this.Container.Bind<Spaceship>()
            .WithId("TestPrefab")
            .FromInstance(testPrefab)
            .AsTransient();
    }
}