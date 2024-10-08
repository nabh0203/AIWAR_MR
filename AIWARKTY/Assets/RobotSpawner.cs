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

            //로봇과 로봇 컨트롤러에게 게이지와 게임마스터 스크립트를 변수를 통해 참조해준다.
            rC = Robot.GetComponent<RobotController>();
            rC.gm = FindObjectOfType<GameMaster>();
            rC.gauge = FindObjectOfType<Gauge>();
            rC.aM = FindObjectOfType<AudioManagerKTY>();


            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
