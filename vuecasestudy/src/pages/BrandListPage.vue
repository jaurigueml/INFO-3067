<template>
  <div class="text-center">
    <div class="logo">
      <img src="shoes.jpg" alt="Logo" class="logo-image" />
    </div>
    <div class="text-h2 q-mt-lg">Brands</div>

    <div class="status q-mt-md text-subtitle2 text-negative" text-color="red">
      {{ state.status }}
    </div>

    <p></p>
    <q-select
      class="q-mt-lg q-ml-lg"
      v-if="state.brands.length > 0"
      style="width: 50vw; margin-bottom: 4vh; background-color: #fff"
      :option-value="'id'"
      :option-label="'name'"
      model-value=""
      :options="state.brands"
      label="Select a Brand"
      v-model="state.selectedBrandId"
      @update:model-value="getProducts()"
      emit-value
      map-options
    />

    <div
      class="text-h6 text-bold text-center text-primary"
      v-if="state.products.length > 0"
    >
      {{ state.selectedBrand.name }} ITEMS
    </div>
    <q-scroll-area style="height: 55vh">
      <q-card class="q-ma-md">
        <q-list separator>
          <q-item
            avatar
            clickable
            v-for="item in state.products"
            :key="item.id"
            @click="selectProduct(item.id)"
          >
            <q-avatar style="height: 125px; width: 90px" square>
              <img :src="`/img/${item.graphicName}`" />
            </q-avatar>
            <q-item-section class="text-left">
              {{ item.productName }}
            </q-item-section>
          </q-item>
        </q-list>
      </q-card>
    </q-scroll-area>
  </div>
  <q-dialog
    v-model="state.itemSelected"
    transition-show="rotate"
    transition-hide="rotate"
  >
    <q-card>
      <q-card-actions align="right">
        <q-btn flat label="X" color="primary" v-close-popup class="text-h5" />
      </q-card-actions>
      <q-card-section>
        <div class="text-subtitle2 text-center">
          {{ state.selectedProduct.productName }}
        </div>
      </q-card-section>
      <q-card-section avatar class="text-center text-positive">
        {{ state.dialogStatus }}
        <q-avatar style="height: 125px; width: 90px" square>
          <img :src="`/img/${state.selectedProduct.graphicName}`" />
        </q-avatar>
      </q-card-section>
      <q-card-section>
        <q-chip icon="bookmark" color="primary" text-color="white" text-center
          >Nutrional Info
          <q-tooltip
            transition-show="flip-right"
            transition-hide="flip-left"
            text-color="white"
          >
            {{ state.selectedProduct.price }}
          </q-tooltip>
        </q-chip>
      </q-card-section>
      <q-card-section>
        <div class="row">
          <q-input
            v-model.number="state.qty"
            type="number"
            filled
            placeholder="qty"
            hint="Qty"
            dense
            style="max-width: 12vw"
          />&nbsp;
        </div>
        <q-btn
          color="primary"
          label="Add To Tray"
          :disable="state.qty < 0"
          @click="addToTray()"
          style="max-width: 25vw; margin-left: 3vw"
        />
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<style>
.logo-image {
  width: 150px;
  height: auto;
}
</style>

<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../utils/apiutil";

export default {
  setup() {
    onMounted(() => {
      loadBrands();
    });
    let state = reactive({
      status: "",
      brands: [],
      products: [],
      selectedBrand: {},
      selectedBrandId: "",
      selectedProduct: {},
      dialogStatus: "",
      itemSelected: false,
      qty: 0,
      tray: [],
    });
    const addToTray = () => {
      let index = -1;
      if (state.tray.length > 0) {
        index = state.tray.findIndex(
          // is item already on the tray
          (item) => item.id === state.selectedProduct.id
        );
      }
      if (state.qty > 0) {
        index === -1 // add
          ? state.tray.push({
              id: state.selectedProduct.id,
              qty: state.qty,
              item: state.selectedProduct,
            })
          : (state.tray[index] = {
              // replace
              id: state.selectedProduct.id,
              qty: state.qty,
              item: state.selectedProduct,
            });
        state.dialogStatus = `${state.qty} item(s) added`;
      } else {
        index === -1 ? null : state.tray.splice(index, 1); // remove
        state.dialogStatus = `item(s) removed`;
      }
      sessionStorage.setItem("tray", JSON.stringify(state.tray));
      state.qty = 0;
    };
    const selectProduct = async (productid) => {
      console.log(state.products);
      try {
        // q-item click sends us the Product item id, so we need to find the object
        state.selectedProduct = state.products.find(
          (item) => item.id === productid
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
    const getProducts = async () => {
      try {
        state.selectedBrand = state.brands.find(
          (brand) => brand.id === state.selectedBrandId
        );
        state.status = `finding products for brands ${state.selectedBrand}...`;
        state.products = await fetcher(`Product/${state.selectedBrand.id}`);
        state.status = `loaded ${state.products.length} product items for ${state.selectedBrand.name}`;
        console.log(state.products);
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    const loadBrands = async () => {
      try {
        state.status = "finding Brands ...";
        state.brands = await fetcher(`Brand`);

        state.status = `found ${state.brands.length} brands`;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    return {
      loadBrands,
      getProducts,
      selectProduct,
      addToTray,
      state,
    };
  },
};
</script>
