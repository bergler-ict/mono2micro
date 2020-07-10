using Monolith.Data.Dtos;
using Monolith.Logic;
using System.Collections.Generic;
using System.Linq;

namespace Monolith.Data
{
    public static class DatabaseFunctions
    {
        public static IEnumerable<string> GetAllProductCategories()
        {
            return Database.ExecuteSql<string>("SELECT * FROM Production.ProductCategory", dr => dr.GetString(1));
        }

        public static byte[] GetProductImageContent(int productId)
        {
            string sql =
                "SELECT     LargePhoto " +
                "FROM       Production.ProductPhoto pp " +
                "INNER JOIN Production.ProductProductPhoto ppp ON " +
                "           pp.ProductPhotoID = ppp.ProductPhotoID " +
                "WHERE      ppp.ProductID = @ProductID";

            var imageContent = Database.ExecuteSql<byte[]>(
                sql,
                new { ProductID = productId },
                r => (byte[])r[0]).FirstOrDefault();

            if (imageContent == null)
            {
                sql =
                    "SELECT     LargePhoto " +
                    "FROM       Production.ProductPhoto pp " +
                    "WHERE      pp.ProductPhotoID = 1";

                imageContent = Database.ExecuteSql<byte[]>(
                    sql,
                    r => (byte[])r[0]).First();
            }

            return imageContent;
        }

        public static IEnumerable<Product> GetProductsByCategory(string category)
        {
            string sql = @"
                SELECT		T.Name, T.ProductID, T.Description, ProductNumber, Price = prc.ListPrice
                FROM (
                SELECT		Name = ISNULL(pm.Name, p1.Name), ISNULL(pd.Description, '') AS Description, ProductID = MIN(p1.ProductID)
                FROM		Production.Product p1
                LEFT JOIN	Production.ProductModel pm ON pm.ProductModelID = p1.ProductModelID
                INNER JOIN	Production.ProductSubcategory psc ON p1.ProductSubcategoryID = psc.ProductSubcategoryID
                INNER JOIN	Production.ProductCategory pc ON pc.ProductCategoryID = psc.ProductCategoryID
                LEFT JOIN   Production.vProductAndDescription pd ON pd.ProductID = p1.ProductID AND CultureID = 'en'
                INNER JOIN  Production.ProductProductPhoto ppd ON ppd.ProductID = p1.ProductID AND ppd.[Primary] = 1 AND ppd.ProductPhotoID <> 1
                INNER JOIN  Production.ProductPhoto pp ON pp.ProductPhotoID = ppd.ProductPhotoID 
                WHERE		pc.Name = @ProductCategory 
                GROUP BY	ISNULL(pm.Name, p1.Name), pd.Description) AS T
                INNER JOIN  Production.Product p2 ON p2.ProductID = T.ProductID
                INNER JOIN  Production.ProductListPriceHistory prc ON prc.ProductID = T.ProductID AND prc.EndDate IS NULL
                ORDER BY    T.Name;";

            return Database.ExecuteSql<Product>(
                sql,
                new { ProductCategory = category },
                r => new Product()
                {
                    Id = r.GetInt32(r.GetOrdinal("ProductID")),
                    Name = r.GetString(r.GetOrdinal("Name")),
                    Number = r.GetString(r.GetOrdinal("ProductNumber")),
                    Description = r.GetString(r.GetOrdinal("Description")),
                    Price = r.GetDecimal(r.GetOrdinal("Price"))
                });
        }

        public static Cart GetCartById(string id)
        {
            var sql = @"
                SELECT      ShoppingCartID, ProductID, Quantity
                FROM        Sales.ShoppingCartItem
                WHERE       [ShoppingCartID] = @ShoppingCartID";

            var shoppingCartItems = Database.ExecuteSql<ShoppingCartItem>(
                sql,
                new { ShoppingCartID = id },
                r =>
                    new ShoppingCartItem()
                    {
                        ShoppingCartID = r.GetString(r.GetOrdinal("ShoppingCartID")),
                        ProductID = r.GetInt32(r.GetOrdinal("ProductID")),
                        Quantity = r.GetInt32(r.GetOrdinal("Quantity"))
                    });

            List<CartItem> cartItems = new List<CartItem>();
            foreach (var shoppingCartItem in shoppingCartItems)
            {
                cartItems.Add(new CartItem()
                {
                    Product = GetProductById(shoppingCartItem.ProductID),
                    Quantity = shoppingCartItem.Quantity
                });
            }

            return new Cart(id, cartItems);
        }

