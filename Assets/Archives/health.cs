using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public int healthVal = 100;
    public int mana = 50;
    [SerializeField]
    private int experience = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        healthVal -= 20;
    }

    // Update is called once per frame
    void Update()
    {
        experience += 1;
    }
}
