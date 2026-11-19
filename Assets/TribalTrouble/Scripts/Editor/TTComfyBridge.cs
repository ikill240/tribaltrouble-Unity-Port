using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    /// <summary>
    /// A bridge to the ComfyUI running on this computer, for making art (textures, backdrops) with it unattended.
    /// Drop a job into Benchmarks/comfy/in/&lt;name&gt;.json and the editor sends it to ComfyUI's local API:
    /// <list type="bullet">
    /// <item>{"get": "/object_info/CheckpointLoaderSimple"}: the reply is saved as out/&lt;name&gt;.json (what's installed)</item>
    /// <item>{"workflow": { ComfyUI API-format graph }}: queued, waited for, and every image it saves is downloaded to
    /// out/&lt;name&gt;_&lt;n&gt;.png</item>
    /// </list>
    /// out/&lt;name&gt;.log says how it went. ComfyUI is looked for on ports 8188 and 8000 (the desktop app), or the one in
    /// comfy/port.txt. Jobs run one at a time, in the background (the editor stays usable).
    /// </summary>
    [InitializeOnLoad]
    static class TTComfyBridge
    {
        static double s_next;
        static volatile bool s_busy;
        static double s_busy_since;   // a job that hangs (ComfyUI stuck) is given up after 4 minutes
        static readonly HttpClient s_http = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };

        static TTComfyBridge() { EditorApplication.update += Poll; }

        static string Root => Path.Combine(Path.GetDirectoryName(Application.dataPath), "Benchmarks", "comfy");

        static void Poll()
        {
            if (s_busy && EditorApplication.timeSinceStartup - s_busy_since > 240.0) s_busy = false;
            if (s_busy || EditorApplication.timeSinceStartup < s_next) return;
            s_next = EditorApplication.timeSinceStartup + 2.0;
            string inDir = Path.Combine(Root, "in");
            if (!Directory.Exists(inDir)) return;
            string[] jobs;
            try { jobs = Directory.GetFiles(inDir, "*.json"); } catch { return; }
            if (jobs.Length == 0) return;
            Array.Sort(jobs, StringComparer.Ordinal);
            string job = jobs[0];
            string name = Path.GetFileNameWithoutExtension(job);
            string text;
            try
            {
                text = File.ReadAllText(job);
                string taken = Path.Combine(inDir, name + ".taken");
                if (File.Exists(taken)) File.Delete(taken);   // a job sent again under the same name
                File.Move(job, taken);
            }
            catch { return; }
            s_busy = true;
            s_busy_since = EditorApplication.timeSinceStartup;
            string outDir = Path.Combine(Root, "out");
            Directory.CreateDirectory(outDir);
            string portFile = Path.Combine(Root, "port.txt");
            string port = File.Exists(portFile) ? File.ReadAllText(portFile).Trim() : null;
            Task.Run(async () =>
            {
                var log = new StringBuilder();
                try { await Run(name, text, outDir, port, log); }
                catch (Exception e) { log.AppendLine("failed: " + e.GetType().Name + ": " + e.Message); }
                finally
                {
                    try { File.WriteAllText(Path.Combine(outDir, name + ".log"), log.ToString()); } catch { }
                    s_busy = false;
                }
            });
        }

        static async Task<string> Base(string port, StringBuilder log)
        {
            foreach (var p in port != null ? new[] { port } : new[] { "8188", "8000" })
            {
                string b = "http://127.0.0.1:" + p;
                try
                {
                    var r = await s_http.GetAsync(b + "/system_stats");
                    if (r.IsSuccessStatusCode) { log.AppendLine("ComfyUI at " + b); return b; }
                }
                catch (Exception) { }
            }
            throw new Exception("ComfyUI not answering on " + (port ?? "8188 or 8000") + " (is it running? put its port in Benchmarks/comfy/port.txt)");
        }

        static async Task Run(string name, string text, string outDir, string port, StringBuilder log)
        {
            string b = await Base(port, log);
            string get = Field(text, "\"get\"");
            if (get != null)
            {
                var s = await s_http.GetStringAsync(b + get);
                File.WriteAllText(Path.Combine(outDir, name + ".json"), s);
                log.AppendLine("saved " + get + " (" + s.Length + " chars)");
                return;
            }
            // images to send first: "uploads": ["file.png", ...] (paths under Benchmarks/comfy/src), each uploaded to
            // ComfyUI's input folder under its own name, for LoadImage nodes
            int ui = text.IndexOf("\"uploads\"", StringComparison.Ordinal);
            if (ui >= 0)
            {
                int a = text.IndexOf('[', ui), z = text.IndexOf(']', a);
                foreach (var part in text.Substring(a + 1, z - a - 1).Split(','))
                {
                    string f = part.Trim().Trim('"');
                    if (f.Length == 0) continue;
                    string path = Path.Combine(Root, "src", f.Replace('/', Path.DirectorySeparatorChar));
                    using var form = new MultipartFormDataContent();
                    var img = new ByteArrayContent(File.ReadAllBytes(path));
                    img.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                    form.Add(img, "image", Path.GetFileName(f));
                    form.Add(new StringContent("true"), "overwrite");
                    form.Add(new StringContent("input"), "type");
                    var up = await s_http.PostAsync(b + "/upload/image", form);
                    if (!up.IsSuccessStatusCode) throw new Exception("upload of " + f + " refused: " + await up.Content.ReadAsStringAsync());
                    log.AppendLine("uploaded " + f);
                }
            }
            // the workflow object, sent as {"prompt": ..., "client_id": ...}
            int wi = text.IndexOf("\"workflow\"", StringComparison.Ordinal);
            if (wi < 0) throw new Exception("job has neither \"get\" nor \"workflow\"");
            int open = text.IndexOf('{', wi);
            int close = Matching(text, open);
            string wf = text.Substring(open, close - open + 1);
            var body = new StringContent("{\"prompt\":" + wf + ",\"client_id\":\"tt-bridge\"}", Encoding.UTF8, "application/json");
            var resp = await s_http.PostAsync(b + "/prompt", body);
            string rs = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode) throw new Exception("queue refused: " + rs);
            string id = Field(rs, "\"prompt_id\"");
            log.AppendLine("queued " + id);
            var t0 = DateTime.UtcNow;
            string hist = null;
            while ((DateTime.UtcNow - t0).TotalMinutes < 3.5)
            {
                await Task.Delay(1500);
                hist = await s_http.GetStringAsync(b + "/history/" + id);
                if (hist.Contains("\"outputs\"") && (hist.Contains("\"completed\": true") || hist.Contains("\"completed\":true") || hist.Contains("\"status_str\": \"error\"") || hist.Contains("\"status_str\":\"error\"")))
                    break;
            }
            if (hist == null || !hist.Contains("\"outputs\"")) throw new Exception("timed out waiting for " + id);
            if (hist.Contains("\"status_str\": \"error\"") || hist.Contains("\"status_str\":\"error\"")) log.AppendLine("ComfyUI reported an error: " + hist.Substring(0, Math.Min(hist.Length, 3000)));
            // every {"filename": ..., "subfolder": ..., "type": "output"} in the outputs
            int n = 0, at = hist.IndexOf("\"outputs\"", StringComparison.Ordinal);
            while ((at = hist.IndexOf("\"filename\"", at + 1, StringComparison.Ordinal)) >= 0)
            {
                int objStart = hist.LastIndexOf('{', at), objEnd = hist.IndexOf('}', at);
                string obj = hist.Substring(objStart, objEnd - objStart + 1);
                string fn = Field(obj, "\"filename\""), sub = Field(obj, "\"subfolder\"") ?? "", type = Field(obj, "\"type\"") ?? "output";
                if (fn == null || type != "output") continue;
                var bytes = await s_http.GetByteArrayAsync(b + "/view?filename=" + Uri.EscapeDataString(fn) + "&subfolder=" + Uri.EscapeDataString(sub) + "&type=" + type);
                string dst = Path.Combine(outDir, name + "_" + n++ + Path.GetExtension(fn));
                File.WriteAllBytes(dst, bytes);
                log.AppendLine("saved " + Path.GetFileName(dst) + " (" + bytes.Length + " bytes) from " + fn);
            }
            log.AppendLine(n + " images in " + (DateTime.UtcNow - t0).TotalSeconds.ToString("0") + " s");
        }

        /// <summary>A JSON string field's value (first occurrence), or null.</summary>
        static string Field(string json, string key)
        {
            int i = json.IndexOf(key, StringComparison.Ordinal);
            if (i < 0) return null;
            i = json.IndexOf(':', i + key.Length);
            if (i < 0) return null;
            int q = json.IndexOf('"', i + 1);
            if (q < 0) return null;
            // a value that isn't a string (a number, object): not this field
            string between = json.Substring(i + 1, q - i - 1).Trim();
            if (between.Length > 0) return null;
            var sb = new StringBuilder();
            for (int k = q + 1; k < json.Length; k++)
            {
                char c = json[k];
                if (c == '\\' && k + 1 < json.Length) { sb.Append(json[++k]); continue; }
                if (c == '"') return sb.ToString();
                sb.Append(c);
            }
            return null;
        }

        static int Matching(string s, int open)
        {
            int depth = 0; bool str = false;
            for (int k = open; k < s.Length; k++)
            {
                char c = s[k];
                if (str) { if (c == '\\') k++; else if (c == '"') str = false; continue; }
                if (c == '"') str = true;
                else if (c == '{') depth++;
                else if (c == '}' && --depth == 0) return k;
            }
            throw new Exception("unbalanced workflow JSON");
        }
    }
}
