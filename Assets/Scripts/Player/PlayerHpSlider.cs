using UnityEngine;
using UnityEngine.UI;

public class PlayerHpSlider : MonoBehaviour
{
    public Slider hpSlider;
    public PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        Slider hpSlider = GetComponent<Slider>();
        if (hpSlider != null)
        {
            // �X���C�_�[�̍ő�l�ƌ��ݒl��ݒ�
            hpSlider.maxValue = playerController.hp;
            hpSlider.value = playerController.maxHp;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        hpSlider.value = playerController.hp;
      
        if (playerController.isPossession)
        {
            // hpSlider.colors=new Color(200,200,200);
        }
    }

    /// <summary>
    /// HPslider�ɔC�ӂ�HP��ݒ肷��
    /// </summary>
    public void SetPlayerHp(PlayerController player)
    {
        playerController = player;
        hpSlider.maxValue = player.maxHp;
    }
}
