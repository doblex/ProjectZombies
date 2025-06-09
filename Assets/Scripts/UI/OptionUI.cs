using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionUI : BaseUI
{
    [Header("Debugging Variables")]
    [ReadOnly] public UIManager.GameUI previousGameUI;

    public void GoToMainMenu()
    {
        UIManager.instance.ShowUI(previousGameUI);
    }
}
