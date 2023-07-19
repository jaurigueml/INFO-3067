<template>
  <div class="text-center">
    <div class="text-center">
      <div class="text-h4 q-mt-md text-primary">Tray Contents</div>
      <q-avatar class="q-mt-md" size="xl" square>
        <img src="~assets/tray.png" />
      </q-avatar>
      <div class="text-h6 text-positive">{{ state.status }}</div>
    </div>

    <q-scroll-area style="height: 55vh">
      <q-item style="bottom: -2vh">
        <q-item-section class="col-2 q-ml-sm text-h6 text-primary"
          >QTY</q-item-section
        >
        <q-item-section class="q-ml-sm text-h6 text-primary"
          >DESCRIPTION</q-item-section
        >
      </q-item>

      <q-card class="q-ma-md">
        <q-list separator>
          <q-item avatar v-for="item in state.tray" :key="item.id">
            <q-avatar>
              <img :src="`/burger.jpg`" />
            </q-avatar>
            <q-item-section class="text-left">
              {{ item.item.description }}
            </q-item-section>
          </q-item>
        </q-list>
      </q-card>
    </q-scroll-area>
    <q-chip icon="bookmark" color="primary" text-color="white" text-center
      >Nutrional Info
      <q-tooltip
        transition-show="flip-right"
        transition-hide="flip-left"
        text-color="white"
      >
        Protein 45gm Calories 610 Carbs. <br />
        50 Fibre 3gm Choles. 125mg Salt <br />
        1750mg Fat 26gm
        {{ state.fbrTot }}gm.
      </q-tooltip>
    </q-chip>
    <br />
    <q-btn
      icon="delete"
      color="primary"
      label="Clear Tray"
      @click="clearTray()"
    />
  </div>
</template>
<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../utils/apiutil";
import { useRouter } from "vue-router";
export default {
  setup() {
    onMounted(() => {
      state.tray.forEach((trayItem) => {
        state.fbrTot += trayItem.item.fibre * trayItem.qty;
      });

      loadCategories();
    });
    let state = reactive({
      status: "",
      categories: [],
      menuitems: [],
      selectedCategory: {},
      selectedCategoryId: "",
      selectedMenuItem: {},
      dialogStatus: "",
      itemSelected: false,
      qty: 0,
      tray: [],
    });
    const router = useRouter();
    const viewTray = () => {
      router.push("tray");
    };
    const clearTray = () => {
      sessionStorage.removeItem("tray");
      state.tray = [];
      state.status = "tray cleared";
    };
    const addToTray = () => {
      let index = -1;
      if (state.tray.length > 0) {
        index = state.tray.findIndex(
          // is item already on the tray
          (item) => item.id === state.selectedMenuItem.id
        );
      }
      if (state.qty > 0) {
        index === -1 // add
          ? state.tray.push({
              id: state.selectedMenuItem.id,
              qty: state.qty,
              item: state.selectedMenuItem,
            })
          : (state.tray[index] = {
              // replace
              id: state.selectedMenuItem.id,
              qty: state.qty,
              item: state.selectedMenuItem,
            });
        state.dialogStatus = `${state.qty} item(s) added`;
      } else {
        index === -1 ? null : state.tray.splice(index, 1); // remove
        state.dialogStatus = `item(s) removed`;
      }
      sessionStorage.setItem("tray", JSON.stringify(state.tray));
      state.qty = 0;
    };
    const selectMenuItem = async (menuitemid) => {
      try {
        // q-item click sends us the menu item id, so we need to find the object
        state.selectedMenuItem = state.menuitems.find(
          (item) => item.id === menuitemid
        );
        state.itemSelected = true;
        state.dialogStatus = "";
        if (sessionStorage.getItem("tray")) {
          state.tray = JSON.parse(sessionStorage.getItem("tray"));
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    const getMenuitems = async () => {
      try {
        state.selectedCategory = state.categories.find(
          (category) => category.id === state.selectedCategoryId
        );
        state.status = `finding menuitems for category ${state.selectedCategory}...`;
        state.menuitems = await fetcher(
          `Menuitem/${state.selectedCategory.id}`
        );
        state.status = `loaded ${state.menuitems.length} menu items for
        ${state.selectedCategory.name}`;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    const loadCategories = async () => {
      try {
        state.status = "finding categories ...";
        state.status = await fetcher(`Data`);
        state.categories = await fetcher(`Category`);
        state.tray = JSON.parse(sessionStorage.getItem("tray"));
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    return {
      loadCategories,
      getMenuitems,
      selectMenuItem,
      addToTray,
      clearTray,
      state,
    };
  },
};
</script>
