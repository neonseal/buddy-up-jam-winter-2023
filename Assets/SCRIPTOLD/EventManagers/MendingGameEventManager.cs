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

    public event Action onDashTriggered;
    public void DashTriggered() {
        if (this.onDashTriggered != null) {
            this.onDashTriggered();
        }
    }

    // Mending Game Complete 
    public event Action<PlushieDamage_old> onMendingGameComplete;
    public void MendingGameComplete(PlushieDamage_old plushieDamage) {
        if (this.onMendingGameComplete != null) {
            this.onMendingGameComplete(plushieDamage);
        }
    }
}
