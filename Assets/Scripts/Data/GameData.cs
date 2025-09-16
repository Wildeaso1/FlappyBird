using System;
using UnityEngine;
using UnityEngine.UIElements;
using Walls;

namespace Data
{
    public class GameData : MonoBehaviour
    {
        public string ScoreLabelText = null;
        [SerializeField] private UIDocument uiDocument;
        private Label _scoreLabel;
        private WallManager _wallManager;
        public int Score { get; private set; }

        private void Awake()
        {
            _scoreLabel = uiDocument.rootVisualElement.Q<Label>(ScoreLabelText);
            _wallManager = GetComponent<WallManager>();
            SpeedUp();
        }

        public void IncreaseScore(int value)
        {
            Score += value;
            _scoreLabel.text = $"Score: {Score}";
            SpeedUp();
        }
        private void SpeedUp()
        {
            switch (Score)
            {
                
                case 0:
                case 10:
                case 20:
                case 30:
                case 50:
                case 100:
                    _wallManager.IncreaseWallsSpeed();
                    break;
                default:
                    break;
            }
        }
    }
}
