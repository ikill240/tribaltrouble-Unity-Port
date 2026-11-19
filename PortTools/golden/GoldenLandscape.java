import com.oddlabs.tt.procedural.Landscape;
import java.io.*;
import java.util.List;
import java.util.zip.CRC32;

/** Runs the real Java island generator and writes everything gameplay-relevant to a binary golden file. */
public class GoldenLandscape {
    // args: out_file players meters NATIVE|VIKING hills vegetation supplies seed initial_units random_start archipelago
    public static void main(String[] a) throws Exception {
        int players = Integer.parseInt(a[1]), meters = Integer.parseInt(a[2]);
        Landscape.TerrainType terrain = Landscape.TerrainType.valueOf(a[3]);
        float hills = Float.parseFloat(a[4]), veg = Float.parseFloat(a[5]), sup = Float.parseFloat(a[6]);
        int seed = Integer.parseInt(a[7]), units = Integer.parseInt(a[8]);
        float rnd = Float.parseFloat(a[9]); boolean arch = Boolean.parseBoolean(a[10]);
        float detail_prefade = 0.15f; // IslandGenerator value at default settings; only affects the detail texture
        long t0 = System.nanoTime();
        Landscape l = new Landscape(players, meters, terrain, detail_prefade, hills, veg, sup, seed, units, rnd, arch, null, false, null);
        long ms = (System.nanoTime() - t0) / 1_000_000;
        try (DataOutputStream o = new DataOutputStream(new BufferedOutputStream(new FileOutputStream(a[0])))) {
            o.writeInt(0x54544C31); // "TTL1"
            o.writeFloat(l.getSeaLevelMeters());
            float[][] h = l.getHeight();
            o.writeInt(h.length); o.writeInt(h[0].length);
            for (float[] row : h) for (float v : row) o.writeFloat(v);
            boolean[][] acc = l.getAccessGrid();
            o.writeInt(acc.length); o.writeInt(acc[0].length);
            for (boolean[] row : acc) for (boolean v : row) o.writeByte(v ? 1 : 0);
            writeBytes(o, l.getBuildGrid()); writeBytes(o, l.getDockGrid()); writeBytes(o, l.getWaterGrid());
            int[][] ids = l.getIslandIds();
            o.writeInt(ids.length); o.writeInt(ids[0].length);
            for (int[] row : ids) for (int v : row) o.writeInt(v);
            writeList(o, l.getTrees()); writeList(o, l.getPalmtrees()); writeList(o, l.getRock()); writeList(o, l.getIron());
            writeFloats(o, l.getPlants()); writeFloats(o, l.getStartingLocations());
        }
        CRC32 c = new CRC32(); c.update(java.nio.file.Files.readAllBytes(java.nio.file.Path.of(a[0])));
        System.out.printf("%s crc=%08x trees=%d palms=%d rock=%d iron=%d plants=%d starts=%d time=%dms%n", a[0], c.getValue(),
            l.getTrees().size(), l.getPalmtrees().size(), l.getRock().size(), l.getIron().size(), l.getPlants().length, l.getStartingLocations().length, ms);
    }
    static void writeBytes(DataOutputStream o, byte[][] g) throws IOException {
        o.writeInt(g.length); o.writeInt(g[0].length); for (byte[] r : g) o.write(r);
    }
    static void writeList(DataOutputStream o, List<int[]> l) throws IOException {
        o.writeInt(l.size()); for (int[] p : l) { o.writeInt(p.length); for (int v : p) o.writeInt(v); }
    }
    static void writeFloats(DataOutputStream o, float[][] f) throws IOException {
        o.writeInt(f.length); for (float[] p : f) { o.writeInt(p.length); for (float v : p) o.writeFloat(v); }
    }
}
