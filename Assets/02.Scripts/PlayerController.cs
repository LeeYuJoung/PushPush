using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimation;

    Ray ray;
    RaycastHit hit;
    RaycastHit hit2;

    public float currentTime = 0;
    public float animCoolTime = 0.25f;

    void Start()
    {
        playerAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentTime = 0;
            CheckOthers(Vector3.right);
            playerAnimation.SetBool("MOVE", true);
            playerAnimation.SetFloat("WALKX", 1);
            playerAnimation.SetFloat("WALKY", 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentTime = 0;
            CheckOthers(Vector3.left);
            playerAnimation.SetBool("MOVE", true);
            playerAnimation.SetFloat("WALKX", -1);
            playerAnimation.SetFloat("WALKY", 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentTime = 0;
            CheckOthers(Vector3.up);
            playerAnimation.SetBool("MOVE", true);
            playerAnimation.SetFloat("WALKY", 1);
            playerAnimation.SetFloat("WALKX", 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentTime = 0;
            CheckOthers(Vector3.down);
            playerAnimation.SetBool("MOVE", true);
            playerAnimation.SetFloat("WALKY", -1);
            playerAnimation.SetFloat("WALKX", 0);
        }

        if(currentTime > animCoolTime)
        {
            currentTime = 0;
            playerAnimation.SetBool("MOVE", false);
        }
    }

    public void CheckOthers(Vector3 dir)
    {
        GameManager.Instance().CheckBackPosition();

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
