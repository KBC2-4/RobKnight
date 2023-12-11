using System.Collections;
using UnityEngine;

public class BGMChanger : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource; // 再生に使用するAudioSource
    [SerializeField] AudioClip bgmClip; // 再生するBGM
    [Header("クロスフェード")]
    [SerializeField, Tooltip("クロスフェードをするには2つのAudioSourceが必要です。")]
    bool isCrossFade = false; // クロスフェードをするかのフラグ
    [SerializeField] AudioSource newBgmSource; // 新しいBGM用のAudioSource
    [Header("オプション")]
    [SerializeField, Range(0f, 30f)] float fadeTime = 1.0f; // フェードにかかる時間（秒）
    private bool isFadingIn = false; // フェードインされているかのフラグ

    public void FadeOut()
    {
        StartCoroutine(FadeMusic(false)); // フェードアウト開始
    }

    public void FadeIn(AudioClip newClip)
    {
        StartCoroutine(FadeMusic(true, newClip)); // フェードイン開始
    }

    public void ChangeBGM(AudioClip newClip)
    {

        // 現在再生中のBGMと新しいBGMが同じでない場合のみ、切り替え処理を行う
        if (bgmSource.clip != newClip)
        {
            if (isCrossFade)
            {
                if (newBgmSource.clip != newClip)
                {
                    StartCoroutine(CrossFade(newClip));
                }
            }
            else
            {
                // フェードイン・フェードアウトを行う
                StartCoroutine(FadeOutIn(newClip));
            }
        }
        else
        {
            // Debug.Log("同じBGMが既に再生中です。");
        }
    }

    private IEnumerator FadeMusic(bool fadeIn, AudioClip newClip = null)
    {
        // Debug.Log("フェード" + (fadeIn ? "イン" : "アウト"));

        isFadingIn = true;

        // フェード開始時の音量を保存
        float startVolume = bgmSource.volume;

        if (fadeIn)
        {
            // 新しいクリップを設定
            bgmSource.clip = newClip;
            // 新しいクリップを再生開始
            bgmSource.Play();
            // 音量を0に設定してフェードインを開始
            bgmSource.volume = 0f;    
        }

        // フェード処理にかかる時間を計測
        float time = 0;

        // フェード処理の実行
        while (time < fadeTime)
        {
            // 経過時間を加算
            time += Time.deltaTime;
            // フェードインの場合は音量を徐々に上げ、フェードアウトの場合は音量を徐々に下げる
            bgmSource.volume = fadeIn ? (time / fadeTime) * startVolume : (1 - time / fadeTime) * startVolume;
            // 次のフレームまで待機
            yield return null;
        }

        if (!fadeIn)
        {
            // フェードアウト後は音楽を停止
            bgmSource.Stop();
        }
        else
        {
            // フェードイン後は元の音量に戻す
            bgmSource.volume = startVolume;
        }

        isFadingIn = false;
    }

    private IEnumerator FadeOutIn(AudioClip newClip)
    {
        // フェードアウト
        float startVolume = bgmSource.volume;
        for (float time = 0; time < fadeTime; time += Time.deltaTime)
        {
            bgmSource.volume = (1 - time / fadeTime) * startVolume;
            yield return null;
        }

        // 新しいBGMを設定
        bgmSource.clip = newClip;
        bgmSource.Play();
        for (float time = 0; time < fadeTime; time += Time.deltaTime)
        {
            bgmSource.volume = (time / fadeTime) * startVolume;
            yield return null;
        }
    }

    private IEnumerator CrossFade(AudioClip newClip)
    {
        float currentTime = 0f;
        float startVolume = bgmSource.volume;

        // 新しいBGMをフェードイン用のAudioSourceで再生
        newBgmSource.clip = newClip;
        newBgmSource.Play();
        newBgmSource.volume = 0f;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            // 既存のBGMをフェードアウト
            bgmSource.volume = (1 - currentTime / fadeTime) * startVolume;
            // 新しいBGMをフェードイン
            newBgmSource.volume = (currentTime / fadeTime) * startVolume;
            yield return null;
        }

        // 既存のBGMの再生を停止し、新しいBGMに切り替え
        // bgmSource.Stop();
        bgmSource.clip = newBgmSource.clip; // 再生クリップを引継ぎ設定
        bgmSource.time = newBgmSource.time; // 再生位置を引継ぎ設定
        bgmSource.volume = newBgmSource.volume; // 音量を引継ぎ設定
        bgmSource.Play();

        // 新しいBGMのAudioSourceを停止
        newBgmSource.Stop();
        newBgmSource.clip = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Triggered by: " + other.gameObject.name);
        if (other.gameObject.tag == "Player" && !isFadingIn) // プレイヤーがトリガーに入ったか確認
        {
            // FadeOut();
            // FadeIn(bgmClip);
            ChangeBGM(bgmClip);
        }
    }
}
