using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Walls;

namespace Data
{
    public class GameData : MonoBehaviour
    {
        public string ScoreLabelText = null;
        public bool IsGameOver { get; set; }
        [SerializeField] private UIDocument uiDocument;
        private Label _scoreLabel;
        private Label _highScoreText;
        private VisualElement _loseScreen;
        private WallManager _wallManager;
        public int Score { get; private set; }

        private void Awake()
        {
            _scoreLabel = uiDocument.rootVisualElement.Q<Label>(ScoreLabelText);
            _loseScreen = uiDocument.rootVisualElement.Q<VisualElement>("LoseScreen");
            _highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScore");
            _loseScreen.style.display = DisplayStyle.None;
            _wallManager = GetComponent<WallManager>();
            SpeedUp();
        }

        public void RestartGame()
        {
            if (!IsGameOver) return;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            if(!IsGameOver) return;
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
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
                case 40:
                case 50:
                case 75:
                case 100:
                    _wallManager.IncreaseWallsSpeed();
                    break;
                default:
                    break;
            }
        }

        public void ActivateLoseScreen()
        {
            IsGameOver = true;
            int highScore = PlayerPrefs.GetInt("Score", 0);
            if (Score > highScore)
                PlayerPrefs.SetInt("Score",Score);
            _highScoreText.text = $"Your score: {Score} \nHigh Score: {PlayerPrefs.GetInt("Score", 0)}";
            _loseScreen.style.display = DisplayStyle.Flex;
        }
    }
}
