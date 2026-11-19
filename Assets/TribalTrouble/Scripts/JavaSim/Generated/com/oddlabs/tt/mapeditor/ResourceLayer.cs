// Translated from ResourceLayer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 40
    public class ResourceLayer : java.lang.Object
    {
        public const float FULL_DENSITY_SPACING = 2.8f;
        // Java: 48
        public class Stroke : java.lang.Object
        {
            public java.util.List added;
            public java.util.List removed;
            // Java: 52
            public bool isEmpty()
            {
                return added.isEmpty() && removed.isEmpty();
            }
            private void init_Stroke()
            {
                this.added = new java.util.ArrayList();
                this.removed = new java.util.ArrayList();
            }
            public Stroke() : base()
            {
                init_Stroke();
            }
        }
        // Java: 58
        public interface ChangeListener
        {
            // Java: 60
            void resourcesChanged(com.oddlabs.tt.mapeditor.ResourceLayer layer, int x0, int y0, int x1, int y1);
        }
        public sealed class ChangeListener_L : java.lang.Object, ChangeListener
        {
            readonly System.Action<com.oddlabs.tt.mapeditor.ResourceLayer, int, int, int, int> f;
            public ChangeListener_L(System.Action<com.oddlabs.tt.mapeditor.ResourceLayer, int, int, int, int> f) { this.f = f; }
            public void resourcesChanged(com.oddlabs.tt.mapeditor.ResourceLayer p0, int p1, int p2, int p3, int p4) { f(p0, p1, p2, p3, p4); }
        }
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.mapeditor.AccessMap access;
        public int size;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain;
        public com.oddlabs.tt.mapeditor.ResourceLayer.ChangeListener listener;
        public com.oddlabs.tt.mapeditor.Resource[] kinds;
        public object[] live;
        public int[] disk;
        public int disk_radius;
        public bool trees_changed;
        public com.oddlabs.tt.pathfinder.Region opened_region;
        // Java: 92
        public void indexTrees(com.oddlabs.tt.landscape.AbstractTreeGroup group)
        {
            group.visit(new ResourceLayer.Anon1_TreeNodeVisitor(this));
        }
        // Java: 112
        public void indexSupplies()
        {
            com.oddlabs.tt.model.AbstractElementNode root = world.getElementRoot();
            root.visit(new ResourceLayer.Anon2_ElementNodeVisitor(this));
        }
        // Java: 137
        public void put(int x, int y, com.oddlabs.tt.mapeditor.Resource kind, object @object)
        {
            kinds[y * size + x] = kind;
            live[y * size + x] = @object;
        }
        // Java: 143
        public java.util.List positions(com.oddlabs.tt.mapeditor.Resource kind)
        {
            return positionsIn(kind, 0, 0, size - 1, size - 1);
        }
        // Java: 148
        public java.util.List positionsIn(com.oddlabs.tt.mapeditor.Resource kind, int x0, int y0, int x1, int y1)
        {
            java.util.List positions_v = new java.util.ArrayList();
            for (int y = java.lang.Math.max(0, y0); y <= java.lang.Math.min(size - 1, y1); y++)
            {
                for (int x = java.lang.Math.max(0, x0); x <= java.lang.Math.min(size - 1, x1); x++)
                {
                    if (((object)kinds[y * size + x] == (object)kind))
                    {
                        positions_v.add(new int[] { x, y });
                    }
                }
            }
            return positions_v;
        }
        // Java: 165
        public void paint(com.oddlabs.tt.mapeditor.Resource kind, float cx, float cy, float radius, float density, com.oddlabs.tt.mapeditor.ResourceLayer.Stroke stroke)
        {
            int center_x = java.lang.Math.round((float)(cx / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID));
            int center_y = java.lang.Math.round((float)(cy / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID));
            int r = java.lang.Math.max(0, java.lang.Math.round((float)(radius / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID)));
            int[] cells = diskCells(r);
            float spacing = (density <= 0.0f ? java.lang.Float.POSITIVE_INFINITY : (float)(com.oddlabs.tt.mapeditor.ResourceLayer.FULL_DENSITY_SPACING / ((float)(java.lang.Math.sqrt(density)))));
            if (spacing > 2 * r)
            {
                if (anyWithin(kind, center_x, center_y, r))
                {
                    return;
                }
                spacing = java.lang.Float.POSITIVE_INFINITY;
            }
            int reach = (java.lang.Float.isInfinite(spacing) ? 0 : java.lang.JC.d2i(java.lang.Math.ceil(spacing)));
            for (int i = 0; i < cells.Length; i += 2)
            {
                int x = center_x + cells[i];
                int y = center_y + cells[i + 1];
                if (!canPlace(x, y) || (reach > 0 && kindWithin(kind, x, y, spacing, reach)))
                {
                    continue;
                }
                place(kind, x, y);
                stroke.added.add(new int[] { kind.ordinal(), x, y });
                changed(x, y);
                if (reach == 0)
                {
                    break;
                }
            }
            finish();
        }
        // Java: 193
        public void erase(com.oddlabs.tt.mapeditor.Resource kind, float cx, float cy, float radius, com.oddlabs.tt.mapeditor.ResourceLayer.Stroke stroke)
        {
            int center_x = java.lang.Math.round((float)(cx / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID));
            int center_y = java.lang.Math.round((float)(cy / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID));
            int[] cells = diskCells(java.lang.Math.max(0, java.lang.Math.round((float)(radius / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID))));
            for (int i = 0; i < cells.Length; i += 2)
            {
                int x = center_x + cells[i];
                int y = center_y + cells[i + 1];
                com.oddlabs.tt.mapeditor.Resource found = (inside(x, y) ? kinds[y * size + x] : (com.oddlabs.tt.mapeditor.Resource)(null));
                if (found != null && (kind == null || ((object)found == (object)kind)))
                {
                    remove(x, y);
                    stroke.removed.add(new int[] { found.ordinal(), x, y });
                    changed(x, y);
                }
            }
            finish();
        }
        // Java: 211
        public void undo(com.oddlabs.tt.mapeditor.ResourceLayer.Stroke stroke)
        {
            foreach (int[] added in java.lang.JIter.of(stroke.added))
            {
                if (kinds[added[2] * size + added[1]] != null)
                {
                    remove(added[1], added[2]);
                    changed(added[1], added[2]);
                }
            }
            foreach (int[] removed in java.lang.JIter.of(stroke.removed))
            {
                if (kinds[removed[2] * size + removed[1]] == null)
                {
                    place(com.oddlabs.tt.mapeditor.Resource.values()[removed[0]], removed[1], removed[2]);
                    changed(removed[1], removed[2]);
                }
            }
            finish();
        }
        // Java: 234
        public void redo(com.oddlabs.tt.mapeditor.ResourceLayer.Stroke stroke)
        {
            foreach (int[] removed in java.lang.JIter.of(stroke.removed))
            {
                if (kinds[removed[2] * size + removed[1]] != null)
                {
                    remove(removed[1], removed[2]);
                    changed(removed[1], removed[2]);
                }
            }
            foreach (int[] added in java.lang.JIter.of(stroke.added))
            {
                if (kinds[added[2] * size + added[1]] == null)
                {
                    place(com.oddlabs.tt.mapeditor.Resource.values()[added[0]], added[1], added[2]);
                    changed(added[1], added[2]);
                }
            }
            finish();
        }
        // Java: 250
        public bool prune(com.oddlabs.tt.mapeditor.ResourceLayer.Stroke stroke)
        {
            int removed = stroke.removed.size();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    com.oddlabs.tt.mapeditor.Resource found = kinds[y * size + x];
                    if (found != null && access.leftRegion(x, y))
                    {
                        remove(x, y);
                        stroke.removed.add(new int[] { found.ordinal(), x, y });
                        changed(x, y);
                    }
                }
            }
            finish();
            return stroke.removed.size() > removed;
        }
        // Java: 266
        public int[] diskCells(int r)
        {
            if (r != disk_radius)
            {
                java.util.List offsets = new java.util.ArrayList();
                for (int dy = -r; dy <= r; dy++)
                {
                    for (int dx = -r; dx <= r; dx++)
                    {
                        if (dx * dx + dy * dy <= r * r)
                        {
                            offsets.add(new int[] { dx, dy });
                        }
                    }
                }
                offsets.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object o_l) =>
                {
                    int[] o = (int[])o_l;
                    return o[0] * o[0] + o[1] * o[1];
                })));
                disk = new int[offsets.size() * 2];
                for (int i = 0; i < offsets.size(); i++)
                {
                    disk[2 * i] = ((int[])offsets.get(i))[0];
                    disk[2 * i + 1] = ((int[])offsets.get(i))[1];
                }
                disk_radius = r;
            }
            return disk;
        }
        // Java: 284
        public bool inside(int x, int y)
        {
            return x > 0 && y > 0 && x < size - 1 && y < size - 1;
        }
        // Java: 289
        public bool canPlace(int x, int y)
        {
            if (!inside(x, y) || kinds[y * size + x] != null || isOccupied(x, y))
            {
                return false;
            }
            if (((object)access.get(x, y) != (object)com.oddlabs.tt.mapeditor.AccessMap.Kind.REGION))
            {
                return false;
            }
            for (int dy = (-2); dy <= 2; dy++)
            {
                for (int dx = (-2); dx <= 2; dx++)
                {
                    if (java.lang.Math.abs(dx) == 2 && java.lang.Math.abs(dy) == 2)
                    {
                        continue;
                    }
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && ny >= 0 && nx < size && ny < size && kinds[ny * size + nx] != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // Java: 310
        public bool anyWithin(com.oddlabs.tt.mapeditor.Resource kind, int cx, int cy, int r)
        {
            return kindWithin(kind, cx, cy, (float)(r + 0.5f), r);
        }
        // Java: 314
        public bool kindWithin(com.oddlabs.tt.mapeditor.Resource kind, int cx, int cy, float distance, int reach)
        {
            float limit = (float)(distance * distance);
            for (int dy = -reach; dy <= reach; dy++)
            {
                int y = cy + dy;
                if (y < 0 || y >= size)
                {
                    continue;
                }
                for (int dx = -reach; dx <= reach; dx++)
                {
                    int x = cx + dx;
                    if (x >= 0 && x < size && dx * dx + dy * dy < limit && ((object)kinds[y * size + x] == (object)kind))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 330
        public com.oddlabs.tt.pathfinder.Region openedRegion(int x, int y)
        {
            com.oddlabs.tt.pathfinder.Region region = opened_region;
            if (region == null)
            {
                region = new com.oddlabs.tt.pathfinder.Region();
                region.setPosition(x, y);
                opened_region = region;
            }
            return region;
        }
        // Java: 344
        public bool isOccupied(int x, int y)
        {
            com.oddlabs.tt.pathfinder.Occupant occupant = world.getUnitGrid().getOccupant(x, y);
            return occupant != null && !((occupant is com.oddlabs.tt.pathfinder.StaticOccupant));
        }
        // Java: 349
        public void place(com.oddlabs.tt.mapeditor.Resource kind, int x, int y)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = world.getUnitGrid();
            com.oddlabs.tt.pathfinder.Occupant mark = grid.getOccupant(x, y);
            if ((mark is com.oddlabs.tt.pathfinder.StaticOccupant))
            {
                grid.freeGrid(x, y, mark);
            }
            if (grid.getRegion(x, y) == null)
            {
                grid.setRegion(x, y, openedRegion(x, y));
            }
            object @object = default;
            if (kind.isTree())
            {
                @object = world.getTreeRoot().plantTree(world, kind.treeType(terrain), x, y);
                trees_changed = true;
            }
            else
            {
                com.oddlabs.tt.render.SpriteKey[] sprites = (((object)kind == (object)com.oddlabs.tt.mapeditor.Resource.ROCK) ? world.getLandscapeResources().getRockFragments() : world.getLandscapeResources().getIronFragments());
                com.oddlabs.tt.render.SpriteKey sprite = sprites[world.getRandom().nextInt(sprites.Length)];
                float px = (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(x) + ((float)(world.getRandom().nextFloat() - 0.5f)));
                float py = (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(y) + ((float)(world.getRandom().nextFloat() - 0.5f)));
                float rotation = (float)(world.getRandom().nextFloat() * 360.0f);
                @object = (((object)kind == (object)com.oddlabs.tt.mapeditor.Resource.ROCK) ? (object)(new com.oddlabs.tt.model.RockSupply(world, sprite, 2.0f, x, y, px, py, rotation, true)) : (object)(new com.oddlabs.tt.model.IronSupply(world, sprite, 2.0f, x, y, px, py, rotation, true)));
            }
            put(x, y, kind, @object);
        }
        // Java: 376
        public void remove(int x, int y)
        {
            object @object = live[y * size + x];
            if ((@object is com.oddlabs.tt.landscape.TreeSupply tree))
            {
                world.getTreeRoot().removeTree(tree);
                trees_changed = true;
            }
            else if ((@object is com.oddlabs.tt.model.SupplyModel supply))
            {
                supply.removeFromWorld();
            }
            put(x, y, null, null);
        }
        public int changed_x0;
        public int changed_y0;
        public int changed_x1;
        public int changed_y1;
        // Java: 393
        public void changed(int x, int y)
        {
            changed_x0 = java.lang.Math.min(changed_x0, x);
            changed_y0 = java.lang.Math.min(changed_y0, y);
            changed_x1 = java.lang.Math.max(changed_x1, x);
            changed_y1 = java.lang.Math.max(changed_y1, y);
        }
        // Java: 400
        public void finish()
        {
            if (trees_changed)
            {
                world.getTreeRoot().refreshBounds();
                trees_changed = false;
            }
            if (changed_x0 <= changed_x1)
            {
                listener.resourcesChanged(this, changed_x0, changed_y0, changed_x1, changed_y1);
            }
            changed_x0 = java.lang.Integer.MAX_VALUE;
            changed_y0 = java.lang.Integer.MAX_VALUE;
            changed_x1 = java.lang.Integer.MIN_VALUE;
            changed_y1 = java.lang.Integer.MIN_VALUE;
        }
        // Java: 414
        public com.oddlabs.tt.mapeditor.Resource get(int x, int y)
        {
            return kinds[y * size + x];
        }
        private void init_ResourceLayer()
        {
            this.disk = new int[0];
            this.disk_radius = (-1);
            this.changed_x0 = java.lang.Integer.MAX_VALUE;
            this.changed_y0 = java.lang.Integer.MAX_VALUE;
            this.changed_x1 = java.lang.Integer.MIN_VALUE;
            this.changed_y1 = java.lang.Integer.MIN_VALUE;
        }
        public ResourceLayer(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.mapeditor.AccessMap access, com.oddlabs.tt.mapeditor.MapSettings settings, com.oddlabs.tt.mapeditor.ResourceLayer.ChangeListener listener) : base()
        {
            init_ResourceLayer();
            this.world = world;
            this.access = access;
            this.size = access.getSize();
            this.terrain = com.oddlabs.tt.procedural.Landscape.TerrainType.values()[settings.terrain()];
            this.listener = listener;
            this.kinds = new com.oddlabs.tt.mapeditor.Resource[size * size];
            this.live = new object[size * size];
            indexTrees(world.getTreeRoot());
            indexSupplies();
        }
        // Java: anonymous class at line 93
        public sealed class Anon1_TreeNodeVisitor : java.lang.Object, com.oddlabs.tt.landscape.TreeNodeVisitor
        {
            public com.oddlabs.tt.mapeditor.ResourceLayer this_0;
            public Anon1_TreeNodeVisitor(com.oddlabs.tt.mapeditor.ResourceLayer this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 94
            public void visitLeaf(com.oddlabs.tt.landscape.TreeLeaf leaf)
            {
                leaf.visitTrees(this);
            }
            // Java: 99
            public void visitNode(com.oddlabs.tt.landscape.TreeGroup node)
            {
                node.visitChildren(this);
            }
            // Java: 104
            public void visitTree(com.oddlabs.tt.landscape.TreeSupply tree)
            {
                if (!tree.isHidden())
                {
                    this_0.put(tree.getGridX(), tree.getGridY(), com.oddlabs.tt.mapeditor.Resource.ofTree(tree.getTreeType()), tree);
                }
            }
        }
        // Java: anonymous class at line 115
        public sealed class Anon2_ElementNodeVisitor : java.lang.Object, com.oddlabs.tt.model.ElementNodeVisitor
        {
            public com.oddlabs.tt.mapeditor.ResourceLayer this_0;
            public Anon2_ElementNodeVisitor(com.oddlabs.tt.mapeditor.ResourceLayer this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 116
            public void visitNode(com.oddlabs.tt.model.ElementNode node)
            {
                node.visitElements(this);
                node.visitChildren(this);
            }
            // Java: 122
            public void visitLeaf(com.oddlabs.tt.model.ElementLeaf leaf)
            {
                leaf.visitElements(this);
            }
            // Java: 127
            public void visit(com.oddlabs.tt.model.Element model_p)
            {
                com.oddlabs.tt.model.Model model = (com.oddlabs.tt.model.Model)model_p;
                if ((model is com.oddlabs.tt.model.RockSupply rock) && !rock.isEmpty())
                {
                    this_0.put(rock.getGridX(), rock.getGridY(), com.oddlabs.tt.mapeditor.Resource.ROCK, rock);
                }
                else if ((model is com.oddlabs.tt.model.IronSupply iron) && !iron.isEmpty())
                {
                    this_0.put(iron.getGridX(), iron.getGridY(), com.oddlabs.tt.mapeditor.Resource.IRON, iron);
                }
            }
        }
    }
}
