using UnityEngine;
using System.Collections;

public class ZombieAI : MonoBehaviour
{
    private float m_timeLeft;
    private GameObject m_player;
    [SerializeField]
    private LayerMask m_ignoreZombies;
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
    private SpriteRenderer m_spriteRenderer;

    [SerializeField]
    private LayerMask m_draggableMask;

    // Use this for initialization
    void Start()
    {
        m_spriteRenderer.sprite = mp_sprites[Random.Range(0, mp_sprites.Length)];
        gameObject.AddComponent<BoxCollider2D>();
        m_rigidbody = gameObject.GetComponent<Rigidbody2D>();
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
                hit = Physics2D.Raycast(transform.position, corpse.transform.position - transform.position, 
                    Vector2.Distance(transform.position, corpse.transform.position), m_ignoreZombies);

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
            }

            if (Random.Range(0, m_turnProbability) > 90)
            {
                //Dynamic Wander
                m_rigidbody.AddTorque(Random.Range(m_minTorque, m_maxTorque));
            }

            m_rigidbody.AddForce(transform.up * m_accelearation * Time.deltaTime);
        }

        //Drop a corpse and die
        GameObject.Instantiate(mp_corpse, transform.position, transform.rotation, null);
        GameObject.Instantiate(mp_draggable, transform.position, transform.rotation, null);
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

        float angle = (Mathf.Atan2(m_rigidbody.velocity.y, m_rigidbody.velocity.x) + 90) * Mathf.Rad2Deg;
        m_spriteGameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
