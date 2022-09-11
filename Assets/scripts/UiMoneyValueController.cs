using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMoneyValueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyValue;
    private GameManager _gameManager;
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        moneyValue.text = _gameManager.Money.ToString();
    }
}
