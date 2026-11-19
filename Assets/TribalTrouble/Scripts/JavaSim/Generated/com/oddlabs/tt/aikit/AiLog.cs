// Translated from AiLog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.aikit
{
    // Java: 44
    public class AiLog : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public const int MAX_ERROR_LINES = 20;
        public const int STACK_ERRORS = 3;
        public const int STACK_FRAMES = 12;
        public static java.lang.@ref.WeakReference bound_world;
        public static com.oddlabs.tt.aikit.AiLog[] handles;
        public java.lang.@ref.WeakReference world;
        public int slot;
        public java.nio.file.Path file;
        public string header;
        public java.io.Writer sink;
        public bool off;
        public int errors_f;
        public string first_error;
        public java.util.SortedMap counters_f;
        // Java: 78
        public static com.oddlabs.tt.aikit.AiLog of(com.oddlabs.tt.player.Player player)
        {
            com.oddlabs.tt.landscape.World world = player.getWorld();
            if (((object)world != (object)((com.oddlabs.tt.landscape.World)com.oddlabs.tt.aikit.AiLog.bound_world.get())))
            {
                com.oddlabs.tt.aikit.AiLog.begin(world, null, "");
            }
            return com.oddlabs.tt.aikit.AiLog.handles[com.oddlabs.tt.aikit.AiLog.slotOf(world, player)];
        }
        // Java: 86
        public void log(string topic, string message)
        {
            if (on())
            {
                write(topic, message);
            }
        }
        // Java: 93
        public void log(string topic, java.util.function.Supplier message)
        {
            if (on())
            {
                string text = default;
                try
                {
                    text = ((string)message.get());
                }
                catch (System.Exception e) when (e is System.Exception || e is System.Exception || e is System.Exception)
                {
                    text = "LOG-FAILED " + java.lang.JStr.valueOf(e);
                }
                write(topic, text);
            }
        }
        // Java: 106
        public void count(string key)
        {
            counters_f.merge(key, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
        }
        // Java: 111
        public void error(string @where, System.Exception t)
        {
            errors_f++;
            if (first_error == null)
            {
                first_error = java.lang.JStr.valueOf(@where) + ": " + java.lang.JStr.valueOf(t);
            }
            if (on() && errors_f <= com.oddlabs.tt.aikit.AiLog.MAX_ERROR_LINES)
            {
                java.lang.StringBuilder text = new java.lang.StringBuilder("#" + (errors_f) + " " + java.lang.JStr.valueOf(@where) + ": " + java.lang.JStr.valueOf(t));
                if (errors_f <= com.oddlabs.tt.aikit.AiLog.STACK_ERRORS)
                {
                    java.lang.StackTraceElement[] stack = t.getStackTrace();
                    for (int i = 0; i < java.lang.Math.min(com.oddlabs.tt.aikit.AiLog.STACK_FRAMES, stack.Length); i++)
                    {
                        text.append(" | at ").append(stack[i]);
                    }
                }
                write("ERROR", java.lang.JO.toString(text));
            }
        }
        // Java: 130
        public int errors()
        {
            return errors_f;
        }
        // Java: 134
        public string firstError()
        {
            return first_error;
        }
        // Java: 138
        public java.util.SortedMap counters()
        {
            return java.util.Collections.unmodifiableSortedMap(counters_f);
        }
        // Java: 147
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static void begin(com.oddlabs.tt.landscape.World world, java.util.function.IntFunction file_of_slot, string header)
        {
            com.oddlabs.tt.aikit.AiLog.end();
            com.oddlabs.tt.aikit.AiLog.bound_world = new java.lang.@ref.WeakReference(world);
            com.oddlabs.tt.player.Player[] players = world.getPlayers();
            com.oddlabs.tt.aikit.AiLog.handles = new com.oddlabs.tt.aikit.AiLog[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                com.oddlabs.tt.aikit.AiLog.handles[i] = new com.oddlabs.tt.aikit.AiLog(world, i, (file_of_slot == null ? (java.nio.file.Path)(null) : ((java.nio.file.Path)file_of_slot.apply(i))), header);
            }
        }
        // Java: 159
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static com.oddlabs.tt.aikit.AiLog peek(com.oddlabs.tt.landscape.World world, int slot)
        {
            return (((object)world == (object)((com.oddlabs.tt.landscape.World)com.oddlabs.tt.aikit.AiLog.bound_world.get())) && slot < com.oddlabs.tt.aikit.AiLog.handles.Length ? com.oddlabs.tt.aikit.AiLog.handles[slot] : (com.oddlabs.tt.aikit.AiLog)(null));
        }
        // Java: 163
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static void flushAll()
        {
            foreach (com.oddlabs.tt.aikit.AiLog handle in com.oddlabs.tt.aikit.AiLog.handles)
            {
                handle.flush();
            }
        }
        // Java: 170
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static void end()
        {
            foreach (com.oddlabs.tt.aikit.AiLog handle in com.oddlabs.tt.aikit.AiLog.handles)
            {
                handle.close();
            }
            com.oddlabs.tt.aikit.AiLog.handles = new com.oddlabs.tt.aikit.AiLog[0];
            com.oddlabs.tt.aikit.AiLog.bound_world = new java.lang.@ref.WeakReference(null);
        }
        // Java: 179
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static void endFor(com.oddlabs.tt.landscape.World world)
        {
            if (((object)world == (object)((com.oddlabs.tt.landscape.World)com.oddlabs.tt.aikit.AiLog.bound_world.get())))
            {
                com.oddlabs.tt.aikit.AiLog.end();
            }
        }
        // Java: 186
        public static int slotOf(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player player)
        {
            com.oddlabs.tt.player.Player[] players = world.getPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                if (((object)players[i] == (object)player))
                {
                    return i;
                }
            }
            throw new System.ArgumentException("player is not part of its world");
        }
        // Java: 198
        public bool on()
        {
            return file != null && !off;
        }
        // Java: 202
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void write(string topic, string message)
        {
            java.nio.file.Path path = file;
            com.oddlabs.tt.landscape.World game = ((com.oddlabs.tt.landscape.World)world.get());
            if (path == null || off || game == null)
            {
                return;
            }
            try
            {
                java.io.Writer @out = sink;
                if (@out == null)
                {
                    @out = java.nio.file.Files.newBufferedWriter(path, java.nio.charset.StandardCharsets.UTF_8);
                    sink = @out;
                    @out.write(headerLine(game));
                }
                string text = java.lang.JString.valueOf(message).replace('\n', ' ');
                @out.write(java.lang.JString.format(java.util.Locale.ROOT, "%8.2f s%d %-5s %s\n", com.oddlabs.tt.aikit.GameTime.seconds(game), slot, topic, text));
            }
            catch (System.IO.IOException e)
            {
                disable("write", e);
            }
        }
        // Java: 224
        public string headerLine(com.oddlabs.tt.landscape.World game)
        {
            string name = game.getPlayers()[slot].getPlayerInfo().getName();
            return "# ai-log v1 slot=" + (slot) + " name=" + java.lang.JStr.valueOf(name) + java.lang.JStr.valueOf(((header.isEmpty() ? "" : " " + java.lang.JStr.valueOf(header)))) + "\n";
        }
        // Java: 229
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void flush()
        {
            java.io.Writer @out = sink;
            if (@out == null)
            {
                return;
            }
            try
            {
                @out.flush();
            }
            catch (System.IO.IOException e)
            {
                disable("flush", e);
            }
        }
        // Java: 242
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void close()
        {
            off = true;
            java.io.Writer @out = sink;
            if (@out == null)
            {
                return;
            }
            try
            {
                @out.close();
                sink = null;
            }
            catch (System.IO.IOException e)
            {
                disable("close", e);
            }
        }
        // Java: 257
        public void disable(string method, System.IO.IOException e)
        {
            off = true;
            com.oddlabs.tt.aikit.AiLog.logger.logp(java.util.logging.Level.WARNING, typeof(com.oddlabs.tt.aikit.AiLog).getName(), method, "AI log " + java.lang.JStr.valueOf(file) + " disabled", e);
        }
        static AiLog()
        {
            AiLog.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.aikit.AiLog).getName());
            AiLog.bound_world = new java.lang.@ref.WeakReference(null);
            AiLog.handles = new com.oddlabs.tt.aikit.AiLog[0];
        }
        private void init_AiLog()
        {
            this.counters_f = new java.util.TreeMap();
        }
        public AiLog(com.oddlabs.tt.landscape.World world, int slot, java.nio.file.Path file, string header) : base()
        {
            init_AiLog();
            this.world = new java.lang.@ref.WeakReference(world);
            this.slot = slot;
            this.file = file;
            this.header = header;
        }
    }
}
