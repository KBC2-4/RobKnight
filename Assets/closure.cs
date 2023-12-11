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

    private Vector3 spawnpos = Vector3.zero;

    private void Start()
    {
        spawnpos = guard.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (guard.isDeath)
        {
            //紐づけられた敵が倒されたとき、このオブジェクトを削除する
            Destroy(this.gameObject);
        }

        Vector3 AreaPos = transform.position;
        Vector3 EnemyPos = guard.transform.position;
        float AreaX = colliderSize.x / 2 + 3;
        float AreaY = colliderSize.y / 2 + 3;
        float AreaZ = colliderSize.z / 2 + 3;

        if ((AreaPos.x + AreaX < EnemyPos.x || EnemyPos.x < AreaPos.x - AreaX) ||
            (AreaPos.y + AreaY < EnemyPos.y || EnemyPos.y < AreaPos.y - AreaY) ||
            (AreaPos.z + AreaZ < EnemyPos.z || EnemyPos.z < AreaPos.z - AreaZ)) 
        {
            guard.transform.position = spawnpos;
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
