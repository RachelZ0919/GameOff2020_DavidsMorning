using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGround : MonoBehaviour, IGround
{
    public void Enter()
    {
        Debug.Log("You've Enter the Ground");
    }

    public void InteractGround()
    {
        Debug.Log("You're Interacting With Ground");
    }

    public void Leave()
    {
        Debug.Log("You've Leave the Ground");
    }
}
