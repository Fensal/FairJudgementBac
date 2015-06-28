using UnityEngine;
using System.Collections;
using System;


[Serializable]
public struct KeyDataRecord 
{
    public Hashtable counts;

    public int total;
}

public class NGramPredictor
{
    public Hashtable data;

    public int nValue;

    public void Init()
    {
        data = new Hashtable();
    }

    public void RegisterSequence(string[] actions)
    {
        //split the sequence
        string[] prevActions = new string[nValue - 1];

        for(int i=0; i < nValue - 1; i++)
            prevActions[i] = actions[i];

        string currentAction = actions[nValue-1];

        string stringOfArray = string.Join("", prevActions);

        if (nValue == 1)
            stringOfArray = currentAction;


        if(!data.ContainsKey(stringOfArray))
            data[stringOfArray] = new KeyDataRecord();

        KeyDataRecord keyData = (KeyDataRecord)data[stringOfArray];

        if(keyData.counts == null)
            keyData.counts = new Hashtable();

        if(!keyData.counts.ContainsKey(currentAction))
            keyData.counts[currentAction] = 0;

        keyData.counts[currentAction] = (int) keyData.counts[currentAction] + 1;
        keyData.total += 1;

        data[stringOfArray] = keyData;


    }

    public string GetMostLikely(string[] actions)
    {
        string stringOfArray = string.Join("", actions);

        if (data[stringOfArray] == null)
            return string.Empty;

        KeyDataRecord keyData = (KeyDataRecord)data[stringOfArray];

        int highestValue = 0;
        string bestAction = string.Empty;

        ICollection possibleActions = keyData.counts.Keys;

        foreach (string action in possibleActions)
        {
            if ((int)keyData.counts[action] > highestValue)
            {
                highestValue = (int)keyData.counts[action];
                bestAction = action;
            }
        }
        return bestAction;
    }
}

