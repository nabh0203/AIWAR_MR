using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�ٽý���");
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("��¥�ٽý���");
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        
    }
}
