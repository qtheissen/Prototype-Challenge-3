using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip boinkSound;
    public bool isLowEnough = true;
    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && isLowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }
        // if balloon goes above 15.0f, unable to press spacebar
        if (gameObject.transform.position.y >= 15.0f)
        {
            isLowEnough = false;
        }
        // if balloon is below 15.0f, able to press spacebar
        if (gameObject.transform.position.y <= 15.0f)
        {
            isLowEnough = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
        
        // If player collides with floor, it bounces up with boink sound
        else if (other.gameObject.CompareTag("floor"))
        {
            playerAudio.PlayOneShot(boinkSound, 1.0f);
            playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }
}
