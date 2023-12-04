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

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (guard.isDeath)
        {
            //�R�Â���ꂽ�G���|���ꂽ�Ƃ��A���̃I�u�W�F�N�g���폜����
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
