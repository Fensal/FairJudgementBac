using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HierarchicalNGramPredictor{

    private NGramPredictor[] ngrams;

    public int nValue;

    public int threshold;

    public HierarchicalNGramPredictor(int n)
    {
        nValue = n;

        ngrams = new NGramPredictor[nValue];

        for (int i = 0; i < nValue; i++)
        {
            ngrams[i] = new NGramPredictor();
            ngrams[i].nValue = i + 1;
            ngrams[i].Init();

            string filename = "StoreNGram" + i + ".dat";
            object temp = ReadFromFile(filename);
            if (temp != null)
                ngrams[i].data = (Hashtable)temp;
        }
            
    }

    ~HierarchicalNGramPredictor()
    {
        WriteToFile();
    }

    public void WriteToFile()
    {
        for (int i = 0; i < nValue; i++)
        {
            Hashtable data = ngrams[i].data;
            BinaryFormatter bf = new BinaryFormatter();
            string filename = "StoreNGram" + i;
            FileStream file = new FileStream( filename + ".dat", FileMode.OpenOrCreate, FileAccess.Write);
            bf.Serialize(file, data);

            file.Close();
        }
        
    }

    public object ReadFromFile(string filename)
    {
        if (File.Exists( filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);

            object obj = bf.Deserialize(file);

            file.Close();

            return obj;
        }
        

        return null;
    }

    public void RegisterSequence(string[] actions)
    {
        for(int i = 0; i < nValue; i++)
        {
            string[] subActions = new string[i+1];

            int startIndex = (actions.Length - 1) - (subActions.Length - 1);

            for (int j = 0; j < subActions.Length; j++)
            {
                subActions[j] = actions[startIndex];

                startIndex++;
            }

            ngrams[i].RegisterSequence(subActions);
          
        }
    }

    public string GetMostLikely(string[] actions)
    {
        for(int i=0; i< nValue-1; i++)
        {
            NGramPredictor nGram = ngrams[nValue-i-1];

            //get substring
            string[] subActions = new string[nValue-i-1];

            int startIndex = (actions.Length - 1) - (subActions.Length - 1);

            for (int j = 0; j < subActions.Length; j++)
            {
                subActions[j] = actions[startIndex];
                startIndex++;
            }

            string stringOfArray = string.Join("", subActions);

            if (stringOfArray == "")
                return String.Empty;

            if (subActions.Length == 1)
                stringOfArray = subActions[0];

            //check if there are enough entries

            if (nGram.data.ContainsKey(stringOfArray))
            {
                KeyDataRecord data = (KeyDataRecord)nGram.data[stringOfArray];

                if(data.total > threshold)
                    return nGram.GetMostLikely(subActions);
            }
                

        }
        return string.Empty;
    }

    
}
