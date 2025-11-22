using Coppery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;

namespace Integration_Tests.JsonTest
{
    internal class JsonTestCompose
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public void TestSampleJson() {

            var json = """
                [
                  {
                    "productId": 1001,
                    "productName": "Wireless Headphones",
                    "description": "Noise-cancelling wireless headphones with Bluetooth 5.0 and 20-hour battery life.",
                    "brand": "SoundPro",
                    "category": "Electronics",
                    "price": 199.99,
                    "currency": "USD",
                    "stock": {
                      "available": true,
                      "quantity": 50
                    },
                    "images": [
                      "https://example.com/products/1001/main.jpg",
                      "https://example.com/products/1001/side.jpg"
                    ],
                    "variants": [
                      {
                        "variantId": "1001_01",
                        "color": "Black",
                        "price": 199.99,
                        "stockQuantity": 20
                      },
                      {
                        "variantId": "1001_02",
                        "color": "White",
                        "price": 199.99,
                        "stockQuantity": 30
                      }
                    ],
                    "dimensions": {
                      "weight": "0.5kg",
                      "width": "18cm",
                      "height": "20cm",
                      "depth": "8cm"
                    },
                    "ratings": {
                      "averageRating": 4.7,
                      "numberOfReviews": 120
                    },
                    "reviews": [
                      {
                        "reviewId": 501,
                        "userId": 101,
                        "username": "techguy123",
                        "rating": 5,
                        "comment": "Amazing sound quality and battery life!"
                      },
                      {
                        "reviewId": 502,
                        "userId": 102,
                        "username": "jane_doe",
                        "rating": 4,
                        "comment": "Great headphones but a bit pricey."
                      }
                    ]
                  },
                  {
                    "productId": 1002,
                    "productName": "Smartphone Case",
                    "description": "Durable and shockproof case for smartphones, available in multiple colors.",
                    "brand": "CaseMate",
                    "category": "Accessories",
                    "price": 29.99,
                    "currency": "USD",
                    "stock": {
                      "available": true,
                      "quantity": 200
                    },
                    "images": [
                      "https://example.com/products/1002/main.jpg",
                      "https://example.com/products/1002/back.jpg"
                    ],
                    "variants": [
                      {
                        "variantId": "1002_01",
                        "color": "Black",
                        "price": 29.99,
                        "stockQuantity": 100
                      },
                      {
                        "variantId": "1002_02",
                        "color": "Blue",
                        "price": 29.99,
                        "stockQuantity": 100
                      }
                    ],
                    "dimensions": {
                      "weight": "0.2kg",
                      "width": "8cm",
                      "height": "15cm",
                      "depth": "1cm"
                    },
                    "ratings": {
                      "averageRating": 4.4,
                      "numberOfReviews": 80
                    },
                    "reviews": [
                      {
                        "reviewId": 601,
                        "userId": 103,
                        "username": "caseuser456",
                        "rating": 4,
                        "comment": "Very sturdy and fits perfectly."
                      },
                      {
                        "reviewId": 602,
                        "userId": 104,
                        "username": "mobile_fan",
                        "rating": 5,
                        "comment": "Best case I've bought for my phone!"
                      }
                    ]
                  },
                  {
                    "productId": 1003,
                    "productName": "4K Ultra HD Smart TV",
                    "description": "55-inch 4K Ultra HD Smart TV with built-in Wi-Fi and streaming apps.",
                    "brand": "Visionary",
                    "category": "Electronics",
                    "price": 799.99,
                    "currency": "USD",
                    "stock": {
                      "available": true,
                      "quantity": 30
                    },
                    "images": [
                      "https://example.com/products/1003/main.jpg",
                      "https://example.com/products/1003/side.jpg"
                    ],
                    "variants": [
                      {
                        "variantId": "1003_01",
                        "screenSize": "55 inch",
                        "price": 799.99,
                        "stockQuantity": 30
                      }
                    ],
                    "dimensions": {
                      "weight": "15kg",
                      "width": "123cm",
                      "height": "80cm",
                      "depth": "10cm"
                    },
                    "ratings": {
                      "averageRating": 4.8,
                      "numberOfReviews": 250
                    },
                    "reviews": [
                      {
                        "reviewId": 701,
                        "userId": 105,
                        "username": "techlover123",
                        "rating": 5,
                        "comment": "Incredible picture quality, streaming works seamlessly."
                      },
                      {
                        "reviewId": 702,
                        "userId": 106,
                        "username": "homecinema",
                        "rating": 4,
                        "comment": "Great TV, but a little bulky."
                      }
                    ]
                  },
                  {
                    "productId": 1004,
                    "productName": "Bluetooth Speaker",
                    "description": "Portable Bluetooth speaker with 12-hour battery life and water resistance.",
                    "brand": "AudioX",
                    "category": "Electronics",
                    "price": 59.99,
                    "currency": "USD",
                    "stock": {
                      "available": true,
                      "quantity": 100
                    },
                    "images": [
                      "https://example.com/products/1004/main.jpg",
                      "https://example.com/products/1004/side.jpg"
                    ],
                    "variants": [
                      {
                        "variantId": "1004_01",
                        "color": "Red",
                        "price": 59.99,
                        "stockQuantity": 50
                      },
                      {
                        "variantId": "1004_02",
                        "color": "Blue",
                        "price": 59.99,
                        "stockQuantity": 50
                      }
                    ],
                    "dimensions": {
                      "weight": "0.3kg",
                      "width": "15cm",
                      "height": "8cm",
                      "depth": "5cm"
                    },
                    "ratings": {
                      "averageRating": 4.6,
                      "numberOfReviews": 150
                    },
                    "reviews": [
                      {
                        "reviewId": 801,
                        "userId": 107,
                        "username": "musicfan23",
                        "rating": 5,
                        "comment": "Excellent sound quality for its size!"
                      },
                      {
                        "reviewId": 802,
                        "userId": 108,
                        "username": "outdoor_lover",
                        "rating": 4,
                        "comment": "Great for outdoor use, but the battery could last longer."
                      }
                    ]
                  },
                  {
                    "productId": 1005,
                    "productName": "Winter Jacket",
                    "description": "Men's water-resistant winter jacket with a fur-lined hood.",
                    "brand": "ColdTech",
                    "category": "Clothing",
                    "price": 129.99,
                    "currency": "USD",
                    "stock": {
                      "available": true,
                      "quantity": 80
                    },
                    "images": [
                      "https://example.com/products/1005/main.jpg",
                      "https://example.com/products/1005/back.jpg"
                    ],
                    "variants": [
                      {
                        "variantId": "1005_01",
                        "size": "M",
                        "color": "Black",
                        "price": 129.99,
                        "stockQuantity": 30
                      },
                      {
                        "variantId": "1005_02",
                        "size": "L",
                        "color": "Gray",
                        "price": 129.99,
                        "stockQuantity": 50
                      }
                    ],
                    "dimensions": {
                      "weight": "1.5kg",
                      "width": "60cm",
                      "height": "85cm",
                      "depth": "5cm"
                    },
                    "ratings": {
                      "averageRating": 4.5,
                      "numberOfReviews": 60
                    },
                    "reviews": [
                      {
                        "reviewId": 901,
                        "userId": 109,
                        "username": "outdoor_adventurer",
                        "rating": 5,
                        "comment": "Perfect for cold weather, very comfortable!"
                      },
                      {
                        "reviewId": 902,
                        "userId": 110,
                        "username": "winter_gear",
                        "rating": 4,
                        "comment": "Nice jacket, but could be a little warmer."
                      }
                    ]
                  }
                ]
                """;

            var composer = new ObjectComposer([typeof(bool), typeof(int), typeof(string), typeof(double), typeof(Root), typeof(Dimensions), typeof(Ratings), typeof(Review), typeof(Stock), typeof(Variant)]);

            var root = composer.GenerateObject<Root>(JsonNode.Parse(json).AsArray()[0].ToString());

            Assert.That(root.productId, Is.EqualTo(1001));
            Assert.That(root.Stock.available, Is.EqualTo(true));
            Assert.That(root.brand, Is.EqualTo("SoundPro"));
            Assert.That(root.Variants[0].variantId, Is.EqualTo("1001_01"));
            Assert.That(root.Ratings.numberOfReviews, Is.EqualTo(120));

        }

        public class Dimensions
        {
            public string weight { get; set; }
            public string width { get; set; }
            public string height { get; set; }
            public string depth { get; set; }
        }

        public class Ratings
        {
            public double averageRating { get; set; }
            public int numberOfReviews { get; set; }
        }

        public class Review
        {
            public int reviewId { get; set; }
            public int userId { get; set; }
            public string username { get; set; }
            public int rating { get; set; }
            public string comment { get; set; }
        }

        public class Root
        {
            public int productId { get; set; }
            public string productName { get; set; }
            public string description { get; set; }
            public string brand { get; set; }
            public string category { get; set; }
            public double price { get; set; }
            public string currency { get; set; }
            public Stock Stock { get; set; }
            public List<string> Images { get; set; }
            public List<Variant> Variants { get; set; }
            public Dimensions Dimensions { get; set; }
            public Ratings Ratings { get; set; }
            public List<Review> Reviews { get; set; }
        }

        public class Stock
        {
            public bool available { get; set; }
            public int quantity { get; set; }
        }

        public class Variant
        {
            public string variantId { get; set; }
            public string color { get; set; }
            public double price { get; set; }
            public int stockQuantity { get; set; }
            public string screenSize { get; set; }
            public string size { get; set; }
        }


    }


}
