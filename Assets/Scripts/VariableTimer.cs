using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableTimer : MonoBehaviour
{
    private float eventTime;

    public float variableDelay;
    public bool isRecurring = false;
    public string methodName;

    private void Awake()
    {
        eventTime = float.PositiveInfinity;
    }

    private void Start()
    {
        StartTimer(variableDelay);
    }

    private void Update()
    {
        if (Time.time >= eventTime)
        {
            Invoke(methodName, 0);
            if (isRecurring)
            {
                StartTimer(variableDelay);
            }
        }
    }

    public void StartTimer(float delay)
    {
        eventTime = Time.time + delay;
    }
}
