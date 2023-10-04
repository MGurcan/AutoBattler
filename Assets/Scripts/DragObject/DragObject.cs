using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mousePosition;

    public GameObject[] squares;
    public Vector3[] squaresMiddlePoints;

    private int minDistanceSquareIndex = -1;

    public Material squareHoverMaterial;
    public Material squareDefaultMaterial;
    private void Start() {
        squaresMiddlePoints = new Vector3[squares.Length];
        for(int i = 0; i < squares.Length; i++){
            squaresMiddlePoints[i] = GetMiddlePointOfSquare(squares[i].transform);
        }
    }
    private Vector3 GetMousePosition(){
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown() {
        mousePosition = Input.mousePosition - GetMousePosition();
    }
    private void OnMouseUp() {
        transform.position = FindNearestSquare();
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

    private Vector3 FindNearestSquare(){
        float minDistance = float.MaxValue;
        Vector3 minDistanceSquareMiddlePoint = Vector3.zero;

        for(int i = 0; i < squares.Length; i++){
            float distance = Vector3.Distance(transform.position, squaresMiddlePoints[i]);

            if(distance < minDistance){
                minDistance = distance;
                minDistanceSquareMiddlePoint = squaresMiddlePoints[i];
                minDistanceSquareIndex = i;
            }
        }
        if(minDistance < 2f){
            return minDistanceSquareMiddlePoint;
        }
        minDistanceSquareIndex = -1;
        return transform.position;
    }
}
