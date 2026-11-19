// Translated from OpenSkillLeaderboard.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 26
    public class OpenSkillLeaderboard : com.oddlabs.tt.gui.GUIObject
    {
        public const int RANK_WIDTH = 50;
        public const int NAME_WIDTH = 350;
        public const int SCORE_WIDTH = 100;
        public const int HEIGHT = 350;
        // Java: 33
        public class Column : java.lang.Object
        {
            public string caption() { return caption_f; }
            public int width() { return width_f; }
            public com.oddlabs.tt.form.OpenSkillLeaderboard.CellFactory cellFactory() { return cellFactory_f; }
            public Column(string caption, int width, com.oddlabs.tt.form.OpenSkillLeaderboard.CellFactory cellFactory)
            {
                this.caption_f = caption;
                this.width_f = width;
                this.cellFactory_f = cellFactory;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Column r)) return false; return true && java.lang.JO.equals(caption_f, r.caption_f) && java.lang.JO.equals(width_f, r.width_f) && java.lang.JO.equals(cellFactory_f, r.cellFactory_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(caption_f); h = 31 * h + java.lang.JO.hashCode(width_f); h = 31 * h + java.lang.JO.hashCode(cellFactory_f); } return h; }
            public override string toString() { return "Column[" + "caption=" + java.lang.JO.toString(caption_f) + ", width=" + java.lang.JO.toString(width_f) + ", cellFactory=" + java.lang.JO.toString(cellFactory_f) + "]"; }
            public string caption_f;
            public int width_f;
            public com.oddlabs.tt.form.OpenSkillLeaderboard.CellFactory cellFactory_f;
            // Java: 34
            public com.oddlabs.tt.gui.ColumnInfo toColumnInfo()
            {
                return new com.oddlabs.tt.gui.ColumnInfo(caption_f, width_f);
            }
            // Java: 38
            public com.oddlabs.tt.gui.Label createCell(com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry)
            {
                com.oddlabs.tt.font.Font font = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font();
                if (entry == null)
                {
                    return new com.oddlabs.tt.gui.Label("", font, width_f);
                }
                return cellFactory_f.create(entry, font, width_f);
            }
        }
        // Java: 47
        public interface CellFactory
        {
            // Java: 49
            com.oddlabs.tt.gui.Label create(com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry, com.oddlabs.tt.font.Font font, int width);
        }
        public sealed class CellFactory_L : java.lang.Object, CellFactory
        {
            readonly System.Func<com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry, com.oddlabs.tt.font.Font, int, com.oddlabs.tt.gui.Label> f;
            public CellFactory_L(System.Func<com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry, com.oddlabs.tt.font.Font, int, com.oddlabs.tt.gui.Label> f) { this.f = f; }
            public com.oddlabs.tt.gui.Label create(com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry p0, com.oddlabs.tt.font.Font p1, int p2) { return f(p0, p1, p2); }
        }
        public java.util.List columns;
        public com.oddlabs.tt.form.OpenSkillPersonalRankingRow personalRow;
        public com.oddlabs.tt.gui.MultiColumnComboBox rankingTable;
        // Java: 92
        public void addRow(com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry)
        {
            rankingTable.addRow(createRow(entry));
        }
        // Java: 96
        public void clearRows()
        {
            rankingTable.clear();
        }
        // Java: 100
        public void setPersonalEntry(com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry)
        {
            personalRow.setEntry(entry);
        }
        // Java: 104
        public com.oddlabs.tt.gui.Row createRow(com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry)
        {
            com.oddlabs.tt.gui.Label[] cells = ((com.oddlabs.tt.gui.Label[])columns.stream().map(new java.util.function.Function_L((object column_l) =>
            {
                com.oddlabs.tt.form.OpenSkillLeaderboard.Column column = (com.oddlabs.tt.form.OpenSkillLeaderboard.Column)column_l;
                return column.createCell(entry);
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.gui.Label[a0]; })));
            return new com.oddlabs.tt.gui.Row(cells, entry);
        }
        // Java: 109
        public static string formatRating(com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry)
        {
            return (entry.rating()) + java.lang.JStr.valueOf(((entry.provisional() ? "?" : "")));
        }
        // Java: 113
        public static string formatScore(double @value)
        {
            return java.lang.JString.format(java.util.Locale.ROOT, "%.3f", @value);
        }
        public OpenSkillLeaderboard(com.oddlabs.tt.gui.GUIRoot guiRoot, java.util.function.Function i18n) : base()
        {
            columns = java.util.List_.of(new com.oddlabs.tt.form.OpenSkillLeaderboard.Column(((string)i18n.apply("rank")), com.oddlabs.tt.form.OpenSkillLeaderboard.RANK_WIDTH, new com.oddlabs.tt.form.OpenSkillLeaderboard.CellFactory_L((com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry, com.oddlabs.tt.font.Font f, int w) =>
            {
                return (entry.rank() > 0 ? (com.oddlabs.tt.gui.Label)(new com.oddlabs.tt.gui.IntegerLabel(entry.rank(), f)) : new com.oddlabs.tt.gui.Label("-", f, w));
            })), new com.oddlabs.tt.form.OpenSkillLeaderboard.Column(((string)i18n.apply("name")), com.oddlabs.tt.form.OpenSkillLeaderboard.NAME_WIDTH, new com.oddlabs.tt.form.OpenSkillLeaderboard.CellFactory_L((com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry_2, com.oddlabs.tt.font.Font f_2, int w_2) =>
            {
                return new com.oddlabs.tt.gui.Label(entry_2.nick(), f_2, w_2);
            })), new com.oddlabs.tt.form.OpenSkillLeaderboard.Column(((string)i18n.apply("rating")), com.oddlabs.tt.form.OpenSkillLeaderboard.SCORE_WIDTH, new com.oddlabs.tt.form.OpenSkillLeaderboard.CellFactory_L((com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry_3, com.oddlabs.tt.font.Font f_3, int w_3) =>
            {
                return new com.oddlabs.tt.gui.NumericLabel(com.oddlabs.tt.form.OpenSkillLeaderboard.formatRating(entry_3), f_3, w_3, entry_3.rating());
            })), new com.oddlabs.tt.form.OpenSkillLeaderboard.Column(((string)i18n.apply("mu")), com.oddlabs.tt.form.OpenSkillLeaderboard.SCORE_WIDTH, new com.oddlabs.tt.form.OpenSkillLeaderboard.CellFactory_L((com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry_4, com.oddlabs.tt.font.Font f_4, int w_4) =>
            {
                return new com.oddlabs.tt.gui.NumericLabel(com.oddlabs.tt.form.OpenSkillLeaderboard.formatScore(entry_4.mu()), f_4, w_4, entry_4.mu());
            })), new com.oddlabs.tt.form.OpenSkillLeaderboard.Column(((string)i18n.apply("sigma")), com.oddlabs.tt.form.OpenSkillLeaderboard.SCORE_WIDTH, new com.oddlabs.tt.form.OpenSkillLeaderboard.CellFactory_L((com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry_5, com.oddlabs.tt.font.Font f_5, int w_5) =>
            {
                return new com.oddlabs.tt.gui.NumericLabel(com.oddlabs.tt.form.OpenSkillLeaderboard.formatScore(entry_5.sigma()), f_5, w_5, entry_5.sigma());
            })));
            com.oddlabs.tt.gui.ColumnInfo[] columnInfos = ((com.oddlabs.tt.gui.ColumnInfo[])columns.stream().map(new java.util.function.Function_L((object a0) => { return ((com.oddlabs.tt.form.OpenSkillLeaderboard.Column)a0).toColumnInfo(); })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.gui.ColumnInfo[a0]; })));
            int spacing = com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing();
            personalRow = new com.oddlabs.tt.form.OpenSkillPersonalRankingRow(columnInfos, new java.util.function.Function_L((object a0) => { return this.createRow(((com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry)a0)); }));
            rankingTable = new com.oddlabs.tt.gui.MultiColumnComboBox(guiRoot, columnInfos, com.oddlabs.tt.form.OpenSkillLeaderboard.HEIGHT - personalRow.getHeight() - spacing);
            rankingTable.setPos(0, 0);
            personalRow.setPos(0, rankingTable.getHeight() + spacing);
            addChild(personalRow);
            addChild(rankingTable);
            setDim(rankingTable.getWidth(), com.oddlabs.tt.form.OpenSkillLeaderboard.HEIGHT);
            setCanFocus(true);
        }
    }
    // Java: 118
    public class OpenSkillPersonalRankingRow : com.oddlabs.tt.gui.GUIObject
    {
        public com.oddlabs.tt.gui.Box box;
        public com.oddlabs.tt.gui.ColumnInfo[] columnInfos;
        public java.util.function.Function rowFactory;
        public com.oddlabs.tt.gui.Row row;
        // Java: 137
        public void setEntry(com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry)
        {
            if (row != null)
            {
                row.remove();
            }
            row = mountRow(entry);
        }
        // Java: 144
        public com.oddlabs.tt.gui.Row mountRow(com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry)
        {
            com.oddlabs.tt.gui.Row newRow = ((com.oddlabs.tt.gui.Row)rowFactory.apply(entry));
            newRow.setColumnInfos(columnInfos);
            newRow.setPos(box.getLeftOffset(), box.getBottomOffset());
            addChild(newRow);
            return newRow;
        }
        // Java: 152
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            box.render(renderer, 0.0f, 0.0f, getWidth(), getHeight(), com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
        }
        public OpenSkillPersonalRankingRow(com.oddlabs.tt.gui.ColumnInfo[] columnInfos, java.util.function.Function rowFactory) : base()
        {
            this.columnInfos = columnInfos;
            this.rowFactory = rowFactory;
            box = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().box();
            com.oddlabs.tt.gui.Row initialRow = mountRow(null);
            int width = java.util.Arrays.stream(columnInfos).mapToInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.gui.ColumnInfo)a0).width(); })).sum();
            setDim(width, box.getTopOffset() + initialRow.getHeight() + box.getBottomOffset());
        }
    }
}
