using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EmreIlkay.Utility;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 1;
    int numOfSides = 4; //number of sides ex: 4 for Cubes, 6 for Hexagons etc.


    public Direction moveDirection;

    [HideInInspector] public Vector3 targetPosition, startingPosition;

    public bool isAlive = true;



    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((targetPosition - transform.position).magnitude > .01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * MoveSpeed);
        }
        else
        {
            transform.position = targetPosition;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2))
            {
                GridElement element = hit.transform.GetComponent<GridElement>();
                if (element)
                {
                    if (element.MyType == GridElementType.Empty)
                    {
                        element.SetAsTrail();
                    }
                }
            }
            GetNewTarget();
        }

        if (InputManager.instance.ActiveInput != Direction.Nan)
        {
            RaycastHit hit;
            GridElement element;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2))
            {
                element = hit.transform.GetComponent<GridElement>();
                if (element)
                {
                    //element.transform.position += Vector3.up*3;
                    if (element.MyType != GridElementType.ColorFilled)
                    {
                        if (moveDirection == Direction.Up && InputManager.instance.ActiveInput == Direction.Down) { InputManager.instance.ActiveInput = Direction.Nan; return; }
                        if (moveDirection == Direction.Down && InputManager.instance.ActiveInput == Direction.Up) { InputManager.instance.ActiveInput = Direction.Nan; return; }
                        if (moveDirection == Direction.Left && InputManager.instance.ActiveInput == Direction.Right) { InputManager.instance.ActiveInput = Direction.Nan; return; }
                        if (moveDirection == Direction.Right && InputManager.instance.ActiveInput == Direction.Left) { InputManager.instance.ActiveInput = Direction.Nan; return; }

                    }
                }
            }

            moveDirection = InputManager.instance.ActiveInput;
            InputManager.instance.ActiveInput = Direction.Nan;
        }

    }

    void GetNewTarget()
    {
        Vector3 dir;
        switch (moveDirection)
        {
            case Direction.Up: dir = Vector3.forward; break;
            case Direction.Down: dir = Vector3.back; break;
            case Direction.Left: dir = Vector3.left; break;
            case Direction.Right: dir = Vector3.right; break;
            default: return;
        }
        dir += Vector3.down * .5f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 2))
        {
            /*if(hit.transform.CompareTag("Enemy"))
            {
                Die();
                return;
            }*/
            GridElement element = hit.transform.GetComponent<GridElement>();
            if (element)
            {
                if (element.MyType == GridElementType.Empty || element.MyType == GridElementType.ColorFilled)
                {
                    targetPosition = element.transform.position + (Vector3.up);
                }
                else if (element.MyType == GridElementType.TrailFill)
                {
                    Die();
                }

                if (element.MyType == GridElementType.Wall || element.MyType == GridElementType.ColorFilled)
                {
                    if (element.MyType == GridElementType.Wall) moveDirection = Direction.Nan;
                    if (Physics.Raycast(transform.position, Vector3.down, out hit, 2))
                    {
                        element = hit.transform.GetComponent<GridElement>();
                        if (element)
                        {
                            //element.transform.position += Vector3.up*3;
                            if (element.MyType == GridElementType.TrailFill || element.MyType == GridElementType.ColorFilled)
                            {
                                element.FindSmallestEnclosedAreaAndFillIt();
                                GameManager.instance.ActiveGrid.CheckFilledCellCount();
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Error Code 0");
            transform.position = startingPosition;
        }
    }

    public void Die()
    {
        RaycastHit hit;
        GridElement element;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2))
        {
            element = hit.transform.GetComponent<GridElement>();
            if (element)
            {
                //element.transform.position += Vector3.up*3;
                if (element.MyType == GridElementType.ColorFilled)
                {
                    moveDirection = Direction.Nan;
                    InputManager.instance.ActiveInput = Direction.Nan;
                    return;
                }
            }
        }

        isAlive = false;
        Instantiate(PlayerFructure, transform.position, Quaternion.identity);
        GameManager.instance.LevelFailed(1.2f);
        Destroy(this.gameObject);
    }
    public GameObject PlayerFructure;

}
