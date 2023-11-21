using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithPlayer : MonoBehaviour
{
    public Transform playerTransform;

    void Update()
    {
        Vector3 playerRotation = playerTransform.eulerAngles;
        transform.eulerAngles = new Vector3(0, 0, -playerRotation.y);
    }
}
