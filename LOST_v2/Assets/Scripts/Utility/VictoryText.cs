using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Text>().text = GameManager.instance.winner + "\n" + GameManager.instance.winLossRecord;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
