using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcThrust = 200f;
    [SerializeField] float mainThrust = 50f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip goalSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem goalParticles;

    [SerializeField] enum State { Alive, Transcending, Dead };

    Rigidbody rigidBody;
    AudioSource audioSource;

    State state = State.Alive;
    bool collisionDisabled = false;
    float loadLevelDelay = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if( state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
            RespondToEscInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        } 
    }

    private void RespondToEscInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(3);
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionDisabled) return;
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Ok");
                break;
            case "Finish":
                StartGoalSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartGoalSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(goalSound);
        goalParticles.Play();
        Invoke("Load", loadLevelDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("Load", loadLevelDelay);
    }

    private void Load()
    {
        if (state == State.Transcending || state == State.Alive)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            print(nextSceneIndex);
            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }
            SceneManager.LoadScene(nextSceneIndex);
        } else if (state == State.Dead)
        {
            mainEngineParticles.Stop();
            SceneManager.LoadScene(0);
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) //Thrusting
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying && state == State.Alive)
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
        
    }

    private void RespondToRotateInput()
    {
        float rotationThisFrame = rcThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) //Rotating left
        {
            RotateManually(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) //Rotating right
        {
            RotateManually(-rotationThisFrame);
        }
        
    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidBody.freezeRotation = false;
    }
}
