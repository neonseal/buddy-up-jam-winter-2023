using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameLoop
{
    public class GameLoopManager : MonoBehaviour
    {
        internal static PlushieScriptableObject currentPlushieScriptableObject;

        [SerializeField]
        private ClientDialogueManager dialogueManager;
        [SerializeField]
        private List<PlushieScriptableObject> plushieList;
        [SerializeField]
        private CardStack cardStackController;
        [SerializeField]
        private GameObject cardSpawner;
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private GameObject title;
        [SerializeField]
        private TutorialSequenceScriptableObject StartingTutorial;

        private List<ClientCard> clientCardCollection;
        private bool isGameActive;
        private bool _isWorkingOnPlushie;
        private int plushieListIndex;

#if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                SceneHelper.ReloadScene();
            }
        }
#endif

        private void Awake()
        {
            isGameActive = false;
            DOTween.Init();
            // Initialize plushie list cursor
            this.plushieListIndex = -1;
            this._isWorkingOnPlushie = false;
            this.clientCardCollection = new List<ClientCard>();
            // Subscribe methods to event triggers
            CustomEventManager.Current.onGameStart += this.StartGame;
            // Send off plushie when repair is complete
            PlushieLifeCycleEventManager.Current.onFinishPlushieRepair += this.PlushieSendoff;
            // Check for whether to send in next plushie or not (the bell rings regardless)
            PlushieLifeCycleEventManager.Current.onRingBell += this.receiveBellRing;
        }

        private void Start()
        {

        }

        // Update the scene to bring in a new customer's plushie, note, and information
        private void StartGame()
        {
            this.startButton.gameObject.SetActive(false);
            this.title.SetActive(false);
            this.isGameActive = true;
            // Start first tutorial
            TutorialSequenceEventManager.Current.StartTutorialSequence(StartingTutorial);
        }

        IEnumerator StartNextCustomerRoutine()
        {
            this.plushieListIndex++;
            // Set current plushie scriptable object
            currentPlushieScriptableObject = this.plushieList[plushieListIndex];
            this._isWorkingOnPlushie = true;

            // Set client dialogue font
            this.dialogueManager.SetClientStyling(currentPlushieScriptableObject);

            yield return new WaitForSeconds(.5f);

            CustomEventManager.Current.TriggerDialogue(currentPlushieScriptableObject);
        }

        private void PlushieSendoff()
        {
            StartCoroutine(PlushieSendoffRoutine());
        }

        IEnumerator PlushieSendoffRoutine()
        {
            /* Complete Repair and Send Plushie to Customer */
            // Play repair complete fanfare
            // Wait briefly
            yield return new WaitForSeconds(.4f);
            // Move plushie off screen and destroy it
            PlushieLifeCycleEventManager.Current.sendOffPlushie();
            this._isWorkingOnPlushie = false;


            // Wait briefly
            yield return new WaitForSeconds(.65f);

            /* Show Client Resolution Card */
            // Create resolution text object, and instantiate above the screen
            ClientCard clientCard = currentPlushieScriptableObject.resolutionClientCard;
            clientCard.name = currentPlushieScriptableObject.plushieObjectName + "ClientCard";
            clientCard.gameObject.transform.localScale = new Vector3(5, 7, 1);
            ClientCard newCard = Instantiate(clientCard, this.cardSpawner.transform.position, Quaternion.identity, this.cardStackController.transform);

            //yield return new WaitForSeconds(2f);

            // Tween the card into view
            Sequence sequence = DOTween.Sequence();
            sequence.Append(newCard.transform.DOLocalMove(new Vector3(0, 0, -1), 1.5f).SetEase(Ease.OutBack));
            DOTween.Play(sequence);

            clientCardCollection.Add(newCard);
        }

        public void PlayCardAnimation()
        {
            ClientCard card = clientCardCollection[0];

            float yPos = card.transform.localPosition.y;
            float targetY = yPos == 0f ? 1000f : 0f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOLocalMove(new Vector3(0, targetY, -1), 1.5f).SetEase(Ease.InOutBack));
            DOTween.Play(sequence);
        }

        private void receiveBellRing()
        {
            if (!this._isWorkingOnPlushie && isGameActive)
            {
                this.StartCoroutine(StartNextCustomerRoutine());
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe all methods from event managers
            CustomEventManager.Current.onGameStart -= this.StartGame;
            PlushieLifeCycleEventManager.Current.onFinishPlushieRepair -= this.PlushieSendoff;
            PlushieLifeCycleEventManager.Current.onRingBell -= this.receiveBellRing;
        }
    }
}