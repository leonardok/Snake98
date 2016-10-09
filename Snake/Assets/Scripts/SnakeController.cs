using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BodyPart : MonoBehaviour 
{
    public Vector2 m_direction;
    public Vector3 m_position;
    GameObject m_prefab;
    public GameObject m_instance;

    public BodyPart(Vector2 dir, Vector2 pos, GameObject fab)
    {
        m_direction = dir;
        m_position = new Vector3(pos.x, pos.y, 0);
        m_prefab = fab;
    }

    public void instantiate()
    {
        m_instance = Instantiate(m_prefab, m_position, Quaternion.identity) as GameObject;
    }

    public void changePosition(Vector2 newPos)
    {
        m_instance.transform.position = new Vector3(newPos.x, newPos.y, 0);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Game Over");
    }
}

public class Snake
{
    BodyPart m_head;
    List<BodyPart> m_parts;
    Vector2 m_headPosition;
    Vector2 m_lastPartPosition;
    bool has_to_add_part = false;
    GameObject m_body_prefab;


    // Constructor
    public Snake(GameObject body_prefab)
    {
        m_parts = new List<BodyPart>();
        m_body_prefab = body_prefab;
    }

    public void addHead(GameObject head, Vector2 pos)
    {
        Vector2 dir = new Vector2(0, 1);

        m_head = new BodyPart(dir, pos, head);
        m_head.instantiate();


    }

    public void increaseSnakeSize()
    {
        has_to_add_part = true;
    }

    public void addBody(GameObject body, Vector2 pos)
    {
        Vector2 dir = new Vector2(0, 1);

        BodyPart p = new BodyPart(dir, pos, body);
        p.instantiate();
        m_parts.Add(p);
    }

    public void walk()
    {
        Vector2 oldPos = m_head.m_instance.transform.position;
        Vector2 newPos = new Vector2(m_head.m_position.x + 0.64f * m_head.m_direction.x,
                                     m_head.m_position.y + 0.64f * m_head.m_direction.y);
        m_head.m_position = newPos;
        m_head.changePosition(m_head.m_position);

        // for each body part:
        //   - move to last piece position
        foreach (BodyPart p in m_parts)
        {
            newPos = oldPos;
            oldPos = p.m_instance.transform.position;

            p.changePosition(newPos);
        }

        if(has_to_add_part == true)
        {
            addBody(m_body_prefab, newPos);
            has_to_add_part = false;
        }
    }

    public void turnLeft()
    {
        m_head.m_direction = Quaternion.Euler(0, 0, 90) * m_head.m_direction;
        m_head.m_instance.transform.Rotate(new Vector3(0, 0, 90));
        debugDirection();
    }

    public void turnRight()
    {

        m_head.m_direction = Quaternion.Euler(0, 0, -90) * m_head.m_direction;
        m_head.m_instance.transform.Rotate(new Vector3(0, 0, -90));
        debugDirection();
    }

    public Vector2 direction()
    {
        return m_head.m_direction;
    }

    void debugDirection()
    {
        Debug.Log(m_head.m_direction);
    }

    public Vector2 getPosition()
    {
        return new Vector2(m_head.m_instance.transform.position.x, m_head.m_instance.transform.position.y);
    }
}

public class SnakeController : MonoBehaviour {
    public float updateDelta = 1;
    public Vector2 direction;
    public GameObject m_head;
    public GameObject m_body;
    public GameObject camera;
    public int numberOfParts = 3;
    public float increaseVelocityBy = 0.02f;

    private float m_updateIn;
    private Snake m_snake;
    private bool m_paused = false;
    
    private Camera m_camera;

    private DebugUI debugUI;

