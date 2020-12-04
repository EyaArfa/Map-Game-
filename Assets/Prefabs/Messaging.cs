using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Messaging : MonoBehaviour
{
    public CarManipulator script;
    public GameObject menu;
    public Dropdown sendTo;
    public Text msg;
    public Transform rcv;
    public GameObject msgCanvas;
    
    public void Start()
    {
        menu.SetActive(false);
        script = GameObject.FindWithTag("cars").GetComponent<CarManipulator>();
    }
    
    public void MsgReceive(String m)
    {
        msg.text = m;
        Instantiate(msg, rcv);
    }
    public void send(String location)
    {
       
        script.cars[sendTo.value].SendMessage("MsgReceive", "from " + script.cars[script.i].name + ": " + location); 

    }
    public void ShareLocation()
    {
        String location = " location :  " + script.cars[script.i].transform.position.ToString();
        send(location);
    }

    // Update is called once per frame
    void Update()
    {
        if (script.cars[script.i].name == this.name)
        {
            msgCanvas.SetActive(true);
        }
        else
        {
            msgCanvas.SetActive(false);
        }
        sendTo.ClearOptions();
        foreach (GameObject c in script.cars)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = c.name;
            sendTo.options.Add(option);
        }

    }
    
}
