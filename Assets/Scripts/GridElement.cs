using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EmreIlkay.Utility;

public class GridElement : MonoBehaviour
{

    public Material fillMat, emptyMat, WallMat, TempMat, TrailMat;
    public GridElement[] Neighbors;

    public GridElementType MyType;


    int numOfSides = 8; //number of sides ex: 4 for Cubes, 6 for Hexagons etc.

    public static List<GridElement> Tail = new List<GridElement>();
    public void Start()
    {
        Neighbors = new GridElement[numOfSides];
        Collider[] temp;

        for (int i = 0; i < numOfSides; i++)
        {
            Quaternion q = Quaternion.AngleAxis(360 / numOfSides * i, Vector3.up);
            Vector3 b = q * Vector3.forward;
            temp = Physics.OverlapSphere(transform.position + b, .1f);
            if (temp.Length > 0 && temp[0].GetComponent<GridElement>())
            {
                Neighbors[i] = temp[0].GetComponent<GridElement>();
            }
            temp = null;
        }


        ChangeColor();
    }

    public void ChangeColor()
    {
        if (MyType == GridElementType.Empty) GetComponent<Renderer>().material = emptyMat;
        if (MyType == GridElementType.ColorFilled) GetComponent<Renderer>().material = fillMat;
        if (MyType == GridElementType.TempFill) GetComponent<Renderer>().material = TempMat;
        if (MyType == GridElementType.Wall) GetComponent<Renderer>().material = WallMat;
        if (MyType == GridElementType.TrailFill) GetComponent<Renderer>().material = TrailMat;
    }

    [ContextMenu("Flood")]
    public void FindSmallestEnclosedAreaAndFillIt(bool isReverseCall = false)
    {
        int[] temp = new int[numOfSides];
        for (int i = 0; i < numOfSides; i++)
        {
            if (Neighbors[i] != null && Neighbors[i].MyType == GridElementType.Empty)
            {
                temp[i] = Neighbors[i].FloodFill(true);
                Neighbors[i].drainTemps();
            }
            else
            {
                temp[i] = 999999; //Any suficently large number would be enough.
            }

            if (temp[i] <= 0)
            {
                temp[i] = 999999; //Any suficently large number would be enough.
            }
        }
        int indexOfMin = System.Array.IndexOf(temp, temp.Min());

        if (Neighbors[indexOfMin] != null && Neighbors[indexOfMin].MyType == GridElementType.Empty)
        {
            int areaCounter =0;
            for (int l =0; l <temp.Length;l++)
            {
                if(temp[l] < 999999 && temp[l] > 0)
                {
                    int i =0;
                    for (int m =0; m<= l; m++)
                    {
                        if(temp[l]==temp[m]) i++;
                    }
                    if(i<=1)
                    {
                        areaCounter++;
                    }
                }
            }
            if(areaCounter >1) //if this cell create atleast two different sized area
            {
                temp[indexOfMin] = Neighbors[indexOfMin].FloodFill(false); //Fill smallest area
            }
        }
        
        for (int i = 0; i < Neighbors.Length; i++)
        {
            if (Neighbors[i] != null && Neighbors[i].MyType == GridElementType.TrailFill)
            {
               Neighbors[i].TrailFill();
            }
        }
        
        if(Tail.Count== 1)
        {
            Tail[0].MyType = GridElementType.ColorFilled;
            Tail[0].ChangeColor();
        }

        goForGaps();
        Tail.Clear();
    }

    void goForGaps()
    {
        List<GridElement> TailCache = new List<GridElement>(Tail);
        Tail.Clear();

        foreach (var item in TailCache)
        {
            item.FindSmallestEnclosedAreaAndFillIt();
        }
    }


    public int FloodFill(bool isTemp = true)
    {
        int counter = 1;
        MyType = isTemp ? GridElementType.TempFill : GridElementType.ColorFilled;

        foreach (var item in Neighbors)
        {
            if (item != null && item.MyType == GridElementType.Empty)
            {
                counter += item.FloodFill(isTemp) + 1;
            }
        }
        ChangeColor();
        return counter;
    }

    public void TrailFill()
    {
        MyType = GridElementType.ColorFilled;

        foreach (var item in Neighbors)
        {
            if (item != null && item.MyType == GridElementType.TrailFill)
            {
                item.TrailFill();
            }
        }
        
//        Tail.Clear();
        ChangeColor();
    }

    public void SetAsTrail()
    {
        MyType = GridElementType.TrailFill;
        Tail.Add(this);
        ChangeColor();
    }
    void drainTemps()
    {
        MyType = GridElementType.Empty;

        foreach (var item in Neighbors)
        {
            if (item != null && item.MyType == GridElementType.TempFill)
            {
                item.drainTemps();
            }
        }
        ChangeColor();
    }

    public class noNameYet
    {
        public GridElement Element;
        public int CellCount;

        public noNameYet(GridElement element, int cellCount)
        {
            Element = element;
            CellCount = cellCount;
        }
    }

}



/*

 public void FindSmallestEnclosedAreaAndFillIt()
    {
        int[] temp = new int[numOfSides];
        for (int i = 0; i < Neighbors.Length; i++)
        {
            if (Neighbors[i] != null && Neighbors[i].MyType == GridElementType.Empty)
            {
                temp[i] = Neighbors[i].FloodFill(true);
                Neighbors[i].drainTemps();
            }
            else
            {
                temp[i] = 999999; //Any suficently large number would be enough.
            }

            if (temp[i] <= 0)
            {
                temp[i] = 999999; //Any suficently large number would be enough.
            }
        }
        int indexOfMin = System.Array.IndexOf(temp, temp.Min());

        if (Neighbors[indexOfMin] != null && Neighbors[indexOfMin].MyType == GridElementType.Empty)
        {
            int areaCounter =0;
            foreach (var item in temp)
            {
                if(item < 999999 && item > 0) areaCounter++;
            }
            if(areaCounter >1)
            {
                temp[indexOfMin] = Neighbors[indexOfMin].FloodFill(false);
            }
        }

        for (int i = 0; i < Neighbors.Length; i++)
        {
            if (Neighbors[i] != null && Neighbors[i].MyType == GridElementType.TrailFill)
            {
                Neighbors[i].TrailFill();
                
            }
        }
        if(TrailTail.Count ==1)
                {
                    TrailTail[0].MyType = GridElementType.ColorFilled;
                    TrailTail[0].ChangeColor();

                }
    }
*/