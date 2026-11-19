// Translated from MeleeFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 21
    public class MeleeFactory : com.oddlabs.tt.model.weapon.WeaponFactory
    {
        public System.Type weapon_type;
        public int damage;
        public int building_damage;
        public com.oddlabs.tt.audio.Audio[] sounds;
        public const float GRAZE_CHANCE = 0.35f;
        public float spin_radius;
        public int spin_damage;
        public int spin_victims;
        // Java: 43
        public com.oddlabs.tt.model.weapon.MeleeFactory spin(float radius, int damage, int victims)
        {
            spin_radius = radius;
            spin_damage = damage;
            spin_victims = victims;
            return this;
        }
        // Java: 50
        public float getSpinRadius()
        {
            return spin_radius;
        }
        // Java: 54
        public void spinAround(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(src.getPositionX(), src.getPositionY(), spin_radius, src, typeof(com.oddlabs.tt.model.Unit));
            src.getOwner().getWorld().getUnitGrid().scan(filter, src.getGridX(), src.getGridY());
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(filter.getResult()))
            {
                if (n >= spin_victims)
                {
                    break;
                }
                if (((object)u == (object)target) || u.isDead() || u.isMounted() || !src.getOwner().isEnemy(u.getOwner()))
                {
                    continue;
                }
                float dx = (float)(u.getPositionX() - src.getPositionX());
                float dy = (float)(u.getPositionY() - src.getPositionY());
                float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                float inv = (len > 0.0f ? (float)(1.0f / len) : 0.0f);
                int amount = com.oddlabs.tt.model.ExtendedCombat.adjustDamage(src, u, spin_damage);
                u.hit(amount, (float)(dx * inv), (float)(dy * inv), src.getOwner());
                com.oddlabs.tt.model.ExtendedCombat.afterHit(src, u, amount);
                n++;
            }
        }
        // Java: 74
        public override void doAttack(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target)
        {
            if (spin_radius > 0.0f && !src.isDead())
            {
                spinAround(src, target);
            }
            int amount = default;
            if ((target is com.oddlabs.tt.model.Building))
            {
                amount = building_damage;
            }
            else if (hit || src.getOwner().getWorld().getRandom().nextFloat() < com.oddlabs.tt.model.weapon.MeleeFactory.GRAZE_CHANCE)
            {
                amount = damage;
            }
            else
            {
                return;
            }
            float dx = (float)(target.getPositionX() - src.getPositionX());
            float dy = (float)(target.getPositionY() - src.getPositionY());
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            float dir_len_inv = (len > 0.0f ? (float)(1.0f / len) : 0.0f);
            if ((target is com.oddlabs.tt.model.Unit))
            {
                com.oddlabs.tt.landscape.World world = src.getOwner().getWorld();
                world.getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(sounds[world.getRandom().nextInt(sounds.Length)], target.getPositionX(), target.getPositionY(), target.getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_DEATH, (float)(1.0f + (float)(((float)(world.getRandom().nextFloat() - 0.5f)) * (((com.oddlabs.tt.model.UnitTemplate)target.getTemplate())).getDeathPitch()))));
            }
            amount = com.oddlabs.tt.model.ExtendedCombat.adjustDamage(src, target, amount);
            target.hit(amount, (float)(dx * dir_len_inv), (float)(dy * dir_len_inv), src.getOwner());
            com.oddlabs.tt.model.ExtendedCombat.afterHit(src, target, amount);
        }
        // Java: 104
        public int getDamage()
        {
            return damage;
        }
        // Java: 108
        public override System.Type getType()
        {
            return weapon_type;
        }
        public MeleeFactory(System.Type weapon_type, float hit_chance, float release_ratio, int damage, int building_damage, com.oddlabs.tt.audio.Audio[] sounds) : base(hit_chance, 0.0f, release_ratio)
        {
            this.weapon_type = weapon_type;
            this.damage = damage;
            this.building_damage = building_damage;
            this.sounds = sounds;
        }
    }
}
