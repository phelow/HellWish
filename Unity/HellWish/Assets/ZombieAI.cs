using UnityEngine;
using System.Collections;

public class ZombieAI : MonoBehaviour
{
    private float m_timeLeft;
    private GameObject m_player;
    [SerializeField]
    private LayerMask m_ignoreZombies;
    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private float m_searchDistance = 50.0f;
    [SerializeField]
    private float m_accelearation = 100.0f;

    [SerializeField]
    private float m_corpseDistance = 10.0f;

    [SerializeField]
    private GameObject mp_corpse;

    private float m_minTorque = -.5f;
    private float m_maxTorque = .5f;


    // Use this for initialization
    void Start()
    {
    }

    public void InitZombie(float time)
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_timeLeft = time;
        StartCoroutine(ZombieAIRoutine());
    }

    private IEnumerator ZombieAIRoutine()
    {
        while (m_timeLeft > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            m_timeLeft -= Time.deltaTime;

            //See if player is in range
            RaycastHit2D hit = Physics2D.Raycast(transform.position, m_player.transform.position - transform.position, m_searchDistance, m_ignoreZombies);


            //If he is chase him
            if (hit != null && hit.collider != null && hit.collider.tag == "Player")
            {
                MoveTo(m_player);
                continue;
            }

            Corpse bestCorpse = null;

            //Look for a corpse within range with a higher for rating then your corpse
            foreach (GameObject corpse in GameObject.FindGameObjectsWithTag("Corpse"))
            {
                hit = Physics2D.Raycast(transform.position, corpse.transform.position - transform.position, Vector2.Distance(transform.position, corpse.transform.position), m_ignoreZombies);

                if (hit != null && hit.collider != null && hit.collider.tag == "Corpse")
                {
                    Corpse thisCorpse = corpse.GetComponent<Corpse>();

                    if (bestCorpse == null)
                    {
                        bestCorpse = thisCorpse;
                        continue;
                    }

                    if (thisCorpse.GetScore() > bestCorpse.GetScore())
                    {
                        bestCorpse = thisCorpse;
                    }

                }
            }

            //Move towards it if it exists
            if (bestCorpse != null && Vector2.Distance(bestCorpse.transform.position, this.transform.position) < m_corpseDistance)
            {
                MoveTo(bestCorpse.gameObject);
                continue;
            }


            //Dynamic Wander
            m_rigidbody.AddTorque(Random.Range(m_minTorque, m_maxTorque));
            m_rigidbody.AddForce(transform.up * m_accelearation * Time.deltaTime);
        }
        //Drop a corpse and die
        GameObject.Instantiate(mp_corpse, transform.position, transform.rotation, null);
        Destroy(this.gameObject);
    }

    public void MoveTo(GameObject go)
    {
        m_rigidbody.AddForce((go.transform.position - this.transform.position).normalized * m_accelearation * Time.deltaTime);
        Debug.DrawRay(transform.position, (go.transform.position - this.transform.position).normalized * m_accelearation * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
