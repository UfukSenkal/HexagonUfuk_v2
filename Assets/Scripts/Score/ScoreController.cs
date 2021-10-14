using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexagonDemo.Score
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] int _scoreMult = 5;
        [SerializeField] string scoreString = "Score: ";
        int _score = 0;

        public int Score { get => _score;}



        public void ScoreTextUpdate(int hexagonCount)
        {
            _score += (hexagonCount * _scoreMult);
            _scoreText.text = scoreString + Score;
        }
    }
}
