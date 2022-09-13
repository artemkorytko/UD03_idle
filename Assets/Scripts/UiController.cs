using System;
using UnityEngine;

namespace Idle
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private MenuScreenUI menuScreen;
        [SerializeField] private GameScreenUi gameScreen;

        public event Action OnStartGameButtonClick;

        private void Start()
        {
            menuScreen.OnMenuButtonClick += OnClickStartButton;
        }

        private void OnDestroy()
        {
            menuScreen.OnMenuButtonClick -= OnClickStartButton;
        }

        private void OnClickStartButton()
        {
            OnStartGameButtonClick?.Invoke();
        }

        public void ShowMenuScreen()
        {
            gameScreen.gameObject.SetActive(false);
            menuScreen.gameObject.SetActive(true);
        }

        public void ShowGameScreen()
        {
            gameScreen.gameObject.SetActive(true);
            menuScreen.gameObject.SetActive(false);
        }
    }
}