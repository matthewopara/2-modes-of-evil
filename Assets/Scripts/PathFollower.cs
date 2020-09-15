using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    private Node[] pathNodes;
    public GameObject boss;
    public float moveSpeed;
    private float timer;
    private int currentNode = 1;
    private Vector2 startPosition;

    private Vector2 dir;
    private float rot;

    [HideInInspector]
    public Vector3 currentPositionHolder;

    [HideInInspector]
    public bool leaping = true;

    //[HideInInspector]
    public bool endJump = false;

    public int startOrder;
    private SpriteRenderer[] bossRenderer;

    public bool movingLeft;
    private Vector2 scale;

    //change leaping to false when done editing code

    void Awake()
    {
        pathNodes = GetComponentsInChildren<Node>();
        bossRenderer = boss.GetComponentsInChildren<SpriteRenderer>();
    }

    void OnEnable()
    {
        boss.transform.position = pathNodes[0].transform.position;

        if (movingLeft)
        {
            Flip();
        }

        foreach (SpriteRenderer r in bossRenderer)
        {
            r.sortingOrder = startOrder;
        }

        CheckNode();
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime * moveSpeed;

        if (leaping)
        {
        if (boss.transform.position != currentPositionHolder)
        {
            boss.transform.position = Vector3.Lerp(startPosition, currentPositionHolder, timer);
        }
        else
        {
            currentNode++;
            CheckNode();
        }
        }
    }

    void CheckNode()
    {
        timer = 0f;
        startPosition = boss.transform.position;

        if (currentNode < pathNodes.Length)
        {
            currentPositionHolder = pathNodes[currentNode].transform.position;
            dir = (Vector2)currentPositionHolder - startPosition;
            rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            boss.transform.rotation = Quaternion.Euler(0f, 0f, rot);
        }
        else
        {
            Debug.Log("Reached The End");
            endJump = true;
            leaping = false;

            if (movingLeft)
            {
                Flip();
            }
        }
    }

    private void OnDisable()
    {
        currentNode = 1;
        endJump = false;
        leaping = true;
        timer = 0f;
    }

    void Flip()
    {
        scale = boss.transform.localScale;
        scale.y *= -1;
        boss.transform.localScale = scale;
    }
}
