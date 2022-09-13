using System;
using UnityEngine;
using UnityEngine.UI;

namespace Idle
{
    public class MenuScreenUI : MonoBehaviour
    {
        private Button _button;
        public event Action OnMenuButtonClick;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            OnMenuButtonClick?.Invoke();
        }
    }
}