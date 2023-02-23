using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Button btn;
    public Animator animator;
    private Queue<string> sentences;

    private void Start() {       
        sentences = new Queue<string>();

        CustomEventManager.current.onTriggerDialogue += StartDialogue;
    }

    private void StartDialogue(Dialogue dialogue) {
        StartCoroutine(StartDialogueRoutine(dialogue));
    }

    public void DisplayNextSentence() {
        if (sentences.Count <= 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        if (sentences.Count == 0) {
            btn.gameObject.GetComponentInChildren<TMP_Text>().text = "END";
        }
    }

    IEnumerator StartDialogueRoutine(Dialogue dialogue) {
        // Initialize Dialogue Box for current speaker
        nameText.text = dialogue.name;
        dialogueText.text = "";

        animator.SetBool("isOpen", true);
        // Pause briefly to allow animation to finish before rendering text
        yield return new WaitForSeconds(.5f);


        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }

    private void EndDialogue() {
        animator.SetBool("isOpen", false);
    }
}
