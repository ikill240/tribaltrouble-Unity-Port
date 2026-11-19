// Translated from TierContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 16
    public class TierContainer : com.oddlabs.tt.model.DeployContainer
    {
        public const int STONE = 0;
        public const int IRON = 1;
        public const int CHICKEN = 2;
        public const int RUNE = 3;
        public static int[][] COSTS;
        public static float[] SECONDS;
        public int kind;
        public com.oddlabs.tt.model.Building paid_at;
        public bool working;
        // Java: 40
        public int getKind()
        {
            return kind;
        }
        // Java: 44
        public static int[] getCost(int kind)
        {
            return com.oddlabs.tt.model.TierContainer.COSTS[kind];
        }
        // Java: 48
        public static float getSeconds(int kind)
        {
            return com.oddlabs.tt.model.TierContainer.SECONDS[kind];
        }
        // Java: 52
        public com.oddlabs.tt.model.LandBuilding land()
        {
            return ((com.oddlabs.tt.model.LandBuilding)building);
        }
        // Java: 56
        public bool isWorking()
        {
            return working;
        }
        // Java: 61
        public bool isDone()
        {
            com.oddlabs.tt.model.LandBuilding b = land();
            return ((kind) switch { 0 => (bool)(b.getTier() >= 1), 1 => (bool)(b.getTier() >= 2), 3 => (bool)(b.getTier() >= 3), _ => (bool)(b.hasChickenBarbs()) });
        }
        // Java: 72
        public bool isLocked()
        {
            return kind == com.oddlabs.tt.model.TierContainer.IRON && land().getTier() < 1 || kind == com.oddlabs.tt.model.TierContainer.RUNE && land().getTier() < 2;
        }
        // Java: 77
        public bool otherTierWorking()
        {
            if (kind == com.oddlabs.tt.model.TierContainer.CHICKEN)
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.DeployType other in new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.UPGRADE_STONE, com.oddlabs.tt.model.DeployType.UPGRADE_IRON, com.oddlabs.tt.model.DeployType.UPGRADE_RUNE })
            {
                if (((object)other != (object)deploy_type) && (building.getDeployContainer(other) is com.oddlabs.tt.model.TierContainer t) && t.isWorking())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 86
        public bool canAfford()
        {
            return com.oddlabs.tt.model.ExtendedResources.findPayer(building.getOwner(), building, com.oddlabs.tt.model.TierContainer.COSTS[kind]) != null;
        }
        // Java: 90
        public bool canOrder()
        {
            return !working && !isDone() && !isLocked() && !otherTierWorking() && canAfford();
        }
        // Java: 94
        public override void orderSupply(int orders)
        {
            if (orders > 0 && canOrder())
            {
                paid_at = com.oddlabs.tt.model.ExtendedResources.findPayer(building.getOwner(), building, com.oddlabs.tt.model.TierContainer.COSTS[kind]);
                if (paid_at == null)
                {
                    return;
                }
                com.oddlabs.tt.model.ExtendedResources.pay(paid_at, com.oddlabs.tt.model.TierContainer.COSTS[kind], 1);
                working = true;
                time = 0;
                supply_count = 1;
            }
            else if (orders < 0 && working)
            {
                if (paid_at != null && !paid_at.isDead())
                {
                    com.oddlabs.tt.model.ExtendedResources.pay(paid_at, com.oddlabs.tt.model.TierContainer.COSTS[kind], (-1));
                }
                stop();
            }
        }
        // Java: 111
        public void stop()
        {
            working = false;
            time = 0;
            supply_count = 0;
        }
        // Java: 117
        public override int increaseSupply(int amount)
        {
            supply_count += amount;
            return amount;
        }
        // Java: 124
        public override void deploy(float amount)
        {
            if (!working)
            {
                return;
            }
            time += amount;
            if (time >= com.oddlabs.tt.model.TierContainer.SECONDS[kind])
            {
                stop();
                switch ((kind))
                {
                    case 0:
                    {
                        land().setTier(1);
                        break;
                    }
                    case 1:
                    {
                        land().setTier(2);
                        break;
                    }
                    case 3:
                    {
                        land().setTier(3);
                        break;
                    }
                    default:
                    {
                        land().setChickenBarbs();
                        break;
                    }
                }
            }
        }
        // Java: 140
        public override float getBuildProgress()
        {
            return (working ? (float)(time / com.oddlabs.tt.model.TierContainer.SECONDS[kind]) : 0.0f);
        }
        static TierContainer()
        {
            TierContainer.COSTS = new int[][] { new int[] { 10, 20, 0, 0, 0, 0, 0, 0 }, new int[] { 10, 10, 15, 0, 5, 0, 0, 5 }, new int[] { 5, 0, 0, 4, 0, 0, 0, 0 }, new int[] { 20, 20, 20, 0, 8, 6, 0, 4, 1 } };
            TierContainer.SECONDS = new float[] { 30.0f, 45.0f, 20.0f, 60.0f };
        }
        public TierContainer(com.oddlabs.tt.model.LandBuilding building, com.oddlabs.tt.model.DeployType type, int kind) : base(building, com.oddlabs.tt.model.TierContainer.SECONDS[kind], type, null)
        {
            this.kind = kind;
        }
    }
}
