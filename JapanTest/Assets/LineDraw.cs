using UnityEngine;
using System.Collections;

public class LineDraw : MonoBehaviour {

    public GameObject _beforeObj { get; set; }
    public GameObject _afterObj { get; set; }

    [SerializeField]
    private LineRenderer lineRenderer = null;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(_beforeObj == null || _afterObj == null)
        {
            Destroy(gameObject);
            return;
        }

        //lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, _beforeObj.transform.position);
        lineRenderer.SetPosition(1, _afterObj.transform.position);
    }
}
