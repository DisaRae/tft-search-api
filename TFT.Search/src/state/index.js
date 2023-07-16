export default class {
    set = [];
    searchName = '';
    searchSpellDescription = '';
    searchTraits = '';


    getChampionsByName() {
        var result = fetch('https://localhost:7202/CurrentSet/champions/' + state.searchName);
        var championList = JSON.parse(result);
        this.set = championList;
    }
}