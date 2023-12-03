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
        Debug.Log("sliderValue:" + hpSlider.maxValue);
    }
    
    // Update is called once per frame
    void Update()
    {
       
    }

    //HPの更新
    public void UpdateHPSlider()
    {
        hpSlider.value = playerController.GetPlayerHp();
    }

    /// <summary>
    /// HPsliderに任意のHPを設定する
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
