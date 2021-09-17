using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    private Vector3 distance;

    private bool objectCatch;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //primul frame in care mouse-ul este apasat
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
            distance = transform.position - mousePosition;
            distance.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);  //returneza obiectele de sub mouse (Raycast = perpendiculara pe ecran)

            if (hit.transform != null)  // verificam daca a fost "lovit"(hit) un obiect
            {
                if (hit.transform.name == "Circle")   // verificam daca obiectul lovit este cel de interes
                {
                    
                    objectCatch = true;
                }
                else
                {
                    objectCatch = false;  // redundant
                }   
            }
        }
        if (objectCatch) //fiecare frame in care mouse-ul este tinut apasat
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f) + distance;
        }   
        
        if (Input.GetMouseButtonUp(0)) //eliberam obiectul daca mouse-ul nu mai este apasat
        {
            objectCatch = false;
        }
    }
}
