using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPress : MonoBehaviour
{
    public GameObject button;
    public UnityEvent pressed;
    public UnityEvent released;
    private GameObject presser;
    private AudioSource buttonSound;
    private bool isPressed = false;
    private Vector3 firstPosition;  // 버튼의 원래 위치를 저장하는 변수

    // Start is called before the first frame update
    void Start()
    {
        buttonSound = GetComponent<AudioSource>();
        firstPosition = button.transform.localPosition;  // 버튼의 원래 위치를 저장
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = firstPosition + new Vector3(0, -0.4f, 0);  // 원래의 위치에서 아래로 조금 이동
            presser = other.gameObject;
            pressed.Invoke();
            buttonSound.Play();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = firstPosition;  // 원래의 위치로 되돌아감
            released.Invoke();
            isPressed = false;
        }
    }
}
