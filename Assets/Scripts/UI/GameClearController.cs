using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameClearController : MonoBehaviour
{
    [SerializeField]  private EnemyController Boss;
    //�N���A��ʕ\������
    private float count;
    [SerializeField] GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss.isDeath)
        {
            canvas.SetActive(true);
            // �o�ߎ��Ԃ��J�E���g
            count += Time.deltaTime;

            // 3�b��ɉ�ʑJ�ځi�^�C�g���ֈړ��j
            if (count >= 3.0f)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}