using System.Linq;
using TFT.Search.Library.Repositories;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var repo = new TftRepository();
            var data = repo.GetJsonFile();
            var currentSet = data.SetData.AsEnumerable().FirstOrDefault(x => x.Id == 9);
            Assert.NotNull(currentSet);
            var currentChampions = currentSet?.Champions;
            Assert.NotNull(currentChampions);
            var kat = currentChampions?.FirstOrDefault(x => x.Name == "Katarina");
            var championsWithWounds = currentChampions?.Where(x => x?.Ability?.Desc?.Contains("Wound") ?? false);
            Assert.NotNull(championsWithWounds?.Count());
        }
    }
}