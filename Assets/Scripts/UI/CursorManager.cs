using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetCustomCursor();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void SetCustomCursor()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}