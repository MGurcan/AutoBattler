using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Character> characterList = new List<Character>(); 

    public Shop shop;

    public GameObject[] characterPrefabs;
    public GameObject[] squares;

    private void Start()
    {
        StartCoroutine(InstantiateCharactersToBase());
    }

    private void Update() {

    }

    private IEnumerator InstantiateCharactersToBase()
    {
        for(int i = 0; i< characterList.Count; i++){
            InstantiateCharacter(characterList[i]);
            yield return new WaitForSeconds(0.5f);
        }

    }

    public void InstantiateCharacter(Character newCharacter){
        GameObject newObject = Instantiate(characterPrefabs[newCharacter.id], Vector3.zero, Quaternion.Euler(0, 0, 0));
        Transform currentTransform = newObject.transform;
        currentTransform.position = (newCharacter.characterPosition == Vector3.zero) ? FindFirstEmptyBaseSquare() : newCharacter.characterPosition;

        characterList[characterList.Count - 1].characterPosition = currentTransform.position;
        newObject.GetComponent<DragObject>().squares = squares;
        
        newObject.AddComponent<Character>();
        newObject.GetComponent<Character>().id = newCharacter.id;
        newObject.GetComponent<Character>().name = newCharacter.name;
        newObject.GetComponent<Character>().characterPosition = newCharacter.characterPosition;
    }

    private Vector3 FindFirstEmptyBaseSquare(){
        for(int i = 0; i < squares.Length; i++){
            if(squares[i].GetComponent<Square>().isBaseSquare && squares[i].GetComponent<Square>().IsEmpty && squares[i].GetComponent<Square>().IsPlaceble){
                squares[i].GetComponent<Square>().IsEmpty = false;

                return new Vector3(squares[i].GetComponent<Square>().squareTransform.position.x, squares[i].GetComponent<Square>().squareTransform.position.y+1, squares[i].GetComponent<Square>().squareTransform.position.z);
            }
        }
        return Vector3.zero;    // TODO: if returns null add control to "InstantiateCharactersToBase" @mgurcan
    }
}

