using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class HpSlider : MonoBehaviour
{
    public Color highHealthColor = Color.green;
    public Color mediumHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;
    public float updateSpeedSeconds = 0.5f;

    private EnemyController _enemyController;
    private Slider _hpSlider;
    private Image _hpBarImage;
    private float _maxHealth;
    private float _currentHealth;
    private float _oldHealth;


    void Start()
    {
        _enemyController = GetComponentInParent<EnemyController>();
        _hpSlider = GetComponentInChildren<Slider>();
        _hpSlider.maxValue = _enemyController.enemyData.maxHp;
        _hpSlider.value = _enemyController.enemyData.hp;
        _hpBarImage = _hpSlider.fillRect.GetComponent<Image>();

        _maxHealth = _enemyController.enemyData.maxHp;
        _currentHealth = _enemyController.enemyData.hp;
        _oldHealth = _enemyController.enemyData.hp;
    }

    void Update()
    {
        //if (_oldHealth > _currentHealth)
        //{
        //    ReduceHealth(_oldHealth - _currentHealth);
        //}

        // _hpSlider.value = _enemyController.enemyData.hp;

        // HPの値が変更されたか確認
        if (_hpSlider.value != _enemyController.enemyData.hp)
        {
            // HPの更新
            StartCoroutine(ChangeHealth(_enemyController.enemyData.hp));
        }

        // エネミーのHPが0になった場合、HPバーを非表示にする
        if (_enemyController.enemyData.hp <= 0)
        {
            _hpSlider.gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // 常にカメラと同じ向きに設定
            transform.rotation = mainCamera.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Main Cameraがありません。\nMain Cameraタグを設定してください");
        }
    }

    private IEnumerator ChangeHealth(float newHealth)
    {
        float preChangeHealth = _hpSlider.value;
        float elapsed = 0f;

        // 徐々にHPを更新
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            // 線形補間で滑らかにHPを更新
            _hpSlider.value = Mathf.Lerp(preChangeHealth, newHealth, elapsed / updateSpeedSeconds);
            // HPバーの色を更新
            UpdateHealthBarColor(_hpSlider.value / _hpSlider.maxValue);
            // 次のフレームまで待機
            yield return null;
        }
        _hpSlider.value = newHealth;
        UpdateHealthBarColor(_hpSlider.value / _hpSlider.maxValue);
    }

    private void UpdateHealthBarColor(float percentage)
    {
        // HPの割合に基づいて色を設定
        if (percentage > 0.5f)
            // HPが50%以上の場合は高HP色を設定
            _hpBarImage.color = highHealthColor;
        else if (percentage > 0.25f)
            // HPが25%以上50%未満の場合は中間HP色を設定
            _hpBarImage.color = mediumHealthColor;
        else
            // HPが25%未満の場合は低HP色を設定
            _hpBarImage.color = lowHealthColor;
    }

}
