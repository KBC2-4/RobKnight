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
            // スライダーの最大値と現在値を設定
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
    /// HPsliderに任意のHPを設定する
    /// </summary>
    public void SetPlayerHp(PlayerController player)
    {
        playerController = player;
        hpSlider.maxValue = player.maxHp;
    }
}
