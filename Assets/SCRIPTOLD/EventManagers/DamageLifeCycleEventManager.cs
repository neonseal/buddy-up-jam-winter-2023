using System;
using GameData;

public sealed class DamageLifeCycleEventManager    
{    
    // Singleton setup (https://www.c-sharpcorner.com/UploadFile/8911c4/singleton-design-pattern-in-C-Sharp/)
    private static readonly DamageLifeCycleEventManager instance = new DamageLifeCycleEventManager();    
    static DamageLifeCycleEventManager()    
    {    
    }    
    private DamageLifeCycleEventManager()    
    {    
    }    
    public static DamageLifeCycleEventManager Current    
    {    
        get    
        {    
            return DamageLifeCycleEventManager.instance;    
        }    
    }

    // Damage Management Events
    public event Action<PlushieDamage, DamageType> onGenerateDamage;
    public void generateDamage(PlushieDamage plushieDamage, DamageType damageType) {
        if (this.onGenerateDamage != null) {
            this.onGenerateDamage(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage, DamageType> onStartRepairMiniGame;
    public void startRepairMiniGame(PlushieDamage plushieDamage, DamageType damageType) {
        if (this.onStartRepairMiniGame != null) {
            this.onStartRepairMiniGame(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage, DamageType> onRepairDamage_Partial;
    public void repairDamage_Partial(PlushieDamage plushieDamage, DamageType damageType) {
        if (this.onRepairDamage_Partial != null) {
            this.onRepairDamage_Partial(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage> onRepairDamage_Complete;
    public void repairDamage_Complete(PlushieDamage plushieDamage) {
        if (this.onRepairDamage_Complete != null) {
            this.onRepairDamage_Complete(plushieDamage);
        }
    }
}   