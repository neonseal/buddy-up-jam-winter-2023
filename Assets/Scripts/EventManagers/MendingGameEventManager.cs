using System;
using GameData;

public class MendingGameEventManager {
    private static readonly MendingGameEventManager instance = new MendingGameEventManager();
    static MendingGameEventManager() { }
    private MendingGameEventManager() { }
    public static MendingGameEventManager Current {
        get {
            return MendingGameEventManager.instance;
        }
    }

    // Node Interaction
    public event Action<Node> onNodeTriggered;
    public void NodeTriggered(Node node) {
        if (this.onNodeTriggered != null) {
            this.onNodeTriggered(node);
        }
    }

    // Mending Game Complete 
    public event Action onMendingGameComplete;
    public void MendingGameComplete() {
        if (this.onMendingGameComplete != null) {
            this.onMendingGameComplete();
        }
    }
}
