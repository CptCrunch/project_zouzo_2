using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    public GameObject partner;
    public Color rayColor = Color.cyan;

    void Awake ()
    {
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (gameObject.GetComponent<RayCollider>().showRays != CustomDebug.IsTagActive("MapFeature")) { gameObject.GetComponent<RayCollider>().showRays = CustomDebug.IsTagActive("MapFeature"); }
        if (gameObject.GetComponent<RayCollider>().rayColor != rayColor) { gameObject.GetComponent<RayCollider>().rayColor = rayColor; }

        if (gameObject.GetComponent<RayCollider>().collision.value.right)
        {
            Vector3 collisonsPoint = transform.position - gameObject.GetComponent<RayCollider>().collision.gameObject.any.transform.position;
            gameObject.GetComponent<RayCollider>().collision.gameObject.any.transform.position = partner.transform.position - collisonsPoint + new Vector3(0.1f, 0, 0);
        }

        if (gameObject.GetComponent<RayCollider>().collision.value.left)
        {
            Vector3 collisonsPoint = transform.position - gameObject.GetComponent<RayCollider>().collision.gameObject.any.transform.position;
            gameObject.GetComponent<RayCollider>().collision.gameObject.any.transform.position = partner.transform.position - collisonsPoint - new Vector3(0.1f, 0, 0);
        }

    }
}
