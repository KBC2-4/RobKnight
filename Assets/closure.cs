using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class closure : MonoBehaviour
{
    [SerializeField]
    private Vector3 colliderSize; //�����ߗ̈�̃T�C�Y

    [SerializeField]
    private EnemyController guard; //�R�Â���G

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
            //�R�Â���ꂽ�G���|���ꂽ�Ƃ��A���̃I�u�W�F�N�g���폜����
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
        // Cylinder�̐���
        colliderObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        colliderObject.transform.position = transform.position;
        colliderObject.transform.SetParent(transform);
        colliderObject.transform.localScale = new Vector3(colliderSize.x, colliderSize.y, colliderSize.z);

        // Collider�I�u�W�F�N�g�̕`��͕s�v�Ȃ̂�Renderer������
        Destroy(colliderObject.GetComponent<MeshRenderer>());

        // ���X���݂���Collider���폜
        Collider[] colliders = colliderObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i]);
        }

        // ���b�V���̖ʂ��t�ɂ��Ă���MeshCollider��ݒ�
        var mesh = colliderObject.GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        colliderObject.AddComponent<MeshCollider>();

        //�q�I�u�W�F�N�g��L��������
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
