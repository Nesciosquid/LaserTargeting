using UnityEngine;
using System.Collections.Generic;

public class LaserHolderController : MonoBehaviour {

    private GameObject m_Laser;
    [SerializeField]
    private float m_MaxAngle = 15;
    [SerializeField]
    private float m_AngleIncrement = 1;

    private Dictionary<GameObject, List<Vector3>> m_GameObjectDetections = new Dictionary<GameObject, List<Vector3>>();

    private Vector3 m_LastRotation;

    private bool m_Cycling = false;

    [SerializeField]
    private float m_TimeScale = 1;

    [SerializeField]
    private Vector3 m_StartingRotation = new Vector3(0,0,0);

    private void Start()
    {
        m_Laser = GetComponentInChildren<LaserController>().gameObject;
    }

    private void SetLaserRotation(Vector3 angle)
    {
        var newRot = m_StartingRotation + angle;
        gameObject.transform.localRotation = Quaternion.Euler(newRot);
    }

    public Dictionary<GameObject, List<Vector3>> GetDetectedObjects()
    {
        return m_GameObjectDetections;
    }

    public void PointAtGameObject(GameObject target)
    {
        if (m_GameObjectDetections.ContainsKey(target))
        {
            SetLaserRotation(ComputeAverageRotation(m_GameObjectDetections[target]));
        }
    }

    public void Scan()
    {
        Time.timeScale = m_TimeScale;
        m_GameObjectDetections = new Dictionary<GameObject, List<Vector3>>();
        m_LastRotation = new Vector3(0, -m_MaxAngle, -m_MaxAngle);
        m_Cycling = true;
    }

    private void FixedUpdate()
    {
        if (m_Cycling)
        {
            UpdateAngle();
        }
    }

    private void ListDetections()
    {
        foreach (var target in m_GameObjectDetections.Keys)
        {
            Debug.Log(target.name + " detected at: " + ComputeAverageRotation(m_GameObjectDetections[target]));
        }
    }

    private Vector3 ComputeAverageRotation(List<Vector3> rotations)
    {
        Vector3 avg = new Vector3();
        foreach (var rot in rotations)
        {
            avg += rot;
        }
        var scaling = new Vector3(1f / rotations.Count, 1f / rotations.Count, 1f / rotations.Count);
        avg.Scale(scaling);
        return avg;
    }

    public void RegisterDetection(GameObject target)
    {
        if (m_Cycling)
        {
            if (!m_GameObjectDetections.ContainsKey(target))
            {
                m_GameObjectDetections.Add(target, new List<Vector3>());
            }

            m_GameObjectDetections[target].Add(m_LastRotation);
        }
    }

    private float GetNextYRotation(Vector3 lastRotation)
    {
        float newY;
        newY = lastRotation.y + m_AngleIncrement;
        if (newY < m_MaxAngle)
        {
            return newY;
        } else
        {
            return -m_MaxAngle;
        }
    }

    private float GetNextZRotation(Vector3 lastRotation)
    {
        float newZ = lastRotation.z;
        float newY = lastRotation.y + m_AngleIncrement;
        if (newY < m_MaxAngle)
        {
            return newZ;
        } else
        {
            newZ += m_AngleIncrement;
            if (newZ < m_MaxAngle)
            {
                return newZ;
            } else
            {
                Time.timeScale = 1;
                m_Cycling = false;
                return -m_MaxAngle;
            }
        }
    }

    private void UpdateAngle()
    {
        var y = GetNextYRotation(m_LastRotation);
        var z = GetNextZRotation(m_LastRotation);
        m_LastRotation = new Vector3(0, y, z);
        SetLaserRotation(m_LastRotation);
    }
}
