using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Events;

public class CraftBtn : MonoBehaviour
{
    [SerializeField] private GameObject CraftPanel;

    private PlayerController controller;

    [Header("Events")]
    public UnityEvent onOpenCraftPanel;
    public UnityEvent onCloseCraftPanel;

    void Awake()
    {        
        controller = GetComponent<PlayerController>();        
    }
    public void OnCraftPanelButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Toggle();
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
}
