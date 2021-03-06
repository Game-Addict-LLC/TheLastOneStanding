using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFinisherActive : MonoBehaviour
{
    public KeyCode activateFinisher;
    public GameObject finisherObject;
    public List<Animator> objectsToAnimate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(activateFinisher))
        {
            finisherObject.SetActive(true);

            foreach (Animator anim in objectsToAnimate)
            {
                anim.SetTrigger("CallFinisher");
            }
        }
    }
}
