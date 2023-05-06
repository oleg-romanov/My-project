using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class MouseFollow : MonoBehaviour
{

    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer = new byte[1024];

    public Vector2 targetPosition; // Заданные координаты (x,y)


    float mainSpeed = 10.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.05f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Подключение к серверу
        client = new TcpClient();
        client.Connect("localhost", 9097);
    }

    // Update is called once per frame
    void Update()
    {
        // Получение координат
        stream = client.GetStream();
        int bytes = stream.Read(buffer, 0, buffer.Length);
        string data = Encoding.ASCII.GetString(buffer, 0, bytes);

        // Разбор координат
        string[] coordinates = data.Split(',');
        float x = float.Parse(coordinates[0]);
        float y = float.Parse(coordinates[1]);

        Debug.Log($"Coordinates: ({x}, {y})");

        //targetPosition = new Vector2(x, y);

        //Vector3 targetPos = new Vector3(targetPosition.x, targetPosition.y, 0);
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 direction = targetPos - mousePos;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //lastMouse = Input.mousePosition - lastMouse;

        lastMouse = new Vector3(x: x, y: y, 0) - lastMouse;
        //lastMouse = new Vector3(lastMouse.x, lastMouse.y - y, lastMouse.z);

        //lastMouse = lastMouse - new Vector3(x: x, y: y, 0);

        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
        transform.eulerAngles = lastMouse;
        //transform.rotation = Quaternion.Euler(Mathf.Clamp(lastMouse.y, -90f, 90f), lastMouse.x, 0f);
        //lastMouse = Input.mousePosition;
        lastMouse = new Vector3(x: x, y: y, 0);
        //Mouse  camera angle done.  

        //Keyboard commands
        // float f = 0.0f;ß
        Vector3 p = new Vector3(targetPosition.x, targetPosition.y, 0);



        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }
    }
}
