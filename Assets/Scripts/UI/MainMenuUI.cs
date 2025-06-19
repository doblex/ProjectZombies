using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : BaseUI
{
    public void GoToHud()
    {
        UIManager.Instance.ShowUI(UIManager.GameUI.HUD);
    }

    public void GoToOptions()
    {
        UIManager.Instance.ShowUI(UIManager.GameUI.Option);
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
