using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
    public Vector3 offset;
    public Vector3 angle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            Vector3 newPosition = new Vector3();
            newPosition = target.transform.position + offset;

            Vector3 newRotation = new Vector3();
            newRotation = angle;

            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(newRotation);
        }
	}
}
