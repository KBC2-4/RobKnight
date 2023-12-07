using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearNav : MonoBehaviour
{
    //クリア画面表示時間
    private float count;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 経過時間をカウント
        count += Time.deltaTime;

        // 3秒後に画面遷移（タイトルへ移動）
        if (count >= 3.0f)
        {
            SceneManager.LoadScene("Title");
        }
    }
}
