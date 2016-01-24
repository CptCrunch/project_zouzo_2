using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    protected Test() { }
    private static Test _instance = null;

    public int TestInt = 0;

    public static Test Instance { get { return Test._instance == null ? new Test() : Test._instance; } }
	
}
