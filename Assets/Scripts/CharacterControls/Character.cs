using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int id;
    public string name;

    public Vector3 characterPosition = Vector3.zero;

    public int currentSquareIndex = -1;

    public Character(int id, string name){
        this.id = id;
        this.name = name;
    }
}
