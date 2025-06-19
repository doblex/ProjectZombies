using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : BaseUI
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.instance.ShowUI(UIManager.GameUI.HUD);
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToHud()
    {
        UIManager.instance.ShowUI(UIManager.GameUI.HUD);
    }

    public void GoToOptions()
    {
        UIManager.instance.ShowUI(UIManager.GameUI.Option);
        OptionUI optionUI = FindAnyObjectByType<OptionUI>(FindObjectsInactive.Include);

        optionUI.previousGameUI = GetUIType();
    }
}
