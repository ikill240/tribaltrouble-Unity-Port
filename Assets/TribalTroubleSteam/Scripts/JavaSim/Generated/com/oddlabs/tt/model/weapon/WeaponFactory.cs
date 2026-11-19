// Translated from WeaponFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 10
    public abstract class WeaponFactory : java.lang.Object
    {
        public const float TERRAIN_MAX_BONUS = 0.25f;
        public const float TERRAIN_BONUS_PER_HEIGHT = 0.0125f;
        public float hit_chance;
        public float range;
        public float release_ratio;
        // Java: 24
        public float getSecondsPerRelease(float anim_per_second)
        {
            return (float)(release_ratio / anim_per_second);
        }
        // Java: 28
        public float getRange()
        {
            return range;
        }
        // Java: 32
        public static float computeTerrainBonus(com.oddlabs.tt.landscape.HeightMap heightmap, com.oddlabs.tt.util.Target src, com.oddlabs.tt.util.Target dst)
        {
            float src_z = heightmap.getNearestHeight(src.getPositionX(), src.getPositionY());
            float dst_z = heightmap.getNearestHeight(dst.getPositionX(), dst.getPositionY());
            float bonus = (float)(((float)(src_z - dst_z)) * com.oddlabs.tt.model.weapon.WeaponFactory.TERRAIN_BONUS_PER_HEIGHT);
            bonus = java.lang.Math.clamp(bonus, (float)(-com.oddlabs.tt.model.weapon.WeaponFactory.TERRAIN_MAX_BONUS), com.oddlabs.tt.model.weapon.WeaponFactory.TERRAIN_MAX_BONUS);
            return bonus;
        }
        // Java: 40
        public void attack(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, float factor)
        {
            float terrain_bonus = com.oddlabs.tt.model.weapon.WeaponFactory.computeTerrainBonus(src.getOwner().getWorld().getHeightMap(), src, target);
            float difficulty_bonus = src.getOwner().getHitBonus();
            bool hit = target.getOwner().getWorld().getRandom().nextFloat() < (float)((float)(factor * ((float)((float)(difficulty_bonus + terrain_bonus) + hit_chance))) * ((float)(1 - target.getDefenseChance())));
            doAttack(hit, src, target);
        }
        // Java: 48
        public void attack(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target)
        {
            attack(src, target, 1.0f);
        }
        // Java: 52
        public abstract void doAttack(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target);
        // Java: 54
        public abstract System.Type getType();
        public WeaponFactory(float hit_chance, float range, float release_ratio) : base()
        {
            this.hit_chance = hit_chance;
            this.range = range;
            this.release_ratio = release_ratio;
        }
    }
}
