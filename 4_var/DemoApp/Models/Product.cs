using System;
using System.Linq;
using DemoApp.Entities;

namespace DemoApp.Models
{
    public class Product
    {
        private readonly Entities.Product _entity;

        public Product(Entities.Product entity)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        // Делегируем свойства напрямую из сущности
        public string ProductName => _entity.ProductName;
        public string ProductArticle => _entity.ProductArticle;
        public decimal? MinimumCostForPartner => _entity.MinimumCostForPartner;
        
        // Навигационные свойства
        public ProductType ProductType => _entity.ProductType;
        public MaterialType MaterialType => _entity.MaterialType;
        
        // Вычисляемое свойство времени изготовления
        public int TotalManufacturingTimeHours => 
            _entity.ProductWorkshops?
                .Where(pw => pw.ManufacturingTimeHours.HasValue)
                .Sum(pw => Math.Max(0, (int)pw.ManufacturingTimeHours.Value)) ?? 0;

        // Свойство для доступа к оригинальной сущности
        public Entities.Product Entity => _entity;
    }
}