        public static Product GetProductById(int id)
        {
            return Database.ExecuteSql<Product>(
                    @"SELECT		TOP 1 T.Name, T.ProductID, T.Description, ProductNumber, Price = prc.ListPrice
                      FROM (
                      SELECT		Name = ISNULL(pm.Name, p1.Name), ISNULL(pd.Description, '') AS Description, ProductID = MIN(p1.ProductID)
                      FROM		Production.Product p1
                      LEFT JOIN	Production.ProductModel pm ON pm.ProductModelID = p1.ProductModelID
                      INNER JOIN	Production.ProductSubcategory psc ON p1.ProductSubcategoryID = psc.ProductSubcategoryID
                      INNER JOIN	Production.ProductCategory pc ON pc.ProductCategoryID = psc.ProductCategoryID
                      LEFT JOIN   Production.vProductAndDescription pd ON pd.ProductID = p1.ProductID AND CultureID = 'en'
                      INNER JOIN  Production.ProductProductPhoto ppd ON ppd.ProductID = p1.ProductID AND ppd.[Primary] = 1 AND ppd.ProductPhotoID <> 1
                      INNER JOIN  Production.ProductPhoto pp ON pp.ProductPhotoID = ppd.ProductPhotoID 
                      WHERE		p1.ProductID = @ProductID
                      GROUP BY	ISNULL(pm.Name, p1.Name), pd.Description) AS T
                      INNER JOIN  Production.Product p2 ON p2.ProductID = T.ProductID
                      INNER JOIN  Production.ProductListPriceHistory prc ON prc.ProductID = T.ProductID AND prc.EndDate IS NULL
                      ORDER BY    T.Name;",
                    new { ProductID = id },
                    r => new Product()
                    {
                        Id = r.GetInt32(r.GetOrdinal("ProductID")),
                        Name = r.GetString(r.GetOrdinal("Name")),
                        Number = r.GetString(r.GetOrdinal("ProductNumber")),
                        Description = r.GetString(r.GetOrdinal("Description")),
                        Price = r.GetDecimal(r.GetOrdinal("Price"))
                    }).FirstOrDefault();
        }

        public static void SaveCart(Cart cart)
        {
            var sql = @"
                DELETE FROM Sales.ShoppingCartItem
                WHERE       [ShoppingCartID] = @ShoppingCartID";
            Database.ExecuteSql(sql, new { ShoppingCartID = cart.Id });

            sql = @"
                INSERT INTO Sales.ShoppingCartItem (ShoppingCartID, ProductID, Quantity)
                VALUES (@ShoppingCartID, @ProductID, @Quantity)";

            foreach (var item in cart.Items)
            {
                Database.ExecuteSql(sql, new 
                { 
                    ShoppingCartID = cart.Id,
                    ProductID = item.Product.Id,
                    item.Quantity
                });
            }
        }

        public static void RemoveProductFromCart(string id, int productId, bool all = false)
        {
            var cart = GetCartById(id);
            var existingItem = cart.Items.FirstOrDefault(i => i.Product.Id == productId);
            if (existingItem != null)
            {
                existingItem.Quantity--;
                if (existingItem.Quantity <= 0 || all)
                {
                    cart.Items.RemoveAll(i => i.Product.Id == productId);
                }
            }
            SaveCart(cart);
        }

        public static void AddProductToCart(string id, int productId)
        {
            var cart = GetCartById(id);
            var existingItem = cart.Items.FirstOrDefault(i => i.Product.Id == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItem()
                {
                    Product = GetProductById(productId),
                    Quantity = 1
                });
            }
            SaveCart(cart);
        }
    }
}
