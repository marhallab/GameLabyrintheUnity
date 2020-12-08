using UnityEngine;
using System.Collections;
using Assets.MazeGenerator._scriptRA;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//<summary>
//Ball movement controlls and simple third-person-style camera
//</summary>
public class RollerBall : MonoBehaviour
{

    public GameObject ViewCamera1 = null;
    public AudioClip JumpSound = null;
    public AudioClip HitSound = null;
    public AudioClip CoinSound = null;
    public AudioClip BoiteSound = null;

    private Rigidbody mRigidBody = null;
    private AudioSource mAudioSource = null;
    private int nombreCoins = 2;
    public TextMeshProUGUI textNb;
    public Button button;
    public Boolean finished = false;

    void Start()
    {
        mRigidBody = GetComponent<Rigidbody>();
        mAudioSource = GetComponent<AudioSource>();
       
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameFinished() { 
            button.gameObject.SetActive(true);
            button.onClick.AddListener(RestartGame);
            finished = true;
    }

    void FixedUpdate()
    {

        if (nombreCoins == 0) {
            gameFinished();
        }

        textNb.SetText("Il reste " + nombreCoins.ToString() + " coins");
        if (mRigidBody != null && finished.Equals(false))
        {
            if (Input.GetButton("Horizontal"))
            {
                mRigidBody.AddTorque(Vector3.back * Input.GetAxis("Horizontal") * 10);
            }
            if (Input.GetButton("Vertical"))
            {
                mRigidBody.AddTorque(Vector3.right * Input.GetAxis("Vertical") * 10);
            }
            if (Input.GetButtonDown("Jump"))
            {
                if (mAudioSource != null && JumpSound != null)
                {
                    mAudioSource.PlayOneShot(JumpSound);
                }
                mRigidBody.AddForce(Vector3.up * 200);
            }
            if (Input.GetKey(KeyCode.J))
            {
                mRigidBody.AddForce(Vector3.right * 10);
            }
            if (Input.GetKey(KeyCode.K))
            {
                mRigidBody.AddForce(Vector3.left * 10);
            }
            if (Input.GetKey(KeyCode.I))
            {
                mRigidBody.AddForce(Vector3.forward * 10);
            }
            if (Input.GetKey(KeyCode.M))
            {
                mRigidBody.AddForce(Vector3.back * 10);
            }
        }
        if (ViewCamera1 != null)
        {
            Vector3 direction = (Vector3.up * 2 + Vector3.back) * 2;
            RaycastHit hit;
            Debug.DrawLine(transform.position, transform.position + direction, Color.red);
            if (Physics.Linecast(transform.position, transform.position + direction, out hit))
            {
                ViewCamera1.transform.position = hit.point;
            }
            else
            {
                ViewCamera1.transform.position = transform.position + direction;
            }
            ViewCamera1.transform.LookAt(transform.position);
        }

    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag.Equals("Floor"))
        {
            if (mAudioSource != null && HitSound != null && coll.relativeVelocity.y > .5f)
            {
                mAudioSource.PlayOneShot(HitSound, coll.relativeVelocity.magnitude);
            }
        }
        else 
        {
            if (mAudioSource != null && HitSound != null && coll.relativeVelocity.magnitude > 2f)
            {
                mAudioSource.PlayOneShot(HitSound, coll.relativeVelocity.magnitude);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Coin"))
        {
            if (mAudioSource != null && CoinSound != null)
            {
                mAudioSource.PlayOneShot(CoinSound);
                nombreCoins--;

            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag.Equals("Boite"))
        {
            if (mAudioSource != null && BoiteSound != null)
            {
                mAudioSource.PlayOneShot(BoiteSound);
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag.Equals("Destroy")) {
            gameFinished();
        }
    }
}
