using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public GameObject bullet; //�߻��� �Ѿ� 
    public Transform firePos; //�߻��� �Ѿ� ��ġ 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void BulletFire()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("h");
            Instantiate(bullet, firePos.position, firePos.rotation);
        }
    }
    // Update is called once per frame
    void Update()
    {
        BulletFire();  
    }
}
