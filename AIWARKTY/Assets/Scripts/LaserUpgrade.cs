using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUpgrade : MonoBehaviour
{
    public bool LaserItem = false;
    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("������ ���׷��̵�");
            Destroy(gameObject);
            LaserItem = true;
        }
    }
}


