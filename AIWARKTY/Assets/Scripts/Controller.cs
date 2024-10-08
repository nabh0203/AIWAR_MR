using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Controller : MonoBehaviour
{
    public int speedFoward = 12; //전진 속도
    public int speedSide = 12; //옆걸음 속도
    private Transform tr; //플레이어 트랜스폼
                          // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
    // 플레이어 이동 구현
    void MovePlayer()
    {
        float dirX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        float dirZ = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        //이동 방향 설정 후 이동
        Vector3 moveDir = new Vector3(dirX * speedSide, 0, dirZ * speedFoward);
        transform.Translate(moveDir * Time.smoothDeltaTime);
    }
}