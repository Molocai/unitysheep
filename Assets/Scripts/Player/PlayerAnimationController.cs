using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : PlayerBase {

    public GameObject dashParticles;

    private Animator _anim;
    private Rigidbody _body;

	// Use this for initialization
	void Awake () {
        _anim = GetComponent<Animator>();
        _body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        _anim.SetFloat("MovementSpeed", _body.velocity.magnitude);
    }

    public void SetCharge(bool c)
    {
        _anim.SetBool("Charge", c);

        // Start the particles
        ParticleSystem.EmissionModule particles = dashParticles.GetComponent<ParticleSystem>().emission;
        particles.enabled = c;
    }
}
