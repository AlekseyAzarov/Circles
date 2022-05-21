using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointsController : MonoBehaviour
{
    public Action<int> PointsValueChanged;

    private int _currentPoints;

    public int CurrentPoints => _currentPoints;

    public void Init()
    {
        _currentPoints = 0;
    }

    public void RegisterPointsEarner(IPointsEarner pointsEarner)
    {
        pointsEarner.PointsEarned += AddPoints;
    }

    public void DeregisterPointsEarner(IPointsEarner pointsEarner)
    {
        pointsEarner.PointsEarned -= AddPoints;
    }

    private void AddPoints(int value)
    {
        _currentPoints += value;
        PointsValueChanged?.Invoke(_currentPoints);
    }
}
