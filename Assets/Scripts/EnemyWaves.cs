using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyWaves
{
    [Inject(Id = "TimeBetweenWaves")]
    private readonly float _timeBetweenWaves;
    [Inject(Id = "EnemySpaceships")]
    private readonly EnemySpaceship[] _enemySpaceships;
    [Inject(Id = "InnerRadius")]
    private readonly float _innerRadius;
    [Inject(Id = "OuterRadius")]
    private readonly float _outerRadius;

    private MonoInstaller _installer;
    private ObjectPlacer _objectPlacer;
    private GlobalGameState _globalGameState;
    private Transform _playerBase;
    private int _waveCount = 1;
    private float _angle;
    private float _distance;

    [Inject]
    public void Construct(MonoInstaller installer, ObjectPlacer objectPlacer, GlobalGameState globalGameState)
    {
        _installer = installer;
        _objectPlacer = objectPlacer;
        _globalGameState = globalGameState;
        _globalGameState.OnStateChangeEvent += SetPlayerBase;
    }

    private void SpawnWave()
    {
        Debug.Log("Zhopa");
        int shipsCount = Random.Range(_waveCount, _waveCount * 2);
        (GameObject, Vector3)[] ships = new (GameObject, Vector3)[shipsCount];
        List<EnemySpaceship> awailiableSpaceships = new List<EnemySpaceship>();
        foreach(EnemySpaceship spaceship in _enemySpaceships)
        {
            if(spaceship.MinSpawnWave <= _waveCount) awailiableSpaceships.Add(spaceship);
        }
        for(int i = 0; i < shipsCount; i++)
        {
            Vector3 position = GetRandomPointBetweenCircles();
            ships[i] = (awailiableSpaceships[Random.Range(0, awailiableSpaceships.Count)].Prefab, position);
        }
        foreach((GameObject, Vector3) enemy in ships)
        {
            _objectPlacer.PlaceObject(enemy.Item1, enemy.Item2, Quaternion.identity);
        }
        _waveCount++;
        _installer.StartCoroutine(NextWaveTimer(_timeBetweenWaves));
    }

    private IEnumerator NextWaveTimer(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnWave();
    }

    private Vector3 GetRandomPointBetweenCircles()
    {
        do
        {
            _angle = Random.Range(0f, 2f * Mathf.PI);
            _distance = Mathf.Sqrt(Random.Range(0f, 1f)) * _outerRadius;
        }
        while (_distance < _innerRadius);
        float x = _distance * Mathf.Cos(_angle);
        float z = _distance * Mathf.Sin(_angle);

        return _playerBase.position + new Vector3(x, 0f, z);
    }

    private void SetPlayerBase(State state)
    {
        if (state == State.Gameplay)
        {
            _installer.StartCoroutine(NextWaveTimer(_timeBetweenWaves));
            _playerBase = _objectPlacer.playerBase;
        }
    }
}