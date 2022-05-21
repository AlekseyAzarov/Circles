using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CirclesSpawner : MonoBehaviour, ITimeEventListener
{
    public System.Action<Circle> CircleSpawned;
    public System.Action<Circle> CircleDespawned;

    [SerializeField] private List<CircleData> _circleDatas;
    [SerializeField] private ColorData _colorData;
    [SerializeField] private float _additionalSpeedStep;
    [SerializeField] private float _spawnDelay;

    private GameObjectPool _circlesPool;
    private Vector2 _leftBorder;
    private Vector2 _rightBorder;
    private List<Circle> _activeCircles;
    private float _currentAdditionalSpeed;
    private float _currentSpawnDelay;

    private System.Action _actionOnUpdate;

    public void Init(GameObjectPool gameObjectPool)
    {
        Camera cam = Camera.main;

        _leftBorder = cam.ViewportToWorldPoint(new Vector2(0f, -0.2f));
        _rightBorder = cam.ViewportToWorldPoint(new Vector2(1f, -0.2f));

        float largestSize = _circleDatas.Max(x => x.Size);

        _leftBorder.x += largestSize;
        _rightBorder.x -= largestSize;

        _circlesPool = gameObjectPool;

        _currentAdditionalSpeed = 0f;
        _activeCircles = new List<Circle>();
        _actionOnUpdate = CheckSpawnCircle;
    }

    private void Update()
    {
        _actionOnUpdate?.Invoke();
    }

    public void SpawnCircle()
    {
        float xPos = Random.Range(_leftBorder.x, _rightBorder.x);
        float yPos = _leftBorder.y;
        Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

        var circle = _circlesPool.Spawn();
        circle.transform.position = spawnPosition;

        var circleData = _circleDatas[Random.Range(0, _circleDatas.Count)];
        var color = _colorData.GetRandomColor();
        var circleScript = circle.GetComponent<Circle>();
        circleScript.CircleUsed += DespawnCircle;
        circleScript.Init(color, circleData.Speed, _currentAdditionalSpeed,
            _additionalSpeedStep, circleData.Size, circleData.Points);

        _activeCircles.Add(circleScript);
        CircleSpawned(circleScript);
    }

    public void DespawnCircle(Circle circle)
    {
        _activeCircles.Remove(circle);
        _circlesPool.Despawn(circle);
        circle.CircleUsed -= DespawnCircle;
        CircleDespawned(circle);
    }

    public void OnTimeEvent()
    {
        _currentAdditionalSpeed += _additionalSpeedStep;
    }

    public void OnGameEnd()
    {
        _actionOnUpdate = null;
        foreach (var circle in _activeCircles) _circlesPool.Despawn(circle);
    }

    private void CheckSpawnCircle()
    {
        _currentSpawnDelay -= Time.deltaTime;

        if (_currentSpawnDelay <= 0f)
        {
            SpawnCircle();
            _currentSpawnDelay = _spawnDelay;
        }
    }

    private void OnDestroy()
    {
        _actionOnUpdate = null;
    }
}