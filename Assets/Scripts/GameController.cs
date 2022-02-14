using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController Player { get; set; }
    public BallController[] Balls { get; set; }

    public bool EndOfGame { get => Balls.Length < 1 || Player == null; }

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerController>();
        Balls = FindObjectsOfType<BallController>();
    }

    // Update is called once per frame
    void Update()
    {
        Balls = FindObjectsOfType<BallController>();

        if (EndOfGame)
        {
            Debug.Log("End");
            Debug.Log(Player.Points);
            Destroy(gameObject);
        }
    }
}
