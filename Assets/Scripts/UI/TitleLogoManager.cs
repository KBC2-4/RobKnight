using UnityEngine;

public class TitleLogoManager : MonoBehaviour
{
    public static TitleLogoManager Instance;

    public Animator titleLogoAnimator; // タイトルロゴのAnimator
    private bool hasAnimatedOnce = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // このオブジェクトをシーン遷移で破棄しないように設定
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 重複するインスタンスは破棄
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!hasAnimatedOnce)
        {
            titleLogoAnimator.Play("TitleLogoAnimation");
            hasAnimatedOnce = true;
        }
        else
        {
            // アニメーションがない、ロゴの固定位置を示すステート
            titleLogoAnimator.Play("TitleLogoStatic");
        }
    }
}