using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeController : MonoBehaviour
{
    public Action TimeExpired;

    [SerializeField] private float _gameTime;
    [SerializeField] private int _timeEventSteps;

    private HashSet<ITimeEventListener> _timeEventListeners;
    private HashSet<IGameTimeListener> _gameTimeListeners;
    private float _currentGameTime;
    private float _oneStepTime;
    private float _timeStep;

    private Action _actionOnUpdate;

    public void Init()
    {
        _timeEventListeners = new HashSet<ITimeEventListener>();
        _gameTimeListeners = new HashSet<IGameTimeListener>();

        _currentGameTime = _gameTime;
        _oneStepTime = _gameTime / _timeEventSteps;
        _timeStep = _oneStepTime;

        _actionOnUpdate = CountTime;
    }

    public void AddGameTimeListener(IGameTimeListener gameTimeListener)
    {
        _gameTimeListeners.Add(gameTimeListener);
    }

    public void AddTimeEventListener(ITimeEventListener timeEventListener)
    {
        _timeEventListeners.Add(timeEventListener);
    }

    public void RemoveGameTimeListener(IGameTimeListener gameTimeListener)
    {
        _gameTimeListeners.Remove(gameTimeListener);
    }

    public void RemoveTimeEventListener(ITimeEventListener timeEventListener)
    {
        _timeEventListeners.Remove(timeEventListener);
    }

    private void Update()
    {
        _actionOnUpdate?.Invoke();
    }

    private void CountTime()
    {
        _currentGameTime -= Time.deltaTime;
        _timeStep -= Time.deltaTime;

        foreach (var t in _gameTimeListeners) t.ExposeCurrentGameTime(_currentGameTime);

        if (_timeStep <= 0f)
        {
            foreach (var t in _timeEventListeners) t.OnTimeEvent();
            _timeStep = _oneStepTime;
        }

        if (_currentGameTime <= 0f)
        {
            TimeExpired?.Invoke();
            _actionOnUpdate = null;
        }    
    }

    private void OnDestroy()
    {
        _actionOnUpdate = null;
    }
}