using UnityEngine;
using System;
using System.Collections;

public abstract class BaseEnemyType : MonoBehaviour 
{
    public BaseCharacterController m_Character;

    private float m_DistanceToPlayer = 1.4f;

    protected EnemyCharacterController m_PlayerController;
    protected EnemyCharacterController m_EnemyController;

    private System.Random rnd = new System.Random();

    private IBaseState m_NextState;

    private float m_RetreatTimer = 0.0f;
    private float m_PredictionTimer = 0.0f;
    private float m_PursueTimer = 0.0f;

    void Start()
    {
        m_EnemyController = (EnemyCharacterController)m_Character;
        m_PlayerController = (EnemyCharacterController)m_Character.m_Opponent;
    }

    void Update()
    {
        m_PlayerController = (EnemyCharacterController)m_Character.m_Opponent;
        m_RetreatTimer += Time.deltaTime;
        m_PredictionTimer += Time.deltaTime;
        m_PursueTimer += Time.deltaTime;
    }

    public abstract IBaseState GetNextAction();

    public IBaseState PlaystyleDynamic()
    {
        Debug.Log("PlaystyleDynamic");

        if (Mathf.Abs(m_PlayerController.collider.bounds.center.x - m_EnemyController.collider.bounds.center.x) > m_DistanceToPlayer)
        {
            m_NextState = new AIBattlePursue(m_Character);
        }
        else
        {
            m_PredictionTimer = 0.0f;

            string predictedAction = string.Empty;

            predictedAction = m_EnemyController.nGramPredictor.GetMostLikely(m_PlayerController.predictionSequence);

            Debug.Log("Prediction: " + predictedAction);

            if (predictedAction != string.Empty)
            {

                if (predictedAction == "AIBattleHeavyAttackUp")
                {
                    m_NextState = new AIBattleBlockUp(m_Character);
                }
                else if (predictedAction == "AIBattleHeavyAttackDown")
                {
                    m_NextState = new AIBattleBlockDown(m_Character);
                }
                else if (predictedAction == "AIBattleLightAttackUp")
                {
                    m_NextState = new AIBattleBlockUp(m_Character);
                }
                else if (predictedAction == "AIBattleLightAttackDown")
                {
                    m_NextState = new AIBattleBlockDown(m_Character);
                }
                else if (predictedAction == "AIBattleParryUp")
                {
                    m_NextState = new AIBattleLightAttackDown(m_Character);
                }
                else if (predictedAction == "AIBattleParryDown")
                {
                    m_NextState = new AIBattleLightAttackUp(m_Character);
                }
                else if (predictedAction == "AIBattleBlockUp")
                {
                    m_NextState = new AIBattleHeavyAttackDown(m_Character);
                }
                else if (predictedAction == "AIBattleBlockDown")
                {
                    m_NextState = new AIBattleHeavyAttackUp(m_Character);
                }


            }
            else
            {
                m_NextState = new AIBattleLightAttackDown(m_Character);

            }
            
        }

        return m_NextState;
    }

    public IBaseState PlaystyleStatic()
    {
        Debug.Log("PlaystyleStatic");

        if (Mathf.Abs(m_PlayerController.collider.bounds.center.x - m_EnemyController.collider.bounds.center.x) > m_DistanceToPlayer)
        {
            m_NextState = new AIBattlePursue(m_Character);
        }
        else
        {
            m_NextState = GetActionFromString(m_EnemyController.attackSequence[m_EnemyController.attackSequenceIndex]);

            if (m_EnemyController.attackSequence.Length-1 > m_EnemyController.attackSequenceIndex)
                m_EnemyController.attackSequenceIndex++;
            else
                m_EnemyController.attackSequenceIndex = 0;

        }

        return m_NextState;
    }

    public IBaseState GetActionFromString(string action)
    {
        if (action == "AIBattleHeavyAttackUp")
        {
            return new AIBattleHeavyAttackUp(m_Character);
        }
        else if (action == "AIBattleHeavyAttackDown")
        {
            return new AIBattleHeavyAttackDown(m_Character);
        }
        else if (action == "AIBattleLightAttackUp")
        {

            return new AIBattleLightAttackUp(m_Character);
        }
        else if (action == "AIBattleLightAttackDown")
        {
            return new AIBattleLightAttackDown(m_Character);
        }
        else if (action == "AIBattleParryUp")
        {
            return new AIBattleParryUp(m_Character);
        }
        else if (action == "AIBattleParryDown")
        {
            return new AIBattleParryDown(m_Character);
        }
        else if (action == "AIBattleBlockUp")
        {
            return new AIBattleBlockUp(m_Character);
        }
        else if (action == "AIBattleBlockDown")
        {
            return new AIBattleBlockDown(m_Character);
        }
        else
        {
            return new AIBattleIdle(m_Character);
        }
    }



    public IBaseState GetRandomAction()
    {
        int rndNumber = rnd.Next(1, 5);

        Debug.LogWarning("Random:" + rndNumber);

        switch (rndNumber)
        {
            case 1: return new AIBattleLightAttackUp(m_Character);

            case 2: return new AIBattleLightAttackDown(m_Character);

            case 3: return new AIBattleHeavyAttackUp(m_Character);

            case 4: return new AIBattleHeavyAttackDown(m_Character);

            default: return new AIBattleLightAttackUp(m_Character);
                    
        }
    }

    public bool CheckPrediction(string prediction)
    {
        if(m_Character.m_Opponent.m_CurrentState.ToString() == prediction ||
            m_Character.m_Opponent.m_CurrentState.GetNextState().ToString() == prediction)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
