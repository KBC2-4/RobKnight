using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapZoomToggle : MonoBehaviour
{
    public Camera minimapCamera;
    public float zoomLevel1 = 50f;
    public float zoomLevel2 = 130f;
    private bool isZoomedIn = false;
    
    public InputActionAsset inputActions;
    private InputControls controls;

    private void Awake()
    {
        controls = new InputControls();

        controls.Player.ToggleZoom.performed += ctx => ToggleZoom();
    }
    
    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
    
    private void ToggleZoom()
    {
        if (isZoomedIn)
        {
            minimapCamera.orthographicSize = zoomLevel1;
            isZoomedIn = false;
        }
        else
        {
            minimapCamera.orthographicSize = zoomLevel2;
            isZoomedIn = true;
        }
    }
    
    void Update()
    {
        // // 左スティックの押し込みでズームレベルを切り替え
        // if (Gamepad.current.leftStickButton.wasPressedThisFrame)
        // {
        //     if (isZoomedIn)
        //     {
        //         minimapCamera.orthographicSize = zoomLevel1;
        //         isZoomedIn = false;
        //     }
        //     else
        //     {
        //         minimapCamera.orthographicSize = zoomLevel2;
        //         isZoomedIn = true;
        //     }
        // }
    }
}