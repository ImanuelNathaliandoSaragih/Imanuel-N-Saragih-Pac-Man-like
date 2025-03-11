using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : BaseState

{

    public void EnterState(Enemy enemy)

    {

        Debug.Log("Start Retreating");

    }


    public void UpdateState(Enemy enemy)

    {
        if (enemy.Player != null)

        {

            enemy.NavMeshAgent.destination = enemy.transform.position - enemy.Player.transform.position;

        }
        Debug.Log("Retreating");

    }


    public void ExitState(Enemy enemy)

    {

        Debug.Log("Stop Retreating");

    }

}
