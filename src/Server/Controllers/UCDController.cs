using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnicodeBlazor.Server.Model;
using UnicodeBlazor.Shared;

namespace UnicodeBlazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UCDController : ControllerBase
    {
        private readonly UCDService _service;

        public UCDController(UCDService service)
        {
            _service = service;
        }

        [HttpGet("version/latest")]
        public async Task<string> GetLatestVersion()
        {
            return await _service.GetVersionAsync("latest");
        }

        [HttpGet("{version}/index")]
        public async Task<Stream> GetIndex(string version)
        {
            return await _service.GetIndexAsync(version);
        }
        [HttpGet("{version}/data")]
        public async Task<Stream> GetData(string version)
        {
            return await _service.GetDataAsync(version);
        }

        [HttpPost("{version}/index/download")]
        public async Task<UpdateIndexResponse> DownloadIndex(string version, [FromBody] UpdateIndexRequest model)
        {
            var blocks = await _service.GetBlocksAsync(version);
            var blockEntries = await _service.UpdateBlocksAsync(blocks);

            var index = await _service.GetIndexAsync(version);
            var indexEntries = await _service.UpdateIndexAsync(index);

            var response = new UpdateIndexResponse() { Version = version, Message = "Index updated", EntryCount = indexEntries, BlockCount = blockEntries };
            return response;
        }

        [HttpGet("{version}/index/count")]
        public async Task<string> CountIndex(string version)
        {
            return await _service.CountIndexAsync(version);
        }
    }
}
