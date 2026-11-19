// Translated from Session.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 16
    public class Session : java.lang.Object
    {
        public com.oddlabs.router.SessionInfo info;
        public com.oddlabs.router.SessionID session_id;
        public java.util.logging.Logger logger;
        public java.util.Set players;
        public java.util.Set spectators;
        public com.oddlabs.router.SessionManager manager;
        public long initial_time;
        public bool started;
        // Java: 34
        public bool hasClient(int client_id)
        {
            bool[] result = new bool[1];
            visit(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
            {
                if (client.getClientID() == client_id)
                {
                    result[0] = true;
                }
            }));
            return result[0];
        }
        // Java: 43
        public void removePlayer(com.oddlabs.router.RouterClient client)
        {
            players.remove(client);
            if (getNumPlayers() == 0)
            {
                for (java.util.Iterator it = spectators.iterator(); it.hasNext(); )
                {
                    com.oddlabs.router.RouterClient spectator = ((com.oddlabs.router.RouterClient)it.next());
                    it.remove();
                    spectator.doError(false, new System.IO.IOException("Game ended"));
                }
                manager.remove(this);
            }
            checksum();
        }
        // Java: 57
        public void addSpectator(com.oddlabs.router.RouterClient client)
        {
            spectators.add(client);
            client.getInterface().start();
            manager.startTimeout(client);
        }
        // Java: 63
        public void removeSpectator(com.oddlabs.router.RouterClient client)
        {
            spectators.remove(client);
            if (getNumPlayers() == 0)
            {
                manager.remove(this);
            }
        }
        // Java: 68
        public void checksum()
        {
            java.util.Map checksum_to_count = new java.util.HashMap();
            int[] best_checksum = new int[1];
            bool[] missing_checksum = new bool[1];
            visitPlayers(new Session.Anon1_SessionVisitor(this, missing_checksum, checksum_to_count, best_checksum));
            if (checksum_to_count.size() == 1)
            {
                return;
            }
            java.util.List clients_to_be_kicked = new java.util.ArrayList();
            visitPlayers(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
            {
                int? client_checksum = default;
                if (missing_checksum[0])
                {
                    if (client.getChecksums().isEmpty())
                    {
                        return;
                    }
                    client_checksum = ((int?)client.getChecksums().getFirst());
                }
                else
                {
                    client_checksum = ((int?)client.getChecksums().removeFirst());
                }
                if (((int)client_checksum) != best_checksum[0])
                {
                    logger.log(java.util.logging.Level.WARNING, "Kicking client because of checksum error: {0} != {1}", new object[] { client_checksum, best_checksum[0] });
                    clients_to_be_kicked.add(client);
                }
            }));
            foreach (com.oddlabs.router.RouterClient client_2 in java.lang.JIter.of(clients_to_be_kicked))
            {
                client_2.doError(true, new System.IO.IOException("Checksum mismatch"));
            }
        }
        // Java: 117
        public void addPlayer(com.oddlabs.router.RouterClient client)
        {
            players.add(client);
            if (info.num_participants == players.size())
            {
                start();
            }
        }
        // Java: 123
        public int getNextTick()
        {
            return manager.getNextTick(this);
        }
        // Java: 127
        public void startTimeout(com.oddlabs.router.RouterClient client)
        {
            manager.startTimeout(client);
        }
        // Java: 131
        public void start()
        {
            this.started = true;
            visitPlayers(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
            {
                client.getInterface().start();
            }));
            this.initial_time = manager.start(this);
        }
        // Java: 137
        public long getInitialTime()
        {
            return initial_time;
        }
        // Java: 141
        public bool isComplete()
        {
            return started;
        }
        // Java: 145
        public void visitPlayers(com.oddlabs.router.SessionVisitor visitor)
        {
            foreach (com.oddlabs.router.RouterClient client in java.lang.JIter.of(players))
            {
                visitor.visit(client);
            }
        }
        // Java: 151
        public void visit(com.oddlabs.router.SessionVisitor visitor)
        {
            visitPlayers(visitor);
            foreach (com.oddlabs.router.RouterClient client in java.lang.JIter.of(spectators))
            {
                visitor.visit(client);
            }
        }
        // Java: 158
        public void visitSpectators(com.oddlabs.router.SessionVisitor visitor)
        {
            foreach (com.oddlabs.router.RouterClient client in java.lang.JIter.of(spectators))
            {
                visitor.visit(client);
            }
        }
        // Java: 164
        public int getNumPlayers()
        {
            return players.size();
        }
        // Java: 168
        public override string toString()
        {
            string result = "(Session: info = " + java.lang.JStr.valueOf(info) + " players : (";
            result += java.lang.JStr.valueOf(((string)players.stream().map(new java.util.function.Function_L((object a0) => { return java.lang.JO.toString(((com.oddlabs.router.RouterClient)a0)); })).collect(java.util.stream.Collectors.joining(" "))));
            return java.lang.JStr.valueOf(result) + "))";
        }
        private void init_Session()
        {
            this.players = new java.util.HashSet();
            this.spectators = new java.util.HashSet();
        }
        public Session(java.util.logging.Logger logger, com.oddlabs.router.SessionID session_id, com.oddlabs.router.SessionInfo info, com.oddlabs.router.SessionManager manager) : base()
        {
            init_Session();
            this.logger = logger;
            this.manager = manager;
            this.session_id = session_id;
            this.info = info;
        }
        // Java: anonymous class at line 72
        public sealed class Anon1_SessionVisitor : java.lang.Object, com.oddlabs.router.SessionVisitor
        {
            public com.oddlabs.router.Session this_0;
            public bool[] missing_checksum;
            public java.util.Map checksum_to_count;
            public int[] best_checksum;
            public Anon1_SessionVisitor(com.oddlabs.router.Session this_0_, bool[] missing_checksum_c, java.util.Map checksum_to_count_c, int[] best_checksum_c)
            {
                this.this_0 = this_0_;
                this.missing_checksum = missing_checksum_c;
                this.checksum_to_count = checksum_to_count_c;
                this.best_checksum = best_checksum_c;
                init_Anon1_SessionVisitor();
            }
            public int best_checksum_count;
            // Java: 75
            public void visit(com.oddlabs.router.RouterClient client)
            {
                if (client.getChecksums().isEmpty())
                {
                    missing_checksum[0] = true;
                    return;
                }
                int? client_checksum = ((int?)client.getChecksums().getFirst());
                int? count = ((int?)checksum_to_count.get(client_checksum));
                if (count == null)
                {
                    count = 1;
                }
                else
                {
                    count = ((int)count) + 1;
                }
                checksum_to_count.put(client_checksum, count);
                if (best_checksum_count < ((int)count))
                {
                    best_checksum[0] = ((int)client_checksum);
                    best_checksum_count = ((int)count);
                }
            }
            private void init_Anon1_SessionVisitor()
            {
                this.best_checksum_count = 0;
            }
        }
    }
}
