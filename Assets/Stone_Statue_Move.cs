using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Statue_Move : MonoBehaviour
{

	public float pushPower = 2.0f;

	// Start is called before the first frame update
	void Start()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("MaleCharacterPolyart");
        playerController = obj.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

	void OnCollisionEnter(Collision collision)
	{

		Vector3 pushDir = new Vector3(collision.transform.position.x, 0, collision.transform.position.z);

        GetComponent<Rigidbody>().velocity = pushDir * pushPower;
	}

   
}
