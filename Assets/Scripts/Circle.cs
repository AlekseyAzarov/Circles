using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Circle : MonoBehaviour, IPointsEarner, IPooledObject, ITimeEventListener
{
    public event Action<int> PointsEarned;
    public event Action<Circle> CircleUsed;

    private int _points;
    private float _speed;
    private float _additionalSpeedStep;
    private float _currentAdditionalSpeed;
    private Camera _camera;

    public GameObject GameObject => gameObject;

    public void Init(Color color, float speed, float currentAdditionalSpeed, float additionalSpeedStep, float size, int points)
    {
        if (_camera == null) _camera = Camera.main;

        GetComponent<SpriteRenderer>().color = color;
        _speed = speed;
        _points = points;
        _currentAdditionalSpeed = currentAdditionalSpeed;
        _additionalSpeedStep = additionalSpeedStep;
        transform.localScale = new Vector2(size, size);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * (_speed + _currentAdditionalSpeed) * Time.deltaTime);

        if (_camera.WorldToViewportPoint(transform.position).y >= 1.1f) CircleUsed?.Invoke(this);
    }

    private void OnMouseDown()
    {
        PointsEarned?.Invoke(_points);
        CircleUsed?.Invoke(this);
    }

    public void OnSpawn()
    {
    }

    public void OnDespawn()
    {
        _speed = 0;
        _points = 0;
        _currentAdditionalSpeed = 0;
        PointsEarned = null;
    }

    public void OnTimeEvent()
    {
        _currentAdditionalSpeed += _additionalSpeedStep;
    }
}