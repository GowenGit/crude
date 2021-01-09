using System.Collections.Generic;

namespace Crude.Demo.Wasm.Services
{
    public class DummyDataService
    {
        private static readonly List<Product> _productsRepository = new List<Product>
        {
            new Product
            {
                Id = 1,
                DisplayName = "Toilet Paper",
                Price = 2.99m,
                Quantity = 8
            },
            new Product
            {
                Id = 2,
                DisplayName = "GPS Smartwatch",
                Price = 229.95m,
                Quantity = 3
            },
            new Product
            {
                Id = 3,
                DisplayName = "Bluetooth Tracker",
                Price = 24.99m,
                Quantity = 15
            },
            new Product
            {
                Id = 4,
                DisplayName = "Wireless Headphones",
                Price = 99.99m,
                Quantity = 1
            },
            new Product
            {
                Id = 5,
                DisplayName = "Case for iPhone 12",
                Price = 39.95m,
                Quantity = 99
            },
            new Product
            {
                Id = 6,
                DisplayName = "Wireless Earbuds",
                Price = 39.99m,
                Quantity = 35
            },
            new Product
            {
                Id = 7,
                DisplayName = "Lightning to USB A Cable",
                Price = 9.49m,
                Quantity = 7
            },
            new Product
            {
                Id = 8,
                DisplayName = "Car Charger",
                Price = 14.53m,
                Quantity = 41
            },
            new Product
            {
                Id = 9,
                DisplayName = "Heart Rate Monitor",
                Price = 69.99m,
                Quantity = 2
            },
            new Product
            {
                Id = 10,
                DisplayName = "Stereo Receiver",
                Price = 31.99m,
                Quantity = 76
            }
        };

        public IEnumerable<Product> GetProducts()
        {
            return _productsRepository;
        }
    }

    public class Product
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
