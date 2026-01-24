using System.Linq;
using TFT.Search.Library.Repositories;

namespace TestProject
{
    public class IntegrationsTests
    {
        [Fact]
        public void Test1()
        {
            var repo = new TftRepository();
            var data = repo.GetJsonFile();
            var currentSet = data.SetData.AsEnumerable().OrderByDescending(x=>x.Id).FirstOrDefault();
            Assert.NotNull(currentSet);
            var currentChampions = currentSet?.Champions;
            Assert.NotNull(currentChampions);
            var kat = currentChampions?.FirstOrDefault(x => x.Name.Contains("Cho"));
            var championsWithWounds = currentChampions?.Where(x => x?.Ability?.Desc?.Contains("Wound") ?? false);
            Assert.NotNull(championsWithWounds?.Count());
        }

        [Fact]
        public void GetTraits()
        {
            var repo = new TftRepository();
            var service = new TftService(repo);
            TftDataBuilder builder = new TftDataBuilder(service);
            var traits = builder.CurrentSet?.Traits;
            Assert.NotNull(traits);
        }
    }
}