<template>
    <div class="hello">
        <h1>{{ msg }}</h1>
        <p>
            For a guide and recipes on how to configure / customize this project,<br>
            check out the
            <a href="https://cli.vuejs.org" target="_blank" rel="noopener">vue-cli documentation</a>.
        </p>
        <h3>Search Parameters</h3>
        <ul>
            <li><input v-bind="model.searchName" v-on:blur="model.getChampionsByName" /></li>
        </ul>
        <h3>Essential Links</h3>
        <div class="col-md-12" v-for="c in model.set"
             :access-key ="c.name"
             :value ="c.spell.desc">
            <div>
                Name: {{{c.name}}}
            </div>
            <div>
                Spell:  {{{c.spell.desc}}}
            </div>
        </div>
    </div>
</template>

<script>
    import state from '../state/index.js';
    import reactive from 'vue';

    export default {

        name: 'HelloWorld',
        props: {
            msg: String
        },
        methods: {
            getChampionsByName,
        },
        setup() {
            const model = reactive(state);
            return {
                model,
            }
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
