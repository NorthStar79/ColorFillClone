using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EmreIlkay.Utility;

public class Tweener : MonoBehaviour
{
    public Direction MoveDirection;
    public int MoveAmount =1;

    public float MoveSpeed=1;

    Vector3 startingPosition,moveVector;
    
    void Start()
    {
        startingPosition = transform.position;

        switch (MoveDirection)
        {
            case Direction.Up: moveVector = Vector3.forward; break;
            case Direction.Down: moveVector = Vector3.back; break;
            case Direction.Left: moveVector = Vector3.left; break;
            case Direction.Right: moveVector = Vector3.right; break; 
            default: break;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(startingPosition,startingPosition+moveVector*MoveAmount,Mathf.Sin(Time.timeSinceLevelLoad*MoveSpeed)*.5f+.5f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position,Vector3.down, out hit, 2))
        {
            GridElement element = hit.transform.GetComponent<GridElement>();
            if (element)
            {
                if (element.MyType == GridElementType.ColorFilled)
                {
                    GetComponent<Enemy>().Die();
                }else if (element.MyType == GridElementType.TrailFill)
                {
                    if( FindObjectOfType<PlayerController>() && FindObjectOfType<PlayerController>().isAlive)
                    FindObjectOfType<PlayerController>().Die();
                }
            }
        }
    }
}
