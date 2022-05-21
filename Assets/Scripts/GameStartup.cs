using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartup : MonoBehaviour
{
    [SerializeField] private Circle _circlePrefab;
    [SerializeField] private CirclesSpawner _circlesSpawnerPrefab;
    [SerializeField] private GameObjectPool _poolPrefab;
    [SerializeField] private UiController _uiControllerPrefab;
    [SerializeField] private PointsController _pointsControllerPrefab;
    [SerializeField] private TimeController _timeControllerPrefab;
    [SerializeField] private GameRestarter _gameRestarter;

    private CirclesSpawner _circlesSpawner;
    private GameObjectPool _pool;
    private UiController _uiController;
    private PointsController _pointsController;
    private TimeController _timeController;

    private void Awake()
    {
        _pool = Instantiate(_poolPrefab.gameObject).GetComponent<GameObjectPool>();
        _circlesSpawner = Instantiate(_circlesSpawnerPrefab.gameObject).GetComponent<CirclesSpawner>();
        _uiController = Instantiate(_uiControllerPrefab.gameObject).GetComponent<UiController>();
        _pointsController = Instantiate(_pointsControllerPrefab.gameObject).GetComponent<PointsController>();
        _timeController = Instantiate(_timeControllerPrefab.gameObject).GetComponent<TimeController>();

        _pool.Init(_circlePrefab);

        StartGame();
    }

    private void StartGame()
    {
        _circlesSpawner.Init(_pool);
        _timeController.Init();
        _pointsController.Init();

        _uiController.ShowGameUI();

        _timeController.AddGameTimeListener(_uiController);
        _timeController.AddTimeEventListener(_circlesSpawner);

        _circlesSpawner.CircleSpawned += _pointsController.RegisterPointsEarner;
        _circlesSpawner.CircleDespawned += _pointsController.DeregisterPointsEarner;
        _circlesSpawner.CircleSpawned += _timeController.AddTimeEventListener;
        _circlesSpawner.CircleDespawned += _timeController.RemoveTimeEventListener;
        _pointsController.PointsValueChanged += _uiController.UpdatePointsText;

        _timeController.TimeExpired += OnTimeExpired;
    }

    private void OnTimeExpired()
    {
        _uiController.ShowGameEndUI(_pointsController.CurrentPoints);
        _gameRestarter.enabled = true;
        _gameRestarter.TouchDetected += StartGame;
        _circlesSpawner.OnGameEnd();
        EndGame();
    }

    private void EndGame()
    {
        _timeController.RemoveGameTimeListener(_uiController);
        _timeController.RemoveTimeEventListener(_circlesSpawner);

        _circlesSpawner.CircleSpawned -= _pointsController.RegisterPointsEarner;
        _circlesSpawner.CircleDespawned -= _pointsController.DeregisterPointsEarner;
        _circlesSpawner.CircleSpawned -= _timeController.AddTimeEventListener;
        _circlesSpawner.CircleDespawned -= _timeController.RemoveTimeEventListener;
        _pointsController.PointsValueChanged -= _uiController.UpdatePointsText;
        _timeController.TimeExpired -= OnTimeExpired;
    }

    private void OnDestroy()
    {
        EndGame();
    }
}
