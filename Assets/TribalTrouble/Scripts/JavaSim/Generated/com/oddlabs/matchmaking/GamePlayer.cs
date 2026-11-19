// Translated from GamePlayer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 5
    public class GamePlayer : java.lang.Object
    {
        public string _nick;
        public int _team;
        public int _race;
        public com.oddlabs.matchmaking.PlayerTypes _playerType;
        // Java: 33
        public virtual com.oddlabs.matchmaking.PlayerTypes getPlayerType()
        {
            return _playerType;
        }
        // Java: 37
        public virtual string getNick()
        {
            return _nick;
        }
        // Java: 41
        public virtual int getTeam()
        {
            return _team;
        }
        // Java: 45
        public virtual int getRace()
        {
            return _race;
        }
        public GamePlayer(string nick, int team, int race, com.oddlabs.matchmaking.PlayerTypes playerType) : base()
        {
            _nick = nick;
            _team = team;
            _race = race;
            _playerType = playerType;
            if (((object)_playerType != (object)com.oddlabs.matchmaking.PlayerTypes.Human))
            {
                switch ((_playerType).ordinal())
                {
                    case 2 /*AIEasy*/:
                    {
                        _nick = "Easy AI";
                        break;
                    }
                    case 3 /*AINormal*/:
                    {
                        _nick = "Normal AI";
                        break;
                    }
                    case 4 /*AIHard*/:
                    {
                        _nick = "Hard AI";
                        break;
                    }
                    default:
                    {
                        _nick = "Unknown player type";
                        break;
                    }
                }
            }
        }
    }
}
