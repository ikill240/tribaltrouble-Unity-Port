// Translated from AnimationManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.animation
{
    // Java: 25
    public class AnimationManager : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public const long ANIMATION_MILLISECONDS_PER_TICK = 20L;
        public const long ANIMATION_MILLISECONDS_PER_PRECISION_TICK = 4L;
        public const float ANIMATION_SECONDS_PER_TICK = 0.02f;
        public const float ANIMATION_SECONDS_PER_PRECISION_TICK = 0.004f;
        public static long ANIMATION_MILLISECONDS_PER_CHECKSUM;
        public static long MAX_STEP_MILLIS;
        public static com.oddlabs.tt.util.StatCounter pathfindsPerTick;
        public static com.oddlabs.tt.util.StatCounter frameTime;
        public static com.oddlabs.net.MonotoneTimeManager timeSource;
        public static long current_time;
        public static long last_frame_time;
        public static long execution_time;
        public static float execution_time_precision;
        public static long time_warp;
        public static bool time_stopped;
        public static bool time_frozen;
        public static long frozen_start_time;
        public static long frozen_start_time_warped;
        public static long checksum_millisecond_counter;
        public static bool checksum_complain;
        public java.util.Set animations;
        public java.util.Set deleted_animations;
        public int tick;
        // Java: 96
        public static void warpTime(long warp_delta)
        {
            com.oddlabs.tt.animation.AnimationManager.time_warp += warp_delta;
        }
        // Java: 100
        public static long getSystemTime()
        {
            return (com.oddlabs.tt.animation.AnimationManager.time_frozen ? com.oddlabs.tt.animation.AnimationManager.frozen_start_time_warped : com.oddlabs.tt.animation.AnimationManager.timeSource.getMillis() + com.oddlabs.tt.animation.AnimationManager.time_warp);
        }
        // Java: 104
        public static void toggleTimeStop()
        {
            if (com.oddlabs.tt.animation.AnimationManager.time_frozen)
            {
                com.oddlabs.tt.animation.AnimationManager.unfreezeTime();
            }
            else
            {
                com.oddlabs.tt.animation.AnimationManager.freezeTime();
            }
            com.oddlabs.tt.animation.AnimationManager.time_stopped = com.oddlabs.tt.animation.AnimationManager.time_frozen;
            com.oddlabs.tt.animation.AnimationManager.logger.config("time_stopped = " + java.lang.JStr.valueOf(com.oddlabs.tt.animation.AnimationManager.time_stopped));
        }
        // Java: 113
        public static void unfreezeTime()
        {
            if (!com.oddlabs.tt.animation.AnimationManager.time_frozen)
            {
                return;
            }
            com.oddlabs.tt.animation.AnimationManager.time_frozen = false;
            com.oddlabs.tt.animation.AnimationManager.time_warp -= com.oddlabs.tt.animation.AnimationManager.timeSource.getMillis() - com.oddlabs.tt.animation.AnimationManager.frozen_start_time;
        }
        // Java: 120
        public static void freezeTime()
        {
            if (com.oddlabs.tt.animation.AnimationManager.time_frozen)
            {
                return;
            }
            com.oddlabs.tt.animation.AnimationManager.frozen_start_time_warped = com.oddlabs.tt.animation.AnimationManager.getSystemTime();
            com.oddlabs.tt.animation.AnimationManager.time_frozen = true;
            com.oddlabs.tt.animation.AnimationManager.frozen_start_time = com.oddlabs.tt.animation.AnimationManager.timeSource.getMillis();
        }
        // Java: 128
        public static void runGameLoop(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui, bool grab_frames)
        {
            com.oddlabs.tt.render.Renderer.getLocalInput().checkMagicKeys();
            if (com.oddlabs.tt.animation.AnimationManager.time_frozen && !com.oddlabs.tt.animation.AnimationManager.time_stopped)
            {
                com.oddlabs.tt.animation.AnimationManager.unfreezeTime();
            }
            if (grab_frames)
            {
                com.oddlabs.tt.animation.AnimationManager.current_time += com.oddlabs.tt.global.Settings.getSettings().frame_grab_milliseconds_per_frame;
            }
            else
            {
                com.oddlabs.tt.animation.AnimationManager.current_time = com.oddlabs.tt.animation.AnimationManager.getSystemTime();
            }
            long time_diff = com.oddlabs.tt.animation.AnimationManager.current_time - com.oddlabs.tt.animation.AnimationManager.last_frame_time;
            com.oddlabs.tt.animation.AnimationManager.last_frame_time = com.oddlabs.tt.animation.AnimationManager.current_time;
            com.oddlabs.@event.Deterministic deterministic = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic();
            if (time_diff > com.oddlabs.tt.animation.AnimationManager.MAX_STEP_MILLIS && !((com.oddlabs.@event.Deterministic)java.util.Objects.requireNonNull(deterministic)).isPlayback())
            {
                com.oddlabs.tt.animation.AnimationManager.logger.warning("Skipping large time diff: " + (time_diff) + " ms.");
                time_diff = 0;
            }
            com.oddlabs.tt.animation.AnimationManager.frameTime.updateAbsolute(time_diff);
            com.oddlabs.tt.animation.AnimationManager.execution_time_precision += com.oddlabs.tt.animation.AnimationManager.frameTime.getAveragePerUpdate();
            ((com.oddlabs.@event.Deterministic)java.util.Objects.requireNonNull(deterministic)).setEnabled(true);
            while (com.oddlabs.tt.animation.AnimationManager.execution_time_precision >= com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_PRECISION_TICK && !com.oddlabs.tt.render.Renderer.isFinished())
            {
                com.oddlabs.tt.animation.AnimationManager.execution_time_precision -= com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_PRECISION_TICK;
                com.oddlabs.tt.animation.AnimationManager.execution_time += com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_PRECISION_TICK;
                com.oddlabs.tt.@event.LocalEventQueue.getQueue().tickHighPrecision(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_PRECISION_TICK);
                while (com.oddlabs.tt.animation.AnimationManager.execution_time >= com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_TICK && !com.oddlabs.tt.render.Renderer.isFinished())
                {
                    network.tick();
                    com.oddlabs.tt.render.Renderer.getRenderer().syncDisplaySize(gui);
                    com.oddlabs.tt.render.Renderer.getLocalInput().poll(gui.getGUIRoot());
                    if (deterministic.log(com.oddlabs.tt.render.Renderer.getRenderer().getWindow().isOpen() && com.oddlabs.tt.render.Renderer.getRenderer().getWindow().isCloseRequested()))
                    {
                        com.oddlabs.tt.render.Renderer.getRenderer().getWindow().setCloseRequested(false);
                        if (gui.getGUIRoot().isShowingQuitForm())
                        {
                            com.oddlabs.tt.render.Renderer.shutdown();
                        }
                        else
                        {
                            gui.getGUIRoot().addModalForm(new com.oddlabs.tt.form.QuitForm(gui.getGUIRoot()));
                        }
                    }
                    com.oddlabs.tt.animation.AnimationManager.pathfindsPerTick.updateAbsolute(com.oddlabs.tt.pathfinder.PathFinder.stat_pathfinder_per_frame);
                    com.oddlabs.tt.pathfinder.PathFinder.stat_pathfinder_per_frame = 0;
                    com.oddlabs.tt.@event.LocalEventQueue.getQueue().tickLowPrecision(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK);
                    com.oddlabs.tt.animation.AnimationManager.execution_time -= com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_TICK;
                    com.oddlabs.tt.animation.AnimationManager.checksum_millisecond_counter += com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_TICK;
                    if (com.oddlabs.tt.animation.AnimationManager.checksum_millisecond_counter >= com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_CHECKSUM)
                    {
                        com.oddlabs.tt.steam.SteamManager.runCallbacks();
                        com.oddlabs.tt.animation.AnimationManager.checksum_millisecond_counter -= com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_CHECKSUM;
                        int checksum = com.oddlabs.tt.@event.LocalEventQueue.getQueue().computeChecksum();
                        int logged_checksum = deterministic.log(checksum);
                        if (checksum != logged_checksum && com.oddlabs.tt.animation.AnimationManager.checksum_complain)
                        {
                            com.oddlabs.tt.animation.AnimationManager.logger.severe("********** ERROR: Checksum mismatch at tick " + (com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick()) + " | checksum = " + (checksum) + " | logged_checksum = " + (logged_checksum) + " **********");
                            com.oddlabs.tt.animation.AnimationManager.checksum_complain = false;
                        }
                    }
                    if (!com.oddlabs.tt.global.Globals.frustum_freeze)
                    {
                        gui.pickHover();
                    }
                }
            }
            deterministic.setEnabled(false);
        }
        // Java: 214
        public int getTick()
        {
            return tick;
        }
        // Java: 218
        public void registerAnimation(com.oddlabs.tt.animation.Animated anim)
        {
            deleted_animations.remove(anim);
            animations.add(anim);
        }
        // Java: 223
        public void removeAnimation(com.oddlabs.tt.animation.Animated anim)
        {
            if (animations.contains(anim))
            {
                deleted_animations.add(anim);
            }
        }
        // Java: 229
        public void flushAnimations()
        {
            animations.removeAll(deleted_animations);
            deleted_animations.clear();
        }
        // Java: 234
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
            flushAnimations();
            animations.forEach(new java.util.function.Consumer_L((object anim_l) =>
            {
                com.oddlabs.tt.animation.Animated anim = (com.oddlabs.tt.animation.Animated)anim_l;
                anim.updateChecksum(checksum);
            }));
        }
        // Java: 239
        public void runAnimations(float t)
        {
            tick++;
            flushAnimations();
            java.util.function.Predicate notDeleted = (((java.util.function.Predicate)new java.util.function.Predicate_L((object a0) => { return deleted_animations.contains(a0); }))).negate();
            animations.stream().filter(notDeleted).forEach(new java.util.function.Consumer_L((object a_l) =>
            {
                com.oddlabs.tt.animation.Animated a = (com.oddlabs.tt.animation.Animated)a_l;
                a.animate(t);
            }));
        }
        // Java: 246
        public void debugPrintAnimations()
        {
            flushAnimations();
            animations.forEach(new java.util.function.Consumer_L((object anim_l) =>
            {
                com.oddlabs.tt.animation.Animated anim = (com.oddlabs.tt.animation.Animated)anim_l;
                com.oddlabs.tt.animation.AnimationManager.logger.fine("anim = " + java.lang.JStr.valueOf(anim));
            }));
        }
        static AnimationManager()
        {
            AnimationManager.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.animation.AnimationManager).getName());
            AnimationManager.ANIMATION_MILLISECONDS_PER_CHECKSUM = java.util.concurrent.TimeUnit.SECONDS.toMillis(2);
            AnimationManager.MAX_STEP_MILLIS = java.util.concurrent.TimeUnit.SECONDS.toMillis(30);
            AnimationManager.pathfindsPerTick = new com.oddlabs.tt.util.StatCounter(100);
            AnimationManager.frameTime = new com.oddlabs.tt.util.StatCounter(10);
            AnimationManager.timeSource = new com.oddlabs.net.MonotoneTimeManager(new com.oddlabs.net.TimeManager_L(() =>
            {
                return java.util.concurrent.TimeUnit.NANOSECONDS.toMillis(java.lang.JSystem.nanoTime());
            }));
            AnimationManager.execution_time = 0;
            AnimationManager.execution_time_precision = 0;
            AnimationManager.checksum_complain = true;
            {
                com.oddlabs.tt.animation.AnimationManager.current_time = com.oddlabs.tt.animation.AnimationManager.getSystemTime();
                com.oddlabs.tt.animation.AnimationManager.last_frame_time = com.oddlabs.tt.animation.AnimationManager.current_time;
                com.oddlabs.tt.animation.AnimationManager.freezeTime();
            }
        }
        private void init_AnimationManager()
        {
            this.animations = new java.util.concurrent.CopyOnWriteArraySet();
            this.deleted_animations = new java.util.concurrent.CopyOnWriteArraySet();
        }
        public AnimationManager() : base()
        {
            init_AnimationManager();
        }
    }
}
