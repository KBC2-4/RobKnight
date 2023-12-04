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
       
    }

    //HPÇÃçXêV
    public void UpdateHPSlider()
    {
        hpSlider.value = playerController.GetPlayerHp();
    }

    /// <summary>
    /// HPsliderÇ…îCà”ÇÃHPÇê›íËÇ∑ÇÈ
    /// </summary>
    public void SetPlayerHp(PlayerController player,Color setColor)
    {
        playerController = player;
        hpSlider.maxValue = player.GetPlayerMaxHp();
        hpSlider.value = player.GetPlayerHp();

        if (setColor != null)
        {
            hpSlider.fillRect.GetComponent<Image>().color = setColor;
        }
    }
}
