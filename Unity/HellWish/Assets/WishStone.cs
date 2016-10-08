using UnityEngine;
using System.Collections;

public class WishStone : DraggableObject {


	// Use this for initialization
	void Start () {
	
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
