using UnityEngine;
using System;
using System.Collections;

public class EnemyTypeStatic : BaseEnemyType
{

    public override IBaseState GetNextAction()
    {

        return PlaystyleStatic();
    }

}
