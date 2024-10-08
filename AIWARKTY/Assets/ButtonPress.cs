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
    private Vector3 firstPosition;  // ��ư�� ���� ��ġ�� �����ϴ� ����

    // Start is called before the first frame update
    void Start()
    {
        buttonSound = GetComponent<AudioSource>();
        firstPosition = button.transform.localPosition;  // ��ư�� ���� ��ġ�� ����
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = firstPosition + new Vector3(0, -0.4f, 0);  // ������ ��ġ���� �Ʒ��� ���� �̵�
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
            button.transform.localPosition = firstPosition;  // ������ ��ġ�� �ǵ��ư�
            released.Invoke();
            isPressed = false;
        }
    }
}
