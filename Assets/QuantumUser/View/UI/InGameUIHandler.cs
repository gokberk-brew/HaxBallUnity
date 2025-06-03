using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using TMPro;
using UnityEngine;

public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _goalText;
    [SerializeField] private TMP_Text _leftText;
    [SerializeField] private TMP_Text _rightText;
    [SerializeField] private TMP_Text _timerText;
    
    private void OnEnable()
    {
        _leftText.text = 0.ToString();
        _rightText.text = 0.ToString();
        QuantumEvent.Subscribe<EventOnGoalScored>(this, OnGoalScored);
    }
    
    private void Update()
    {
        var game = QuantumRunner.DefaultGame;
        if (game?.Frames?.Verified == null)
            return;

        var frame = game.Frames.Verified;
        if (!frame.TryGetSingleton<GameState>(out var state))
            return;
        
        if(!state.IsGameActive)
            return;

        int totalSeconds = state.RemainingTimeTicks / 60;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        _timerText.text = $"{minutes:D2}:{seconds:D2}";
    }
    
    private void OnGoalScored(EventOnGoalScored callback)
    {
        StartCoroutine(ShowGoalText(callback));
    }

    private IEnumerator ShowGoalText(EventOnGoalScored callback)
    {
        var teamText = callback.ScoredTeam == Team.Left ? "Red" : "Blue";

        _leftText.text = callback.GameState.ScoreLeft.ToString();
        _rightText.text = callback.GameState.ScoreRight.ToString();
        
        _goalText.gameObject.SetActive(true);

        if (!callback.GameState.IsGameActive)
        {
            var winningText = callback.GameState.WinningTeam == Team.Left ? "Red" : "Blue";
            _goalText.text = winningText + " wins!";
        }
        else
        {
            _goalText.text = teamText + " scored!";
        }
        
        yield return new WaitForSeconds(2);
        _goalText.gameObject.SetActive(false);
    }
}
