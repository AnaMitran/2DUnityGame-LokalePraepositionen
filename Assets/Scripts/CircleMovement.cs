using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    private Vector3 distance;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            distance = transform.position - mousePosition;
            distance.z = 0f;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
            
            if (hit.transform != null && hit.transform.name == "Circle")
            {
                transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f) + distance;
            }

        }
        //TO DO: de folosit un bool global pentru a verifica daca obiectul e "prins" de mouse       
    }
}
