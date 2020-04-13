using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 finalPosVector;
    [SerializeField] float period = 2f;

    float movementFactor;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Protection against period being 0
        if (period <= Mathf.Epsilon) return;
        //set movement factor
        float cycles = Time.time / period; //Increases continuously from 0
        const float tau = Mathf.PI * 2f;
        float rawSineWave = Mathf.Sin(cycles * tau); //Goes from -1 to +1

        movementFactor = rawSineWave / 2f + 0.5f;
        Vector3 offset = movementFactor * finalPosVector;
        transform.position = startingPos + offset;
    }
}
