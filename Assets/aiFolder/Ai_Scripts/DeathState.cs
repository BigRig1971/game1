using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : GenericState
{
	public override GenericState RunCurrentState()
	{
		Death();
		return null;
	}

	void Death()
	{
		GSM.anim.SetTrigger("Die");
	}
}
