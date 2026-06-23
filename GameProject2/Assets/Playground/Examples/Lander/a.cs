using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class a : MonoBehaviour
{
    GameObject _Player;
    GameObject _goal;
    Text _Info;
    Rigidbody2D _rb;
    void Start()
    {
        _Player = GameObject.Find("Spaceship");
        _goal = GameObject.Find("EndGoal");
        var temp = GameObject.Find("Text");
        _Info = temp.GetComponent<Text>();
        _rb = _Player.GetComponent<Rigidbody2D>();//C³
    }


    void Update()
    {
        //‹——£‚ğ‹‚ß‚é
        float distance = Vector2.Distance(_Player.transform.position, _goal.transform.position);
        _Info.text = distance.ToString("0.00");
        //‘¬‚³‚ğ‹‚ß‚é
        float velocity = _rb.velocity.magnitude;  //magnitude = ƒxƒNƒgƒ‹‚Ì‘å‚«‚³
        _Info.text += "/" + velocity.ToString("0.00");
    }
}