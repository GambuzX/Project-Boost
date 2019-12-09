using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float thrustSpeed = 1f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelClearSound;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    enum State {
        ALIVE = 0,
        DYING,
        TRANSCENDING
    }

    State state = State.ALIVE;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = mainEngineSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.ALIVE) {
            HandleThrust();
            HandleRotation();
        }
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

        if (state != State.ALIVE) return;

        switch(collision.transform.tag) {
            case "Friendly":
                Debug.Log("Friendly");
                break;

            case "Finished":
                state = State.TRANSCENDING;
                audioSource.clip = levelClearSound;
                audioSource.Play();
                Invoke("LoadSecondLevel", 1f);
                break;

            case "Fuel":
                Debug.Log("Fuel");            
                break;

            default:
                state = State.DYING;
                audioSource.clip = deathSound;
                audioSource.Play();
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadSecondLevel() {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel() {
        SceneManager.LoadScene(0);
    }
}
