using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static EmreIlkay.Utility;


public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public Direction ActiveInput;
    Vector2 touchStartPosition, currentTouchPosition;
    bool isSwiping = false;

    public bool enableKeyboard = false;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            touchStartPosition = Vector2.zero;
            currentTouchPosition = Vector2.zero;
            isSwiping = false;
            //ActiveInput = SwipeDirection.Nan;
        }

        if (Input.GetMouseButton(0)&&isSwiping)
        {
            currentTouchPosition = Input.mousePosition;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPosition = Input.mousePosition;
            currentTouchPosition = touchStartPosition;
            isSwiping = true;
        }

        if (isSwiping && (currentTouchPosition-touchStartPosition).magnitude > Screen.height*.1f)
        {
            ActiveInput =  GetSwipeDirection(currentTouchPosition-touchStartPosition);
        }

        if(enableKeyboard)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow)) ActiveInput = Direction.Up;

            if(Input.GetKeyDown(KeyCode.DownArrow)) ActiveInput = Direction.Down;
            
            if(Input.GetKeyDown(KeyCode.LeftArrow)) ActiveInput = Direction.Left;

            if(Input.GetKeyDown(KeyCode.RightArrow)) ActiveInput = Direction.Right;
        }
    }

    private Direction GetSwipeDirection(Vector3 swipeVector)
    {
        float positiveX = Mathf.Abs(swipeVector.x);
        float positiveY = Mathf.Abs(swipeVector.y);
        Direction swipedDir;
        if (positiveX > positiveY)
        {
            swipedDir = (swipeVector.x > 0) ? Direction.Right : Direction.Left;
        }
        else
        {
            swipedDir = (swipeVector.y > 0) ? Direction.Up : Direction.Down;
        }
        return swipedDir;
    }

    
}
