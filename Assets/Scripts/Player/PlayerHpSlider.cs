using UnityEngine;
using UnityEngine.UI;

public class PlayerHpSlider : MonoBehaviour
{
    public Slider hpSlider;
    public PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        hpSlider.maxValue = playerController.GetPlayerMaxHp();
        hpSlider.value = hpSlider.maxValue;
    }
    
    // Update is called once per frame
    void Update()
    {
        hpSlider.value = playerController.GetPlayerHp();
      
        if (playerController.isPossession)
        {
            // hpSlider.colors=new Color(200,200,200);
        }
    }

    /// <summary>
    /// HPsliderÇ…îCà”ÇÃHPÇê›íËÇ∑ÇÈ
    /// </summary>
    public void SetPlayerHp(PlayerController player)
    {
        playerController = player;
        hpSlider.maxValue = player.GetPlayerMaxHp();
    }
}
