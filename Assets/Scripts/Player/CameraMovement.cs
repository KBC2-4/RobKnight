using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum State
    {
        Follow,
        Shake
    }

    /// <summary>
    /// �v���C���[��Transform
    /// </summary>
    [SerializeField]
    [Tooltip("�Ǐ]���������^�[�Q�b�g")]
    private GameObject target;

    /// <summary>
    /// �^�[�Q�b�g�ƃJ�����̑��Έʒu
    /// </summary>
    public Vector3 offset;

    private Vector3 _centerPosition;
    private Vector3 _shakePower;

    private State _cameraState;
    public State CameraState
    {
        get => _cameraState;
        set=> _cameraState = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraState = State.Follow;

        //�J�����̈ʒu��ݒ�
        Vector3 position = target.transform.position;
        position.z -= 5;
        position.y += 10;
        gameObject.transform.position = position;


        //�Q�[���J�n���ɃJ�����ƃ^�[�Q�b�g�̋������擾
        offset = gameObject.transform.position - target.transform.position;
        _centerPosition = transform.position;
        _shakePower.x = 0.5f;
    }

    private void LateUpdate()
    {
        switch (_cameraState)
        {
            case State.Follow:
                //�^�[�Q�b�g�̈ʒu�ɃJ������Ǐ]������
                gameObject.transform.position = target.transform.position + offset;
                break;
            case State.Shake:
                ShakeCamera();
                break;
            default:
                break;
        }
        
    }

    /// <summary>
    /// �J�����̃^�[�Q�b�g��؂�ւ���
    /// </summary>
    /// <param name="newTarget">�V���Ƀ^�[�Q�b�g�ɂ������I�u�W�F�N�g</param>
    public void SetCameraTarget(GameObject newTarget)
    {
        target = newTarget;

        //�J�����̈ʒu��ݒ�
        Vector3 position = target.transform.position;
        position.z -= 5;
        position.y += 10;
        gameObject.transform.position = position;

        //�J�����ƃ^�[�Q�b�g�̋������擾
        offset = gameObject.transform.position - target.transform.position;
    }

    public void ShakeCamera()
    {
        transform.position = _centerPosition + _shakePower;
        _shakePower *= -1;
    }
}
