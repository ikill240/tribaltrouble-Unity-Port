// Translated from SecureConnection.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 21
    public class SecureConnection : com.oddlabs.net.AbstractConnection, com.oddlabs.net.SecureConnectionInterface
    {
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public com.oddlabs.@event.Deterministic deterministic;
        public com.oddlabs.net.AbstractConnection wrapped_connection;
        public com.oddlabs.net.SecureConnectionInterface secure_interface;
        public javax.crypto.KeyAgreement key_agreement;
        public java.util.List event_backlog;
        public javax.crypto.Cipher decrypt_cipher;
        public javax.crypto.Cipher encrypt_cipher;
        // Java: 66
        public com.oddlabs.net.AbstractConnection getWrappedConnection()
        {
            return wrapped_connection;
        }
        // Java: 70
        public javax.crypto.SealedObject encrypt(com.oddlabs.net.ARMIEvent @event)
        {
            try
            {
                return new javax.crypto.SealedObject(@event, encrypt_cipher);
            }
            catch (System.Exception e) when (e is System.Exception || e is System.IO.IOException)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 78
        public void initAgreement(sbyte[] public_key_encoded)
        {
            if (isConnected())
            {
                return;
            }
            try
            {
                javax.crypto.KeyAgreement key_agreement = this.key_agreement;
                java.security.PublicKey public_key = com.oddlabs.util.KeyManager.readPublicKey(public_key_encoded, com.oddlabs.util.KeyManager.AGREEMENT_ALGORITHM);
                if (key_agreement == null)
                {
                    java.security.KeyPair key_pair = ((java.security.KeyPair)deterministic.log(com.oddlabs.util.KeyManager.generateKeyPairFromKey(public_key)));
                    key_agreement = com.oddlabs.util.KeyManager.generateAgreement(key_pair.getPrivate());
                    secure_interface.initAgreement(key_pair.getPublic().getEncoded());
                }
                decrypt_cipher = com.oddlabs.util.KeyManager.createCipher(javax.crypto.Cipher.DECRYPT_MODE, key_agreement, public_key);
                encrypt_cipher = com.oddlabs.util.KeyManager.createCipher(javax.crypto.Cipher.ENCRYPT_MODE, key_agreement, public_key);
                notifyConnected();
                foreach (com.oddlabs.net.ARMIEvent @event in java.lang.JIter.of(event_backlog))
                {
                    tunnel(@event);
                }
            }
            catch (System.IO.IOException e)
            {
                notifyError(e);
            }
            catch (System.Exception e_2)
            {
                notifyError(new System.IO.IOException(e_2.getMessage()));
            }
        }
        // Java: 103
        public void tunnelEvent(javax.crypto.SealedObject sealed_event)
        {
            try
            {
                if (decrypt_cipher == null)
                {
                    throw new System.IO.IOException("Illegal stream state, event received before key agreement");
                }
                com.oddlabs.net.ARMIEvent @event = ((com.oddlabs.net.ARMIEvent)sealed_event.getObject(decrypt_cipher));
                receiveEvent(@event);
            }
            catch (System.Exception e) when (e is System.Exception || e is System.Exception || e is System.Exception)
            {
                notifyError(new System.IO.IOException(e.getMessage()));
            }
            catch (System.IO.IOException e_2)
            {
                notifyError(e_2);
            }
        }
        // Java: 117
        public com.oddlabs.net.AbstractConnection getWrappedConnectionAndShutdown()
        {
            wrapped_connection.setConnectionInterface(getConnectionInterface());
            return wrapped_connection;
        }
        // Java: 122
        public override void doClose()
        {
            wrapped_connection.close();
        }
        // Java: 127
        public void tunnel(com.oddlabs.net.ARMIEvent @event)
        {
            secure_interface.tunnelEvent(encrypt(@event));
        }
        // Java: 131
        public void processEvent(com.oddlabs.net.ARMIEvent @event)
        {
            try
            {
                @event.execute(interface_methods, this);
            }
            catch (com.oddlabs.net.IllegalARMIEventException e)
            {
                notifyError(new System.IO.IOException(e.getMessage()));
            }
        }
        // Java: 139
        public override void handle(com.oddlabs.net.ARMIEvent @event)
        {
            if (encrypt_cipher == null)
            {
                event_backlog.add(@event);
            }
            else
            {
                tunnel(@event);
            }
        }
        private void init_SecureConnection()
        {
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.net.SecureConnectionInterface));
            this.event_backlog = new java.util.ArrayList();
        }
        public SecureConnection(com.oddlabs.@event.Deterministic deterministic, com.oddlabs.net.AbstractConnection wrapped_conn, java.security.spec.AlgorithmParameterSpec param_spec) : base()
        {
            init_SecureConnection();
            this.deterministic = deterministic;
            setConnectionInterface(wrapped_conn.getConnectionInterface());
            this.wrapped_connection = wrapped_conn;
            wrapped_connection.setConnectionInterface(new SecureConnection.Anon1_ConnectionInterface(this));
            this.secure_interface = ((com.oddlabs.net.SecureConnectionInterface)com.oddlabs.net.ARMIEvent.createProxy(wrapped_connection, typeof(com.oddlabs.net.SecureConnectionInterface)));
            if (param_spec != null)
            {
                java.security.KeyPair key_pair = com.oddlabs.util.KeyManager.generateInitialKeyPair(param_spec);
                this.key_agreement = com.oddlabs.util.KeyManager.generateAgreement(key_pair.getPrivate());
                secure_interface.initAgreement(key_pair.getPublic().getEncoded());
            }
            else
            {
                this.key_agreement = null;
            }
        }
        // Java: anonymous class at line 36
        public sealed class Anon1_ConnectionInterface : java.lang.Object, com.oddlabs.net.ConnectionInterface
        {
            public com.oddlabs.net.SecureConnection this_0;
            public Anon1_ConnectionInterface(com.oddlabs.net.SecureConnection this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 37
            public void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e)
            {
                this_0.notifyError(e);
            }
            // Java: 42
            public void connected(com.oddlabs.net.AbstractConnection conn)
            {
            }
            // Java: 46
            public void handle(object sender, com.oddlabs.net.ARMIEvent @event)
            {
                this_0.processEvent(@event);
            }
            // Java: 51
            public void writeBufferDrained(com.oddlabs.net.AbstractConnection conn)
            {
                this.this_0.writeBufferDrained();
            }
        }
    }
}
