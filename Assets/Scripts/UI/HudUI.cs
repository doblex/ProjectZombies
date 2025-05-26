using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private Example_PlayerMovement player;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;

        player = FindAnyObjectByType<Example_PlayerMovement>(FindObjectsInactive.Include);
        player.OnHealthChanged += SetHealthBar;

        globalVolume = FindAnyObjectByType<Volume>();

        if (globalVolume.profile.TryGet(out vignette))
        {
            Debug.Log("Vignette trovata e pronta per essere modificata!");
        }
        else
        {
            Debug.LogWarning("Nessun effetto Vignette trovato nel profilo del Volume.");
        }
    }

    void OnDestroy()
    {
        player.OnHealthChanged -= SetHealthBar;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.instance.ShowUI(UIManager.GameUI.Pause);
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
}
