﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    [Range(0,1), SerializeField] float movementFactor;

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) return;

        movementFactor = (Mathf.Sin(2 * Mathf.PI * Time.time / period) + 1) / 2;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
