// Translated from UnitInfoText.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 23
    public class UnitInfoText : java.lang.Object
    {
        public static java.util.ResourceBundle bundle;
        // Java: 26
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.UnitInfoText.bundle, key, args);
        }
        // Java: 34
        public static int unitFor(com.oddlabs.tt.model.DeployType type)
        {
            if (type == null)
            {
                return (-1);
            }
            return ((type).ordinal() switch { 0 /*ROCK_WARRIOR*/ => (int)(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK), 1 /*IRON_WARRIOR*/ => (int)(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON), 2 /*RUBBER_WARRIOR*/ => (int)(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER), 3 /*PEON*/ => (int)(com.oddlabs.tt.model.Race.UNIT_PEON), 12 /*CHAMPION*/ => (int)(com.oddlabs.tt.model.Race.UNIT_CHAMPION), 13 /*SCOUT*/ => (int)(com.oddlabs.tt.model.Race.UNIT_SCOUT), 18 /*SHAMAN*/ => (int)(com.oddlabs.tt.model.Race.UNIT_SHAMAN), 19 /*SLINGER*/ => (int)(com.oddlabs.tt.model.Race.UNIT_SLINGER), 26 /*MELEE_LIGHT*/ => (int)(com.oddlabs.tt.model.Race.UNIT_MELEE_LIGHT), 27 /*MELEE_HEAVY*/ => (int)(com.oddlabs.tt.model.Race.UNIT_MELEE_HEAVY), 35 /*UNIQUE_1*/ => (int)(com.oddlabs.tt.model.Race.UNIT_UNIQUE_1), 36 /*UNIQUE_2*/ => (int)(com.oddlabs.tt.model.Race.UNIT_UNIQUE_2), 51 /*CRYSTAL_WARRIOR*/ => (int)(com.oddlabs.tt.model.Race.UNIT_CRYSTAL), 52 /*DIAMOND_GUARD*/ => (int)(com.oddlabs.tt.model.Race.UNIT_DIAMOND), 53 /*UNIQUE_3*/ => (int)(com.oddlabs.tt.model.Race.UNIT_UNIQUE_3), 54 /*MINOR_GOD*/ => (int)(com.oddlabs.tt.model.Race.UNIT_MINOR_GOD), 55 /*MAJOR_GOD*/ => (int)(com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD), 82 /*ELITE_1*/ => (int)(com.oddlabs.tt.model.Race.UNIT_ELITE_1), 83 /*ELITE_2*/ => (int)(com.oddlabs.tt.model.Race.UNIT_ELITE_2), 84 /*ELITE_3*/ => (int)(com.oddlabs.tt.model.Race.UNIT_ELITE_3), 93 /*SPINNER*/ => (int)(com.oddlabs.tt.model.Race.UNIT_SPINNER), _ => (int)((-1)) });
        }
        // Java: 63
        public static int damage(com.oddlabs.tt.model.weapon.WeaponFactory f)
        {
            if ((f is com.oddlabs.tt.model.weapon.MeleeFactory m))
            {
                return m.getDamage();
            }
            System.Type t = f.getType();
            string n = (t == null ? "" : t.getSimpleName());
            return (n.startsWith("Rock") ? 1 : (n.startsWith("Diamond") ? 4 : (n.startsWith("Crystal") ? 3 : 2)));
        }
        // Java: 72
        public static string statsLine(com.oddlabs.tt.model.UnitTemplate t)
        {
            return com.oddlabs.tt.gui.UnitInfoText.statsLine(t, t.getMaxHitPoints() * ((t.getAbilities().isWarrior() ? 4 : 2)));
        }
        // Java: 76
        public static string statsLine(com.oddlabs.tt.model.UnitTemplate t, int hp)
        {
            return com.oddlabs.tt.gui.UnitInfoText.statsLine(t, hp, null);
        }
        // Java: 81
        public static string statsLine(com.oddlabs.tt.model.UnitTemplate t, float[] row)
        {
            int hp = (row != null && row[com.oddlabs.tt.model.NewTidesStats.HEALTH] > 0.0f && !t.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC) ? java.lang.Math.round(row[com.oddlabs.tt.model.NewTidesStats.HEALTH]) : t.getMaxHitPoints() * ((t.getAbilities().isWarrior() ? 4 : 2)));
            return com.oddlabs.tt.gui.UnitInfoText.statsLine(t, hp, row);
        }
        // Java: 87
        public static string statsLine(com.oddlabs.tt.model.UnitTemplate t, int hp, float[] row)
        {
            com.oddlabs.tt.model.weapon.WeaponFactory f = t.getWeaponFactory();
            int dmg = (row != null ? java.lang.Math.round(row[com.oddlabs.tt.model.NewTidesStats.DAMAGE]) : com.oddlabs.tt.gui.UnitInfoText.damage(f));
            float reach = (row != null && row[com.oddlabs.tt.model.NewTidesStats.REACH] > 0.0f && f.getRange() > 1.0f ? row[com.oddlabs.tt.model.NewTidesStats.REACH] : f.getRange());
            float speed = (row != null && row[com.oddlabs.tt.model.NewTidesStats.SPEED] > 0.0f ? row[com.oddlabs.tt.model.NewTidesStats.SPEED] : t.getMetersPerSecond());
            return com.oddlabs.tt.gui.UnitInfoText.i18n("stats", hp, dmg, (reach <= 1.0f ? (object)(com.oddlabs.tt.gui.UnitInfoText.i18n("melee")) : (object)(com.oddlabs.tt.gui.UnitInfoText.i18n("reach", java.lang.Math.round(reach)))), java.lang.JString.format("%.1f", speed));
        }
        // Java: 97
        public static string counterLines(int role)
        {
            java.lang.StringBuilder strong = new java.lang.StringBuilder();
            java.lang.StringBuilder weak = new java.lang.StringBuilder();
            for (int r = 0; r < com.oddlabs.tt.model.Counters.NUM_ROLES; r++)
            {
                if (com.oddlabs.tt.model.Counters.factor(role, r) > 1.0f)
                {
                    strong.append((strong.isEmpty() ? "" : ", ")).append(com.oddlabs.tt.gui.UnitInfoText.i18n("role" + (r)));
                }
                if (com.oddlabs.tt.model.Counters.factor(r, role) > 1.0f)
                {
                    weak.append((weak.isEmpty() ? "" : ", ")).append(com.oddlabs.tt.gui.UnitInfoText.i18n("role" + (r)));
                }
            }
            java.lang.StringBuilder sb = new java.lang.StringBuilder(com.oddlabs.tt.gui.UnitInfoText.i18n("role", com.oddlabs.tt.gui.UnitInfoText.i18n("role" + (role))));
            if (!strong.isEmpty())
            {
                sb.append('\n').append(com.oddlabs.tt.gui.UnitInfoText.i18n("strong", strong));
            }
            if (!weak.isEmpty())
            {
                sb.append('\n').append(com.oddlabs.tt.gui.UnitInfoText.i18n("weak", weak));
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 114
        public static string forTraining(com.oddlabs.tt.player.Player p, int unit)
        {
            com.oddlabs.tt.model.Race race = p.getRace();
            com.oddlabs.tt.model.UnitTemplate t = race.getUnitTemplate(unit);
            float[] row = (p.getWorld().isExtended() ? com.oddlabs.tt.model.NewTidesStats.row(com.oddlabs.tt.model.ExtendedCombat.isVikings(p), unit) : (float[])(null));
            string card = java.lang.JStr.valueOf(com.oddlabs.tt.gui.UnitInfoText.statsLine(t, row)) + "\n" + java.lang.JStr.valueOf(com.oddlabs.tt.gui.UnitInfoText.counterLines(com.oddlabs.tt.model.Counters.role(race, unit, com.oddlabs.tt.model.ExtendedCombat.isVikings(p))));
            if (unit >= com.oddlabs.tt.model.Race.UNIT_ELITE_1 && unit <= com.oddlabs.tt.model.Race.UNIT_ELITE_3)
            {
                card += "\n" + java.lang.JStr.valueOf(com.oddlabs.tt.gui.UnitInfoText.passivesLine(unit - com.oddlabs.tt.model.Race.UNIT_ELITE_1 + 1, 0));
            }
            return card;
        }
        // Java: 125
        public static string passivesLine(int kind, int rank)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            for (int r = 1; r <= com.oddlabs.tt.model.ElitePowers.MAX_RANK; r++)
            {
                if (r > 1)
                {
                    sb.append(", ");
                }
                string name = com.oddlabs.tt.gui.UnitInfoText.i18n(com.oddlabs.tt.model.ElitePowers.passiveKey(kind, r));
                sb.append((r <= rank ? com.oddlabs.tt.gui.UnitInfoText.i18n("elite_learned", name) : com.oddlabs.tt.gui.UnitInfoText.i18n("elite_at_rank", name, r)));
            }
            return com.oddlabs.tt.gui.UnitInfoText.i18n("elite_passives", sb);
        }
        // Java: 137
        public static string forUnit(com.oddlabs.tt.model.Unit u)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder(com.oddlabs.tt.gui.UnitInfoText.i18n("health_now", u.getHitPoints(), u.getEffectiveMaxHitPoints()));
            if (u.getGodKind() != com.oddlabs.tt.model.GodPowers.NONE)
            {
                sb.append("  ").append(com.oddlabs.tt.gui.UnitInfoText.i18n("god_kills", u.getGodKills()));
            }
            int elite = u.getEliteKind();
            if (elite != com.oddlabs.tt.model.ElitePowers.NONE)
            {
                int rank = com.oddlabs.tt.model.ElitePowers.rank(u);
                int next = com.oddlabs.tt.model.ElitePowers.nextRankPoints(u);
                sb.append("  ").append((next > 0 ? com.oddlabs.tt.gui.UnitInfoText.i18n("elite_rank", rank, com.oddlabs.tt.model.ElitePowers.points(u), next) : com.oddlabs.tt.gui.UnitInfoText.i18n("elite_rank_max", rank)));
                sb.append('\n').append(com.oddlabs.tt.gui.UnitInfoText.passivesLine(elite, rank));
            }
            sb.append('\n').append(com.oddlabs.tt.gui.UnitInfoText.statsLine(((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()), u.getEffectiveMaxHitPoints(), u.newTidesRow()));
            sb.append('\n').append(com.oddlabs.tt.gui.UnitInfoText.counterLines(com.oddlabs.tt.model.Counters.role(u)));
            return java.lang.JO.toString(sb);
        }
        // Java: 155
        public static string locked()
        {
            return com.oddlabs.tt.gui.UnitInfoText.i18n("locked");
        }
        static UnitInfoText()
        {
            UnitInfoText.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.UnitInfoText).getName());
        }
        public UnitInfoText() : base()
        {
        }
    }
}
