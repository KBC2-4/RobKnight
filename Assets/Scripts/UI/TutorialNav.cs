using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialNav : MonoBehaviour
{
    //最初にフォーカスするゲームオブジェクト
    [SerializeField] private GameObject NextP2;
    [SerializeField] private GameObject NextP1;

    public GameObject p1;
    public GameObject p2;
    public GameObject playercanvas;

    private PlayerController player;

    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
    }

    public void OnPushButtonP1()
    {
        EventSystem.current.SetSelectedGameObject(NextP2);
        p1.SetActive(true);
        p2.SetActive(false);
        playercanvas.SetActive(false);
    }
    public void OnPushButtonP2()
    {
        p1.SetActive(false);
        p2.SetActive(true);
        playercanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(NextP1);
    }
    public void OnPushButtonFin()
    {
        p1.SetActive(false);
        p2.SetActive(false);
        playercanvas.SetActive(true);
        player?.SetInputAction(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        OnPushButtonP1();

        player?.SetInputAction(false);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
