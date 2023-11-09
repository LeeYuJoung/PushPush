using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimation;

    Ray ray;
    RaycastHit hit;
    RaycastHit hit2;

    void Start()
    {
        playerAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            CheckOthers(Vector3.right);
            playerAnimation.SetBool("MOVE", true);
            playerAnimation.SetFloat("WALKX", 1);
            playerAnimation.SetFloat("WALKY", 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CheckOthers(Vector3.left);
            playerAnimation.SetBool("MOVE", true);
            playerAnimation.SetFloat("WALKX", -1);
            playerAnimation.SetFloat("WALKY", 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CheckOthers(Vector3.up);
            playerAnimation.SetBool("MOVE", true);
            playerAnimation.SetFloat("WALKY", 1);
            playerAnimation.SetFloat("WALKX", 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CheckOthers(Vector3.down);
            playerAnimation.SetBool("MOVE", true);
            playerAnimation.SetFloat("WALKY", -1);
            playerAnimation.SetFloat("WALKX", 0);
        }
        else
        {
            playerAnimation.SetBool("MOVE", false);
        }
    }

    public void CheckOthers(Vector3 dir)
    {
        if (Physics.Raycast(transform.position,transform.TransformDirection(dir), out hit, 1.0f))
        {
            Transform tr = hit.collider.transform;

            switch (hit.collider.tag)
            {
                case "Ball":
                    if (Physics.Raycast(tr.position, tr.TransformDirection(dir), out hit2, 1.0f))
                    {
                        switch (hit2.collider.tag)
                        {
                            case "Wall":
                                Debug.Log("Wall Check");
                                break;
                            case "Ball":
                                Debug.Log("Ball Check");
                                break;
                            default:
                                transform.Translate(dir);
                                tr.Translate(dir);
                                GameManager.Instance().CheckBallPosition();
                                break;
                        }
                    }
                    else
                    {
                        transform.Translate(dir);
                        tr.Translate(dir);
                        GameManager.Instance().CheckBallPosition();
                    }
                    break;
                case "Wall":
                    break;
                default:
                    transform.Translate(dir);
                    GameManager.Instance().CheckBallPosition();
                    break;
            }
        }
        else
        {
            transform.Translate(dir);
            GameManager.Instance().CheckBallPosition();
        }
    }
}
