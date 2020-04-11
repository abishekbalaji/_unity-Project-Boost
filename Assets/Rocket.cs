﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        InputBinding();
    }

    private void InputBinding()
    {
        if (Input.GetKey(KeyCode.Space)) //Thrusting
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            } 
        } else
        {
            audioSource.Stop();
        }
        if (Input.GetKey(KeyCode.A)) //Rotating left
        {
            transform.Rotate(Vector3.forward);
        } else if (Input.GetKey(KeyCode.D)) //Rotating right
        {
            transform.Rotate(-Vector3.forward);
        }
    }
}
