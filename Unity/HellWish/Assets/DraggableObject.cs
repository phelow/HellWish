using UnityEngine;
using System.Collections;

public class DraggableObject : MonoBehaviour {

    protected float ms_dragDistance = .5f;

    [SerializeField]
    private Sprite[] m_sprites;

    [SerializeField]
    protected SpriteRenderer m_spriteRenderer;
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

        ms_dragDistance = this.gameObject.GetComponent<BoxCollider2D>().size.magnitude * .7f;

        rb.useAutoMass = true;
        rb.gravityScale = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        SpringJoint2D springjoint = this.GetComponent<SpringJoint2D>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(springjoint != null)
            {
                Destroy(springjoint);
                return;
            }

            if(Vector2.Distance(PlayerController.GetPlayerPosition(), this.transform.position) < ms_dragDistance)
            {
                SpringJoint2D sj = this.gameObject.AddComponent<SpringJoint2D>();
                sj.connectedBody = PlayerController.GetPlayerRigidBody();
                
            }
            return;
        }



	}
}
