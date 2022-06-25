using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

	public ServerComm serverComm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("Space key was pressed; attempting to leave server");
			StartCoroutine(serverComm.LeaveServer());
        }
    }
}
