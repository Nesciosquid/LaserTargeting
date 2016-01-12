using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

    [SerializeField]
    private GameObject m_TargetMarker = null;

    private GameObject m_CurrentMarker = null;
    
    void CreateMarker(Vector3 pos)
    {
        if (m_TargetMarker != null)
        {
            if (m_CurrentMarker != null)
            {
                DestroyMarker();
            }
            var marker = (GameObject)Instantiate(m_TargetMarker, pos, Quaternion.identity);
            m_CurrentMarker = marker;
            marker.transform.SetParent(transform);
        }
    }

    void UpdateMarker(Vector3 pos)
    {
        if (m_TargetMarker != null && m_CurrentMarker != null)
        {
            m_CurrentMarker.transform.position = pos;
        }
    }

    void DestroyMarker()
    {
        if (m_CurrentMarker != null)
        {
            Destroy(m_CurrentMarker);
            m_CurrentMarker = null;
        }
    }

	void OnParticleCollision(GameObject other)
    {
        Debug.Log(gameObject.name + "Hit by particle!");
    }

    void OnCollisionEnter(Collision coll)
    {
       Debug.Log(gameObject.name + " entering collision with " + coll.gameObject.name);
       if (coll.gameObject.GetComponent<LaserController>())
        {

            CreateMarker(coll.contacts[0].point);
        }
    }

    void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.GetComponent<LaserController>())
        {
            Debug.Log(coll.gameObject.name);
            foreach (var contact in coll.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
            UpdateMarker(coll.contacts[0].point);
            Debug.DrawRay(coll.contacts[0].point, coll.contacts[0].normal, Color.white);
            return;
        }
    }

    void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.GetComponent<LaserController>())
        {
            DestroyMarker();
        }
    }
}
