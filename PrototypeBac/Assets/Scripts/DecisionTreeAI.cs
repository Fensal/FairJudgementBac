using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecisionTreeAI : MonoBehaviour {

    private List<Decision> decisions = new List<Decision>();
    private List<Action> actions = new List<Action>();

    private INode root;
    private INode currentNode;
    public Action currentAction;

    public BaseCharacterController m_Character;


    void Start()
    {
        actions.Insert(0, new PatrolAction(m_Character));
        actions.Insert(1, new BattleAction(m_Character));


        decisions.Insert(0, new PlayerInSightDecision(m_Character));



        decisions[0].trueNode = actions[1];
        decisions[0].falseNode = actions[0];


        root = decisions[0];

    }
    void Update()
    {
        if (m_Character != null)
        {
            currentNode = root.decide();

            do
            {
                currentNode = currentNode.decide();
            }
            while (!(currentNode is Action));


            if (currentNode is Action)
            {

                currentAction = (Action)currentNode;
                //currentAction.action();
            }
        }
    }
}
