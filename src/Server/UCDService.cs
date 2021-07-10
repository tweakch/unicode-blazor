using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnicodeBlazor.Server.Model;

namespace UnicodeBlazor.Server
{
    public class UCDService
    {


        private readonly HttpClient _client;
        private readonly UnicodeBlazorContext _context;

        public UCDService(HttpClient client, UnicodeBlazorContext context)
        {
            _client = client;
            _context = context;
        }

        public Task<string> GetVersionAsync(string version)
        {
            if (version == "latest") return Task.FromResult("13.0.0");
            else return Task.FromResult("?");
        }

        public Task<Stream> GetBlocksAsync(string version)
        {
            return GetUcdFileAsync(version, "Blocks.txt");
        }

        public Task<Stream> GetIndexAsync(string version)
        {
            return GetUcdFileAsync(version, "Index.txt");
        }
        public Task<Stream> GetDataAsync(string version)
        {
            return GetUcdFileAsync(version, "UnicodeData.txt");
        }

        private Task<Stream> GetUcdFileAsync(string version, string filename)
        {
            return _client.GetStreamAsync($"{version}/ucd/{filename}");
        }

        public async Task<int> UpdateBlocksAsync(Stream blocks)
        {
            int numEntries = 0;
            using StreamReader reader = new(blocks);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                {
                    continue;
                }
                var parts = line.Split(";");
                var range = parts[0].Split("..");
                var name = parts[1].Trim();

                var start = range[0];
                var end = range[1];
                var entry = await _context.Blocks.FindAsync(name);
                if (entry == null)
                {
                    await _context.Blocks.AddAsync(new Shared.UnicodeBlockEntry(name, start, end));
                    await _context.SaveChangesAsync();
                }
                numEntries++;
            }
            return numEntries;
        }

        public async Task<int> UpdateIndexAsync(Stream index)
        {
            var blocks = _context.Blocks.OrderBy(b => b.Start).ToDictionary(b => b.Start, b => b);

            using StreamReader reader = new(index);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var values = line.Split("\t");
                var name = values[0];

                try
                {
                    var codepos = Convert.ToInt32(values[1], 16);
                    var entry = await _context.Entries.FindAsync(name, codepos);

                    if (entry == null)
                    {
                        var key = blocks.Keys.First(b => b >= codepos);
                        await _context.Entries.AddAsync(new Shared.UnicodeCharacterEntry() { Name = name, Codepos = codepos, Block = blocks[key] });
                    }
                }
                catch (Exception)
                {
                    if (name == "Betty" && values[1] == "BOOP") continue; // ignore betty BOOP
                    if (name == "the" && values[1] == "DOOD") continue; // ignore the DOOD
                    throw;
                }
            }

            await _context.SaveChangesAsync();
            return _context.Entries.Count();
        }

        public async Task<string> CountIndexAsync(string version)
        {
            using var index = await GetIndexAsync(version);
            using var reader = new StreamReader(index);

            int lines = 0;
            while (!reader.EndOfStream)
            {
                await reader.ReadLineAsync();
                lines++;
            }
            return $"{lines}";
        }
    }
}
