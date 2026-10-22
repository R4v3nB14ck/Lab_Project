
using UnityEngine;

public class LabDoors : MonoBehaviour
{
    public bool DoorOpen = false;
    public Transform LeftDoor, RightDoor;
    public AudioSource audioSource;
    public AudioClip openSound, closeSound, closedSound;

    Quaternion leftOpen = new Quaternion(-0.430459499f, -0.560985506f, -0.560985386f, 0.43045944f);
    Quaternion rightOpen = new Quaternion(-0.430459499f, 0.560985506f, 0.560985386f, 0.43045944f);
    Quaternion closed = Quaternion.Euler(270, 0, 0);

    bool wasOpen;
    bool closingSoundPlayed;

    void Start()
    {
        wasOpen = DoorOpen;
    }

    void Update()
    {
        if (DoorOpen != wasOpen)
        {
            audioSource.PlayOneShot(DoorOpen ? openSound : closeSound);
            wasOpen = DoorOpen;
            closingSoundPlayed = false;
        }

        Quaternion leftTarget = DoorOpen ? leftOpen : closed;
        Quaternion rightTarget = DoorOpen ? rightOpen : closed;

        LeftDoor.rotation = Quaternion.RotateTowards(LeftDoor.rotation, leftTarget, 90f * Time.deltaTime);
        RightDoor.rotation = Quaternion.RotateTowards(RightDoor.rotation, rightTarget, 90f * Time.deltaTime);

        if (!DoorOpen && !closingSoundPlayed &&
            Quaternion.Angle(LeftDoor.rotation, closed) < 0.01f &&
            Quaternion.Angle(RightDoor.rotation, closed) < 0.01f)
        {
            LeftDoor.rotation = closed;
            RightDoor.rotation = closed;
            audioSource.PlayOneShot(closedSound);
            closingSoundPlayed = true;
        }
    }
}
