using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ShowControllerButtonUI : MonoBehaviour
{
    [SerializeField] private DeviceDisplayConfigurator deviceDisplay;
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private string inputBinding;

    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = GameObject.FindObjectOfType<PlayerInput>();

        if (playerInput == null) return;
        
        UpdateImage();
    }

    public void UpdateImage()
    {
        if (playerInput == null) return;

        spriteRenderer.sprite = deviceDisplay.GetDeviceBindingIcon(playerInput, inputBinding);
    }
}
