using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("다시시작");
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("진짜다시시작");
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        
    }
}
