using UnityEngine;
using System.Collections;

public class RotateOverTime : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D m_rb;
	// Use this for initialization
	void Start () {
        m_rb.AddTorque(10.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
