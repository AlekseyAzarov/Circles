using System;

public interface IPointsEarner
{
    event Action<int> PointsEarned;
}
