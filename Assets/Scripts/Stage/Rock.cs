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
        // 衝突したオブジェクトがプレイヤーの場合
        if (other.gameObject.CompareTag("Player"))
        {
            // プレイヤーコントローラーを取得
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            // プレイヤーが憑依している状態か確認します。
            if (playerController != null)
            {
                if (playerController.isPossession)
                {
                    if (playerController.IsAttacking == true)
                    {
                        // 憑依しているエネミーがゴブリンの場合オブジェクトを破壊
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
            // SE再生
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
