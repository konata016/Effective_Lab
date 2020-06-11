using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorldPos : MonoBehaviour
{
    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            float max_distance = 200f;

            if (Physics.Raycast(ray, out hit, max_distance))
            {
                GetPos = hit.point;
                pos = GetPos;
            }
        }
    }

    public static Vector3 GetPos { get; private set; }
}
