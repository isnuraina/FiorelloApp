﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloApp.Models
{
    public class Product:BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public int Count { get; set; }
        [NotMapped]
        public int BasketCount { get; set; }
        public Product()
        {
            ProductImages = new();
        }
    }
}
