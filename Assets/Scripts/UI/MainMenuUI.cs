using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : BaseUI
{
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

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
