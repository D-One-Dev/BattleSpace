using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameObject testPrefab;
    public override void InstallBindings()
    {
        this.Container.Bind<GameObject>()
            .WithId("TestPrefab")
            .FromInstance(testPrefab)
            .AsTransient();
    }
}