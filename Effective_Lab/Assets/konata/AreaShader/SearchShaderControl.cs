using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchShaderControl : MonoBehaviour
{
    
    [SerializeField] float speed = 3;
    Material material;
    float dis;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           

            dis = 0;
        }

        Vector3 pos = MouseWorldPos.GetPos;
        material.SetVector("Vector3_CB8F0829",pos);

        dis += speed * Time.deltaTime;
        material.SetFloat("Vector1_FCA7DA60", dis);
    }
}
