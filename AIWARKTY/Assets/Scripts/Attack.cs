using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public GameObject bullet; //발사할 총알 
    public Transform firePos; //발사할 총알 위치 
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
