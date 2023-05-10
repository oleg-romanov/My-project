using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMouseScript : MonoBehaviour
{
    public string currentFoodPictureName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        // Сравниваем значение в текущем шестиугольнике со значением в главном (центральном)
        // Также передается параметр чтобы сравнивать без учета регистра
        if (MainHexagonScript.currentMainPictureName != null)
        {
            if (currentFoodPictureName.Equals(MainHexagonScript.currentMainPictureName))
            {
                MainHexagonScript.correctFoodPictureIsSelected = true;
            }
        }
    }
}
