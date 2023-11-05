using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public int lastCharacterID = 0;
    public List<Character> characterList = new List<Character>();
    public List<GameObject> charactersOnScene = new List<GameObject>(); 

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

        characterList[characterList.Count - 1].characterPosition = currentTransform.position;
        newObject.GetComponent<DragObject>().squares = squares;
        characterList[characterList.Count - 1].objectID = lastCharacterID;
        newObject.GetComponent<DragObject>().objectID = lastCharacterID++;
        newObject.transform.localScale = new Vector3(newCharacter.level / 5f, newCharacter.level / 5f, newCharacter.level / 5f);
        characterList[characterList.Count - 1].characterPosition = newCharacter.characterPosition;
        characterList[characterList.Count - 1].currentSquareIndex = (newCharacter.characterPosition == Vector3.zero) ? FindFirstEmptyBaseSquareIndex() : newCharacter.currentSquareIndex;
        currentTransform.position = (newCharacter.characterPosition == Vector3.zero) ? GetSquarePosition(characterList[characterList.Count - 1].currentSquareIndex) : newCharacter.characterPosition;

        charactersOnScene.Add(newObject);
    }

    private Vector3 GetSquarePosition(int squareIndex){
        return new Vector3(squares[squareIndex].GetComponent<Square>().squareTransform.position.x, squares[squareIndex].GetComponent<Square>().squareTransform.position.y+1, squares[squareIndex].GetComponent<Square>().squareTransform.position.z);
    }

    private int FindFirstEmptyBaseSquareIndex(){
        for(int i = 0; i < squares.Length; i++){
            if(squares[i].GetComponent<Square>().isBaseSquare && squares[i].GetComponent<Square>().IsEmpty && squares[i].GetComponent<Square>().IsPlaceble){
                squares[i].GetComponent<Square>().IsEmpty = false;
                int index = i;
                return index;
            }
        }
        return -1;  // TODO: if returns null add control to "InstantiateCharactersToBase" @mgurcan
    }

    public void LevelUpCharacter(Character compareCharacter){
        bool recursiveFlag = false;
        int count = 0;
        foreach(Character ch in characterList ){
            if(compareCharacter.id == ch.id && compareCharacter.level == ch.level){
                count++;
            }
        }
        if(count >= 3){
            recursiveFlag = true;
            for(int i = 0; i<characterList.Count; i++){
                if(characterList[i].level == compareCharacter.level && characterList[i].id == compareCharacter.id){
                    squares[characterList[i].currentSquareIndex].GetComponent<Square>().IsEmpty = true;

                    for(int j = 0; j < charactersOnScene.Count; j++){
                        if(charactersOnScene[j].GetComponent<DragObject>().objectID == characterList[i].objectID){
                            Destroy(charactersOnScene[j]);
                            charactersOnScene.RemoveAt(j);
                        }
                    }
                }
            }
            characterList = characterList.Where(ch => !(ch.level == compareCharacter.level && ch.id == compareCharacter.id)).ToList();
            Character newCharacter = new Character(compareCharacter.id, compareCharacter.name);
            newCharacter.level = compareCharacter.level + 1;

            characterList.Add(newCharacter);
            InstantiateCharacter(newCharacter);

            if(recursiveFlag){
                LevelUpCharacter(newCharacter);
            }
        }

    }

    public Character FindCharacterOnList(int objectID){
        foreach(Character ch in characterList)
            if(objectID == ch.objectID) return ch;
        return null;
    }
}

