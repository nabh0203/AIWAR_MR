using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class basketBall : MonoBehaviour
{
    public GameObject ball;
    public UnityEvent goal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ball)
        {
            goal.Invoke();
        }
    }
}
