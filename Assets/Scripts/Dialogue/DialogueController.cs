using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    private Queue<string> sentences;
    private void Start() {
        sentences = new Queue<string>();
        CustomEventManager.current.onTriggerDialogue += StartDialogue;
    }

    private void StartDialogue(Dialogue dialogue) {
        Debug.Log("Starting conversation with " + dialogue.name);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count <= 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
    }

    private void EndDialogue() {
        Debug.Log("END OF CONVERSATION");
    }
}
