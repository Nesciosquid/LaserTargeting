using UnityEngine;
using System.Collections;

public class TargetController : MonoBehaviour {

    [SerializeField]
    private Material m_DefaultMaterial;
    [SerializeField]
    private Material m_HighlightMaterial;

    private MeshRenderer m_Renderer;

	// Use this for initialization
	private void Start () {
        m_Renderer = GetComponent<MeshRenderer>();
        Default();
	}
	
    private void Highlight()
    {
        if (m_HighlightMaterial)
        {
            m_Renderer.material = m_HighlightMaterial;
        }
    }
    
    private void Default()
    {
        if (m_DefaultMaterial)
        {
            m_Renderer.material = m_DefaultMaterial;
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        var holder = coll.gameObject.GetComponentInParent<LaserHolderController>();
        holder.RegisterDetection(gameObject);
        Highlight();
    }

    private void OnTriggerExit(Collider coll)
    {
        Default();
    }
}
