using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : BaseUI
{
    private void OnEnable()
    {
        InputManager.Instance.inputActions.Disable();
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Enable();
    }

    public void GoToMainMenu()
    {
        UIManager.instance.ShowUI(UIManager.GameUI.MainMenu);
        // dipende se serve ricaricare la scena
        // SceneManager.LoadScene(0);
    }
}
