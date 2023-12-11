using UnityEngine;

public class TitleLogoManager : MonoBehaviour
{
    // public static TitleLogoManager Instance;

    [SerializeField] private Animator _titleLogoAnimator; // タイトルロゴのAnimator
    private bool _hasAnimatedOnce = false;

    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        // このオブジェクトをシーン遷移で破棄しないように設定
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        // 重複するインスタンスは破棄
    //        Destroy(gameObject);
    //    }
    //}

    void Start()
    {
        Debug.Log(_hasAnimatedOnce);
        if (!_hasAnimatedOnce)
        {
            //titleLogoAnimator.Play("TitleLogoAnimation");
            _hasAnimatedOnce = true;
        }
        else
        {
            // アニメーションがない、ロゴの固定位置を示すステート
            //titleLogoAnimator.Play("TitleLogoStatic");
            _titleLogoAnimator.SetTrigger("StaticTrigger");
        }
    }
}