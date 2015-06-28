using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class CameraMovement : MonoBehaviour
{
    [HideInInspector]
    public Vector3 m_Destination;
    [SerializeField]
    private PlayerCharacterController m_Character;
    [SerializeField]
    private Transform m_SwapPosition;
    private Vector3 m_ResetPosition;
    [SerializeField]
    private List<Transform> m_Cameras;
    [SerializeField]
    private List<Vector3> m_LocalStartingPositions;
    [SerializeField]
    List<GameObject> m_CameraPaths;
    [SerializeField]
    bool m_ConsiderY = false;
    [SerializeField]
    public Transform m_Focus = null;
    [SerializeField]
    Image m_Overlay;
    [SerializeField]
    Material m_PostShader;

    public GameObject m_ActivePath;
    public List<Transform> m_Nodes;
    public Transform m_LeftNode;
    public Transform m_RightNode;

    public bool m_CharacterBack;
    public bool m_BackPlane;
    private Vector3 m_LocalStartingPoint;
    private Vector3 m_Direction;
    float m_MaxSpeed = 0.1f;
    public bool m_BlockUpdate;
    Vector3 m_Target = Vector3.zero;
    private bool m_CoroutineRunning = false;

    float m_RotMul = 0.0f;
    

    void Awake()
    {
        foreach (Transform t in m_Cameras)
        {
            m_LocalStartingPositions.Add(t.localPosition);
        }

        if (Root.Instance != null)
        {
            m_Character = Root.Instance.m_Character;
            m_Character.GetComponentInChildren<GenerateParticles>().m_CameraScript = this;
        }

        m_LocalStartingPoint = transform.localPosition;
        m_Direction = Vector3.zero;
        m_BackPlane = false;
        m_CharacterBack = false;
        m_BlockUpdate = false;
        m_SwapPosition = Root.Instance.m_Character.gameObject.GetComponentInChildren<GenerateParticles>().transform;
        m_ResetPosition = m_SwapPosition.localPosition;
        m_Character.m_Overlay = m_Overlay;

        if (m_CameraPaths.Capacity > 0)
        {
            m_ActivePath = m_CameraPaths[0];

            LoadNodes();
        }
    }

    void OnEnable()
    {
        m_Character.m_Overlay = m_Overlay;
    }

    void Update()
    {
        if (m_Character.m_Opponent == null && m_Character.transform.position.z == 0.0f)
        {
            m_Character.m_BaseZLevel = 0.0f;
            m_SwapPosition.localPosition = m_ResetPosition;
            m_CharacterBack = false;


            if (m_CameraPaths.Count > 0)
            {
                m_ActivePath = m_CameraPaths[0];
                LoadNodes();
            }
        }
        m_PostShader.SetFloat("_Health", ((float)m_Character.m_CurrentHealth / (float)m_Character.MaxHealth));
    }

    void FixedUpdate()
    {
        if (!m_CoroutineRunning)
        {
            if (m_Character.m_Opponent != null)
            {
                m_MaxSpeed = 0.5f;

                Vector3 PlayerToEnemy = m_Character.m_Opponent.transform.position - m_Character.transform.position;
                float Distance = PlayerToEnemy.magnitude;
                m_Target = m_Character.transform.position + PlayerToEnemy.normalized * (Distance / 2) + new Vector3(0.0f, 1.25f, -2.75f) + new Vector3(0.0f, -0.6f, 16.35f - Distance * 3.0f);
                //m_Target.y = m_Character.transform.position.y + Math.Sign(PlayerToEnemy.y)*0.65f;
            }
            else if(m_CameraPaths.Capacity == 0)
            {
                m_MaxSpeed = 0.05f;

                float XPos;
                float YPos;

                if (m_Character.transform.position.x <= 58.56f)
                {
                    XPos = 58.56f;
                }
                else if (m_Character.transform.position.x >= 72.51f)
                {
                    XPos = 72.51f;
                }
                else
                {
                    XPos = m_Character.transform.position.x;
                }

                
                if(m_Character.m_CurrentState is MovementWalk || m_Character.m_CurrentState is MovementIdle || Math.Abs(transform.position.y - m_Character.transform.position.y + 1.25f)>3.0f)
                {
                    YPos = m_Character.transform.position.y + 1.25f;
                }
                else
                {
                    YPos = transform.position.y;
                }

                m_Target = new Vector3(XPos, YPos, -2.75f);
            }
            else
            {
                m_MaxSpeed = 0.1f;

                float YPos;
                float XPos;
                Quaternion Rot;
                if (!m_LeftNode)
                {
                    YPos = m_Nodes[0].position.y + 1.25f;
                    XPos = m_Nodes[0].position.x;
                    Rot = m_Nodes[0].rotation;
                }
                else if (!m_RightNode)
                {
                    YPos = m_Nodes[m_Nodes.Capacity - 2].position.y + 1.25f;
                    XPos = m_Nodes[m_Nodes.Capacity - 2].position.x;
                    Rot = m_Nodes[m_Nodes.Capacity - 2].rotation;
                }
                else
                {
                    YPos = InterpolateY(m_LeftNode, m_RightNode) + 1.25f;
                    XPos =  m_Character.transform.position.x;
                    Rot = InterpolateRotation(m_LeftNode, m_RightNode);
                }

                m_Target = new Vector3(XPos, YPos, m_ActivePath.transform.position.z - 2.75f);

                //TODO
                //fix rotation lerp
                if ((transform.rotation.eulerAngles - Rot.eulerAngles).magnitude >= 0.1f && m_RotMul == 0.0f)
                {
                    m_RotMul = 1.0f;
                    Rot = Quaternion.Lerp(transform.rotation, Rot, m_RotMul);
                    m_RotMul -= 0.1f;
                }
                transform.rotation = Rot;
            }

            Vector3 Direction = (m_Target - transform.position).normalized;

            if ((m_Target - transform.position).magnitude < m_MaxSpeed)
            {
                transform.position = m_Target;

            }
            else
            {
                if ((m_Target - transform.position).magnitude >= 5.0f)
                {
                    transform.position += Direction * 5.0f;// m_MaxSpeed * 6.0f;
                }
                else
                {
                    transform.position += Direction * m_MaxSpeed;
                }
            }

            SetClosestNodes();
        }
    }

    public IEnumerator Move(Vector3 Destination)
    {
        m_BlockUpdate = true;

        if (m_CharacterBack)
        {
            m_ActivePath = m_CameraPaths[0];
        }
        else
        {
            m_ActivePath = m_CameraPaths[1];
        }

        FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/planeshift", Vector3.zero);
        LoadNodes();


        SwapCharacter(Destination);
        m_CharacterBack = !m_CharacterBack;

        m_BlockUpdate = false;

        return null;
    }

    public IEnumerator MoveWithoutCamera(Vector3 location)
    {
        m_Character.transform.position = location + new Vector3(0.0f, m_Character.m_Height / 2, 0.0f);


        //HACK
        //fixes an issue with character being positioned on a random z level
        if (m_BackPlane)
        {
            m_Character.transform.position = new Vector3(m_Character.transform.position.x, m_Character.transform.position.y, 0.0f);
        }

        FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/planeshift", Vector3.zero);


        if (!m_BackPlane)
        {
            m_Character.m_BaseZLevel = location.z;
        }
        else
        {
            m_Character.m_BaseZLevel = 0.0f;
        }

        m_BackPlane = !m_BackPlane;

        yield return new WaitForSeconds(0.5f);

    }

    public void SwapCharacter(Vector3 location)
    {
        Vector3 temp = new Vector3(m_SwapPosition.position.x, m_SwapPosition.position.y, m_Character.m_BaseZLevel);

        if (!m_CharacterBack)
        {
            m_Character.m_BaseZLevel = location.z;
        }
        else
        {
            m_Character.m_BaseZLevel = 0.0f;
        }

        m_Character.transform.position = new Vector3(location.x, location.y, m_Character.m_BaseZLevel) + new Vector3(0.0f, m_Character.m_Height / 2, 0.0f);

        m_SwapPosition.position = temp;
    }

    public void LoadNodes()
    {
        Transform[] temp = m_ActivePath.GetComponentsInChildren<Transform>();
        m_Nodes = new List<Transform>(temp.Length);

        foreach (Transform t in temp)
        {
            if (t != m_ActivePath.transform)
                m_Nodes.Add(t);
        }

        SetClosestNodes();
    }

    private void SetClosestNodes()
    {
        List<Transform> m_NodesLeft = new List<Transform>();
        List<Transform> m_NodesRight = new List<Transform>();

        foreach (Transform t in m_Nodes)
        {
            if (t.position.x <= m_Character.transform.position.x)
            {
                m_NodesLeft.Add(t);
            }
            else
            {
                m_NodesRight.Add(t);
            }
        }


        if (m_ConsiderY)
        {
            if (m_NodesLeft.Count == 0)
            {
                m_LeftNode = null;
                m_RightNode = FindClosestX(m_NodesRight);
            }
            else if (m_NodesRight.Count == 0)
            {
                m_LeftNode = FindClosestX(m_NodesLeft);
                m_RightNode = null;
            }
            else
            {
                FindClosest(m_Nodes);
            }
        }
        else
        {
            m_LeftNode = FindClosestX(m_NodesLeft);

            m_RightNode = FindClosestX(m_NodesRight);
        }
        
    }

    private Transform FindClosestX(List<Transform> x)
    {
        Transform closest = null;

        foreach (Transform t in x)
        {
            if (closest == null)
            {
                closest = t;
            }
            else
            {
                if (Math.Abs(t.position.x - m_Character.transform.position.x) < Math.Abs(closest.position.x - m_Character.transform.position.x))
                {
                    closest = t;
                }
            }
        }

        return closest;
    }

    private void FindClosest(List<Transform> x)
    {
        m_LeftNode = null;
        m_RightNode = null;


        //find closest node;
        foreach (Transform t in x)
        {
            if (m_LeftNode == null)
            {
                m_LeftNode = t;
            }
            else
            {
                if ((t.position - m_Character.transform.position).magnitude < (m_LeftNode.position - m_Character.transform.position).magnitude)
                {
                    m_LeftNode = t;
                }
            }
        }

        //find second closest node;
        foreach (Transform t in x)
        {
            if (t != m_LeftNode)
            {
                if (m_RightNode == null)
                {
                    m_RightNode = t;
                }
                else
                {
                    if ((t.position - m_Character.transform.position).magnitude < (m_RightNode.position - m_Character.transform.position).magnitude)
                    {
                        m_RightNode = t;
                    }
                }
            }
        }

        if (m_RightNode.transform.position.x < m_LeftNode.transform.position.x)
        {
            Transform temp = m_RightNode;
            m_RightNode = m_LeftNode;
            m_LeftNode = temp;
        }
    }

    private float InterpolateY(Transform l, Transform r)
    {
        float distance = Math.Abs(l.position.x - r.position.x);
        float distanceLeft = Math.Abs(l.position.x - m_Character.transform.position.x);
        float mul = 1.0f / distance * distanceLeft;
        float mul2 = (1.0f - (float)Math.Cos(mul * Math.PI)) / 2;
        
        return (l.transform.position.y * (1.0f - mul2) + r.transform.position.y * mul2);
    }

    private float InterpolateX(Transform l, Transform r)
    {
        float distance = Math.Abs(l.position.x - r.position.x);
        float distanceLeft = Math.Abs(l.position.x - m_Character.transform.position.x);
        float mul = 1.0f / distance * distanceLeft;
        float mul2 = (1.0f - (float)Math.Cos(mul * Math.PI)) / 2;
        Debug.LogError(distance + " " + distanceLeft);
        return (l.transform.position.x * (1.0f - mul2) + r.transform.position.x * mul2);
    }

    private Quaternion InterpolateRotation(Transform l, Transform r)
    {
        float distance = Math.Abs(l.position.x - r.position.x);
        float distanceLeft = Math.Abs(l.position.x - m_Character.transform.position.x);
        float mul = 1.0f / distance * distanceLeft;

        Quaternion rotL = l.rotation;
        Quaternion rotR = r.rotation;

        return Quaternion.Slerp(rotL, rotR, mul);
    }

    public void SwitchPaths(int oldPath, int newPath)
    {
        GameObject temp = m_CameraPaths[oldPath];
        m_CameraPaths[oldPath] = m_CameraPaths[newPath];
        m_CameraPaths[newPath] = temp;

        m_ActivePath = m_CameraPaths[oldPath];
        LoadNodes();
    }

    public IEnumerator FocusLerp(Transform focus, float focustime, float focusspeed = 0.1f)
    {
        m_CoroutineRunning = true;
        float weight = 0.0f;
        Transform startposition = transform;

        while(weight <= 1.0f){
            yield return new WaitForFixedUpdate();

            Vector3 TargetPosition = Vector3.Lerp(startposition.position, focus.position, weight);
            Quaternion TargetRotation = Quaternion.Lerp(startposition.rotation, focus.rotation, weight);

            transform.position = TargetPosition;
            transform.rotation = TargetRotation;

            weight += focusspeed;
        }

        yield return new WaitForSeconds(focustime);



        m_CoroutineRunning = false;
    }
}
