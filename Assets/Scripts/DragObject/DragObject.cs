using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mousePosition;

    public GameObject[] squares;

    private int minDistanceSquareIndex = -1;

    public Material squareHoverMaterial;
    public Material squareDefaultMaterial;

    public int objectID;

    private GameManager gameManager;
    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        squares = gameManager.squares;
    }
    private Vector3 GetMousePosition(){
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown() {
        Character currentCharacter = gameManager.FindCharacterOnList(objectID);
        if(currentCharacter.currentSquareIndex != -1){
            squares[currentCharacter.currentSquareIndex].GetComponent<Square>().IsEmpty = true;
        }
            
        mousePosition = Input.mousePosition - GetMousePosition();
    }
    private void OnMouseUp() {
        Character currentCharacter = gameManager.FindCharacterOnList(objectID);
        if(minDistanceSquareIndex != -1){
            squares[minDistanceSquareIndex].GetComponent<Square>().IsEmpty = false;

            currentCharacter.characterPosition = squares[minDistanceSquareIndex].GetComponent<Square>().squareTransform.position;
            currentCharacter.currentSquareIndex = minDistanceSquareIndex;
        }
        Vector3 temp = squares[currentCharacter.currentSquareIndex].GetComponent<Square>().squareTransform.position;
        transform.position = new Vector3(temp.x, temp.y+1, temp.z);
    }
    private void OnMouseDrag() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        FindNearestSquare();

        for(int i = 0; i < squares.Length; i++){
            squares[i].GetComponent<Renderer>().material = squareDefaultMaterial;
        }

        if(minDistanceSquareIndex != -1){
            squares[minDistanceSquareIndex].GetComponent<Renderer>().material = squareHoverMaterial;
        }
    }

    private Vector3 GetMiddlePointOfSquare(Transform square){
        Vector3 dimensionsOfSqure = square.localScale;
        return square.position + new Vector3(0f, 0.5f * dimensionsOfSqure.y, 0f); /* - 0.5f * dimensionsOfSqure */;
    }

    private void FindNearestSquare(){
        float minDistance = float.MaxValue;

        for(int i = 0; i < squares.Length; i++){
            if(squares[i].GetComponent<Square>().IsEmpty && squares[i].GetComponent<Square>().IsPlaceble){
                float distance = Vector3.Distance(transform.position, squares[i].transform.position);

                if(distance < minDistance){
                    minDistance = distance;
                    minDistanceSquareIndex = i;
                }
            }
        }
        if(minDistance > 2f)
            minDistanceSquareIndex = -1;
    }
}
