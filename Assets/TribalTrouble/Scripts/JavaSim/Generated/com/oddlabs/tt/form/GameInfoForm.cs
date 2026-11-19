// Translated from GameInfoForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 22
    public class GameInfoForm : com.oddlabs.tt.gui.Form
    {
        public static java.util.ResourceBundle bundle;
        // Java: 25
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.GameInfoForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.HorizButton ok_button;
        // Java: 127
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                ok_button.setFocus(direction);
            }
        }
        static GameInfoForm()
        {
            GameInfoForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.GameInfoForm).getName());
        }
        public GameInfoForm(com.oddlabs.matchmaking.Game game) : base()
        {
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("game_info"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.gui.Group types = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Group values = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_name = new com.oddlabs.tt.gui.Label(i18n("name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_name_value = new com.oddlabs.tt.gui.Label(game.getName(), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_name);
            values.addChild(label_name_value);
            com.oddlabs.tt.gui.Label label_size = new com.oddlabs.tt.gui.Label(i18n("size"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_size_value = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.ServerMessageBundler.getSizeString(game.getSize()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_size);
            values.addChild(label_size_value);
            com.oddlabs.tt.gui.Label label_terrain_type = new com.oddlabs.tt.gui.Label(i18n("terrain_type"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_terrain_type_value = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.ServerMessageBundler.getTerrainTypeString(game.getTerrainType()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_terrain_type);
            values.addChild(label_terrain_type_value);
            com.oddlabs.tt.gui.Label label_hills = new com.oddlabs.tt.gui.Label(i18n("hills"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_hills_value = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.ServerMessageBundler.getHillsString(game.getHills()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_hills);
            values.addChild(label_hills_value);
            com.oddlabs.tt.gui.Label label_trees = new com.oddlabs.tt.gui.Label(i18n("trees"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_trees_value = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.ServerMessageBundler.getTreesString(game.getTrees()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_trees);
            values.addChild(label_trees_value);
            com.oddlabs.tt.gui.Label label_supplies = new com.oddlabs.tt.gui.Label(i18n("supplies"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_supplies_value = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.ServerMessageBundler.getSuppliesString(game.getSupplies()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_supplies);
            values.addChild(label_supplies_value);
            com.oddlabs.tt.gui.Label label_rated = new com.oddlabs.tt.gui.Label(i18n("rated"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_rated_value = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.ServerMessageBundler.getRatedString(game.isRated()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_rated);
            values.addChild(label_rated_value);
            com.oddlabs.tt.gui.Label label_gamespeed = new com.oddlabs.tt.gui.Label(i18n("gamespeed"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_gamespeed_value = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(game.getGamespeed()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_gamespeed);
            values.addChild(label_gamespeed_value);
            com.oddlabs.tt.gui.Label label_mapcode = new com.oddlabs.tt.gui.Label(i18n("mapcode"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_mapcode_value = new com.oddlabs.tt.gui.Label(game.getMapcode(), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_mapcode);
            values.addChild(label_mapcode_value);
            label_name.place();
            label_rated.place(label_name, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_gamespeed.place(label_rated, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_terrain_type.place(label_gamespeed, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_size.place(label_terrain_type, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_hills.place(label_size, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_trees.place(label_hills, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_supplies.place(label_trees, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_mapcode.place(label_supplies, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            types.compileCanvas();
            addChild(types);
            label_name_value.place();
            label_rated_value.place(label_name_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_gamespeed_value.place(label_rated_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_terrain_type_value.place(label_gamespeed_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_size_value.place(label_terrain_type_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_hills_value.place(label_size_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_trees_value.place(label_hills_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_supplies_value.place(label_trees_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_mapcode_value.place(label_supplies_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            values.compileCanvas();
            addChild(values);
            ok_button = new com.oddlabs.tt.gui.OKButton(100);
            addChild(ok_button);
            ok_button.addMouseClickListener(new com.oddlabs.tt.gui.OKListener(this));
            label_headline.place();
            types.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            values.place(types, com.oddlabs.tt.gui.Placement.RIGHT_TOP);
            ok_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            centerPos();
        }
    }
}
