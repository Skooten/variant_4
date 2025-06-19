using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Entities;
using DemoApp.Helpers;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace DemoApp.ViewModels
{
    public partial class EditProductViewModel : ViewModelBase
    {
        private readonly DbyDemoFdbContext _context;
        private readonly ProductViewModel? _parentViewModel;
        private readonly Product _productEntity;

        [ObservableProperty]
        private string? _productArticle;

        [ObservableProperty]
        private string? _productName;

        [ObservableProperty]
        private decimal? _minimumCostForPartner;

        [ObservableProperty]
        private ObservableCollection<ProductType> _productTypes = new();

        [ObservableProperty]
        private ObservableCollection<MaterialType> _materialTypes = new();

        [ObservableProperty]
        private string? _errorMessage;
        [ObservableProperty]
        private ProductType? _selectedProductType;

        [ObservableProperty]
        private MaterialType? _selectedMaterialType;


        public EditProductViewModel(DbyDemoFdbContext context, Product? product = null, ProductViewModel? parentViewModel = null)
        {
            _context = context;
            _parentViewModel = parentViewModel;
            _productEntity = product ?? new Product();

            LoadProductTypes();
            LoadMaterialTypes();
            LoadProductData();
        }

       private void LoadProductTypes()
{
    var types = _context.ProductTypes.AsNoTracking().ToList(); // AsNoTracking предотвращает отслеживание объектов
    ProductTypes = new ObservableCollection<ProductType>(types);
}

private void LoadMaterialTypes()
{
    var types = _context.MaterialTypes.AsNoTracking().ToList(); // AsNoTracking предотвращает отслеживание объектов
    MaterialTypes = new ObservableCollection<MaterialType>(types);
}

        private void LoadProductData()
        {
            ProductArticle = _productEntity.ProductArticle;
            ProductName = _productEntity.ProductName;
            MinimumCostForPartner = _productEntity.MinimumCostForPartner;
            SelectedProductType = _productEntity.ProductType;
            SelectedMaterialType = _productEntity.MaterialType;
        }

        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(ProductArticle))
            {
                ErrorMessage = "Артикул продукта обязателен для заполнения";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ProductName))
            {
                ErrorMessage = "Название продукта обязательно для заполнения";
                return false;
            }

            if (MinimumCostForPartner == null || MinimumCostForPartner < 0)
            {
                ErrorMessage = "Минимальная стоимость для партнера должна быть неотрицательным числом";
                return false;
            }

            if (SelectedProductType == null)
            {
                ErrorMessage = "Выберите тип продукта";
                return false;
            }

            if (SelectedMaterialType == null)
            {
                ErrorMessage = "Выберите тип материала";
                return false;
            }

            ErrorMessage = null;
            return true;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            try
            {
                if (!ValidateData()) return;

                _productEntity.ProductArticle = ProductArticle;
                _productEntity.ProductName = ProductName;
                _productEntity.MinimumCostForPartner = MinimumCostForPartner;

                // Установите только идентификаторы типов продуктов и материалов
                _productEntity.ProductTypeId = SelectedProductType?.ProductTypeId;
                _productEntity.MaterialTypeId = SelectedMaterialType?.MaterialTypeId;

                if (_productEntity.ProductId == 0)
                {
                    _context.Products.Add(_productEntity);
                }
                else
                {
                    _context.Products.Update(_productEntity);
                }

                await _context.SaveChangesAsync();

                // Убедимся, что родительская VM доступна
                if (_parentViewModel != null && _parentViewModel._mainViewModel != null)
                {
                    _parentViewModel.LoadProducts();
                    _parentViewModel._mainViewModel.ShowProductsList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving data: {ex.Message}");
                // Сообщение об ошибке пользователю
                var errorMsg = $"Ошибка при сохранении данных: {ex.Message}";
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", errorMsg, ButtonEnum.Ok);
                await box.ShowAsync();
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            _parentViewModel?._mainViewModel.ShowProductsList();
        }
    }
}
