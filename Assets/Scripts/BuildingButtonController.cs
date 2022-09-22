using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Idle
{
    public class BuildingButtonController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI buttonText;

        private Button _button;
        private float _cost;
        
        public event Action OnClick;
        
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
            OnClick?.Invoke();
        }

        private void SetState(bool isActive)
        {
            _button.interactable = isActive;
        }

        public void OnMoneyValueChanged(float value)
        {
            SetState(_cost <= value);
        }

        public void UpdateButton(string text, float cost)
        {
            buttonText.text = text;
            costText.text = cost.ToString();
            _cost = cost;
        }
    }
}