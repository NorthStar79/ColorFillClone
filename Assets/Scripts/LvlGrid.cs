using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EmreIlkay.Utility;


public class LvlGrid : MonoBehaviour
{
    public Transform StartingCell;
    GameObject Player;
    Vector3 startPos;

    GridElement[] elements;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        startPos = StartingCell.position+Vector3.up;
        Player.GetComponent<PlayerController>().startingPosition = startPos;
        elements = transform.GetComponentsInChildren<GridElement>();
    }

    public void CheckFilledCellCount()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(elements[i].MyType == GridElementType.Empty)
            {
                return;
            }
        }
        GameManager.instance.LevelCompleted();
    }
}
