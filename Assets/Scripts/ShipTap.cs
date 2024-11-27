using UnityEngine;
using Zenject;

public class ShipTap
{
    [Inject(Id = "EnemyShipsLayer")]
    private readonly LayerMask _enemyShipsLayer;
    private Controls _controls;

    [Inject]
    public void Construct(Controls controls)
    {
        _controls = controls;

        _controls.Gameplay.PrimaryTouch.performed += ctx => Tap();
        _controls.Enable();
    }

    private void Tap()
    {
        Vector2 touchPos = _controls.Gameplay.PrimaryTouchPosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _enemyShipsLayer))
        {
            hit.transform.gameObject.GetComponent<EnemyShipHealth>().TakeDamage();
        }
    }
}