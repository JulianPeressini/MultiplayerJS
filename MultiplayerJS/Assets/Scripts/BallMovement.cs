using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    private Rigidbody2D self;
    private NetworkIdentity selfIdentity;

    [SerializeField] private float ballSpeed;

    public NetworkIdentity SelfIdentity { get { return selfIdentity; } }

    void Start()
    {
        self = GetComponent<Rigidbody2D>();
        selfIdentity = GetComponent<NetworkIdentity>();
    }

    void Update()
    {

    }

    public void Awake()
    {
        Invoke("SetRandomDir", 2);
    }

    private void SetRandomDir()
    {

        float rmd = Random.Range(0, 2);
        float rmdAngle = Random.Range(0, 2);
        float rmdDir;

        if (rmdAngle < 1)
        {
            rmdDir = Random.Range(15, 20);
        }
        else
        {
            rmdDir = Random.Range(-15, -20);
        }

        if (rmd < 1)
        {
            self.AddForce(new Vector2((ballSpeed), rmdDir));
        }
        else
        {
            self.AddForce(new Vector2(-ballSpeed, rmdDir));
        }
    }

    public void ResetBall()
    {
        self.velocity = Vector2.zero;
        transform.position = new Vector2(0, 0);
        Invoke("SetRandomDir", 2);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player1") || coll.collider.CompareTag("Player2"))
        {
            Vector2 newVelocity;
            newVelocity.x = self.velocity.x;
            newVelocity.y = (self.velocity.y / 2) + (coll.collider.attachedRigidbody.velocity.y);
            self.velocity = newVelocity;
        }
    }
}
