using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballScript : MonoBehaviour
{

    public GameScript gameScript;
    public new Rigidbody2D rigidbody;
    public Transform containerTransform;
    public new ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule emission;

    public float maxSpeed, maxAccel, maxJumpPower, minSize, maxSize, growth, rotationFactor, pitchFactor, volumeChange;

    private bool grounded;
    private bool jumping;

    private float goalSpeed;
    private float speed;
    private float topSpeed;
    private float accel;
    private float size;
    private float jumpPower;

    private AudioSource moveSound, jumpSound, landSound, resetSound, hurtSound, goalSound;

    void Awake() {
        goalSpeed = 0;
        speed = 0;
        size = minSize;
        transform.parent.localScale = new Vector3(size, size, 1);
        emission = particleSystem.emission;

        moveSound = SoundScript.GetSound("Move");
        jumpSound = SoundScript.GetSound("Jump");
        landSound = SoundScript.GetSound("Land");
        resetSound = SoundScript.GetSound("Reset");
        hurtSound = SoundScript.GetSound("Hurt");
        goalSound = SoundScript.GetSound("Goal");
    }

    void Start()
    {
        moveSound.Play();
    }

    void Update() {
        if(Input.GetKeyDown("r")) {
            resetSound.Play();
            gameScript.Reset();
        }
    // }

    // void FixedUpdate()
    // {
        if(!jumping) {
            float dist = speed * Time.fixedDeltaTime * Time.fixedDeltaTime;
            size = Mathf.Sqrt(size*size + Mathf.Abs(dist)*growth);
            float agility = minSize / size;
            accel = maxAccel * agility;
            topSpeed = maxSpeed * agility;
            jumpPower = maxJumpPower * agility;
            
            transform.localEulerAngles += new Vector3(0, 0, -dist / (size * Mathf.PI) * 360f * rotationFactor);
        }

        goalSpeed = Input.GetAxisRaw("Horizontal")  * topSpeed;
        speed += Mathf.Clamp(goalSpeed - speed, -accel, accel);
        float ySpeed = rigidbody.velocity.y;

        if(grounded && (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Jump") > 0.5f)) {
            ySpeed = jumpPower;
            jumping = true;
            if(!jumpSound.isPlaying) jumpSound.Play();
        }

        float pitch = Mathf.Abs(speed) * Time.fixedDeltaTime * pitchFactor;
        float goalVolume = size*4f;
        bool rolling = pitch > 0.1f && grounded;
        if(rolling) {
            moveSound.pitch = pitch;
        } else {
            goalVolume = 0;
        }
        moveSound.volume += Mathf.Clamp(goalVolume - moveSound.volume, -volumeChange*Time.fixedDeltaTime, volumeChange*Time.fixedDeltaTime);

        emission.enabled = rolling;
        transform.parent.localScale = new Vector3(size, size, 1);
        rigidbody.velocity = new Vector2(speed, ySpeed);
        //containerTransform.Translate(new Vector3(speed, 0, 0), Space.World);
    }

    public void SetGrounded(bool val) {
        grounded = val;
        if(grounded) {
            jumping = false;
            if(!landSound.isPlaying) landSound.Play();
            particleSystem.Emit(5);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch (other.tag) {
            case "Goal":
                goalSound.Play();
                gameScript.NextLevel();
                break;
            case "Harm":
                emission.enabled = false;
                particleSystem.Emit(5);
                particleSystem.transform.parent = null;
                Destroy(particleSystem.gameObject, 1f);
                
                hurtSound.Play();
                gameScript.Reset();
                break;
        }
    }
}
