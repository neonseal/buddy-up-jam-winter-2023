using System;

public sealed class PlushieLifeCycleEventManager    
{    
    // Singleton setup (https://www.c-sharpcorner.com/UploadFile/8911c4/singleton-design-pattern-in-C-Sharp/)
    private static readonly PlushieLifeCycleEventManager instance = new PlushieLifeCycleEventManager();    
    static PlushieLifeCycleEventManager()    
    {    
    }    
    private PlushieLifeCycleEventManager()    
    {    
    }    
    public static PlushieLifeCycleEventManager Current    
    {    
        get    
        {    
            return PlushieLifeCycleEventManager.instance;    
        }    
    }   

     // Plushie spawning event
    public event Action<PlushieScriptableObject> onGeneratePlushie;
    public void generatePlushie(PlushieScriptableObject plushieScriptableObject) {
        if (this.onGeneratePlushie != null) {
            this.onGeneratePlushie(plushieScriptableObject);
        }
    }

    // Plushie overall repair complete event
    public event Action onFinishPlushieRepair;
    public void finishPlushieRepair() {
        if (this.onFinishPlushieRepair != null) {
            this.onFinishPlushieRepair();
        }
    }

    // Plushie sendoff event
    public event Action onSendOffPlushie;
    public void sendOffPlushie() {
        if (this.onSendOffPlushie != null) {
            this.onSendOffPlushie();
        }
    }

    // Plushie deletion event
    public event Action onDeletePlushie;
    public void deletePlushie() {
        if (this.onDeletePlushie != null) {
            this.onDeletePlushie();
        }
    }
}   