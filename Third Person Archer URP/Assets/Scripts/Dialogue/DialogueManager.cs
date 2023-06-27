using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image avatar;
    [SerializeField] private Button continueButton;

    private Animator animator;
    private Queue<string> sentencesTyping;
    private Queue<string> sentencesSkip;
    private Coroutine typingSentence;

    // Start is called before the first frame update
    void Start()
    {
        sentencesTyping = new Queue<string>();
        sentencesSkip = new Queue<string>();
        animator = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Animator>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        avatar.sprite = dialogue.avatar;
        sentencesTyping.Clear();

        // Get dialogue sentence for typing effect
        foreach (string sentence in dialogue.sentences)
        {
            sentencesTyping.Enqueue(sentence);
        }

        // Get dialogue sentence for skip typing effect
        sentencesSkip.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentencesSkip.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentencesTyping.Count > 0)
        {
            string sentence = sentencesTyping.Dequeue();
            typingSentence = StartCoroutine(StartTyping(sentence));
            continueButton.interactable = false;
        }
        else if (sentencesTyping.Count == 0)
        {
            EndDialogue();
            return;
        }
    }

    public void SkipTyping()
    {
        if (typingSentence != null)
        {
            string sentence = sentencesSkip.Dequeue();
            StopCoroutine(typingSentence);
            typingSentence = null;
            dialogueText.text = sentence;
            continueButton.interactable = true;
        }
    }

    private IEnumerator StartTyping (string sentence)
    {
        float typeSpeed = 0.05f;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
            if (dialogueText.text.Length == sentence.Length)
            {
                sentencesSkip.TryDequeue(out sentence);
                continueButton.interactable = true;
            }
        }
    }

    private void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

}
