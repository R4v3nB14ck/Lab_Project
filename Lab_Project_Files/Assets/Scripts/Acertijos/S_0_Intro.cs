using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class S_0_Intro : MonoBehaviour
{
    [Header("Alarm")]
    bool AlarmOn = false;
    public Light alarmLight;
    public AudioSource alarmAS;
    public AudioClip alarmSound;

    [Header("Dialogue")]
    public string[] firstDialogueLines;
    bool firstDialoguestarted = false;

    public string[] secondDialogueLines;
    bool secondDialoguestarted = false;

    [Header("References")]
    public LabDoors labDoorScript;
    public GameManager gameManager;
    public GameObject playButton;
    public GameObject Acertijo1;

    private void Start()
    {
        //Activar playbutton al iniciar el juego, y abrir las puertas.
        labDoorScript.DoorOpen = true;
        playButton.SetActive(true);
    }

    public void StartIntro() //Ejecutado por el botón PlayButton
    {
        gameManager.dialogueScript.startDialogue(firstDialogueLines);
        firstDialoguestarted = true;
    }

    public void CloseDoorsTrigger()
    {
        StartCoroutine(CloseDoors());
    }


    IEnumerator CloseDoors()
    {
        gameManager.LockPlayerMovement = true;

        // HERE TURN ON ALARM AND SOUND
        AlarmOn = true;
        alarmAS.PlayOneShot(alarmSound);

        labDoorScript.DoorOpen = false;

        // MOVE PLAYER
        Transform objeto = gameManager.Player.transform;

        Vector3 inicio = objeto.position;
        Vector3 destino = new Vector3(10f, 0f, 7.5f);

        Quaternion rotacionInicial = objeto.rotation;
        Quaternion rotacionFinal = Quaternion.Euler(0f, -90f, 0f);

        float tiempo = 0f;
        float duracion = 1.5f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            float t = Mathf.Clamp01(tiempo / duracion);
            float suavizado = Mathf.SmoothStep(0f, 1f, t);

            objeto.position = Vector3.Lerp(inicio, destino, suavizado);
            objeto.rotation = Quaternion.Slerp(
                rotacionInicial,
                rotacionFinal,
                suavizado
            );

            yield return null;
        }

        objeto.position = destino;
        objeto.rotation = rotacionFinal;

        yield return new WaitForSeconds(1f);

        AlarmOn = false;

        // MOSTRAR DIALOGO
        gameManager.dialogueScript.startDialogue(secondDialogueLines);
        secondDialoguestarted = true;
    }


    void Update()
    {
        AfterDialogue();
        AlarmLight();
    }

    void AlarmLight()
    {
        if (!AlarmOn)
        {
            alarmLight.gameObject.SetActive(false);
            return;
        }

        alarmLight.gameObject.SetActive(true);
        alarmLight.intensity = Mathf.Lerp(0.2f, 2.5f, (Mathf.Sin(Time.time * 7f) + 1f) / 2f);
    }

    void AfterDialogue()
    {
        //activar personaje después de terminar el primer dialogo.
        if (firstDialoguestarted && !gameManager.dialogueScript.DialoguePage.activeSelf)
        {
            firstDialoguestarted = false;
            gameManager.UnPause();
        }

        //Activar y mover personaje después del segundo dialogo.
        if (secondDialoguestarted && !gameManager.dialogueScript.DialoguePage.activeSelf)
        {
            secondDialoguestarted = false;
            gameManager.LockPlayerMovement = false;
            Acertijo1.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
