using System;
using System.Linq;
using System.Text.RegularExpressions;
using DemoApp.Entities;

namespace DemoApp.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Проверка артикула продукта (например, не пустой, может быть с буквами и цифрами)
        /// </summary>
        public static bool IsValidProductArticle(string? article)
        {
            if (string.IsNullOrWhiteSpace(article)) return false;
            // Пример: разрешаем буквы, цифры, дефисы, подчеркивания, длина 1-20 символов
            return Regex.IsMatch(article, @"^[A-Za-z0-9-_]{1,20}$");
        }

        /// <summary>
        /// Проверка названия продукта (не пустое, длина не более 100 символов)
        /// </summary>
        public static bool IsValidProductName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return name.Length <= 100;
        }

        /// <summary>
        /// Проверка минимальной стоимости для партнера (неотрицательное число)
        /// </summary>
        public static bool IsValidMinimumCost(decimal? cost)
        {
            return cost.HasValue && cost.Value >= 0;
        }

        /// <summary>
        /// Расчет необходимого материала на продукцию с учетом параметров и брака
        /// </summary>
        public static int CalculateRequiredMaterial(
            int productTypeId,
            int materialTypeId,
            int productQuantity,
            decimal parameter1,
            decimal parameter2)
        {
            using var context = new DbyDemoFdbContext();

            try
            {
                if (productQuantity <= 0 || parameter1 <= 0 || parameter2 <= 0)
                    return -1;

                var productType = context.ProductTypes.FirstOrDefault(pt => pt.ProductTypeId == productTypeId);
                var materialType = context.MaterialTypes.FirstOrDefault(mt => mt.MaterialTypeId == materialTypeId);

                if (productType == null || materialType == null)
                    return -1;

                // Расчет базового количества материала на единицу продукции
                decimal? baseAmount = parameter1 * parameter2 * productType.ProductTypeCoefficient;

                // Расчет общего количества с учетом количества продукции
                decimal? totalAmount = baseAmount * productQuantity;

                // Учет процента брака
                decimal? wasteMultiplier = 1 + materialType.PercentageDefectiveMaterial;
                decimal? finalAmount = totalAmount * wasteMultiplier;

                // Округление до целого числа в большую сторону
                return (int)Math.Ceiling(finalAmount ?? 0);
            }
            catch
            {
                return -1;
            }
        }
    }
}
