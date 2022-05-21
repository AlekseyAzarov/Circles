using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiController : MonoBehaviour, IGameTimeListener
{
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private GameObject _gameEndUI;
    [SerializeField] private TextMeshProUGUI _gameEndText;
    
    public void ExposeCurrentGameTime(float time)
    {
        UpdateTimeLeftText(time);
    }

    public void UpdatePointsText(int newValue)
    {
        _pointsText.text = $"Points: {newValue}";
    }

    public void UpdateTimeLeftText(float newValue)
    {
        _timeText.text = $"Time left: {string.Format("{0:00}", newValue)}s";
    }

    public void ShowGameEndUI(int score)
    {
        _pointsText.enabled = false;
        _timeText.enabled = false;

        _gameEndUI.SetActive(true);
        _gameEndText.text = score.ToString();
    }

    public void ShowGameUI()
    {
        _pointsText.enabled = true;
        _timeText.enabled = true;
        _pointsText.text = $"Points: {0}";

        _gameEndUI.SetActive(false);
    }
}
