using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationController : MonoBehaviour
{
    public static VibrationController Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void VibrateForDuration(float duration, float strength)
    {
        StartCoroutine(Vibrate(duration, strength));
    }

    private IEnumerator Vibrate(float duration, float strength)
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(strength, strength);
            yield return new WaitForSeconds(duration / 1000.0f); // ƒ~ƒŠ•b‚©‚ç•b‚Ö•ÏŠ·
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

    void OnDestroy()
    {
        StopAllVibrations();
    }

    private void StopAllVibrations()
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

}