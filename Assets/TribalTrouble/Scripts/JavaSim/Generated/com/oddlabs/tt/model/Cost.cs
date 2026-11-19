// Translated from Cost.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 11
    public class Cost : java.lang.Object
    {
        public System.Type[] supply_types;
        public int[] supply_amounts;
        // Java: 21
        public System.Type[] getSupplyTypes()
        {
            return supply_types;
        }
        // Java: 25
        public int[] getSupplyAmounts()
        {
            return supply_amounts;
        }
        // Java: 29
        public java.util.List iconList()
        {
            java.util.List result = new java.util.ArrayList();
            for (int i = 0; i < supply_types.Length; i++)
            {
                com.oddlabs.tt.gui.IconQuad icon = getIconQuad(supply_types[i]);
                for (int j = 0; j < supply_amounts[i]; j++)
                {
                    result.add(icon);
                }
            }
            return result;
        }
        // Java: 40
        public com.oddlabs.tt.gui.IconQuad getIconQuad(System.Type supply_type)
        {
            com.oddlabs.tt.gui.IconQuad icon = default;
            if (((object)supply_type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
            {
                icon = com.oddlabs.tt.gui.GUIIcons.getIcons().getTreeStatusIcon();
            }
            else if (((object)supply_type == (object)typeof(com.oddlabs.tt.model.RockSupply)))
            {
                icon = com.oddlabs.tt.gui.GUIIcons.getIcons().getRockStatusIcon();
            }
            else if (((object)supply_type == (object)typeof(com.oddlabs.tt.model.IronSupply)))
            {
                icon = com.oddlabs.tt.gui.GUIIcons.getIcons().getIronStatusIcon();
            }
            else if (((object)supply_type == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
            {
                icon = com.oddlabs.tt.gui.GUIIcons.getIcons().getRubberStatusIcon();
            }
            else if (com.oddlabs.tt.model.ExtendedResources.isRare(supply_type))
            {
                icon = com.oddlabs.tt.gui.ExtendedIcons.getIcons().getRareStatus(com.oddlabs.tt.model.ExtendedResources.rareIndex(supply_type));
            }
            else
            {
                throw new System.Exception("Wrong supply_type");
            }
            return icon;
        }
        public Cost(System.Type[] supply_types, int[] supply_amounts) : base()
        {
            this.supply_types = supply_types;
            this.supply_amounts = supply_amounts;
        }
    }
}
