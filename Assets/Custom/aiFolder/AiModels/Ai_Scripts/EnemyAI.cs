using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region PUBLIC VARIABLES

    public ENEMY_STATE states;

    #endregion
    #region UNITY METHODS

    public void Awake()
    {
        states = ENEMY_STATE.IDLE;
    }

    public void Start()
    {
        StartCoroutine(EnemyFSM());
    }

    #endregion

    #region ENEMY COROUTINES

    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(states.ToString());
        }
    }

    
    IEnumerator IDLE()
    {
        // ENTER THE IDLE STATE
        Debug.Log("Alright, seems no evil Player is around, I can chill!");

        // EXECUTE IDLE STATE
        while (states == ENEMY_STATE.IDLE)
        {

            yield return null;
        }

        // EXIT THE IDLE STATE

        Debug.Log("Uh, I guess I smell a Player!");
    }
    #endregion
}

#region enums

public enum ENEMY_STATE
{
    IDLE = 0,
    CHASE = 1,
    ATTACK = 2
}

#endregion



