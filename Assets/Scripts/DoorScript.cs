﻿using UnityEngine;using System.Collections;public class DoorScript : MonoBehaviour{    private void Start()    {        if (gameObject.name == "EndDoor")            transform.rotation = new Quaternion(0, 180, 0, 0);    }    private void OnTriggerEnter2D(Collider2D other)    {        if (gameObject.name == "EndDoor" && other.tag == "Player")        {            Debug.Log("Win!!!!!!!");        }    }}