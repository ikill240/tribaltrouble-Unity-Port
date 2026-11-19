// Translated from GameLog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 30
    public class GameLog : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public java.nio.file.Path file;
        public java.lang.StringBuilder pending;
        public bool failed;
        // Java: 41
        public void add(string line)
        {
            pending.append(line).append(java.lang.JSystem.lineSeparator());
        }
        // Java: 45
        public void flush()
        {
            if (pending.isEmpty() || failed)
            {
                return;
            }
            try
            {
                java.nio.file.Files.writeString(file, pending, java.nio.file.StandardOpenOption.CREATE, java.nio.file.StandardOpenOption.APPEND);
            }
            catch (System.IO.IOException e)
            {
                failed = true;
                com.oddlabs.tt.player.ai.GameLog.logger.log(java.util.logging.Level.WARNING, "Cannot write the expert AI log " + java.lang.JStr.valueOf(file), e);
            }
            pending.setLength(0);
        }
        // Java: 58
        public static string clock(float seconds)
        {
            int tenths = java.lang.Math.round((float)(seconds * 10.0f));
            return java.lang.JString.format(java.util.Locale.ROOT, "%2d:%02d.%d", tenths / 600, tenths % 600 / 10, tenths % 10);
        }
        // Java: 64
        public static string describe(com.oddlabs.tt.player.Player p)
        {
            int peons = 0;
            int[] warriors = new int[com.oddlabs.tt.player.ai.Intel.WarriorType.values().Length];
            int chief_hp = (-1);
            int quarters = 0;
            int armories = 0;
            int towers = 0;
            int manned = 0;
            int sites = 0;
            int iron = 0;
            int chicken = 0;
            int rock = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if (s.isDead())
                {
                    continue;
                }
                if ((s is com.oddlabs.tt.model.Unit u))
                {
                    if (u.isMounted())
                    {
                        continue;
                    }
                    if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                    {
                        chief_hp = u.getHitPoints();
                    }
                    else if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
                    {
                        warriors[com.oddlabs.tt.player.ai.Intel.warriorType(u).ordinal()]++;
                    }
                    else if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
                    {
                        peons++;
                    }
                }
                else if ((s is com.oddlabs.tt.model.Building b))
                {
                    if (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getType() != com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING)
                    {
                        continue;
                    }
                    if (!b.isComplete())
                    {
                        sites++;
                        continue;
                    }
                    switch ((((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID()))
                    {
                        case 0:
                        {
                            quarters++;
                            break;
                        }
                        case 1:
                        {
                            armories++;
                            iron += b.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies();
                            chicken += b.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
                            rock += b.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
                            break;
                        }
                        case 2:
                        {
                            towers++;
                            if (com.oddlabs.tt.player.ai.Intel.isTowerManned(b))
                            {
                                manned++;
                            }
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }
            }
            int @out = peons + warriors[0] + warriors[1] + warriors[2] + ((chief_hp >= 0 ? 1 : 0)) + manned;
            int total = p.getUnitCountContainer().getNumSupplies();
            string race = (p.getPlayerInfo().getRace() == com.oddlabs.tt.model.RacesResources.RACE_VIKINGS ? "vikings" : "natives");
            string chief = (chief_hp >= 0 ? "chieftain " + (chief_hp) + "hp" : "no chieftain");
            java.lang.StringBuilder line = new java.lang.StringBuilder();
            line.append(java.lang.JString.format(java.util.Locale.ROOT, "%s (%s, %s, team %d): %d units, %d inside buildings", p.getPlayerInfo().getName(), race, com.oddlabs.tt.player.ai.GameLog.controller(p), p.getPlayerInfo().getTeam(), total, java.lang.Math.max(0, total - @out)));
            line.append(java.lang.JString.format(java.util.Locale.ROOT, " | out: %d peons, warriors r%d i%d c%d, %s", peons, warriors[0], warriors[1], warriors[2], chief));
            line.append(java.lang.JString.format(java.util.Locale.ROOT, " | Q%d A%d T%d (%d manned) +%d sites", quarters, armories, towers, manned, sites));
            line.append(java.lang.JString.format(java.util.Locale.ROOT, " | armory stock i%d c%d r%d", iron, chicken, rock));
            line.append(java.lang.JString.format(java.util.Locale.ROOT, " | killed %d, lost %d, razed %d", p.getUnitsKilled(), p.getUnitsLost(), p.getBuildingsDestroyed()));
            return java.lang.JO.toString(line);
        }
        // Java: 131
        public static string controller(com.oddlabs.tt.player.Player p)
        {
            if (p.getAI() == null)
            {
                return "human";
            }
            if ((p.getAI() is com.oddlabs.tt.player.ai.ExpertAI))
            {
                return "expert AI";
            }
            return ((p.getAI() is com.oddlabs.tt.player.AdvancedAI) ? "AI" : "computer");
        }
        static GameLog()
        {
            GameLog.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.player.ai.GameLog).getName());
        }
        private void init_GameLog()
        {
            this.pending = new java.lang.StringBuilder();
        }
        public GameLog(java.nio.file.Path file) : base()
        {
            init_GameLog();
            this.file = file;
        }
    }
}
