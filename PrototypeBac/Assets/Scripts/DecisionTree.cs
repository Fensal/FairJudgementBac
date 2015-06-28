using UnityEngine;
using System.Collections;


public interface INode
{
    BaseCharacterController m_Character { get; set; }

    INode decide();
}

public abstract class Action : INode
{
    public BaseCharacterController m_Character { get; set; }

    public Action(BaseCharacterController baseCharacterController)
    {
        m_Character = baseCharacterController;
    }

    public abstract IBaseState action();
    public abstract string getTypeToString();

    public virtual INode decide()
    {
        return this;
    }
}

public class Decision : INode
{
    public INode trueNode;
    public INode falseNode;
    public BaseCharacterController m_Character { get; set; }

    public Decision(BaseCharacterController baseCharacterController)
    {
        m_Character = baseCharacterController;
    }

    public virtual INode decide()
    {
        return this;
    }


}

public class BattleAction : Action
{
    public BattleAction(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
    }
    public override IBaseState action()
    {
        return new AIBattleIdle(m_Character);
    }

    public override string getTypeToString()
    {
        return "AIBattleIdle";
    }
}

public class PatrolAction : Action
{
    public PatrolAction(BaseCharacterController baseCharactercontroller)
        : base(baseCharactercontroller)
    {
    }

    public override IBaseState action()
    {

        return new AIMovementPatrol(m_Character);
    }

    public override string getTypeToString()
    {
        return "AIMovementPatrol";
    }
}

public class PlayerInSightDecision : Decision
{

    public PlayerInSightDecision(BaseCharacterController baseCharactercontroller)
        : base(baseCharactercontroller)
    {
    }


    public override INode decide()
    {
        if (Mathf.Abs(m_Character.m_Opponent.transform.position.x - m_Character.transform.position.x) < m_Character.m_BattleDistance &&
            Mathf.Abs(m_Character.m_Opponent.transform.position.z - m_Character.transform.position.z) < m_Character.m_BattleDistanceY &&
            Mathf.Abs(m_Character.m_Opponent.transform.position.y - m_Character.transform.position.y) < 1.0f)
        {
            if(!m_Character.m_IsDead)
                m_Character.m_Opponent.m_Opponent = m_Character;
            return trueNode;
        }
            
        else
            return falseNode;
    }
}




