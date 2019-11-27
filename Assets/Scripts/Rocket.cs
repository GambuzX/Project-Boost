using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] float thrustSpeed = 1f;
    [SerializeField] float rotationSpeed = 1f;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    void Update()
    {
        HandleThrust();
        HandleRotation();
    }

    private void HandleThrust() {
        if(Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up * Time.deltaTime * thrustSpeed);
        }

        if(Input.GetKeyDown(KeyCode.Space)) audioSource.Play();
        if(Input.GetKeyUp(KeyCode.Space)) audioSource.Stop();        
    }

    private void HandleRotation() {
        // take manual control of rotation
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
        }
        else if(Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back * Time.deltaTime * rotationSpeed);
        }

        // resume physics control of rotation
        rigidBody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision) {
        switch(collision.transform.tag) {
            case "Friendly":
                Debug.Log("Friendly");
                break;

            case "Fuel":
                Debug.Log("Fuel");            
                break;

            default:
                Debug.Log("Dead");
                break;
        }
    }
}
