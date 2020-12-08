using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public enum Direction
{
    Start,
    Right,
    Front,
    Left,
    Back,
};
//<summary>
//Class for representing concrete maze cell.
//</summary>
public class MazeCell
{
    public bool IsVisited = false;
    public bool WallRight = false;
    public bool WallFront = false;
    public bool WallLeft = false;
    public bool WallBack = false;
    public bool IsGoal = false;

    public int row;
    public int column;

    public bool isGoal()
    {
        return (!WallRight && WallFront && WallLeft && WallBack)
        || (!WallBack && WallRight && WallFront && WallLeft)
        || (!WallLeft && WallBack && WallRight && WallFront)
        || (!WallFront && WallLeft && WallBack && WallRight);
    }
}
