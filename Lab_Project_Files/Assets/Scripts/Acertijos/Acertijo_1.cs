using TMPro;
using UnityEngine;

public class Acertijo_1 : MonoBehaviour
{
    public bool CanOpenDoor = false;
    public Transform LeftDoor;
    public Transform RightDoor;

    public TMP_Dropdown Enunciado1;
    public TMP_Dropdown Enunciado2;
    public TMP_Dropdown Enunciado3;

    public void CheckCombination()
    {
        if(Enunciado1.value == 3 && Enunciado2.value == 1 && Enunciado3.value == 2)
        {
            Enunciado1.interactable = false;
            Enunciado2.interactable = false;
            Enunciado3.interactable = false;
            CanOpenDoor = true;
        }
    }

    private void Update()
    {
        if(CanOpenDoor)
        {
            LeftDoor.rotation = Quaternion.Lerp(LeftDoor.rotation, new Quaternion(-0.430459499f, -0.560985506f, -0.560985386f, 0.43045944f), Time.deltaTime * 2);
            RightDoor.rotation = Quaternion.Lerp(RightDoor.rotation, new Quaternion(-0.430459499f, 0.560985506f, 0.560985386f, 0.43045944f), Time.deltaTime * 2);
        }
        else
        {
            LeftDoor.eulerAngles = new Vector3(270, 0, 0);
            RightDoor.eulerAngles = new Vector3(270, 0, 0);
        }
    }
}
