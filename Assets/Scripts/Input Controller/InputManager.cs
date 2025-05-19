using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Asset delle Input Actions")]
    public InputActionAsset inputActions;

    [Header("Azioni da gestire")]
    [Tooltip("Input Action da gestire, definita nell'InputActionAsset")]
    public List<InputActionReference> actionsToManage = new List<InputActionReference>();

    // Runtime storage
    public class BindingInfo
    {
        public InputActionReference actionReference;
        public InputBinding positive;
        public InputBinding negative;
    }
    private List<BindingInfo> keyboardMouseBindings = new List<BindingInfo>();
    private List<BindingInfo> controllerBindings = new List<BindingInfo>();

    private const string PREFS_KEY = "NewInputSystem_Rebinds";

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;

        inputActions.Enable();
        LoadBindings();
        BuildRuntimeBindings();
    }

    private void LoadBindings()
    {
        if (PlayerPrefs.HasKey(PREFS_KEY))
        {
            var json = PlayerPrefs.GetString(PREFS_KEY);
            inputActions.LoadBindingOverridesFromJson(json);
        }
    }

    private void BuildRuntimeBindings()
    {
        keyboardMouseBindings.Clear();
        controllerBindings.Clear();

        foreach (var entry in actionsToManage)
        {
            var action = entry?.action;
            if (action == null) continue;

            var bindings = action.bindings;
            if (bindings.Count < 2) continue;

            InputBinding posBind = bindings[0];
            InputBinding negBind = bindings[1];

            if (posBind.isPartOfComposite)
            {
                var up = bindings.FirstOrDefault(b => b.name == "up");
                var down = bindings.FirstOrDefault(b => b.name == "down");
                if (up != default) posBind = up;
                if (down != default) negBind = down;
            }

            bool isController = posBind.groups?.Contains("Gamepad") == true;
            var list = isController ? controllerBindings : keyboardMouseBindings;
            list.Add(new BindingInfo { actionReference = entry, positive = posBind, negative = negBind });
        }
    }

    /// <summary>
    /// Rebind usando direttamente la reference, evita stringhe.
    /// </summary>
    public async Task RebindActionAsync(InputActionReference actionRef, bool positive)
    {
        var action = actionRef?.action;
        if (action == null) return;

        // Trova index: 0 = positivo, 1 = negativo
        int index = positive ? 0 : 1;
        var rebind = action.PerformInteractiveRebinding(index)
            .WithControlsExcluding("<Pointer>/position")
            .WithControlsExcluding("<Pointer>/delta")
            .WithCancelingThrough("<Keyboard>/escape")
            .Start();

        var tcs = new TaskCompletionSource<bool>();
        rebind.OnComplete(op =>
        {
            op.Dispose();
            SaveOverrides();
            BuildRuntimeBindings();
            Debug.Log($"Rebind completato: {action.name} -> {action.bindings[index].effectivePath}");
            tcs.SetResult(true);
        });
        await tcs.Task;
    }

    private void SaveOverrides()
    {
        var json = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(PREFS_KEY, json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Ripristina ai default dell'asset.
    /// </summary>
    public void ResetToDefaults()
    {
        inputActions.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey(PREFS_KEY);
        PlayerPrefs.Save();
        BuildRuntimeBindings();
        Debug.Log("Binding ripristinati ai valori default.");
    }

    void OnDestroy()
    {
        inputActions.Disable();
    }
}
