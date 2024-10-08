using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    public GameObject RobotPrefab;
    public List<Transform> SpawnPoints;
    public float minSpawnDelay = 3f;
    public float maxSpawnDelay = 7f;
    
    public RobotController rC;




    public IEnumerator SpawnRobot()
    {


        while (true)
        {
            int randomIndex = Random.Range(0, SpawnPoints.Count);
            Transform spawnPoint = SpawnPoints[randomIndex];
            GameObject Robot = Instantiate(RobotPrefab, spawnPoint.position, spawnPoint.rotation);

            //�κ��� �κ� ��Ʈ�ѷ����� �������� ���Ӹ����� ��ũ��Ʈ�� ������ ���� �������ش�.
            rC = Robot.GetComponent<RobotController>();
            rC.gm = FindObjectOfType<GameMaster>();
            rC.gauge = FindObjectOfType<Gauge>();
            rC.aM = FindObjectOfType<AudioManagerKTY>();


            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
