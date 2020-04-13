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
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return;
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
        Invoke("Load", 1f);
    }

    private void StartDeathSequence()
    {
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("Load", 1f);
    }

    private void Load()
    {
        if (state == State.Transcending)
        {
            SceneManager.LoadScene(1);
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
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying && state == State.Alive)
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
        
    }

    private void RespondToRotateInput()
    {
        float rotationThisFrame = rcThrust * Time.deltaTime;
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A)) //Rotating left
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) //Rotating right
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
    }
}
