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
        public static long FRAME_SIMULATION_BUDGET_NANOS;
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
        public java.util.ArrayList pass;
        public bool passing;
        public int tick;
        // Java: 106
        public static void warpTime(long warp_delta)
        {
            com.oddlabs.tt.animation.AnimationManager.time_warp += warp_delta;
        }
        // Java: 110
        public static long getSystemTime()
        {
            return (com.oddlabs.tt.animation.AnimationManager.time_frozen ? com.oddlabs.tt.animation.AnimationManager.frozen_start_time_warped : com.oddlabs.tt.animation.AnimationManager.timeSource.getMillis() + com.oddlabs.tt.animation.AnimationManager.time_warp);
        }
        // Java: 114
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
        // Java: 123
        public static void unfreezeTime()
        {
            if (!com.oddlabs.tt.animation.AnimationManager.time_frozen)
            {
                return;
            }
            com.oddlabs.tt.animation.AnimationManager.time_frozen = false;
            com.oddlabs.tt.animation.AnimationManager.time_warp -= com.oddlabs.tt.animation.AnimationManager.timeSource.getMillis() - com.oddlabs.tt.animation.AnimationManager.frozen_start_time;
        }
        // Java: 130
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
        // Java: 138
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
            long frame_start = java.lang.JSystem.nanoTime();
            while (com.oddlabs.tt.animation.AnimationManager.execution_time_precision >= com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_PRECISION_TICK && !com.oddlabs.tt.render.Renderer.isFinished())
            {
                com.oddlabs.tt.animation.AnimationManager.execution_time_precision -= com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_PRECISION_TICK;
                com.oddlabs.tt.animation.AnimationManager.execution_time += com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_PRECISION_TICK;
                com.oddlabs.tt.@event.LocalEventQueue.getQueue().tickHighPrecision(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_PRECISION_TICK);
                while (com.oddlabs.tt.animation.AnimationManager.execution_time >= com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_TICK && !com.oddlabs.tt.render.Renderer.isFinished())
                {
                    network.tick();
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
                if (!deterministic.isPlayback() && java.lang.JSystem.nanoTime() - frame_start > com.oddlabs.tt.animation.AnimationManager.FRAME_SIMULATION_BUDGET_NANOS)
                {
                    com.oddlabs.tt.animation.AnimationManager.execution_time_precision = 0;
                    break;
                }
            }
            deterministic.setEnabled(false);
        }
        // Java: 229
        public int getTick()
        {
            return tick;
        }
        // Java: 233
        public void registerAnimation(com.oddlabs.tt.animation.Animated anim)
        {
            deleted_animations.remove(anim);
            animations.add(anim);
        }
        // Java: 239
        public bool isRegistered(com.oddlabs.tt.animation.Animated anim)
        {
            return animations.contains(anim) && !deleted_animations.contains(anim);
        }
        // Java: 243
        public void removeAnimation(com.oddlabs.tt.animation.Animated anim)
        {
            if (animations.contains(anim))
            {
                deleted_animations.add(anim);
            }
        }
        // Java: 249
        public void flushAnimations()
        {
            animations.removeAll(deleted_animations);
            deleted_animations.clear();
        }
        // Java: 254
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
            flushAnimations();
            foreach (com.oddlabs.tt.animation.Animated anim in java.lang.JIter.of(new java.util.ArrayList(animations)))
            {
                anim.updateChecksum(checksum);
            }
        }
        public bool contain_errors;
        public int errors_logged;
        // Java: 267
        public void containErrors()
        {
            contain_errors = true;
        }
        // Java: 271
        public void animateOne(com.oddlabs.tt.animation.Animated a, float t)
        {
            if (!contain_errors)
            {
                a.animate(t);
                return;
            }
            try
            {
                a.animate(t);
            }
            catch (System.Exception e) when (e is System.Exception || e is System.Exception)
            {
                if (errors_logged < 20)
                {
                    errors_logged++;
                    com.oddlabs.tt.animation.AnimationManager.logger.log(java.util.logging.Level.WARNING, "Error animating " + java.lang.JStr.valueOf(a) + " (it starts over)", e);
                }
                if ((a is com.oddlabs.tt.model.Selectable s))
                {
                    s.recoverFromError();
                }
            }
        }
        // Java: 288
        public void runAnimations(float t)
        {
            tick++;
            flushAnimations();
            if (passing)
            {
                foreach (com.oddlabs.tt.animation.Animated a in java.lang.JIter.of(new java.util.ArrayList(animations)))
                {
                    if (!deleted_animations.contains(a))
                    {
                        animateOne(a, t);
                    }
                }
                return;
            }
            pass.clear();
            pass.addAll(animations);
            passing = true;
            try
            {
                for (int i = 0; i < pass.size(); i++)
                {
                    com.oddlabs.tt.animation.Animated a_2 = ((com.oddlabs.tt.animation.Animated)pass.get(i));
                    if (!deleted_animations.contains(a_2))
                    {
                        animateOne(a_2, t);
                    }
                }
            }
            finally
            {
                passing = false;
                pass.clear();
            }
        }
        // Java: 312
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
            AnimationManager.FRAME_SIMULATION_BUDGET_NANOS = java.util.concurrent.TimeUnit.MILLISECONDS.toNanos(80);
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
            this.animations = new java.util.LinkedHashSet();
            this.deleted_animations = new java.util.LinkedHashSet();
            this.pass = new java.util.ArrayList();
        }
        public AnimationManager() : base()
        {
            init_AnimationManager();
        }
    }
}
