using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnicodeBlazor.Server;
using UnicodeBlazor.Server.Model;

namespace UnicodeBlazor.Shared.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            UnicodeBlockEntry block = new UnicodeBlockEntry("Currencysymbols","20A0","20CF");
            Assert.AreEqual(48, block.Count);

        }

        [TestMethod]
        public async Task AddEntry()
        {
            var mockSet = new Mock<DbSet<UnicodeCharacterEntry>>();
            var mockContext = new Mock<UnicodeBlazorContext>();
            mockContext.Setup(m => m.Entries).Returns(mockSet.Object);


            var service = new UCDService(null, mockContext.Object);
            var line = "A WITH ACUTE, LATIN CAPITAL LETTER	00C1";

            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(line));
            
            var entries = await service.UpdateIndexAsync(stream);
            Assert.AreEqual(1, entries);
        }
    }
}
