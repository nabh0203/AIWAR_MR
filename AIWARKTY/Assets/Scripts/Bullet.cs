using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 2000.0f; // 여기에서 원하는 스피드 값을 지정할 수 있습니다.
    public bool Laser;
    public LaserUpgrade LaserItem;
    public GameObject Hit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Robot") || other.CompareTag("Laser"))
        {
            Destroy(gameObject);
            GameObject projectile = Instantiate(Hit, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(projectile, 1f);
        }
    }
}
