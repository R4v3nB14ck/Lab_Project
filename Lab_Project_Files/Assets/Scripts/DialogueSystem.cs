using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    [Header("Config")]
    public string[] lines;
    public float textSpeed;

    [Header("References")]
    public GameObject DialoguePage;
    public TextMeshProUGUI textBox;
    public InputActionReference InteractAction;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip StartSound;
    public AudioClip textSound;
    public AudioClip SkipSound;
    public AudioClip EndSound;

    [Header("Animación")]
    public RectTransform dialoguePanel;
    public float slideDistance = 300f;
    public float slideDuration = 0.4f;

    Vector2 originalPosition;
    Coroutine dialogueAnimation;
    bool isAnimating;

    private int index;

    void Start()
    {
        originalPosition = dialoguePanel.anchoredPosition;
    }

    private void Update()
    {
        if(DialoguePage.activeSelf && !isAnimating && InteractAction.action.WasPressedThisFrame() && lines.Length > 0)
        {
            if (audioSource) audioSource.PlayOneShot(SkipSound);

            if (textBox.text == lines[index])
            {
                nextLine();
            }
            else
            {
                StopAllCoroutines();
                textBox.text = lines[index];
            }
        }
    }

    public void startDialogue(string[] dialoguelines)
    {
        StopAllCoroutines();

        lines = dialoguelines;
        index = 0;
        textBox.text = string.Empty;

        DialoguePage.SetActive(true);
        dialoguePanel.anchoredPosition = originalPosition - Vector2.up * slideDistance;

        if (audioSource) audioSource.PlayOneShot(StartSound);

        dialogueAnimation = StartCoroutine(ShowDialogue());
    }


    IEnumerator ShowDialogue()
    {
        isAnimating = true;
        float t = 0f;
        Vector2 start = dialoguePanel.anchoredPosition;

        while (t < slideDuration)
        {
            t += Time.unscaledDeltaTime;
            float smooth = Mathf.SmoothStep(0f, 1f, t / slideDuration);
            dialoguePanel.anchoredPosition = Vector2.Lerp(start, originalPosition, smooth);
            yield return null;
        }

        dialoguePanel.anchoredPosition = originalPosition;
        isAnimating = false;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textBox.text += c;
            if (audioSource) audioSource.PlayOneShot(textSound);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void nextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            textBox.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            StartCoroutine(HideDialogue());
        }
    }

    IEnumerator HideDialogue()
    {
        isAnimating = true;
        textBox.text = string.Empty;

        if (audioSource) audioSource.PlayOneShot(EndSound);

        float t = 0f;
        Vector2 start = dialoguePanel.anchoredPosition;
        Vector2 target = originalPosition - Vector2.up * slideDistance;

        while (t < slideDuration)
        {
            t += Time.unscaledDeltaTime;
            float smooth = Mathf.SmoothStep(0f, 1f, t / slideDuration);
            dialoguePanel.anchoredPosition = Vector2.Lerp(start, target, smooth);
            yield return null;
        }

        dialoguePanel.anchoredPosition = target;
        DialoguePage.SetActive(false);
        dialoguePanel.anchoredPosition = originalPosition;
        lines = new string[0];
        isAnimating = false;
    }
}
