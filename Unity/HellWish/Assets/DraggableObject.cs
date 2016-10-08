using UnityEngine;
using System.Collections;

public class DraggableObject : MonoBehaviour {

    private const float ms_dragDistance = 3.0f;

	// Use this for initializati0on
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpringJoint2D springjoint = this.GetComponent<SpringJoint2D>();
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
        }
	}
}
