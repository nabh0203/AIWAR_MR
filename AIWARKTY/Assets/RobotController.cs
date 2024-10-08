using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{

    private Animator animator;
    private bool Item = true;
    public Transform Itempos;
    public Gauge gauge;
    public GameMaster gm;
    public AudioManagerKTY aM;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.gameObject.CompareTag("Floor"))
        {
            StartCoroutine(Shoot());
        }
        */

        //���⿡ �Ѿ� ���� �ۼ�.
        //�Ѿ� ���� ��
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (Item)
            {
                Item = false;
            }
            //death �ִϸ��̼� Ŵ
            animator.SetBool("Death", true);
            //3�� �Ŀ� �κ� �����
            Destroy(gameObject, 3f);
            gauge.GaugeFill();
            aM.PlaySfx(AudioManagerKTY.Sfx.Robot);

        }
    }

}
