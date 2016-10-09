using UnityEngine;
using System.Collections;

public class WishStone : DraggableObject {


	// Use this for initialization
	void Start ()
    {
        this.gameObject.AddComponent<BoxCollider2D>();
        Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        float dragDistance = this.gameObject.GetComponent<BoxCollider2D>().size.magnitude * 2.0f;

        CircleCollider2D cd = this.gameObject.AddComponent<CircleCollider2D>();

        cd.radius = dragDistance;
        cd.isTrigger = true;

        rb.useAutoMass = true;
        rb.gravityScale = 0.0f;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Nexus")
        {
            coll.gameObject.GetComponent<Nexus>().GrantAWish();
            Destroy(this.gameObject);
        }
    }
}
