using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HudUI : BaseUI
{
    // ci sarà sempre e solo un HudUI nella scena
    // quindi si può usare il singleton senza problemi di concorrenza
    public static HudUI Instance { get; private set; }

    [Header("Red Vignette")]
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private Vignette vignette;

    [Header("Ammo Left")]
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Wave Rounds")]
    [SerializeField] private TextMeshProUGUI waveInfoText;

    [Header("Money")]
    [SerializeField] private TextMeshProUGUI moneyText;


    [System.Serializable]
    private struct InteractionObjs
    {
        public InteractionType interactionType;
        public List<ButtonsType> interactableButton;
        public string interactableText;
    }

    [System.Serializable]
    public struct ButtonsType
    {
        public InputUIType.UIType uiType;
        public Sprite buttonSprite;
    }

    [Header("Player Input")]
    [SerializeField] private GameObject interactablePopUp;
    [SerializeField] private Image interactableButton;
    [SerializeField] private TextMeshProUGUI interactableText;
    [SerializeField] private List<InteractionObjs> interactionObjs;


    private PlayerInputManager player;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;

        globalVolume = FindAnyObjectByType<Volume>();

        if (globalVolume == null)
        {
            Debug.LogWarning("Nessun Volume trovato nella scena. Assicurati di avere un Volume con un profilo assegnato.");
            return;
        }

        if (globalVolume.profile.TryGet(out vignette))
        {
            Debug.Log("Vignette trovata e pronta per essere modificata!");
        }
        else
        {
            Debug.LogWarning("Nessun effetto Vignette trovato nel profilo del Volume.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.Instance.ShowUI(UIManager.GameUI.Pause);
        }
    }

    #region Health Bar
    public void SetHealthBar(float currentHp, float maxHp)
    {
        playerHealthBar.value = currentHp / maxHp;
        if (currentHp <= 0)
        {
            vignette.intensity.value = 0.5f;
        }
        else
        {
            vignette.intensity.value = (1 - (currentHp / maxHp)) * 0.5f;
        }
    }
    #endregion

    #region Ammo
    public void SetAmmoText(int currentAmmo, int totalAmmo)
    {
        ammoText.text = currentAmmo.ToString() + "/" + totalAmmo.ToString();
    }
    #endregion

    #region Wave Rounds
    public void SetWaveInfoText(int currentWave, int totalWaves)
    {
        waveInfoText.text = "Wave: " + currentWave.ToString() + "/" + totalWaves.ToString();
    }
    #endregion

    #region Money
    public void SetMoneyText(int money)
    {
        moneyText.text = "$" + money.ToString();
    }
    public void AddMoneyText(int money)
    {
        moneyText.text = "$" + (int.Parse(moneyText.text.Substring(1)) + money).ToString();
    }
    public void SubtractMoneyText(int money)
    {
        moneyText.text = "$" + (int.Parse(moneyText.text.Substring(1)) - money).ToString();
    }
    public void ResetMoneyText()
    {
        moneyText.text = "$0";
    }

    #endregion

    internal void ShowRebuildInteract(InteractionType interactionType)
    {
        interactablePopUp.SetActive(true);

        foreach (InteractionObjs interactionObj in interactionObjs)
        {
            if (interactionObj.interactionType == interactionType)
            {
                interactableButton.sprite = interactionObj.interactableButton[GetInteractionInputIndex(interactionType)].buttonSprite;
                interactableText.text = interactionObj.interactableText;
                return;
            }
        }
    }

    private int GetInteractionInputIndex(InteractionType interactionType)
    {
        // AVERE UN INPUT MANAGER
        PlayerInput playerInput = FindAnyObjectByType<PlayerInput>();

        foreach (InteractionObjs interactionObj in interactionObjs)
        {
            if (interactionObj.interactionType == interactionType)
            {
                for (int i = 0; i < interactionObj.interactableButton.Count; i++)
                {
                    if (interactionObj.interactableButton[i].uiType == InputUIType.Instance.GetUIType())
                    {
                        return i; // Return the index of the matching UI type
                    }
                }
            }
        }

        return -1; // Not found
    }

    internal void HideRebuildInteract()
    {
        interactablePopUp.SetActive(false);
    }
}


public enum InteractionType
{
    Rebuild,
    Buy,
}