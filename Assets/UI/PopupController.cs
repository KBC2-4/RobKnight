using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupController : MonoBehaviour
{
    private VisualElement popup;
    private UIDocument _root;

    void Start()
    {
        // var visualTree = Resources.Load<VisualTreeAsset>("Tutor ialUXMLTemplate");
        // var styleSheet = Resources.Load<StyleSheet>("TutorialUSS");
        //
        // popup = visualTree.CloneTree();
        // popup.styleSheets.Add(styleSheet);
        // GetComponent<UIDocument>().rootVisualElement.Add(popup);
        //
        // // 初期状態は非表示
        // popup.transform.scale = Vector3.zero;
    }

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>();
    }

    public void ShowPopup()
    {
        // 0.5秒でスケールアップ
        StartCoroutine(AnimateScale(popup, Vector3.one, 0.5f));
        _root.transform.localScale= new Vector3(5,5,5);
        
    }

    private IEnumerator AnimateScale(VisualElement element, Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 startScale = element.transform.scale;

        while (time < duration)
        {
            time += Time.deltaTime;
            element.transform.scale = Vector3.Lerp(startScale, targetScale, time / duration);
            yield return null;
        }

        element.transform.scale = targetScale;
    }
}