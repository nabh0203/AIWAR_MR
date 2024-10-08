using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Controller : MonoBehaviour
{
    public int speedFoward = 12; //���� �ӵ�
    public int speedSide = 12; //������ �ӵ�
    private Transform tr; //�÷��̾� Ʈ������
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
    // �÷��̾� �̵� ����
    void MovePlayer()
    {
        float dirX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        float dirZ = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        //�̵� ���� ���� �� �̵�
        Vector3 moveDir = new Vector3(dirX * speedSide, 0, dirZ * speedFoward);
        transform.Translate(moveDir * Time.smoothDeltaTime);
    }
}