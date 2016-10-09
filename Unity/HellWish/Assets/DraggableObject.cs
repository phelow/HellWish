using UnityEngine;
using System.Collections;

public class DraggableObject : MonoBehaviour {
   

    [SerializeField]
    private Sprite[] m_sprites;

    [SerializeField]
    protected SpriteRenderer m_spriteRenderer;

    protected bool m_inRange = false;
	// Use this for initializati0on
	void Start () {
        Draggable();
    }

    protected void Draggable()
    {
        int i = Random.Range(0, m_sprites.Length);
        m_spriteRenderer.sprite = m_sprites[i];
        this.gameObject.AddComponent<BoxCollider2D>();
        Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        float dragDistance = this.gameObject.GetComponent<BoxCollider2D>().size.magnitude * .6f;

        CircleCollider2D cd = this.gameObject.AddComponent<CircleCollider2D>();

        cd.radius = dragDistance;
        cd.isTrigger = true;

        rb.useAutoMass = true;
        rb.gravityScale = 0.0f;

        StartCoroutine(DestroyLater());
    }

    private IEnumerator DestroyLater()
    {
        yield return new WaitForSeconds(Random.Range(100.0f, 500.0f));
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController.SaySpace();
            m_spriteRenderer.color = Color.red;
            m_inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.DontSaySpace();
            m_spriteRenderer.color = Color.white;
            m_inRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpringJoint2D springjoint = this.GetComponent<SpringJoint2D>();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(springjoint != null)
            {
                Destroy(springjoint);
                return;
            }

            if(m_inRange)
            {
                SpringJoint2D sj = this.gameObject.AddComponent<SpringJoint2D>();
                sj.connectedBody = PlayerController.GetPlayerRigidBody();
                m_spriteRenderer.color = Color.blue;
            }
            return;
        }



	}
}
