using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private AudioSource _audioSorce;
    [SerializeField] private AudioClip _audioClip;

    void Start()
    {
        _audioSorce = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider other)
    {
        // �Փ˂����I�u�W�F�N�g���v���C���[�̏ꍇ
        if (other.gameObject.CompareTag("Player"))
        {
            // �v���C���[�R���g���[���[���擾
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            // �v���C���[���߈˂��Ă����Ԃ��m�F���܂��B
            if (playerController != null)
            {
                if (playerController.isPossession)
                {
                    if (playerController.IsAttacking == true)
                    {
                        // �߈˂��Ă���G�l�~�[���S�u�����̏ꍇ�I�u�W�F�N�g��j��
                        DestroyObject();
                        //Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void DestroyObject()
    {

        if (_audioSorce != null && _audioClip != null)
        {
            // SE�Đ�
            _audioSorce.PlayOneShot(_audioClip);
        }
        

        var random = new System.Random();
        var min = -3;
        var max = 3;
        gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r => {
            r.isKinematic = false;
            r.transform.SetParent(null);
            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        });
        Destroy(gameObject);
    }
}
