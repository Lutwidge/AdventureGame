using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float forwardMovement = Input.GetAxis("Vertical");
        float lateralMovement = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(lateralMovement, 0.0f, forwardMovement);

        GetComponent<Rigidbody>().AddForce(direction);

        transform.forward = direction;

        GetComponent<Animator>().SetFloat("Speed", Mathf.Sqrt(forwardMovement * forwardMovement + lateralMovement * lateralMovement));
    }
}
