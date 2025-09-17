using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private AudioClip _clickSound;
        private UIDocument _uiDocument;
        private Button _startButton;
        private Button _quitButton;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _startButton = _uiDocument.rootVisualElement.Q<Button>("B_Start");
            _quitButton = _uiDocument.rootVisualElement.Q<Button>("B_Exit");
            
            _startButton.clicked += StartGame;
            _quitButton.clicked += QuitGame;
        }

        public void StartGame()
        {
            StartCoroutine(DelayAction(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            }, 0.5f));
        }
    
        public void QuitGame()
        {
            StartCoroutine(DelayAction(() =>
            {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }, 0.5f));
        }

        private IEnumerator DelayAction(Action action, float delay)
        {
            PlayClickSound();
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        private void PlayClickSound()
        {
            if(_clickSound != null)
                AudioSource.PlayClipAtPoint(_clickSound, Camera.main.transform.position);
        }
    }
}
