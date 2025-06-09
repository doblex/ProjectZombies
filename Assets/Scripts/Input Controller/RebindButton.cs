using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindButton : MonoBehaviour
{
    public enum AxisDimension { X, Y }
    public enum DirectionPart { Positive, Negative }

    [Header("Configurazione Rebind")]
    public InputActionReference actionReference;
    [Tooltip("Se l'azione è Vector2, scegli qui se ribindare X o Y")]
    public AxisDimension dimension = AxisDimension.X;
    [Tooltip("Se l'azione è Vector2, scegli qui se ribindare la parte positiva (Right/Up) o negativa (Left/Down)")]
    public DirectionPart part = DirectionPart.Positive;

    [Header("UI Elements")]
    public TMP_Text bindingLabel;
    private Button rebindButton;
    private InputAction action;

    void Awake()
    {
        rebindButton = GetComponentInChildren<Button>();
        action = actionReference?.action;
        if (action == null)
        {
            Debug.LogError("RebindButton: actionReference mancante o non valida.");
            enabled = false;
            return;
        }

        rebindButton.onClick.AddListener(StartRebind);
    }

    void OnEnable()
    {
        UpdateBindingLabel();
    }

    /// <summary>
    /// Nome della parte composite (up/down/left/right) in base a dimensione e parte
    /// </summary>
    string GetCompositePartName()
    {
        if (dimension == AxisDimension.X)
            return part == DirectionPart.Positive ? "right" : "left";
        else
            return part == DirectionPart.Positive ? "up" : "down";
    }

    /// <summary>
    /// Mostra la lettera associata (es. W/A/S/D)
    /// </summary>
    public void UpdateBindingLabel()
    {
        int bindingIndex = GetBindingIndex();
        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
        {
            bindingLabel.text = "-";
            return;
        }

        var path = action.bindings[bindingIndex].effectivePath;  // es. "<Keyboard>/w"
        string key = path.Split('/').LastOrDefault();
        bindingLabel.text = string.IsNullOrEmpty(key) ? "-" : key.ToUpper();
    }

    /// <summary>
    /// Restituisce l'indice del binding target, gestendo composite 2D Vector.
    /// </summary>
    int GetBindingIndex()
    {
        // Se l'azione usa un composite (es. WASD o Arrow keys)
        if (action.bindings.Any(b => b.isComposite))
        {
            string partName = GetCompositePartName();
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var b = action.bindings[i];
                if (b.isPartOfComposite &&
                    b.name.Equals(partName, System.StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }
        // Azione semplice: binding 0=positivo, 1=negativo
        return (part == DirectionPart.Positive ? 0 : 1);
    }

    /// <summary>
    /// Avvia l'Interactive Rebinding: prima clic, poi attende pressione tasto.
    /// </summary>
    private async void StartRebind()
    {
        rebindButton.interactable = false;
        bindingLabel.text = "...";

        int bindingIndex = GetBindingIndex();
        if (bindingIndex < 0)
        {
            Debug.LogError($"RebindButton: binding non trovato per {action.name}");
            rebindButton.interactable = true;
            UpdateBindingLabel();
            return;
        }

        var rebindOp = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Pointer>/position")
            .WithControlsExcluding("<Pointer>/delta")
            .WithCancelingThrough("<Keyboard>/escape");

        // Mostra in anteprima tasto premuto
        rebindOp.OnPotentialMatch(ctx =>
        {
            var pressed = ctx.action.controls[0].name.ToUpper();
            bindingLabel.text = pressed;
        });

        var tcs = new TaskCompletionSource<bool>();
        rebindOp.OnComplete(op =>
        {
            op.Dispose();
            // Salva override
            var json = action.actionMap.asset.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString("NewInputSystem_Rebinds", json);
            PlayerPrefs.Save();
            tcs.SetResult(true);
        });

        rebindOp.Start();
        await tcs.Task;

        UpdateBindingLabel();
        rebindButton.interactable = true;
    }
}
