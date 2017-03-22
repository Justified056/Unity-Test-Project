using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 0.25f;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 0.25f;

        transform.Translate(x, y, 0.0f);
	}
}
