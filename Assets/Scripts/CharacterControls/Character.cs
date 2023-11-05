using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Character
{
    public int id;
    public int objectID;
    public string name;

    public int level = 1;
    public Vector3 characterPosition = Vector3.zero;

    public int currentSquareIndex = -1;

    public Character(int id, string name){
        this.id = id;
        this.name = name;
    }
}
