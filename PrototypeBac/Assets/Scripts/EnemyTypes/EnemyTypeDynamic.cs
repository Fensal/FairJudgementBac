using UnityEngine;
using System;
using System.Collections;

public class EnemyTypeDynamic : BaseEnemyType
{


    public override IBaseState GetNextAction()
    {
        return PlaystyleDynamic();
    }

}
