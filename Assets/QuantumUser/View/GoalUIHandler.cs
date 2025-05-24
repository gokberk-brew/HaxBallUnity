using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using TMPro;
using UnityEngine;

public class GoalUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _goalText;
    [SerializeField] private TMP_Text _leftText;
    [SerializeField] private TMP_Text _rightText;
    
    private void OnEnable()
    {
        _leftText.text = 0.ToString();
        _rightText.text = 0.ToString();
        QuantumEvent.Subscribe<EventOnGoalScored>(this, OnGoalScored);
    }

    private void OnGoalScored(EventOnGoalScored callback)
    {
        StartCoroutine(ShowGoalText(callback));
    }

    private IEnumerator ShowGoalText(EventOnGoalScored callback)
    {
        var teamText = callback.ScoredTeam == Team.Left ? "Red" : "Blue";

        _leftText.text = callback.ScoreSate.ScoreLeft.ToString();
        _rightText.text = callback.ScoreSate.ScoreRight.ToString();
        
        _goalText.gameObject.SetActive(true);

        if (callback.ScoreSate.GameEnded)
        {
            var winningText = callback.ScoreSate.WinningTeam == Team.Left ? "Red" : "Blue";
            _goalText.text = winningText + " wins!";
            // StartCoroutine(DisconnectAllAfterDelay(3f)); // Optional delay
        }
        else
        {
            _goalText.text = teamText + " scored!";
        }
        
        yield return new WaitForSeconds(2);
        _goalText.gameObject.SetActive(false);
    }
    
    private IEnumerator DisconnectAllAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // If called from inside a Quantum callback, use this:
        QuantumRunner.ShutdownAll();

        // If called from outside (e.g. UI), you can use:
        // await QuantumRunner.ShutdownAllAsync();
    }
}
