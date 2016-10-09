using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {

    private const float m_minSpawnInterval = 10.0f;
    private const float m_maxSpawnInterval = 20.0f;

    [SerializeField]
    private GameObject m_zombie;

    private float m_zombieLifeTime = 25.0f;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(SpawnZombies());

    }
    

    private IEnumerator SpawnZombies()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(m_minSpawnInterval, m_maxSpawnInterval));

            GameObject go = (GameObject.Instantiate(m_zombie, transform.position, transform.rotation, null) as GameObject);
            go.GetComponent<ZombieAI>().InitZombie(m_zombieLifeTime);

            go.transform.rotation = new Quaternion(go.transform.rotation.x, go.transform.rotation.y, Random.Range(0.0f, 360.0f),go.transform.rotation.w);

            m_zombieLifeTime += 5.0f;
        }
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
