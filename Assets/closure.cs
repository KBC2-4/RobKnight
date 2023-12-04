using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class closure : MonoBehaviour
{
    [SerializeField]
    private Vector3 colliderSize; //閉じ込め領域のサイズ

    [SerializeField]
    private EnemyController guard; //紐づける敵

    private bool isActivated = false;
    private GameObject colliderObject;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (guard.isDeath)
        {
            //紐づけられた敵が倒されたとき、このオブジェクトを削除する
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider c)
    {
        if (!isActivated && c.gameObject.CompareTag("Player"))
        {
            isActivated = true;
            CreateInverseCollider();
        }
    }
    private void CreateInverseCollider()
    {
        // Cylinderの生成
        colliderObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        colliderObject.transform.position = transform.position;
        colliderObject.transform.SetParent(transform);
        colliderObject.transform.localScale = new Vector3(colliderSize.x, colliderSize.y, colliderSize.z);

        // Colliderオブジェクトの描画は不要なのでRendererを消す
        Destroy(colliderObject.GetComponent<MeshRenderer>());

        // 元々存在するColliderを削除
        Collider[] colliders = colliderObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i]);
        }

        // メッシュの面を逆にしてからMeshColliderを設定
        var mesh = colliderObject.GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        colliderObject.AddComponent<MeshCollider>();

        //子オブジェクトを有効化する
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
