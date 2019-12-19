using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    private Rigidbody2D self;
    private NetworkIdentity selfIdentity;
    private NetworkTransform selfTransform;
    [SerializeField] private float speed;
    [SerializeField] private float boundY;
    private float horiPos;

    [SerializeField] private GameObject mainBody;
    [SerializeField] private GameObject bottomSpiker;
    [SerializeField] private GameObject topmostSpiker;

    private BallData data;

    private bool teamJoined;
    private bool canPick;

    public bool TeamJoined { get { return teamJoined; } }
    public bool CanPick { set { canPick = value; } }
    public NetworkIdentity SelfIdentity { get { return selfIdentity; } }

    void Awake() 
    {
        self = GetComponent<Rigidbody2D>();
        selfIdentity = GetComponent<NetworkIdentity>();
        selfTransform = GetComponent<NetworkTransform>();
    }

    void Start()
    {
        data = new BallData();
        data.position = new Position();
        data.direction = new Position();
    }

    void Update() 
    {
        if (selfIdentity.HasControl)
        {
            Vector2 velocity = self.velocity;

            if (Input.GetKey(KeyCode.W))
            {
                velocity.y = speed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                velocity.y = -speed;
            }
            else
            {
                velocity.y = 0;
            }

            Vector2 desiredPos = new Vector2(transform.position.x, transform.position.y);

            if (desiredPos.y > boundY)
            {
                desiredPos.y = boundY;
            }
            else if (desiredPos.y < -boundY)
            {
                desiredPos.y = -boundY;
            }

            self.velocity = velocity;

            transform.position = desiredPos;

            if (!teamJoined && canPick)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    selfTransform.assignTeam("Pink");
                    teamJoined = true;              
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    selfTransform.assignTeam("Cyan");
                    teamJoined = true;
                }
            } 
        } 
    }

    public void SetPlayer(string team)
    {
        if (team == "Cyan")
        {
            mainBody.GetComponent<SpriteRenderer>().color = new Color(0, 0.9607843f, 1);
            bottomSpiker.GetComponent<SpriteRenderer>().color = new Color(0, 0.9607843f, 1);
            topmostSpiker.GetComponent<SpriteRenderer>().color = new Color(0, 0.9607843f, 1);
            transform.position = new Vector3(15, 0, 0);
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (team == "Pink")
        {
            mainBody.GetComponent<SpriteRenderer>().color = new Color(1, 0.2039216f, 0.6117647f);
            bottomSpiker.GetComponent<SpriteRenderer>().color = new Color(1, 0.2039216f, 0.6117647f);
            topmostSpiker.GetComponent<SpriteRenderer>().color = new Color(1, 0.2039216f, 0.6117647f);
            transform.position = new Vector3(-15, 0, 0);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}