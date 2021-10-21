using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GenericState : MonoBehaviour
{
	public AiMonsterMachine GSM;
    public abstract GenericState RunCurrentState();
	
}
