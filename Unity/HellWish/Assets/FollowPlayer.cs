using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D m_rigidbody;
    [SerializeField]
    private GameObject m_target;

    [SerializeField]
    private float m_cameraForce = 100.0f;

    [SerializeField]
    private float m_minDistance = 1.0f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(transform.position, m_target.transform.position);

        Debug.Log(distance);

        if (distance < m_minDistance)
        {
            return;
        }

        Vector3 dir = m_target.transform.position - transform.position;
        
        m_rigidbody.AddForce(new Vector2(dir.x, dir.y).normalized * Time.deltaTime * m_cameraForce * distance);
	}
}
