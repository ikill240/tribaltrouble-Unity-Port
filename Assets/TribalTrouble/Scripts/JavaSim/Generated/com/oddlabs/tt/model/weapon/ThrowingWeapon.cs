// Translated from ThrowingWeapon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 18
    public abstract class ThrowingWeapon : com.oddlabs.tt.model.Accessories, com.oddlabs.tt.animation.Animated
    {
        public const float GRAVITY = (-58.92f);
        public const float NO_DETAIL_SIZE = 0.5f;
        public const float OFFSET_X = 1.316f;
        public const float OFFSET_Y = (-0.347f);
        public const float OFFSET_Z = 1.382f;
        public com.oddlabs.tt.audio.AbstractAudioPlayer audio_player;
        public com.oddlabs.tt.audio.Audio[] hit_sounds;
        public com.oddlabs.tt.player.Player owner;
        public bool hit;
        public com.oddlabs.tt.model.Unit unit;
        public float miss_x;
        public float miss_y;
        public float damage_ratio;
        public com.oddlabs.tt.model.Selectable target;
        public float start_x;
        public float start_y;
        public float end_x;
        public float end_y;
        public float dir_x;
        public float dir_y;
        public float time_limit;
        public float time;
        public float z_speed;
        public float deterministic_z;
        // Java: 79
        public override string toString()
        {
            return "ThrowingWeapon: start_x = " + java.lang.JStr.valueOf(start_x) + " | start_y = " + java.lang.JStr.valueOf(start_y) + " | end_x = " + java.lang.JStr.valueOf(end_x) + " | end_y = " + java.lang.JStr.valueOf(end_y) + " | target = " + java.lang.JStr.valueOf(target) + "  " + java.lang.JStr.valueOf(base.toString());
        }
        // Java: 84
        public void setTarget(com.oddlabs.tt.model.Selectable target)
        {
            if ((target is com.oddlabs.tt.model.Ship ship))
            {
                com.oddlabs.tt.model.Selectable shipVictim = ship.pickVictim();
                if (shipVictim != null)
                {
                    target = shipVictim;
                }
            }
            this.target = target;
            float hit_error = unit.getHitError();
            float rand_x = (float)(owner.getWorld().getRandom().nextFloat() - 0.5f);
            float rand_y = (float)(owner.getWorld().getRandom().nextFloat() - 0.5f);
            miss_x = (float)((float)(hit_error * rand_x) * 2.0f);
            miss_y = (float)((float)(hit_error * rand_y) * 2.0f);
            float s = target.getSize();
            if (!hit && hit_error == 0.0f && owner.getWorld().isExtended())
            {
                float ox = (rand_x == 0.0f && rand_y == 0.0f ? 0.5f : rand_x);
                float oy = rand_y;
                float len = ((float)(java.lang.Math.sqrt((float)((float)(ox * ox) + (float)(oy * oy)))));
                float dx = (float)(target.getPositionX() - getPositionX());
                float dy = (float)(target.getPositionY() - getPositionY());
                float spread = (float)(0.6f + (float)(0.08f * ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))))));
                float r = (float)((float)((float)(s * 0.5f) + 0.4f) + (float)(spread * ((float)(0.5f + (float)((float)(0.5f * java.lang.Math.abs(ox)) * 2.0f)))));
                miss_x = (float)((float)(ox / len) * r);
                miss_y = (float)((float)(oy / len) * r);
            }
            damage_ratio = (float)(1.0f - java.lang.Math.min(1.0f, (float)(((float)((float)(miss_x * miss_x) + (float)(miss_y * miss_y))) / ((float)(s * s)))));
            updateDirection();
            calcNumUpdatesAndZSpeed();
        }
        // Java: 114
        public void calcNumUpdatesAndZSpeed()
        {
            start_x = getPositionX();
            start_y = getPositionY();
            updateTarget();
            float dx = (float)(end_x - start_x);
            float dy = (float)(end_y - start_y);
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            time_limit = (float)(len / getMetersPerSecond());
            time = 0;
            float dest_vec_z = (float)((float)(owner.getWorld().getHeightMap().getNearestHeight(end_x, end_y) + target.getHitOffsetZ()) - ((float)(getPositionZ() + deterministic_z)));
            z_speed = (float)((float)((dest_vec_z) / time_limit) - (float)((float)(com.oddlabs.tt.model.weapon.ThrowingWeapon.GRAVITY * time_limit) / 2.0f));
        }
        // Java: 128
        public abstract float getMetersPerSecond();
        // Java: 130
        public void updateTarget()
        {
            end_x = (float)(target.getPositionX() + miss_x);
            end_y = (float)(target.getPositionY() + miss_y);
        }
        // Java: 135
        public void updateDirection()
        {
            float dx = (float)(target.getPositionX() - getPositionX());
            float dy = (float)(target.getPositionY() - getPositionY());
            float len = java.lang.Math.max(((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))), 0.01f);
            float len_inv = (float)(1.0f / len);
            dir_x = (float)(dx * len_inv);
            dir_y = (float)(dy * len_inv);
            setDirection(dir_x, dir_y);
        }
        // Java: 145
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
            checksum.update(time);
        }
        // Java: 150
        public override float getOffsetZ()
        {
            return deterministic_z;
        }
        // Java: 155
        public virtual void animate(float t)
        {
            if (time >= time_limit)
            {
                hitTarget(hit, owner, target);
                return;
            }
            if (hit)
            {
                updateTarget();
            }
            time += t;
            float progress = (float)(time / time_limit);
            float x = default;
            float y = default;
            if (progress < 1.0f)
            {
                x = (float)(start_x + (float)(((float)(end_x - start_x)) * progress));
                y = (float)(start_y + (float)(((float)(end_y - start_y)) * progress));
            }
            else
            {
                x = end_x;
                y = end_y;
            }
            deterministic_z += (float)(z_speed * t);
            z_speed += (float)(com.oddlabs.tt.model.weapon.ThrowingWeapon.GRAVITY * t);
            setPosition(x, y);
            reinsert();
            audio_player.setPos(getPositionX(), getPositionY(), getPositionZ());
        }
        // Java: 186
        public virtual void hitTarget(bool hit, com.oddlabs.tt.player.Player owner, com.oddlabs.tt.model.Selectable target)
        {
            owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
            audio_player.stop();
            remove();
            if (hit)
            {
                damageTarget(target);
            }
        }
        // Java: 194
        public void damageTarget(com.oddlabs.tt.model.Selectable target)
        {
            if ((target is com.oddlabs.tt.model.Unit))
            {
                owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(hit_sounds[owner.getWorld().getRandom().nextInt(hit_sounds.Length)], target.getPositionX(), target.getPositionY(), target.getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_DEATH, (float)(1.0f + (float)(((float)(owner.getWorld().getRandom().nextFloat() - 0.5f)) * (((com.oddlabs.tt.model.UnitTemplate)target.getTemplate())).getDeathPitch()))));
            }
            int damage = java.lang.StrictMath.round((float)(getDamage() * damage_ratio));
            if (unit != null)
            {
                damage = com.oddlabs.tt.model.ExtendedCombat.adjustThrown(unit, target, damage);
            }
            target.hit(damage, dir_x, dir_y, owner);
            if (unit != null)
            {
                com.oddlabs.tt.model.ExtendedCombat.afterHit(unit, target, damage);
            }
        }
        // Java: 212
        public abstract int getDamage();
        // Java: 215
        public com.oddlabs.tt.model.Unit getThrower()
        {
            return unit;
        }
        // Java: 219
        public float getZSpeed()
        {
            return z_speed;
        }
        // Java: 223
        public override float getNoDetailSize()
        {
            return com.oddlabs.tt.model.weapon.ThrowingWeapon.NO_DETAIL_SIZE;
        }
        private void init_ThrowingWeapon()
        {
            this.miss_x = 0.0f;
            this.miss_y = 0.0f;
            this.damage_ratio = 1.0f;
        }
        public ThrowingWeapon(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.render.SpriteKey sprite_renderer, com.oddlabs.tt.audio.Audio throw_sound, com.oddlabs.tt.audio.Audio[] hit_sounds) : base(target.getOwner().getWorld(), sprite_renderer)
        {
            init_ThrowingWeapon();
            this.hit = hit;
            this.hit_sounds = hit_sounds;
            this.unit = src;
            owner = src.getOwner();
            setPosition((float)((float)(src.getPositionX() + (float)(com.oddlabs.tt.model.weapon.ThrowingWeapon.OFFSET_X * src.getDirectionX())) - (float)(com.oddlabs.tt.model.weapon.ThrowingWeapon.OFFSET_Y * src.getDirectionY())), (float)((float)(src.getPositionY() + (float)(com.oddlabs.tt.model.weapon.ThrowingWeapon.OFFSET_X * src.getDirectionY())) - (float)(com.oddlabs.tt.model.weapon.ThrowingWeapon.OFFSET_Y * src.getDirectionX())));
            deterministic_z = (float)(com.oddlabs.tt.model.weapon.ThrowingWeapon.OFFSET_Z + src.getMountOffset());
            setTarget(target);
            reinsert();
            audio_player = target.getOwner().getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(throw_sound, getPositionX(), getPositionY(), getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_WEAPON_ATTACK, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_WEAPON_ATTACK, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_WEAPON_ATTACK, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_WEAPON_ATTACK, (float)((float)(target.getOwner().getWorld().getRandom().nextFloat() * 0.2f) + 0.9f)));
            target.getOwner().getWorld().getAnimationManagerGameTime().registerAnimation(this);
            src.getOwner().weaponThrown();
        }
    }
}
