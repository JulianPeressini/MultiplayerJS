using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogic : MonoBehaviour
{
    [SerializeField] string opposingPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Ball")
        {
            ScoreManager.Instance.AddScore(opposingPlayer);
            other.GetComponent<BallMovement>().ResetBall();
        }
    }
}
