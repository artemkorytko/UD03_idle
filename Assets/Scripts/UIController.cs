
using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
   [SerializeField] private MenuScreenUi menuScreen;
   [SerializeField] private GameScreenUi gameScreen;

   public event Action OnStartGameButtonClick;

   private void Start()
   {
      menuScreen.OnClick += StartButtonClick;
   }

   private void OnDestroy()
   {
      menuScreen.OnClick -= StartButtonClick;
   }

   private void StartButtonClick()
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
