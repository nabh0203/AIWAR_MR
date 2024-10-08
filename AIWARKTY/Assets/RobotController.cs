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

        //여기에 총알 로직 작성.
        //총알 닿을 시
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (Item)
            {
                Item = false;
            }
            //death 애니메이션 킴
            animator.SetBool("Death", true);
            //3초 후에 로봇 사라짐
            Destroy(gameObject, 3f);
            gauge.GaugeFill();
            aM.PlaySfx(AudioManagerKTY.Sfx.Robot);

        }
    }

}
