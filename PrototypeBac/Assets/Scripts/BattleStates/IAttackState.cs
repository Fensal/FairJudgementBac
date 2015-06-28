using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IAttackState
{
    void WeaponCollide(Collider other);
}