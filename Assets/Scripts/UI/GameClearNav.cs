using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearNav : MonoBehaviour
{
    //�N���A��ʕ\������
    private float count;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // �o�ߎ��Ԃ��J�E���g
        count += Time.deltaTime;

        // 3�b��ɉ�ʑJ�ځi�^�C�g���ֈړ��j
        if (count >= 3.0f)
        {
            SceneManager.LoadScene("Title");
        }
    }
}
