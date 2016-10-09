using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour {
    private SpawnStuff spawner;
    private ScoreManager scoreBoard;
    private SnakeController snake;

	// Use this for initialization
	void Start () {
        spawner = GameObject.Find("_GM").GetComponent<SpawnStuff>();
        scoreBoard = GameObject.Find("_GM").GetComponent<ScoreManager>();
        snake = GameObject.Find("_GM").GetComponent<SnakeController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Game Over by collistion");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Fruit")
        {
            Debug.Log("Got point!");
            Destroy(other.gameObject);

            spawner.fruit.hasFruitDeployed = false;
            scoreBoard.IncreaseScore(10);
            snake.increaseSnakeSize();
            snake.decreaseUpdateTime();
        }
        else
        {
            Debug.Log("Game Over by trigger");
            scoreBoard.gameOver();
        }
    }
}
