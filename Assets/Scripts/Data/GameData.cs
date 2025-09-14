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
        public int Score { get; private set; }

        private void Awake()
        {
            _scoreLabel = uiDocument.rootVisualElement.Q<Label>(ScoreLabelText);
        }

        public void IncreaseScore(int value)
        {
            Score += value;
            _scoreLabel.text = $"Score: {Score}";
            print($"Score is {Score}");
        }
    }
}
