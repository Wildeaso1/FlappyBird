using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Data
{
    public class GameData : MonoBehaviour
    {
        public string ScoreLabelText = null;
        [SerializeField] private UIDocument uiDocument;
        private Label _scoreLabel;
        private int _score;

        private void Awake()
        {
            _scoreLabel = uiDocument.rootVisualElement.Q<Label>(ScoreLabelText);
        }

        public void IncreaseScore(int value)
        {
            _score += value;
            _scoreLabel.text = $"Score: {_score}";
            print($"Score is {_score}");
        }
    }
}
