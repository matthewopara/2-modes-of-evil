using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMovement : MonoBehaviour
{
    public bool startLeft;
    private Vector2 startPos;
    private Vector2 endPos;
    private bool reverseDir;
    private float timer = 0f;
    private float marker = 0f;
    private GameObject player;
    public float yPosOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("MoveFog");
    }

    IEnumerator MoveFog()
    {
        while (true)
        {
            startPos = transform.position;
            SetEndPos();
            reverseDir = false;
            marker = 0f;
            timer = 0f;

            while (!reverseDir)
            {
                timer += Time.deltaTime;
                marker = timer / 3;

                if (marker > 1f)
                {
                    marker = 1f;
                }

                transform.position = Vector2.Lerp(startPos, endPos, marker);

                SetEndPos();

                transform.position = new Vector2(transform.position.x, player.transform.position.y);

                if (marker == 1f)
                {
                    reverseDir = true;
                    startLeft = !startLeft;
                }
                yield return null;
            }
        }
    }


    void SetEndPos()
    {
        if (startLeft)
        {
            endPos = new Vector2(player.transform.position.x + 5, player.transform.position.y + yPosOffset);
        }
        else
        {
            endPos = new Vector2(player.transform.position.x - 5, transform.position.y);
        }
    }
}
