using UnityEngine;
using Zenject;

public class SpaceshipSelector : MonoBehaviour
{
    private ObjectPlacer _objectPlacer;

    [Inject]
    public void Construct(ObjectPlacer objectPlacer)
    {
        _objectPlacer = objectPlacer;
    }
    public void SelectSpaceship(GameObject spaceship)
    {
        _objectPlacer.SetCurrentObject(spaceship);
    }
}