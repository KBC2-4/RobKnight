using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangger : MonoBehaviour
{
    public void BtnOnClick()
    {
        Debug.Log("OK!");
        SceneManager.LoadScene("GameMainScene");
    }
}
