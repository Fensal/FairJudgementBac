using UnityEngine;
using System.Collections;
using System.IO;

public class Constants : MonoBehaviour
{

    public TextReader config = new StreamReader("config.txt");

    void Start()
    {
        Debug.Log(config.ReadLine());
    }


}
