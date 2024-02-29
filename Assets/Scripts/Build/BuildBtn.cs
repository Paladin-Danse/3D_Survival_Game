using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Events;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class BuildBtn : MonoBehaviour
{
    [SerializeField] public GameObject CraftPanel; // °Ç¹° Áþ±â UI

    public Vector3 Brotation = new Vector3(0f, 0f, 0f);

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
        if (callbackContext.phase == InputActionPhase.Started && !_builder.isPreviewActivated)
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

    public void OnBuildClick(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            _builder.Build();
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
            _builder.go_Prefab = null;
            CraftPanel.SetActive(false);
            onCancel?.Invoke();
            controller.ToggleCursor(false);
        }                    
    }

    public void OnBuildingRotateQ(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Brotation += new Vector3(0f, -45f, 0f);
        }
    }

    public void OnBuildingRotateE(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Brotation += new Vector3(0f, +45f, 0f);
        }
    }
}
