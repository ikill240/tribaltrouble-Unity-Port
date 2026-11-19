// Translated from GameDataModel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver.models
{
    // Java: 6
    public class GameDataModel : java.lang.Object
    {
        public java.sql.Timestamp timeCreate;
        public string name;
        public string rated;
        public int? speed;
        public int? size;
        public int? hills;
        public int? trees;
        public int? resources;
        public string mapcode;
        public string status;
        public int? id;
        public int? winner;
        public java.sql.Timestamp timeStop;
        public java.sql.Timestamp timeStart;
        public java.util.ArrayList players;
        // Java: 31
        public java.sql.Timestamp getTimeCreate()
        {
            return timeCreate;
        }
        // Java: 35
        public void setTimeCreate(java.sql.Timestamp timeCreate)
        {
            this.timeCreate = timeCreate;
        }
        // Java: 39
        public string getName()
        {
            return name;
        }
        // Java: 43
        public void setName(string name)
        {
            this.name = name;
        }
        // Java: 47
        public string getRated()
        {
            return rated;
        }
        // Java: 51
        public void setRated(string rated)
        {
            this.rated = rated;
        }
        // Java: 55
        public int? getSpeed()
        {
            return speed;
        }
        // Java: 59
        public void setSpeed(int? speed)
        {
            this.speed = speed;
        }
        // Java: 63
        public int? getSize()
        {
            return size;
        }
        // Java: 67
        public void setSize(int? size)
        {
            this.size = size;
        }
        // Java: 71
        public int? getHills()
        {
            return hills;
        }
        // Java: 75
        public void setHills(int? hills)
        {
            this.hills = hills;
        }
        // Java: 79
        public int? getTrees()
        {
            return trees;
        }
        // Java: 83
        public void setTrees(int? trees)
        {
            this.trees = trees;
        }
        // Java: 87
        public int? getResources()
        {
            return resources;
        }
        // Java: 91
        public void setResources(int? resources)
        {
            this.resources = resources;
        }
        // Java: 95
        public string getMapcode()
        {
            return mapcode;
        }
        // Java: 99
        public void setMapcode(string mapcode)
        {
            this.mapcode = mapcode;
        }
        // Java: 103
        public string getStatus()
        {
            return status;
        }
        // Java: 107
        public void setStatus(string status)
        {
            this.status = status;
        }
        // Java: 111
        public int? getId()
        {
            return id;
        }
        // Java: 115
        public void setId(int? id)
        {
            this.id = id;
        }
        // Java: 119
        public int? getWinner()
        {
            return winner;
        }
        // Java: 123
        public void setWinner(int? winner)
        {
            this.winner = winner;
        }
        // Java: 127
        public java.sql.Timestamp getTimeStop()
        {
            return timeStop;
        }
        // Java: 131
        public void setTimeStop(java.sql.Timestamp timeStop)
        {
            this.timeStop = timeStop;
        }
        // Java: 135
        public java.sql.Timestamp getTimeStart()
        {
            return timeStart;
        }
        // Java: 139
        public void setTimeStart(java.sql.Timestamp timeStart)
        {
            this.timeStart = timeStart;
        }
        // Java: 143
        public java.util.ArrayList getPlayers()
        {
            return players;
        }
        // Java: 147
        public void setPlayers(java.util.ArrayList players)
        {
            this.players = players;
        }
        private void init_GameDataModel()
        {
            this.players = new java.util.ArrayList();
        }
        public GameDataModel() : base()
        {
            init_GameDataModel();
        }
    }
}
