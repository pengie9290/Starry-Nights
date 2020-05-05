using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    //Sets OfficeManager as a singleton (part 1)
    public static OfficeManager Instance;

    //Stores basic office displays
    public GameObject FrontView;
    public GameObject RightView;
    public GameObject BackView;
    public GameObject LeftView;

    //Defines directions
    public enum Direction
    {
        ReturnToLeft, Front, Right, Back, Left, ReturnToFront
    }

    //Defines bars on doors
    public GameObject LeftBars;
    public GameObject RightBars;

    //Tracks when one or both doors are barred
    private bool leftDoorBarred = false;
    private bool rightDoorBarred = false;
    public bool LeftDoorBarred
    {
        get
        {
            return leftDoorBarred;
        }
        set
        {
            if (leftDoorBarred == value) return;
            if (LeftBars != null)
            {
                LeftBars.SetActive(value);
            }
            leftDoorBarred = value;
        }
    }
    public bool RightDoorBarred
    {
        get
        {
            return rightDoorBarred;
        }
        set
        {
            if (rightDoorBarred == value) return;
            if (RightBars != null)
            {
                RightBars.SetActive(value);
            }
            rightDoorBarred = value;
        }
    }

    //Opens/Closes doors
    public void ToggleLeftDoor()
    {
        LeftDoorBarred = !LeftDoorBarred;
    }
    public void ToggleRightDoor()
    {
        RightDoorBarred = !RightDoorBarred;
    }
    public void ToggleCurrentDoor()
    {
        if (FacingDirection == Direction.Left) ToggleLeftDoor();
        if (FacingDirection == Direction.Right) ToggleRightDoor();
    }

    //Tracks where the player is looking
    private Direction _facingDirection = Direction.ReturnToFront;
    public Direction FacingDirection
    {
        get
        {
            return _facingDirection;
        }
        set
        {
            Direction OldDirection = _facingDirection;
            Direction NewDirection = value;
            if (NewDirection == Direction.ReturnToLeft) NewDirection = Direction.Left;
            if (NewDirection == Direction.ReturnToFront) NewDirection = Direction.Front;
            _facingDirection = NewDirection;
            if (NewDirection != OldDirection) DirectionHasChanged();
        }
    }

    //Updates view to reflect new direction
    void DirectionHasChanged()
    {
        switch (FacingDirection)
        {
            case Direction.Front:
            default:
                FrontView.SetActive(true);
                LeftView.SetActive(false);
                BackView.SetActive(false);
                RightView.SetActive(false);
                break;
            case Direction.Left:
                FrontView.SetActive(false);
                LeftView.SetActive(true);
                BackView.SetActive(false);
                RightView.SetActive(false);
                break;
            case Direction.Back:
                FrontView.SetActive(false);
                LeftView.SetActive(false);
                BackView.SetActive(true);
                RightView.SetActive(false);
                break;
            case Direction.Right:
                FrontView.SetActive(false);
                LeftView.SetActive(false);
                BackView.SetActive(false);
                RightView.SetActive(true);
                break;
        }
    }

    //"Rotates" office view
    void RotateDirection(int changeDirection)
    {
        if (changeDirection != 1 && changeDirection != -1) return;
        FacingDirection = FacingDirection + changeDirection;
    }




    void Start()
    {
        //Sets OfficeManager as a singleton (part 2)
        if (OfficeManager.Instance == null) OfficeManager.Instance = this;
        else Destroy(gameObject);

        //Default view to the front of the office
        FacingDirection = Direction.Front;
    }

    void Update()
    {
        //Looks around office
        if (Input.GetKeyDown(KeyCode.RightArrow)) RotateDirection(1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) RotateDirection(-1);

        //Opens/Closes doors
        if (Input.GetKeyDown(KeyCode.A)) ToggleLeftDoor();
        if (Input.GetKeyDown(KeyCode.D)) ToggleRightDoor();

    }
}
