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

        // HP�̒l���ύX���ꂽ���m�F
        if (_hpSlider.value != _enemyController.enemyData.hp)
        {
            // HP�̍X�V
            StartCoroutine(ChangeHealth(_enemyController.enemyData.hp));
        }

        // �G�l�~�[��HP��0�ɂȂ����ꍇ�AHP�o�[���\���ɂ���
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
            // ��ɃJ�����Ɠ��������ɐݒ�
            transform.rotation = mainCamera.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Main Camera������܂���B\nMain Camera�^�O��ݒ肵�Ă�������");
        }
    }

    private IEnumerator ChangeHealth(float newHealth)
    {
        float preChangeHealth = _hpSlider.value;
        float elapsed = 0f;

        // ���X��HP���X�V
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            // ���`��ԂŊ��炩��HP���X�V
            _hpSlider.value = Mathf.Lerp(preChangeHealth, newHealth, elapsed / updateSpeedSeconds);
            // HP�o�[�̐F���X�V
            UpdateHealthBarColor(_hpSlider.value / _hpSlider.maxValue);
            // ���̃t���[���܂őҋ@
            yield return null;
        }
        _hpSlider.value = newHealth;
        UpdateHealthBarColor(_hpSlider.value / _hpSlider.maxValue);
    }

    private void UpdateHealthBarColor(float percentage)
    {
        // HP�̊����Ɋ�Â��ĐF��ݒ�
        if (percentage > 0.5f)
            // HP��50%�ȏ�̏ꍇ�͍�HP�F��ݒ�
            _hpBarImage.color = highHealthColor;
        else if (percentage > 0.25f)
            // HP��25%�ȏ�50%�����̏ꍇ�͒���HP�F��ݒ�
            _hpBarImage.color = mediumHealthColor;
        else
            // HP��25%�����̏ꍇ�͒�HP�F��ݒ�
            _hpBarImage.color = lowHealthColor;
    }

}
