using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JealousyPathFollower : MonoBehaviour
{
    private Node[] pathNodes;
    public GameObject[] bossParts;
    private Vector2[] startPositions;
    public float moveSpeed;
    private int currentLastNode = 1;
    private float timer;

    private Vector2 dir;
    private float rot;

    [HideInInspector]
    public Vector3[] currentPositionHolders;

    //NEW VARIABLE
    [HideInInspector]
    public bool leaping = true;

    //[HideInInspector]
    public bool endJump = false;

    //NEW VARIABLES 27-31
    public int startOrder;
    public SpriteRenderer[] bossRenderers;

    public bool movingLeft;
    private Vector2 scale;

    public bool isIdlePath;
    private Vector3 resetPos;

    public bool isDeadAnim;
    private WaitForSeconds pause;

    public GameObject deadJealousy;

    // Start is called before the first frame update
    void Awake()
    {
        pathNodes = GetComponentsInChildren<Node>();
        currentPositionHolders = new Vector3[bossParts.Length];
        startPositions = new Vector2[bossParts.Length];
        resetPos = new Vector3(-50f, 0f, 0f);
        pause = new WaitForSeconds(2.5f);
    }

    void OnEnable()
    {
        for (int i = 0; i < bossParts.Length; i++)
        {
            bossParts[i].transform.position = pathNodes[i].transform.position;
        }

        if (movingLeft)
        {
            Flip();
        }

        foreach (SpriteRenderer r in bossRenderers)
        {
            r.sortingOrder = startOrder;
        }

        CheckNode();
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime * moveSpeed;

        //if the last boss part is not at the correct position
        if (leaping)
        {
            if (bossParts[bossParts.Length - 1].transform.position != currentPositionHolders[bossParts.Length - 1])
            {
                for (int i = 0; i < bossParts.Length; i++)
                {
                    if (bossParts[i].transform.position != currentPositionHolders[i])
                    {
                        bossParts[i].transform.position = Vector3.Lerp(startPositions[i], currentPositionHolders[i], timer);
                    }
                }
            }
            else
            {
                currentLastNode++;
                CheckNode();
            }
        }
    }

    void CheckNode()
    {
        timer = 0f;

        for (int i = 0; i < bossParts.Length; i++)
        {
            startPositions[i] = bossParts[i].transform.position;
        }

        if (currentLastNode < pathNodes.Length)
        {
            for (int i = 0; i < bossParts.Length; i++)
            {
                if (bossParts[bossParts.Length - 1].transform.position != pathNodes[pathNodes.Length - 1].transform.position)
                {
                    currentPositionHolders[i] = pathNodes[i + currentLastNode].transform.position;
                    dir = (Vector2)currentPositionHolders[i] - startPositions[i];
                    rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    bossParts[i].transform.rotation = Quaternion.Euler(0f, 0f, rot);
                }
            }
        }
        else
        {
            Debug.Log("End");
            endJump = true;
            leaping = false;

            foreach (GameObject part in bossParts)
            {
                part.transform.position = resetPos;
            }

            if (isDeadAnim)
            {
                //StartCoroutine("Disintegrate");
                deadJealousy.SetActive(true);
                gameObject.SetActive(false);
            }


            if (movingLeft)
            {
                Flip();
            }

            if (isIdlePath)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        currentLastNode = 1;
        endJump = false;
        leaping = true;
        timer = 0f;
    }

    void Flip()
    {
        foreach (GameObject part in bossParts)
        {
            scale = part.transform.localScale;
            scale.y *= -1;
            part.transform.localScale = scale;
        }
    }

    IEnumerator Disintegrate()
    {

        yield return pause;
        Debug.Log("Died");
        //summon particle system in shape on sprite
        //change positions of bossParts
    }
}
