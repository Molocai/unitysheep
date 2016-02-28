using UnityEngine;
using System.Collections;

public class PlayerSoundController : PlayerBase {

    public AudioClip[] footSteps;
    public float timeBetweenSteps = 0.2f;

    private Rigidbody _body;
    private AudioSource _as;

    private float nextSound = 0f;

	// Use this for initialization
	void Start () {
        _body = GetComponent<Rigidbody>();
        _as = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update () {
        {
            if (!PlayerController.isDashing && _body.velocity.magnitude > 0.5f && Time.time >= nextSound)
            {
                AudioClip fs = footSteps[Random.Range(0, footSteps.Length)];
                _as.PlayOneShot(fs);

                nextSound = Time.time + timeBetweenSteps;
            }
        }
	}
}
