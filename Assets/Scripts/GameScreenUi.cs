using UnityEngine;
using TMPro;

namespace Idle
{
    public class GameScreenUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;

        private void Start()
        {
            OnMoneyChange(GameManager.Instance.Money);
            GameManager.Instance.OnMoneyValueChange += OnMoneyChange;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnMoneyValueChange -= OnMoneyChange;
        }

        private void OnMoneyChange(float value)
        {
            moneyText.text = value.ToString();
        }
    }
}