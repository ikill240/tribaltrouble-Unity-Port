// Translated from Orders.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 33
    public class Orders : java.lang.Object
    {
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public com.oddlabs.tt.player.fable.AiLog log;
        // Java: 44
        public com.oddlabs.tt.player.Player getPlayer()
        {
            return player;
        }
        // Java: 51
        public bool usable(com.oddlabs.tt.model.Unit unit)
        {
            return unit != null && !unit.isDead() && ((object)unit.getOwner() == (object)player) && !unit.isMounted();
        }
        // Java: 56
        public bool usable(com.oddlabs.tt.model.Building building)
        {
            return building != null && !building.isDead() && ((object)building.getOwner() == (object)player);
        }
        // Java: 61
        public bool complete(com.oddlabs.tt.model.Building building)
        {
            return usable(building) && building.isPlaced() && building.isComplete();
        }
        // Java: 65
        public static bool alive(com.oddlabs.tt.util.Target target)
        {
            return target != null && !target.isDead();
        }
        // Java: 69
        public int clampGrid(int c)
        {
            return java.lang.Math.clamp(c, 0, grid.getGridSize() - 1);
        }
        // Java: 73
        public com.oddlabs.tt.model.Selectable[] filterUnits(java.util.Collection units)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (usable(u))
                {
                    n++;
                }
            }
            com.oddlabs.tt.model.Selectable[] result = com.oddlabs.tt.model.Selectable.newArray(n);
            int i = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(units))
            {
                if (usable(u_2))
                {
                    result[i++] = u_2;
                }
            }
            return result;
        }
        // Java: 89
        public int attackMove(java.util.Collection units, int gx, int gy)
        {
            com.oddlabs.tt.model.Selectable[] sel = filterUnits(units);
            if (sel.Length == 0)
            {
                return 0;
            }
            player.setLandscapeTarget(sel, clampGrid(gx), clampGrid(gy), com.oddlabs.tt.model.Action.ATTACK, true);
            return sel.Length;
        }
        // Java: 97
        public void attackMove(com.oddlabs.tt.model.Unit unit, int gx, int gy)
        {
            if (!usable(unit))
            {
                return;
            }
            player.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(unit), clampGrid(gx), clampGrid(gy), com.oddlabs.tt.model.Action.ATTACK, true);
        }
        // Java: 104
        public int move(java.util.Collection units, int gx, int gy)
        {
            com.oddlabs.tt.model.Selectable[] sel = filterUnits(units);
            if (sel.Length == 0)
            {
                return 0;
            }
            player.setLandscapeTarget(sel, clampGrid(gx), clampGrid(gy), com.oddlabs.tt.model.Action.MOVE, false);
            return sel.Length;
        }
        // Java: 112
        public void move(com.oddlabs.tt.model.Unit unit, int gx, int gy)
        {
            if (!usable(unit))
            {
                return;
            }
            player.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(unit), clampGrid(gx), clampGrid(gy), com.oddlabs.tt.model.Action.MOVE, false);
        }
        // Java: 119
        public int defend(java.util.Collection units, int gx, int gy)
        {
            com.oddlabs.tt.model.Selectable[] sel = filterUnits(units);
            if (sel.Length == 0)
            {
                return 0;
            }
            player.setLandscapeTarget(sel, clampGrid(gx), clampGrid(gy), com.oddlabs.tt.model.Action.DEFEND, true);
            return sel.Length;
        }
        // Java: 128
        public int attack(java.util.Collection units, com.oddlabs.tt.model.Selectable target)
        {
            if (!com.oddlabs.tt.player.fable.Orders.alive(target))
            {
                return 0;
            }
            com.oddlabs.tt.model.Selectable[] sel = filterUnits(units);
            if (sel.Length == 0)
            {
                return 0;
            }
            player.setTarget(sel, target, com.oddlabs.tt.model.Action.ATTACK, true);
            return sel.Length;
        }
        // Java: 138
        public void attack(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Selectable target)
        {
            if (!usable(unit) || !com.oddlabs.tt.player.fable.Orders.alive(target))
            {
                return;
            }
            player.setTarget(com.oddlabs.tt.model.Selectable.newArray(unit), target, com.oddlabs.tt.model.Action.ATTACK, true);
        }
        // Java: 147
        public void gather(com.oddlabs.tt.model.Unit peon, com.oddlabs.tt.model.Supply supply)
        {
            if (!usable(peon) || supply.isEmpty() || supply.isDead())
            {
                return;
            }
            player.setTarget(com.oddlabs.tt.model.Selectable.newArray(peon), supply, com.oddlabs.tt.model.Action.DEFAULT, false);
        }
        // Java: 154
        public void enter(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Building building)
        {
            if (!usable(unit) || !complete(building))
            {
                return;
            }
            player.setTarget(com.oddlabs.tt.model.Selectable.newArray(unit), building, com.oddlabs.tt.model.Action.DEFAULT, false);
        }
        // Java: 161
        public void repair(com.oddlabs.tt.model.Unit peon, com.oddlabs.tt.model.Building building)
        {
            if (!usable(peon) || !usable(building) || !building.isPlaced())
            {
                return;
            }
            player.setTarget(com.oddlabs.tt.model.Selectable.newArray(peon), building, com.oddlabs.tt.model.Action.GATHER_REPAIR, false);
        }
        // Java: 168
        public void build(com.oddlabs.tt.model.Unit peon, com.oddlabs.tt.model.Building building)
        {
            if (!usable(peon) || !usable(building))
            {
                return;
            }
            player.setTarget(com.oddlabs.tt.model.Selectable.newArray(peon), building, com.oddlabs.tt.model.Action.DEFAULT, false);
        }
        // Java: 178
        public com.oddlabs.tt.model.Building placeBuilding(int template_id, int gx, int gy, java.util.Collection peons)
        {
            if (!player.canBuild(template_id))
            {
                return null;
            }
            com.oddlabs.tt.model.BuildingTemplate template = player.getRace().getBuildingTemplate(template_id);
            gx = clampGrid(gx);
            gy = clampGrid(gy);
            if (!template.isPlacingLegal(grid, gx, gy))
            {
                return null;
            }
            com.oddlabs.tt.model.Selectable[] sel = filterUnits(peons);
            if (sel.Length == 0)
            {
                return null;
            }
            com.oddlabs.tt.model.Building building = template.create(player, gx, gy);
            player.setTarget(sel, building, com.oddlabs.tt.model.Action.DEFAULT, false);
            log.info("place " + java.lang.JStr.valueOf(com.oddlabs.tt.player.fable.Orders.templateName(template_id)) + " at " + (gx) + "," + (gy) + " with " + (sel.Length) + " peons");
            return building;
        }
        // Java: 196
        public static string templateName(int template_id)
        {
            return ((template_id) switch { 0 => (string)("quarters"), 1 => (string)("armory"), 2 => (string)("tower"), _ => (string)("building" + (template_id)) });
        }
        // Java: 207
        public void deploy(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.DeployType type, int count)
        {
            if (count <= 0 || !complete(building))
            {
                return;
            }
            player.deployUnits(building, type, count);
        }
        // Java: 213
        public void setRally(com.oddlabs.tt.model.Building building, int gx, int gy)
        {
            if (!complete(building))
            {
                return;
            }
            player.setRallyPoint(building, new com.oddlabs.tt.landscape.LandscapeTarget(clampGrid(gx), clampGrid(gy)));
        }
        // Java: 219
        public void setRally(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.Building target)
        {
            if (!complete(building) || !complete(target))
            {
                return;
            }
            player.setRallyPoint(building, target);
        }
        // Java: 229
        public void queueWeapons(com.oddlabs.tt.model.Building armory, bool rock, bool iron, bool rubber)
        {
            if (!complete(armory) || !armory.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
            {
                return;
            }
            setProduction(armory, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), rock);
            setProduction(armory, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), iron);
            setProduction(armory, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), rubber);
        }
        // Java: 237
        public void setProduction(com.oddlabs.tt.model.Building armory, System.Type type, bool on)
        {
            com.oddlabs.tt.model.BuildSupplyContainer container = armory.getBuildSupplyContainer(type);
            if (container == null)
            {
                return;
            }
            int queued = container.getNumSupplies();
            if (on && queued < com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT)
            {
                orderWeapons(armory, type, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
            }
            else if (!on && queued > 0)
            {
                orderWeapons(armory, type, -queued, false);
            }
        }
        // Java: 249
        public void orderWeapons(com.oddlabs.tt.model.Building armory, System.Type type, int amount, bool infinite)
        {
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)))
            {
                player.buildRockWeapons(armory, amount, infinite);
            }
            else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)))
            {
                player.buildIronWeapons(armory, amount, infinite);
            }
            else
            {
                player.buildRubberWeapons(armory, amount, infinite);
            }
        }
        // Java: 259
        public void trainChieftain(com.oddlabs.tt.model.Building quarters, bool start)
        {
            if (!complete(quarters) || !quarters.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
            {
                return;
            }
            if (start && !quarters.canBuildChieftain())
            {
                return;
            }
            if (!start && !quarters.canStopChieftain())
            {
                return;
            }
            player.trainChieftain(quarters, start);
        }
        // Java: 269
        public void exitTower(com.oddlabs.tt.model.Building tower)
        {
            if (!complete(tower) || !tower.canExitTower())
            {
                return;
            }
            player.exitTower(tower);
        }
        // Java: 275
        public void recallGatherers(com.oddlabs.tt.model.Building armory, System.Type type, int count)
        {
            if (count <= 0 || !complete(armory))
            {
                return;
            }
            player.recallGatherers(armory, type, count);
        }
        // Java: 283
        public bool stun(com.oddlabs.tt.model.Unit chieftain)
        {
            if (!usable(chieftain) || !chieftain.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN))
            {
                return false;
            }
            player.doMagic(chieftain, com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN);
            log.info("chieftain STUN at " + (chieftain.getGridX()) + "," + (chieftain.getGridY()));
            return true;
        }
        // Java: 291
        public bool blast(com.oddlabs.tt.model.Unit chieftain)
        {
            if (!usable(chieftain) || !chieftain.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST))
            {
                return false;
            }
            player.doMagic(chieftain, com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST);
            log.info("chieftain BLAST at " + (chieftain.getGridX()) + "," + (chieftain.getGridY()));
            return true;
        }
        public Orders(com.oddlabs.tt.player.Player player, com.oddlabs.tt.player.fable.AiLog log) : base()
        {
            this.player = player;
            this.grid = player.getWorld().getUnitGrid();
            this.log = log;
        }
    }
}
