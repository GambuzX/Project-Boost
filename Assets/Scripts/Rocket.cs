﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float thrustSpeed = 1f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelClearSound;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem explosionParticles;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    private bool collisionsEnabled;

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
        collisionsEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.ALIVE) {
            HandleThrust();
            HandleRotation();
        }

        if (Debug.isDebugBuild) {
            HandleDebugKeys();
        }
    }

    private void HandleThrust() {
        
        if(Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up * Time.deltaTime * thrustSpeed);
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            audioSource.Play();
            mainEngineParticles.Play();
        }
        if(Input.GetKeyUp(KeyCode.Space)) {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
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

    private void HandleDebugKeys() {
        if(Input.GetKey(KeyCode.L)) {
            LoadNextLevel();
        }

        if(Input.GetKey(KeyCode.C)) {
            collisionsEnabled = !collisionsEnabled;
        }
    }

    void OnCollisionEnter(Collision collision) {

        if (state != State.ALIVE || !collisionsEnabled) return;

        switch(collision.transform.tag) {
            case "Friendly":
                Debug.Log("Friendly");
                break;

            case "Finished":
                state = State.TRANSCENDING;
                winParticles.Play();
                audioSource.clip = levelClearSound;
                audioSource.Play();
                Invoke("LoadNextLevel", levelLoadDelay);
                break;

            case "Fuel":
                Debug.Log("Fuel");            
                break;

            default:
                state = State.DYING;
                explosionParticles.Play();
                audioSource.clip = deathSound;
                audioSource.Play();
                Invoke("LoadFirstLevel", levelLoadDelay);
                break;
        }
    }

    private void LoadSecondLevel() {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex+1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
