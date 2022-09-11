using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Button resetButton;

    public event Action OnResetClick;

    private void Start()
    {
        resetButton.onClick.AddListener(OnResetButtonClick);
    }

    private void OnDestroy()
    {
        resetButton.onClick.RemoveListener(OnResetButtonClick);
    }
    

    private void OnResetButtonClick()
    {
        OnResetClick?.Invoke();
    }

    public void StartClick()
    {
        Instantiate(gameManager);
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    
}