    // Use this for initialization
    void Start () {
        m_updateIn = updateDelta;
        m_snake = new Snake(m_body);
        m_camera = camera.GetComponent<Camera>();

        // set the debug component
        debugUI = GameObject.Find("_GM").GetComponent<DebugUI>();

        m_snake.addHead(m_head, new Vector2(0, 0));

        // Add 3 body parts
        for (int i = 0; i < numberOfParts; i++ )
        {            
            m_snake.addBody(m_body, new Vector2(0, (i+1) * -0.64f));
        }
    }
    
    // Update is called once per frame
    void Update () {
        // This pauses the snake.
        // used on game over
        if (m_paused == true)
            return;

        m_updateIn -= Time.deltaTime;

        if (m_updateIn <= 0)
        {
            this.m_snake.walk();
            m_updateIn = updateDelta;
        }

        checkInput();
    }

    public void increaseSnakeSize()
    {
        m_snake.increaseSnakeSize();
    }

    public void decreaseUpdateTime()
    {
        updateDelta -= increaseVelocityBy;
    }

    public void pauseGame()
    {
        m_paused = true;
    }

    void checkInput()
    {
        Vector2 up =    new Vector2( 0f,  1f);
        Vector2 left =  new Vector2(-1f,  0f);
        Vector2 right = new Vector2( 1f,  0f);

        /*
         * Keybord Controls
         */
        if(Input.GetKeyDown("left"))
        {
            debugUI.debug("KeyDown - Left");
            m_snake.turnLeft();
        }

        else if(Input.GetKeyDown("right"))
        {
            debugUI.debug("KeyDown - Right");
            m_snake.turnRight();
        }

        Vector2 snake_position = m_camera.WorldToScreenPoint(m_snake.getPosition());
        // Debug.Log(snake_position);

        /*
         * Touch controls
         */
        for (var i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {                
                Vector2 difference_vector = new Vector2(touch.position.x - snake_position.x, touch.position.y - snake_position.y);

                // hold the head direction
                Vector2 direction = m_snake.direction();

                if (difference_vector.x > 0 && difference_vector.y > 0)
                {
                    // First Quad
                    debugUI.debug("First Quad");

                    // if its going UP or LEFT, turn right
                    if (direction == up || direction == left) m_snake.turnRight();
                    else m_snake.turnLeft();
                }
                else if (difference_vector.x < 0 && difference_vector.y > 0 )
                {
                    // Second quad
                    debugUI.debug("Second Quad");

                    // if right or up, turn right
                    if (direction == up || direction == right) m_snake.turnLeft();
                    else m_snake.turnRight();
                }
                else if (difference_vector.x < 0 && difference_vector.y < 0)
                {
                    // Third quad
                    debugUI.debug("Third Quad");

                    // if up or left turn left
                    if (direction == up || direction == left) m_snake.turnLeft();
                    else m_snake.turnRight();
                }
                else
                {
                    // Fourth quad
                    debugUI.debug("Fourth Quad");

                    // if right or up, turn right
                    if (direction == up || direction == right) m_snake.turnRight();
                    else m_snake.turnLeft();
                }

                /*


                if (touch.position.x > (Screen.width / 2))
                {
                    if (touch.position.y > Screen.height / 2)
                    {
                        // First Quadrant                        

                        // if its going UP or LEFT, turn right
                        if (direction == up || direction == left) m_snake.turnRight();
                        else m_snake.turnLeft();

                    }
                    else
                    {
                        // Fourth quadrant

                        // if right or up, turn right
                        if (direction == up || direction == right) m_snake.turnRight();
                        else m_snake.turnLeft();
                    }
                }
                else
                {
                    if (touch.position.y > Screen.height / 2)
                    {
                        // Second Quadrant

                        // if up or right turn left
                        if (direction == up || direction == right) m_snake.turnLeft(); 
                        else m_snake.turnRight();
                    }
                    else
                    {
                        // Third quadrant

                        // if up or left turn left
                        if (direction == up || direction == left) m_snake.turnLeft();
                        else m_snake.turnRight();
                    }
                }
                 * */
            }
        }
    }
}
