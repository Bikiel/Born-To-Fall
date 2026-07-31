using UnityEngine;


public class PlayerFx : MonoBehaviour
{

    public Rigidbody2D playerRb;




    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    //public AudioClip yellSound;
    //public AudioClip crashSound;
    //private AudioSource playerAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        playerRb = GetComponent<Rigidbody2D>();


        //playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //playerRb.rotation = Quaternion.Euler(0, 0, 0);
            playerRb.MoveRotation(0);

            /*if (!playerAudio.isPlaying)
            {
                //playerAudio.PlayOneShot(crashSound, 1.0f);
            }*/
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //isOnGround = true;

        if (collision.gameObject.CompareTag("Ground"))
        {

            dirtParticle.Play();
            //playerAudio.PlayOneShot(yellSound, 1.0f);



        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {

            playerRb.constraints = ~RigidbodyConstraints2D.FreezePositionY;
            /*if (!playerAudio.isPlaying)
            {
                //playerAudio.PlayOneShot(crashSound, 1.0f);
            }*/
            explosionParticle.Play();
            //dirtParticle.Stop();

        }


    }

}
