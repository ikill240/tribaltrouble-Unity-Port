// Translated from RacesResources.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 54
    public class RacesResources : java.lang.Object
    {
        public const int QUARTERS_SIZE = 5;
        public const int ARMORY_SIZE = 5;
        public const int TOWER_SIZE = 3;
        public const int SHIP_SIZE = 12;
        public static int MAX_BUILDING_SIZE;
        public const int QUARTERS_HIT_POINTS = 200;
        public const int ARMORY_HIT_POINTS = 200;
        public const int TOWER_HIT_POINTS = 100;
        public const int SHIP_HIT_POINTS = 250;
        public const int VIKING_CHIEFTAIN_HIT_POINTS = 60;
        public const int NATIVE_CHIEFTAIN_HIT_POINTS = 40;
        public const int RACE_NATIVES = 0;
        public const int RACE_VIKINGS = 1;
        public const int NUM_MAGIC = 2;
        public const int INDEX_MAGIC_POISON = 0;
        public const int INDEX_MAGIC_LIGHTNING = 1;
        public const int INDEX_MAGIC_STUN = 0;
        public const int INDEX_MAGIC_BLAST = 1;
        public const float THROW_RANGE = 6.0f;
        public static com.oddlabs.tt.procedural.GeneratorHalos DEFAULT_SHADOW_DESC;
        public static java.util.ResourceBundle bundle;
        // Java: 83
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.model.RacesResources.bundle, key, args);
        }
        public static string[] race_names;
        public const int MAX_UNIT_RESOURCES = 1;
        public com.oddlabs.tt.render.TextureKey[] smoke_textures;
        public com.oddlabs.tt.render.TextureKey[] damage_smoke_textures;
        public com.oddlabs.tt.render.TextureKey[] poison_textures;
        public com.oddlabs.tt.render.TextureKey lightning_texture;
        public com.oddlabs.tt.render.TextureKey[] note_textures;
        public com.oddlabs.tt.render.TextureKey[] star_textures;
        public com.oddlabs.tt.audio.Audio[] tree_fall_sound;
        public com.oddlabs.tt.audio.Audio[] building_hit_sound;
        public com.oddlabs.tt.audio.Audio gas_sound;
        public com.oddlabs.tt.audio.Audio bubbling_sound;
        public com.oddlabs.tt.audio.Audio lightning_sound;
        public com.oddlabs.tt.audio.Audio cloud_sound;
        public com.oddlabs.tt.audio.Audio[] stun_sound;
        public com.oddlabs.tt.audio.Audio[] blast_lur_sound;
        public com.oddlabs.tt.audio.Audio blast_rumble_sound;
        public com.oddlabs.tt.audio.Audio blast_blast_sound;
        public com.oddlabs.tt.audio.Audio armory_sound;
        public com.oddlabs.tt.audio.Audio building_collapse_sound;
        public java.util.Map harvest_sounds;
        public com.oddlabs.tt.render.SpriteKey[] wood_fragment_sprites;
        public com.oddlabs.tt.render.SpriteKey[] treasure_sprites;
        public com.oddlabs.tt.model.Race[] races;
        public java.util.Map skins;
        // Java: 116
        public static bool isValidRace(int race)
        {
            return race == com.oddlabs.tt.model.RacesResources.RACE_NATIVES || race == com.oddlabs.tt.model.RacesResources.RACE_VIKINGS;
        }
        // Java: 120
        public static com.oddlabs.tt.model.BuildingTemplate createBuildingTemplate(com.oddlabs.tt.render.RenderQueues queues, int template_id, int type, string built_name, float built_selection_radius, float built_selection_height, string halfbuilt_name, float halfbuilt_selection_radius, float halfbuilt_selection_height, string start_name, float start_selection_radius, float start_selection_height, float shadow_diameter, float ring_thickness, int placing_size, float smoke_radius, float smoke_height, int num_fragments, int max_hit_points, com.oddlabs.tt.model.UnitContainerFactory unit_container_factory, com.oddlabs.tt.model.Abilities abilities, float[] hit_offset_z, float mount_offset, float no_detail_size, float rally_x, float rally_y, float rally_z, float chimney_x, float chimney_y, float chimney_z, bool is_vikings, string name)
        {
            float ring_mid = 0.38f;
            float fadeout = 0.005f;
            java.util.function.Supplier building_shadow_desc = new com.oddlabs.tt.procedural.GeneratorHalos(256, new float[][] { new float[] { 0.15f, 0.5f }, new float[] { 0.5f, 0.0f } }, new float[][] { new float[] { (float)((float)(ring_mid - (float)(ring_thickness / 2)) - fadeout), 0.0f }, new float[] { (float)(ring_mid - (float)(ring_thickness / 2)), 1.0f }, new float[] { (float)(ring_mid + (float)(ring_thickness / 2)), 1.0f }, new float[] { (float)((float)(ring_mid + (float)(ring_thickness / 2)) + fadeout), 0.0f } });
            com.oddlabs.tt.render.ShadowListKey shadow_renderer = queues.registerSelectableShadowList(building_shadow_desc);
            com.oddlabs.tt.resource.SpriteFile building = new com.oddlabs.tt.resource.SpriteFile(built_name, com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, false, true, false);
            com.oddlabs.tt.resource.SpriteFile building_halfbuilt = new com.oddlabs.tt.resource.SpriteFile(halfbuilt_name, com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, false, true, false);
            com.oddlabs.tt.resource.SpriteFile building_start = new com.oddlabs.tt.resource.SpriteFile(start_name, com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, false, true, false);
            com.oddlabs.tt.render.SpriteKey built_key = queues.register(building);
            com.oddlabs.tt.render.SpriteKey halfbuilt_key = queues.register(building_halfbuilt);
            com.oddlabs.tt.render.SpriteKey start_key = queues.register(building_start);
            java.util.Map props = new java.util.EnumMap(typeof(com.oddlabs.tt.model.Building.BuildState));
            props.put(com.oddlabs.tt.model.Building.BuildState.BUILT, queues.getProps(built_key));
            props.put(com.oddlabs.tt.model.Building.BuildState.HALFBUILT, queues.getProps(halfbuilt_key));
            props.put(com.oddlabs.tt.model.Building.BuildState.START, queues.getProps(start_key));
            return new com.oddlabs.tt.model.BuildingTemplate(template_id, type, placing_size, smoke_radius, smoke_height, num_fragments, shadow_diameter, shadow_renderer, built_key, built_selection_radius, built_selection_height, halfbuilt_key, halfbuilt_selection_radius, halfbuilt_selection_height, start_key, start_selection_radius, start_selection_height, max_hit_points, unit_container_factory, abilities, hit_offset_z, mount_offset, no_detail_size, 0.0f, rally_x, rally_y, rally_z, chimney_x, chimney_y, chimney_z, is_vikings, name, props);
        }
        // Java: 883
        public com.oddlabs.tt.render.TextureKey[] getSmokeTextures()
        {
            return smoke_textures;
        }
        // Java: 887
        public com.oddlabs.tt.render.TextureKey[] getDamageSmokeTextures()
        {
            return damage_smoke_textures;
        }
        // Java: 891
        public com.oddlabs.tt.render.TextureKey[] getPoisonTextures()
        {
            return poison_textures;
        }
        // Java: 895
        public com.oddlabs.tt.render.TextureKey getLightningTexture()
        {
            return lightning_texture;
        }
        // Java: 899
        public com.oddlabs.tt.render.TextureKey[] getNoteTextures()
        {
            return note_textures;
        }
        // Java: 903
        public com.oddlabs.tt.render.TextureKey[] getStarTextures()
        {
            return star_textures;
        }
        // Java: 907
        public com.oddlabs.tt.audio.Audio getHarvestSound(System.Type key, java.util.Random random)
        {
            com.oddlabs.tt.audio.Audio[] sounds = ((com.oddlabs.tt.audio.Audio[])harvest_sounds.get(key));
            return sounds[random.nextInt(sounds.Length)];
        }
        // Java: 912
        public com.oddlabs.tt.audio.Audio[] getTreeFallSound()
        {
            return tree_fall_sound;
        }
        // Java: 916
        public com.oddlabs.tt.audio.Audio getBuildingHitSound(java.util.Random random)
        {
            return building_hit_sound[random.nextInt(building_hit_sound.Length)];
        }
        // Java: 920
        public com.oddlabs.tt.audio.Audio getGasSound()
        {
            return gas_sound;
        }
        // Java: 924
        public com.oddlabs.tt.audio.Audio getBubblingSound()
        {
            return bubbling_sound;
        }
        // Java: 928
        public com.oddlabs.tt.audio.Audio getLightningSound()
        {
            return lightning_sound;
        }
        // Java: 932
        public com.oddlabs.tt.audio.Audio getCloudSound()
        {
            return cloud_sound;
        }
        // Java: 936
        public com.oddlabs.tt.audio.Audio getStunSound(java.util.Random random)
        {
            return stun_sound[random.nextInt(stun_sound.Length)];
        }
        // Java: 940
        public com.oddlabs.tt.audio.Audio getBlastLurSound(java.util.Random random)
        {
            return blast_lur_sound[random.nextInt(blast_lur_sound.Length)];
        }
        // Java: 944
        public com.oddlabs.tt.audio.Audio getBlastRumbleSound()
        {
            return blast_rumble_sound;
        }
        // Java: 948
        public com.oddlabs.tt.audio.Audio getBlastBlastSound()
        {
            return blast_blast_sound;
        }
        // Java: 952
        public com.oddlabs.tt.audio.Audio getArmorySound()
        {
            return armory_sound;
        }
        // Java: 956
        public com.oddlabs.tt.audio.Audio getBuildingCollapseSound()
        {
            return building_collapse_sound;
        }
        // Java: 960
        public com.oddlabs.tt.model.Race getRace(int i)
        {
            return races[i];
        }
        // Java: 964
        public com.oddlabs.tt.player.PlayerSkins getSkins(string name)
        {
            return ((com.oddlabs.tt.player.PlayerSkins)skins.getOrDefault(name, com.oddlabs.tt.player.PlayerSkins.NONE));
        }
        // Java: 968
        public static string getRaceName(int i)
        {
            return com.oddlabs.tt.model.RacesResources.race_names[i];
        }
        // Java: 972
        public static int getNumRaces()
        {
            return com.oddlabs.tt.model.RacesResources.race_names.Length;
        }
        // Java: 976
        public com.oddlabs.tt.render.SpriteKey[] getWoodFragments()
        {
            return wood_fragment_sprites;
        }
        // Java: 980
        public com.oddlabs.tt.render.SpriteKey[] getTreasures()
        {
            return treasure_sprites;
        }
        static RacesResources()
        {
            RacesResources.MAX_BUILDING_SIZE = java.util.stream.IntStream_.of(com.oddlabs.tt.model.RacesResources.QUARTERS_SIZE, com.oddlabs.tt.model.RacesResources.ARMORY_SIZE, com.oddlabs.tt.model.RacesResources.TOWER_SIZE).max().orElseThrow();
            RacesResources.DEFAULT_SHADOW_DESC = new com.oddlabs.tt.procedural.GeneratorHalos(128, new float[][] { new float[] { 0.0f, 0.75f }, new float[] { 0.5f, 0.0f } }, new float[][] { new float[] { 0.4f, 0.0f }, new float[] { 0.41f, 1.0f }, new float[] { 0.48f, 1.0f }, new float[] { 0.49f, 0.0f } });
            RacesResources.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.model.RacesResources).getName());
            RacesResources.race_names = new string[] { com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.model.RacesResources.bundle, "natives"), com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.model.RacesResources.bundle, "vikings") };
        }
        private void init_RacesResources()
        {
            this.smoke_textures = new com.oddlabs.tt.render.TextureKey[1];
            this.damage_smoke_textures = new com.oddlabs.tt.render.TextureKey[1];
            this.poison_textures = new com.oddlabs.tt.render.TextureKey[1];
            this.note_textures = new com.oddlabs.tt.render.TextureKey[8];
            this.star_textures = new com.oddlabs.tt.render.TextureKey[1];
            this.harvest_sounds = new java.util.HashMap();
            this.wood_fragment_sprites = new com.oddlabs.tt.render.SpriteKey[4];
            this.treasure_sprites = new com.oddlabs.tt.render.SpriteKey[6];
        }
        public RacesResources(com.oddlabs.tt.render.RenderQueues queues) : base()
        {
            init_RacesResources();
            int num_progress = 25;
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            java.util.Map native_supply_sprite_lists = com.oddlabs.tt.model.RegistryItems.carried(queues, "natives");
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            java.util.Map viking_supply_sprite_lists = com.oddlabs.tt.model.RegistryItems.carried(queues, "vikings");
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            smoke_textures[0] = queues.registerTexture(new com.oddlabs.tt.procedural.GeneratorSmoke(), 0);
            damage_smoke_textures[0] = queues.registerTexture(new com.oddlabs.tt.procedural.GeneratorDamageSmoke(), 0);
            poison_textures[0] = queues.registerTexture(new com.oddlabs.tt.procedural.GeneratorPoison(), 0);
            lightning_texture = queues.registerTexture(new com.oddlabs.tt.procedural.GeneratorLightning(), 0);
            for (int i = 0; i < note_textures.Length; i++)
            {
                note_textures[i] = queues.registerTexture(new com.oddlabs.tt.resource.TextureFile("/textures/effects/note" + ((i + 1)), com.oddlabs.tt.global.Globals.COMPRESSED_RGBA_FORMAT, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE));
            }
            star_textures[0] = queues.registerTexture(new com.oddlabs.tt.resource.TextureFile("/textures/effects/star", com.oddlabs.tt.global.Globals.COMPRESSED_RGBA_FORMAT, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE));
            com.oddlabs.tt.audio.Audio death_peon_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/death_peon.ogg")));
            com.oddlabs.tt.audio.Audio death_viking1_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/death_viking_warrior1.ogg")));
            com.oddlabs.tt.audio.Audio death_viking2_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/death_viking_warrior2.ogg")));
            com.oddlabs.tt.audio.Audio death_native1_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/death_native_warrior1.ogg")));
            com.oddlabs.tt.audio.Audio death_native2_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/death_native_warrior2.ogg")));
            com.oddlabs.tt.audio.Audio axe_throw_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/weapon_axe.ogg")));
            com.oddlabs.tt.audio.Audio spear_throw_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/weapon_spear.ogg")));
            tree_fall_sound = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/felling_tree.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/felling_palmtree.ogg"))) };
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            building_hit_sound = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_wood1.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_wood2.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_wood3.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_wood4.ogg"))) };
            gas_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/gas.ogg")));
            bubbling_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/bubbling.ogg")));
            lightning_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/flash.ogg")));
            cloud_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/crackling_cloud.ogg")));
            armory_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/armory.ogg")));
            building_collapse_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/building_crash.ogg")));
            stun_sound = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/lur_stun1.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/lur_stun2.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/lur_stun3.ogg"))) };
            blast_lur_sound = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/lur_blast1.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/lur_blast2.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/lur_blast3.ogg"))) };
            blast_rumble_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/rumble.ogg")));
            blast_blast_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/lurblast.ogg")));
            com.oddlabs.tt.audio.Audio[] tree_cut_sound = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_wood1.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_wood2.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_wood3.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_wood4.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_wood5.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_wood6.ogg"))) };
            com.oddlabs.tt.audio.Audio[] rock_cut_sound = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_stone1.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_stone2.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_stone3.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_stone4.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/axe_cutting_stone5.ogg"))) };
            com.oddlabs.tt.audio.Audio[] meat_cut_sound = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat1.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat2.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat3.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat4.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat5.ogg"))) };
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            harvest_sounds.put(typeof(com.oddlabs.tt.landscape.TreeSupply), tree_cut_sound);
            harvest_sounds.put(typeof(com.oddlabs.tt.model.RockSupply), rock_cut_sound);
            harvest_sounds.put(typeof(com.oddlabs.tt.model.IronSupply), rock_cut_sound);
            harvest_sounds.put(typeof(com.oddlabs.tt.model.RubberSupply), meat_cut_sound);
            com.oddlabs.tt.model.BuildingTemplate viking_quarters_template = com.oddlabs.tt.model.RacesResources.createBuildingTemplate(queues, com.oddlabs.tt.model.Race.BUILDING_QUARTERS, com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING, "/geometry/vikings/quarters.binsprite", 3.5f, 7.0f, "/geometry/vikings/quarters_halfbuilt.binsprite", 3.5f, 6.0f, "/geometry/vikings/quarters_start.binsprite", 5.0f, 1.0f, 22.0f, 0.001f, com.oddlabs.tt.model.RacesResources.QUARTERS_SIZE, 6.0f, 9.0f, 30, com.oddlabs.tt.model.RacesResources.QUARTERS_HIT_POINTS, new com.oddlabs.tt.model.ReproduceUnitContainerFactory(), new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.REPRODUCE | com.oddlabs.tt.model.Abilities.RALLY_TO | com.oddlabs.tt.model.Abilities.TARGET), new float[] { 0.0f, 1.0f, 3.0f }, 0.0f, 6.0f, 3.65f, 0.25f, 8.0f, 0.0f, 0.0f, 0.0f, true, i18n("quarters"));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.model.BuildingTemplate viking_armory_template = com.oddlabs.tt.model.RacesResources.createBuildingTemplate(queues, com.oddlabs.tt.model.Race.BUILDING_ARMORY, com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING, "/geometry/vikings/armory.binsprite", 3.5f, 7.0f, "/geometry/vikings/armory_halfbuilt.binsprite", 3.5f, 6.0f, "/geometry/vikings/armory_start.binsprite", 5.0f, 1.0f, 22.0f, 0.001f, com.oddlabs.tt.model.RacesResources.ARMORY_SIZE, 6.0f, 9.0f, 30, com.oddlabs.tt.model.RacesResources.ARMORY_HIT_POINTS, new com.oddlabs.tt.model.WorkerUnitContainerFactory(), new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER | com.oddlabs.tt.model.Abilities.BUILD_ARMIES | com.oddlabs.tt.model.Abilities.RALLY_TO | com.oddlabs.tt.model.Abilities.TARGET), new float[] { 0.0f, 1.0f, 3.0f }, 0.0f, 6.0f, 0.0f, 2.25f, 10.0f, 0.25f, (float)(-2.8f), 13.1f, true, i18n("armory"));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.model.BuildingTemplate viking_tower_template = com.oddlabs.tt.model.RacesResources.createBuildingTemplate(queues, com.oddlabs.tt.model.Race.BUILDING_TOWER, com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING, "/geometry/vikings/tower.binsprite", 1.25f, 11.0f, "/geometry/vikings/tower_halfbuilt.binsprite", 2.0f, 7.0f, "/geometry/vikings/tower_start.binsprite", 2.5f, 1.0f, 10.0f, 0.009f, com.oddlabs.tt.model.RacesResources.TOWER_SIZE, 3.0f, 12.0f, 20, com.oddlabs.tt.model.RacesResources.TOWER_HIT_POINTS, new com.oddlabs.tt.model.MountUnitContainerFactory(), new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.RALLY_TO | com.oddlabs.tt.model.Abilities.TARGET), new float[] { 0.0f, 2.0f, 7.5f }, 9.55f, 2.5f, 0.85f, 0.85f, 9.5f, 0.0f, 0.0f, 0.0f, true, i18n("tower"));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.model.BuildingTemplate native_quarters_template = com.oddlabs.tt.model.RacesResources.createBuildingTemplate(queues, com.oddlabs.tt.model.Race.BUILDING_QUARTERS, com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING, "/geometry/natives/quarters.binsprite", 4.0f, 8.0f, "/geometry/natives/quarters_halfbuilt.binsprite", 4.0f, 6.0f, "/geometry/natives/quarters_start.binsprite", 5.0f, 1.0f, 16.0f, 0.004f, com.oddlabs.tt.model.RacesResources.QUARTERS_SIZE, 6.0f, 9.0f, 30, com.oddlabs.tt.model.RacesResources.QUARTERS_HIT_POINTS, new com.oddlabs.tt.model.ReproduceUnitContainerFactory(), new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.REPRODUCE | com.oddlabs.tt.model.Abilities.RALLY_TO | com.oddlabs.tt.model.Abilities.TARGET), new float[] { 0.0f, 1.0f, 3.0f }, 0.0f, 6.0f, (float)(-1.15f), (float)(-0.77f), 11.0f, 0.0f, 0.0f, 0.0f, false, i18n("quarters"));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.model.BuildingTemplate native_armory_template = com.oddlabs.tt.model.RacesResources.createBuildingTemplate(queues, com.oddlabs.tt.model.Race.BUILDING_ARMORY, com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING, "/geometry/natives/armory.binsprite", 4.0f, 8.0f, "/geometry/natives/armory_halfbuilt.binsprite", 4.0f, 6.0f, "/geometry/natives/armory_start.binsprite", 5.0f, 1.0f, 16.0f, 0.004f, com.oddlabs.tt.model.RacesResources.ARMORY_SIZE, 6.0f, 9.0f, 30, com.oddlabs.tt.model.RacesResources.ARMORY_HIT_POINTS, new com.oddlabs.tt.model.WorkerUnitContainerFactory(), new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER | com.oddlabs.tt.model.Abilities.BUILD_ARMIES | com.oddlabs.tt.model.Abilities.RALLY_TO | com.oddlabs.tt.model.Abilities.TARGET), new float[] { 0.0f, 1.0f, 3.0f }, 0.0f, 6.0f, 0.0f, (float)(-0.4f), 12.0f, 0.0f, (float)(-1.0f), 11.5f, false, i18n("armory"));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.model.BuildingTemplate native_tower_template = com.oddlabs.tt.model.RacesResources.createBuildingTemplate(queues, com.oddlabs.tt.model.Race.BUILDING_TOWER, com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING, "/geometry/natives/tower.binsprite", 1.0f, 14.0f, "/geometry/natives/tower_halfbuilt.binsprite", 1.0f, 14.0f, "/geometry/natives/tower_start.binsprite", 1.5f, 2.0f, 5.0f, 0.025f, com.oddlabs.tt.model.RacesResources.TOWER_SIZE, 3.0f, 12.0f, 20, com.oddlabs.tt.model.RacesResources.TOWER_HIT_POINTS, new com.oddlabs.tt.model.MountUnitContainerFactory(), new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.RALLY_TO | com.oddlabs.tt.model.Abilities.TARGET), new float[] { 0.0f, 11.5f, 11.5f }, 13.0f, 2.5f, 0.95f, 0.0f, 13.0f, 0.0f, 0.0f, 0.0f, false, i18n("tower"));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.model.BuildingTemplate native_ship_template = com.oddlabs.tt.model.RacesResources.createBuildingTemplate(queues, com.oddlabs.tt.model.Race.BUILDING_SHIP, com.oddlabs.tt.model.BuildingTemplate.TYPE_SHIP, "/geometry/natives/ship.binsprite", 3.5f, 7.0f, "/geometry/natives/ship_halfbuilt.binsprite", 3.5f, 6.0f, "/geometry/natives/ship_start.binsprite", 5.0f, 1.0f, 22.0f, 0.001f, com.oddlabs.tt.model.RacesResources.SHIP_SIZE, 6.0f, 9.0f, 100, com.oddlabs.tt.model.RacesResources.SHIP_HIT_POINTS, null, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER | com.oddlabs.tt.model.Abilities.SAIL | com.oddlabs.tt.model.Abilities.RALLY_TO | com.oddlabs.tt.model.Abilities.TARGET), new float[] { 0.0f, 1.0f, 3.0f }, 1.9f, 6.0f, (float)(-0.5f), 0.0f, 3.1f, 1.0f, 0.0f, 5.0f, false, com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.model.RacesResources.bundle, "ship"));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.model.BuildingTemplate viking_ship_template = com.oddlabs.tt.model.RacesResources.createBuildingTemplate(queues, com.oddlabs.tt.model.Race.BUILDING_SHIP, com.oddlabs.tt.model.BuildingTemplate.TYPE_SHIP, "/geometry/vikings/ship.binsprite", 3.5f, 7.0f, "/geometry/vikings/ship_halfbuilt.binsprite", 3.5f, 6.0f, "/geometry/vikings/ship_start.binsprite", 5.0f, 1.0f, 22.0f, 0.001f, com.oddlabs.tt.model.RacesResources.SHIP_SIZE, 6.0f, 9.0f, 100, com.oddlabs.tt.model.RacesResources.SHIP_HIT_POINTS, null, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER | com.oddlabs.tt.model.Abilities.SAIL | com.oddlabs.tt.model.Abilities.RALLY_TO | com.oddlabs.tt.model.Abilities.TARGET), new float[] { 0.0f, 1.0f, 3.0f }, 1.9f, 6.0f, (float)(-0.5f), 0.0f, 3.1f, 1.0f, 0.0f, 5.0f, true, com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.model.RacesResources.bundle, "ship"));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            float shadow_diameter_warrior = 1.9f;
            float shadow_diameter_peon = 1.6f;
            float shadow_diameter_chieftain = 2.2f;
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.resource.SpriteFile sprite_list_warrior = new com.oddlabs.tt.resource.SpriteFile("/geometry/vikings/warrior.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.resource.SpriteFile sprite_list_chieftain = new com.oddlabs.tt.resource.SpriteFile("/geometry/vikings/chieftain.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.resource.SpriteFile sprite_list_native_chieftain = new com.oddlabs.tt.resource.SpriteFile("/geometry/natives/chieftain.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            com.oddlabs.tt.resource.SpriteFile sprite_list_peon = new com.oddlabs.tt.resource.SpriteFile("/geometry/vikings/peon.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.resource.SpriteFile sprite_list_native_peon = new com.oddlabs.tt.resource.SpriteFile("/geometry/natives/peon.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.resource.SpriteFile sprite_list_native_warrior = new com.oddlabs.tt.resource.SpriteFile("/geometry/natives/warrior.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.resource.SpriteFile viking_warrior_axe = new com.oddlabs.tt.resource.SpriteFile("/geometry/vikings/axe.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.resource.SpriteFile native_warrior_spear = new com.oddlabs.tt.resource.SpriteFile("/geometry/natives/spear.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.audio.Audio[] unit_hit_sounds = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat1.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat2.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat3.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat4.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/impact_meat5.ogg"))) };
            com.oddlabs.tt.model.weapon.WeaponFactory viking_warrior_rock_weapon = new com.oddlabs.tt.model.weapon.ThrowingFactory(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), new com.oddlabs.tt.model.weapon.ThrowingFactory.WeaponConstructor_L((bool a0, com.oddlabs.tt.model.Unit a1, com.oddlabs.tt.model.Selectable a2, com.oddlabs.tt.render.SpriteKey a3, com.oddlabs.tt.audio.Audio a4, com.oddlabs.tt.audio.Audio[] a5) => { return new com.oddlabs.tt.model.weapon.RockAxeWeapon(a0, a1, a2, a3, a4, a5); }), 0.5f, com.oddlabs.tt.model.RacesResources.THROW_RANGE, (float)(29.0f / 58.0f), queues.register(viking_warrior_axe, com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK), axe_throw_sound, unit_hit_sounds);
            com.oddlabs.tt.model.weapon.WeaponFactory viking_warrior_iron_weapon = new com.oddlabs.tt.model.weapon.ThrowingFactory(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), new com.oddlabs.tt.model.weapon.ThrowingFactory.WeaponConstructor_L((bool a0, com.oddlabs.tt.model.Unit a1, com.oddlabs.tt.model.Selectable a2, com.oddlabs.tt.render.SpriteKey a3, com.oddlabs.tt.audio.Audio a4, com.oddlabs.tt.audio.Audio[] a5) => { return new com.oddlabs.tt.model.weapon.IronAxeWeapon(a0, a1, a2, a3, a4, a5); }), 0.75f, com.oddlabs.tt.model.RacesResources.THROW_RANGE, (float)(29.0f / 58.0f), queues.register(viking_warrior_axe, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON), axe_throw_sound, unit_hit_sounds);
            com.oddlabs.tt.model.weapon.WeaponFactory viking_warrior_rubber_weapon = new com.oddlabs.tt.model.weapon.ThrowingFactory(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), new com.oddlabs.tt.model.weapon.ThrowingFactory.WeaponConstructor_L((bool a0, com.oddlabs.tt.model.Unit a1, com.oddlabs.tt.model.Selectable a2, com.oddlabs.tt.render.SpriteKey a3, com.oddlabs.tt.audio.Audio a4, com.oddlabs.tt.audio.Audio[] a5) => { return new com.oddlabs.tt.model.weapon.RubberAxeWeapon(a0, a1, a2, a3, a4, a5); }), 0.95f, com.oddlabs.tt.model.RacesResources.THROW_RANGE, (float)(29.0f / 58.0f), queues.register(viking_warrior_axe, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER), axe_throw_sound, unit_hit_sounds);
            com.oddlabs.tt.model.weapon.WeaponFactory native_warrior_rock_weapon = new com.oddlabs.tt.model.weapon.ThrowingFactory(typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon), new com.oddlabs.tt.model.weapon.ThrowingFactory.WeaponConstructor_L((bool a0, com.oddlabs.tt.model.Unit a1, com.oddlabs.tt.model.Selectable a2, com.oddlabs.tt.render.SpriteKey a3, com.oddlabs.tt.audio.Audio a4, com.oddlabs.tt.audio.Audio[] a5) => { return new com.oddlabs.tt.model.weapon.RockSpearWeapon(a0, a1, a2, a3, a4, a5); }), 0.5f, com.oddlabs.tt.model.RacesResources.THROW_RANGE, (float)(46.0f / 100.0f), queues.register(native_warrior_spear, com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK), spear_throw_sound, unit_hit_sounds);
            com.oddlabs.tt.model.weapon.WeaponFactory native_warrior_iron_weapon = new com.oddlabs.tt.model.weapon.ThrowingFactory(typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon), new com.oddlabs.tt.model.weapon.ThrowingFactory.WeaponConstructor_L((bool a0, com.oddlabs.tt.model.Unit a1, com.oddlabs.tt.model.Selectable a2, com.oddlabs.tt.render.SpriteKey a3, com.oddlabs.tt.audio.Audio a4, com.oddlabs.tt.audio.Audio[] a5) => { return new com.oddlabs.tt.model.weapon.IronSpearWeapon(a0, a1, a2, a3, a4, a5); }), 0.75f, com.oddlabs.tt.model.RacesResources.THROW_RANGE, (float)(46.0f / 100.0f), queues.register(native_warrior_spear, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON), spear_throw_sound, unit_hit_sounds);
            com.oddlabs.tt.model.weapon.WeaponFactory native_warrior_rubber_weapon = new com.oddlabs.tt.model.weapon.ThrowingFactory(typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon), new com.oddlabs.tt.model.weapon.ThrowingFactory.WeaponConstructor_L((bool a0, com.oddlabs.tt.model.Unit a1, com.oddlabs.tt.model.Selectable a2, com.oddlabs.tt.render.SpriteKey a3, com.oddlabs.tt.audio.Audio a4, com.oddlabs.tt.audio.Audio[] a5) => { return new com.oddlabs.tt.model.weapon.RubberSpearWeapon(a0, a1, a2, a3, a4, a5); }), 0.95f, com.oddlabs.tt.model.RacesResources.THROW_RANGE, (float)(46.0f / 100.0f), queues.register(native_warrior_spear, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER), spear_throw_sound, unit_hit_sounds);
            com.oddlabs.tt.audio.Audio[] native_chieftain_hit_sounds = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/hit3.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/hit4.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/hit5.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/hit6.ogg"))) };
            com.oddlabs.tt.audio.Audio[] viking_chieftain_hit_sounds = new com.oddlabs.tt.audio.Audio[] { ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/hit1.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/hit2.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/hit6.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/hit7.ogg"))) };
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.render.ShadowListKey default_shadow_list = queues.registerSelectableShadowList(com.oddlabs.tt.model.RacesResources.DEFAULT_SHADOW_DESC);
            com.oddlabs.tt.model.UnitTemplate viking_warrior_rock_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.2f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET | com.oddlabs.tt.model.Abilities.THROW), 4.0f, viking_warrior_rock_weapon, queues.register(sprite_list_warrior, com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK), shadow_diameter_warrior, default_shadow_list, null, death_viking1_sound, 0.25f, new float[] { 1.2f }, 1.0f, 0.5f, i18n("rock_warrior"), 1, 0.0f, 0.0f, 2.0f, 3, com.oddlabs.tt.model.RegistryItems.slots(queues, "vikings", "warrior", com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK), com.oddlabs.tt.model.RegistryItems.defaultSlots("vikings", "warrior"));
            com.oddlabs.tt.model.UnitTemplate viking_warrior_iron_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.2f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET | com.oddlabs.tt.model.Abilities.THROW), 4.0f, viking_warrior_iron_weapon, queues.register(sprite_list_warrior, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON), shadow_diameter_warrior, default_shadow_list, null, death_viking2_sound, 0.25f, new float[] { 1.2f }, 1.0f, 0.7f, i18n("iron_warrior"), 1, 0.0f, 0.0f, 2.0f, 5, com.oddlabs.tt.model.RegistryItems.slots(queues, "vikings", "warrior", com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON), com.oddlabs.tt.model.RegistryItems.defaultSlots("vikings", "warrior"));
            com.oddlabs.tt.model.UnitTemplate viking_warrior_rubber_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.2f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET | com.oddlabs.tt.model.Abilities.THROW), 4.0f, viking_warrior_rubber_weapon, queues.register(sprite_list_warrior, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER), shadow_diameter_warrior, default_shadow_list, null, death_viking2_sound, 0.25f, new float[] { 1.2f }, 1.0f, 0.7f, i18n("chicken_warrior"), 1, 0.0f, 0.0f, 2.0f, 10, com.oddlabs.tt.model.RegistryItems.slots(queues, "vikings", "warrior", com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER), com.oddlabs.tt.model.RegistryItems.defaultSlots("vikings", "warrior"));
            com.oddlabs.tt.model.UnitTemplate native_warrior_rock_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.2f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET | com.oddlabs.tt.model.Abilities.THROW), 4.0f, native_warrior_rock_weapon, queues.register(sprite_list_native_warrior, com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK), shadow_diameter_warrior, default_shadow_list, null, death_native1_sound, 0.25f, new float[] { 1.2f }, 1.0f, 0.5f, i18n("rock_warrior"), 1, 0.0f, 0.0f, 2.0f, 3, com.oddlabs.tt.model.RegistryItems.slots(queues, "natives", "warrior", com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK), com.oddlabs.tt.model.RegistryItems.defaultSlots("natives", "warrior"));
            com.oddlabs.tt.model.UnitTemplate native_warrior_iron_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.2f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET | com.oddlabs.tt.model.Abilities.THROW), 4.0f, native_warrior_iron_weapon, queues.register(sprite_list_native_warrior, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON), shadow_diameter_warrior, default_shadow_list, null, death_native2_sound, 0.25f, new float[] { 1.2f }, 1.0f, 0.7f, i18n("iron_warrior"), 1, 0.0f, 0.0f, 2.0f, 5, com.oddlabs.tt.model.RegistryItems.slots(queues, "natives", "warrior", com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON), com.oddlabs.tt.model.RegistryItems.defaultSlots("natives", "warrior"));
            com.oddlabs.tt.model.UnitTemplate native_warrior_rubber_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.2f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET | com.oddlabs.tt.model.Abilities.THROW), 4.0f, native_warrior_rubber_weapon, queues.register(sprite_list_native_warrior, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER), shadow_diameter_warrior, default_shadow_list, null, death_native2_sound, 0.25f, new float[] { 1.2f }, 1.0f, 0.7f, i18n("chicken_warrior"), 1, 0.0f, 0.0f, 2.0f, 10, com.oddlabs.tt.model.RegistryItems.slots(queues, "natives", "warrior", com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER), com.oddlabs.tt.model.RegistryItems.defaultSlots("natives", "warrior"));
            com.oddlabs.tt.model.UnitTemplate viking_peon_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.1f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.BUILD | com.oddlabs.tt.model.Abilities.HARVEST | com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET), 5.0f, new com.oddlabs.tt.model.weapon.InstantHitFactory((float)(1 / 5.0f), 0.0f, (float)(11.0f / 38.0f), unit_hit_sounds), queues.register(sprite_list_peon), shadow_diameter_peon, default_shadow_list, new com.oddlabs.tt.model.UnitSupplyContainerFactory(com.oddlabs.tt.model.RacesResources.MAX_UNIT_RESOURCES, viking_supply_sprite_lists), death_peon_sound, 0.25f, new float[] { 0.7f }, 1.0f, 0.0f, i18n("peon"), 1, 0.1f, 0.0f, 1.75f, 1, com.oddlabs.tt.model.RegistryItems.slots(queues, "vikings", "peon", com.oddlabs.tt.model.RegistryItems.DEFAULT_TEXTURE), com.oddlabs.tt.model.RegistryItems.defaultSlots("vikings", "peon"));
            com.oddlabs.tt.model.UnitTemplate native_peon_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.1f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.BUILD | com.oddlabs.tt.model.Abilities.HARVEST | com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET), 5.0f, new com.oddlabs.tt.model.weapon.InstantHitFactory((float)(1 / 5.0f), 0.0f, (float)(51.0f / 83.0f), unit_hit_sounds), queues.register(sprite_list_native_peon), shadow_diameter_peon, default_shadow_list, new com.oddlabs.tt.model.UnitSupplyContainerFactory(com.oddlabs.tt.model.RacesResources.MAX_UNIT_RESOURCES, native_supply_sprite_lists), death_peon_sound, 0.25f, new float[] { 0.7f }, 1.0f, 0.0f, i18n("peon"), 1, 0.0f, 0.0f, 1.75f, 1, com.oddlabs.tt.model.RegistryItems.slots(queues, "natives", "peon", com.oddlabs.tt.model.RegistryItems.DEFAULT_TEXTURE), com.oddlabs.tt.model.RegistryItems.defaultSlots("natives", "peon"));
            com.oddlabs.tt.model.UnitTemplate viking_chieftain_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.4f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET | com.oddlabs.tt.model.Abilities.MAGIC), 4.0f, new com.oddlabs.tt.model.weapon.InstantHitFactory((float)(3 / 4.0f), 0.0f, (float)(75.0f / 119.0f), viking_chieftain_hit_sounds), queues.register(sprite_list_chieftain), shadow_diameter_chieftain, default_shadow_list, null, death_viking2_sound, 0.15f, new float[] { 1.7f }, 1.0f, 0.5f, i18n("chieftain"), com.oddlabs.tt.model.RacesResources.VIKING_CHIEFTAIN_HIT_POINTS, (float)(-0.07f), 0.312f, 2.7f, 40, com.oddlabs.tt.model.RegistryItems.slots(queues, "vikings", "chieftain", com.oddlabs.tt.model.RegistryItems.DEFAULT_TEXTURE), com.oddlabs.tt.model.RegistryItems.defaultSlots("vikings", "chieftain"));
            com.oddlabs.tt.model.UnitTemplate native_chieftain_template = new com.oddlabs.tt.model.UnitTemplate(0.4f, 1.4f, new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.ATTACK | com.oddlabs.tt.model.Abilities.TARGET | com.oddlabs.tt.model.Abilities.MAGIC), 4.0f, new com.oddlabs.tt.model.weapon.InstantHitFactory((float)(3 / 4.0f), 0.0f, (float)(75.0f / 129.0f), native_chieftain_hit_sounds), queues.register(sprite_list_native_chieftain), shadow_diameter_chieftain, default_shadow_list, null, death_native2_sound, 0.15f, new float[] { 1.7f }, 1.0f, 0.5f, i18n("chieftain"), com.oddlabs.tt.model.RacesResources.NATIVE_CHIEFTAIN_HIT_POINTS, 0.878f, 0.151f, 2.8f, 40, com.oddlabs.tt.model.RegistryItems.slots(queues, "natives", "chieftain", com.oddlabs.tt.model.RegistryItems.DEFAULT_TEXTURE), com.oddlabs.tt.model.RegistryItems.defaultSlots("natives", "chieftain"));
            com.oddlabs.tt.model.weapon.MagicFactory[] native_magic = new com.oddlabs.tt.model.weapon.MagicFactory[com.oddlabs.tt.model.RacesResources.NUM_MAGIC];
            native_magic[com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_POISON] = new com.oddlabs.tt.model.weapon.PoisonFogFactory(0.9f, 0.0f, 0.55f, 26.0f, 0.5f, 2.0f, 20.0f, 10, 5.0f, (float)(80.0f / 224.0f), (float)(163.0f / 224.0f));
            native_magic[com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_LIGHTNING] = new com.oddlabs.tt.model.weapon.LightningCloudFactory(0.9f, 0.0f, 0.55f, 22.0f, 1.0f, 8.0f, 1.0f, 30, 18.0f, 5.0f, (float)(80.0f / 224.0f), (float)(163.0f / 224.0f));
            com.oddlabs.tt.model.weapon.MagicFactory[] viking_magic = new com.oddlabs.tt.model.weapon.MagicFactory[com.oddlabs.tt.model.RacesResources.NUM_MAGIC];
            viking_magic[com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN] = new com.oddlabs.tt.model.weapon.StunFactory(2.57f, 0.0f, 3.8f, 36.0f, 30.0f, 10.0f, 6.0f, (float)(57.0f / 159.0f), (float)(100.0f / 159.0f));
            viking_magic[com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST] = new com.oddlabs.tt.model.weapon.SonicBlastFactory(2.57f, 0.0f, 3.8f, 36.0f, 17.0f, 2.0f, 150, 30, 0.8f, 6.0f, (float)(57.0f / 159.0f), (float)(100.0f / 159.0f));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.gui.GUIIcons icons = com.oddlabs.tt.gui.GUIIcons.getIcons();
            com.oddlabs.tt.model.Race natives_race = new com.oddlabs.tt.model.Race(native_quarters_template, native_armory_template, native_tower_template, native_ship_template, native_warrior_rock_template, native_warrior_iron_template, native_warrior_rubber_template, native_peon_template, native_chieftain_template, queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/natives/rally_point.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false)), icons.getNativeIcons(), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/attacknotify_native.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/buildingnotify_native.ogg"))), native_magic, new com.oddlabs.tt.player.NativeChieftainAI(), "/music/native.ogg");
            com.oddlabs.tt.model.Race vikings_race = new com.oddlabs.tt.model.Race(viking_quarters_template, viking_armory_template, viking_tower_template, viking_ship_template, viking_warrior_rock_template, viking_warrior_iron_template, viking_warrior_rubber_template, viking_peon_template, viking_chieftain_template, queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/vikings/rally_point.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false)), icons.getVikingIcons(), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/attacknotify_viking.ogg"))), ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/buildingnotify_viking.ogg"))), viking_magic, new com.oddlabs.tt.player.VikingChieftainAI(), "/music/viking.ogg");
            races = new com.oddlabs.tt.model.Race[] { natives_race, vikings_race };
            java.util.Set carried = new java.util.HashSet(native_supply_sprite_lists.values());
            carried.addAll(viking_supply_sprite_lists.values());
            skins = com.oddlabs.tt.model.RegistryItems.skins(queues, races, carried);
            wood_fragment_sprites[0] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/wood_2.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false), 0);
            wood_fragment_sprites[1] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/wood_3.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false));
            wood_fragment_sprites[2] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/wood_4.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false));
            wood_fragment_sprites[3] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/wood_5.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false));
            treasure_sprites[0] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/icon.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false));
            treasure_sprites[1] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/treasure_1.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false));
            treasure_sprites[2] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/treasure_2.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false));
            treasure_sprites[3] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/treasure_3.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false));
            treasure_sprites[4] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/treasure_4.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false));
            treasure_sprites[5] = queues.register(new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/treasure_5.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
        }
    }
}
