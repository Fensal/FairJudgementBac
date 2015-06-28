using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterController : MonoBehaviour {
    [SerializeField]
    public List<Transform> m_Water;


    void Update()
    {
        foreach (Transform t in m_Water)
        {
            t.localPosition = (t.localPosition - new Vector3(0.0f, 0.0f, 0.001f));
            if (t.localPosition.z <= -5)
            {
                t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 5.0f);
            }
        }
    }
}
