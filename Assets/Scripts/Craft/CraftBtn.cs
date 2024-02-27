using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Events;

public class CraftBtn : MonoBehaviour
{
    [SerializeField] public GameObject CraftPanel; // °Ç¹° Áþ±â UI

    private PlayerController controller;
    private Builder _builder;

    [Header("Events")]
    public UnityEvent onOpenCraftPanel;
    public UnityEvent onCloseCraftPanel;
    public UnityEvent onCancel;

    void Awake()
    {        
        controller = FindObjectOfType<PlayerController>();     
        _builder = GetComponent<Builder>();
    }
    public void OnCraftPanelButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }

    public void OnCancelButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Cancel();
        }
    }

    public void Toggle()
    {
        if (CraftPanel.activeInHierarchy)
        {
            CraftPanel.SetActive(false);
            onCloseCraftPanel?.Invoke();
            controller.ToggleCursor(false);
        }
        else
        {
            CraftPanel.SetActive(true);
            onOpenCraftPanel?.Invoke();
            controller.ToggleCursor(true);
        }
    }

    public void Cancel()
    {
        if (_builder.isPreviewActivated == true)
        {
            Destroy(_builder.go_Preview);
            _builder.isPreviewActivated = false;
            _builder.go_Preview = null;
            CraftPanel.SetActive(false);
            onCancel?.Invoke();
            controller.ToggleCursor(false);
        }                    
    }
}
