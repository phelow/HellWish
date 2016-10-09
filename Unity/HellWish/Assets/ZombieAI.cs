using UnityEngine;
using System.Collections;

public class ZombieAI : MonoBehaviour
{
    [SerializeField]
    private GameObject mp_explosion;
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

    [SerializeField]
    private float m_turnProbability = 90.0f;

    private GameObject m_curSeekingCorpse;
    [SerializeField]
    private GameObject m_curSeekingPosition;

    [SerializeField]
    private float m_minPositionOffset = 5.0f;

    [SerializeField]
    private GameObject mp_draggable;

    [SerializeField]
    private Sprite[] mp_sprites;

    [SerializeField]
    private GameObject m_spriteGameObject;

    [SerializeField]
    private GameObject[] m_powerUps;

    [SerializeField]
    private float m_spawnProbability = 90;

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    [SerializeField]
    private LayerMask m_draggableMask;
   
    // Use this for initialization
    void Start()
    {
        m_spriteRenderer.sprite = mp_sprites[Random.Range(0, mp_sprites.Length)];
        m_rigidbody.gravityScale = 0.0f;
    }

    public void InitZombie(float time)
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_timeLeft = time;
        StartCoroutine(ZombieAIRoutine());
    }

    private IEnumerator ZombieAIRoutine()
    {
        float tmp = m_timeLeft;
        for (int j = 0; j < 2; j++)
        {
            m_timeLeft = 5.0f;

            transform.rotation = new Quaternion(0, 0, Random.Range(0.0f, 360.0f), 0.0f);
            while (m_timeLeft > 0.0f)
            {
                yield return new WaitForEndOfFrame();
                m_timeLeft -= Time.deltaTime;
                m_rigidbody.AddForce(transform.up * m_accelearation * Time.deltaTime * 3.0f);
            }
            yield return new WaitForEndOfFrame();
        }
        m_timeLeft = tmp;
        while (m_timeLeft > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            m_timeLeft -= Time.deltaTime;
            RaycastHit2D hit;
            if (PlayerController.GetInvisible() == false)
            {
                //See if player is in range
                hit = Physics2D.Raycast(transform.position, m_player.transform.position - transform.position, m_searchDistance, m_ignoreZombies);


                //If he is chase him
                if (hit != null && hit.collider != null && hit.collider.tag == "Player")
                {
                    MoveTo(m_player);
                    continue;
                }
            }
            Corpse bestCorpse = null;

            //Look for a corpse within range with a higher for rating then your corpse
            foreach (GameObject corpse in GameObject.FindGameObjectsWithTag("Corpse"))
            {
                hit = Physics2D.Raycast(transform.position, corpse.transform.position - transform.position, 
                    m_searchDistance, m_ignoreZombies);

                Debug.DrawRay(transform.position, (corpse.transform.position - transform.position)* m_searchDistance,
                    Color.red,.1f);

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

            if (bestCorpse != null)
            {
                MoveTo(bestCorpse.gameObject);
                continue;
            }
        }

        for (int j = 0; j < 2; j++)
        {
            m_timeLeft = 5.0f;

            transform.rotation = new Quaternion(0, 0, Random.Range(0.0f, 360.0f), 0.0f);
            while (m_timeLeft > 0.0f)
            {
                yield return new WaitForEndOfFrame();
                m_timeLeft -= Time.deltaTime;
                m_rigidbody.AddForce(transform.up * m_accelearation * Time.deltaTime * 3.0f);
            }
            yield return new WaitForEndOfFrame();
        }

        if(Random.Range(0,100) > m_spawnProbability)
        {
            GameObject.Instantiate(this.m_powerUps[Random.Range(0, this.m_powerUps.Length)], new Vector2(transform.position.x + Random.Range(-10.0f, 10.0f), transform.position.y + Random.Range(-10.0f,10.0f)), transform.rotation, null);

        }

        //Drop a corpse and die
        GameObject.Instantiate(mp_corpse, transform.position, transform.rotation, null);
        GameObject.Instantiate(mp_draggable, transform.position, transform.rotation, null);
        DestroyZombie();
    }

    public void DestroyZombie()
    {


        GameObject.Instantiate(mp_explosion, transform.position, transform.rotation, null); ;

        Destroy(this.gameObject);
    }

    public void MoveTo(GameObject go)
    {
        m_rigidbody.AddForce((go.transform.position - this.transform.position).normalized * m_accelearation * Time.deltaTime);
        Debug.DrawRay(transform.position, (go.transform.position - this.transform.position).normalized * m_accelearation * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rigidbody.velocity.magnitude > 0.1f)
        {
            float angle = (Mathf.Atan2(m_rigidbody.velocity.y, m_rigidbody.velocity.x) + 90) * Mathf.Rad2Deg;
            m_spriteGameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
