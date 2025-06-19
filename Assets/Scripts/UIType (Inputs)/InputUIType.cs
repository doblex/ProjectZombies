using UnityEngine;

public class InputUIType : MonoBehaviour
{
    public static InputUIType Instance { get; private set; }

    public enum UIType
    {
        Keyboard,
        Gamepad
    }

    public UIType uiType = UIType.Keyboard;

    public void SetUIType(UIType uiType)
    {
        this.uiType = uiType;
    }

    public UIType GetUIType()
    {
        return uiType;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
}
