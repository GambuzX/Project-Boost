using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    public float thrustSpeed = 1f;
    public float rotationSpeed = 1f;

    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }
    private void ProcessInput() {
        if(Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up * Time.deltaTime * thrustSpeed);
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
        }
        else if(Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back * Time.deltaTime * rotationSpeed);
        }
    }
}
