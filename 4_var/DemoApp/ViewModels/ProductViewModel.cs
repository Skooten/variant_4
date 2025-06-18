using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Entities;
using DemoApp.Models;
using DemoApp.Views;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.ViewModels
{
    public partial class ProductViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<Models.Product> _products = new();

        [ObservableProperty]
        private Models.Product? _selectedProduct;

        private readonly DbyDemoFdbContext _context;
        internal readonly MainWindowViewModel _mainViewModel;

        public ProductViewModel(DbyDemoFdbContext context, MainWindowViewModel mainViewModel)
        {
            _context = context;
            _mainViewModel = mainViewModel;
            LoadProducts();
        }

        public void LoadProducts()
{
    var products = _context.Products
        .Include(p => p.ProductType)
        .Include(p => p.MaterialType)
        .Include(p => p.ProductWorkshops)
        .AsNoTracking()
        .ToList();

    Products = new ObservableCollection<Models.Product>(products.Select(p => new Models.Product(p)));
}

[RelayCommand]
private void EditProduct()
{
    if (SelectedProduct == null) return;
    
    var editControl = new EditProductView 
    { 
        DataContext = new EditProductViewModel(_context, SelectedProduct.Entity, this) 
    };
    _mainViewModel.ShowEditProduct(editControl);
}

        [RelayCommand]
        private void AddProduct()
        {
            var editControl = new EditProductView 
            { 
                DataContext = new EditProductViewModel(_context, parentViewModel: this) 
            };
            _mainViewModel.ShowEditProduct(editControl);
        }

        // Метод для обновления списка после редактирования/добавления
        public void RefreshProducts()
        {
            LoadProducts();
        }
    }
}