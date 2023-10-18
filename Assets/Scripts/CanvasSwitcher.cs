using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject Canvas1;
    public GameObject Canvas2;

    private bool isCanvas1Active = true;

    public void SwitchCanvas()
    {
        isCanvas1Active = !isCanvas1Active;
        Canvas1.SetActive(isCanvas1Active);
        Canvas2.SetActive(!isCanvas1Active);
    }
}
