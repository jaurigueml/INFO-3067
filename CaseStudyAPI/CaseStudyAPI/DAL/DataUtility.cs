using CaseStudyAPI.DAL.DomainClasses;
using System.Text.Json;
namespace CaseStudyAPI.DAL
{
    public class DataUtility
    {
        private readonly AppDbContext _db;

        public DataUtility(AppDbContext context)
        {
            _db = context;
        }

        private async Task<bool> LoadBrands(dynamic jsonObjectArray)
        {
            bool loadedBrands = false;
            try
            {
                // Clear out the old rows
                _db.Brands?.RemoveRange(_db.Brands);
                await _db.SaveChangesAsync();

                List<string> allBrands = new List<string>();

                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    if (element.TryGetProperty("BRAND", out JsonElement brandJson))
                    {
                        allBrands.Add(brandJson.GetString()!);
                    }
                }

                IEnumerable<string> brandNames = allBrands.Distinct();

                foreach (string brandName in brandNames)
                {
                    Brand brand = new Brand();
                    brand.Name = brandName;
                    await _db.Brands!.AddAsync(brand);
                    await _db.SaveChangesAsync();
                }

                loadedBrands = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }

            return loadedBrands;
        }

        private async Task<bool> LoadProducts(dynamic jsonObjectArray)
        {
            bool loadedProducts = false;
            try
            {
                List<Brand> brands = _db.Brands!.ToList();

                // Clear out the old rows
                _db.Products?.RemoveRange(_db.Products);
                await _db.SaveChangesAsync();

                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    Product product = new Product();
                    product.CostPrice = Convert.ToDecimal(element.GetProperty("COSTPRICE").GetString());
                    product.MSRP = Convert.ToDecimal(element.GetProperty("MSRP").GetString());
                    product.QtyOnHand = Convert.ToInt32(element.GetProperty("QTYONHAND").GetString());
                    product.QtyOnBackOrder = Convert.ToInt32(element.GetProperty("QTYONBACKORDER").GetString());
                    product.ProductName = element.GetProperty("PRODUCTNAME").GetString();
                    product.GraphicName = element.GetProperty("GRAPHICNAME").GetString();
                    product.Description = element.GetProperty("DESCRIPTION").GetString();

                    string? brandName = element.GetProperty("BRAND").GetString();

                    // Add the foreign key reference here
                    foreach (Brand brand in brands)
                    {
                        if (brand.Name == brandName)
                        {
                            product.Brand = brand;
                            break;
                        }
                    }

                    await _db.Products!.AddAsync(product);
                    await _db.SaveChangesAsync();
                }

                loadedProducts = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }

            return loadedProducts;
        }

        public async Task<bool> LoadDataFromWebToDb(string stringJson)
        {
            bool brandsLoaded = false;
            bool productsLoaded = false;

            try
            {
                dynamic objectJson = JsonSerializer.Deserialize<object>(stringJson);
                brandsLoaded = await LoadBrands(objectJson);
                productsLoaded = await LoadProducts(objectJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return brandsLoaded && productsLoaded;
        }




    }
}