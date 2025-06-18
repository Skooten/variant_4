using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using DemoApp.Entities;
using DemoApp.Views;

namespace DemoApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly DbyDemoFdbContext _context;

    [ObservableProperty]
    private Control? _content;

    public MainWindowViewModel()
    {
        _context = new DbyDemoFdbContext();
        ShowProductsList(); // По умолчанию показываем список продуктов
    }

    public void ShowContent(Control content)
    {
        Content = content;
    }

    // Показать список партнеров
    public void ShowPartnersList()
    {
        Content = new ProductView
        {
            DataContext = new ProductViewModel(_context, this)
        };
    }

    // Показать список продуктов
    public void ShowProductsList()
    {
        Content = new ProductView
        {
            DataContext = new ProductViewModel(_context, this)
        };
    }

    // Показать форму редактирования продукта
    public void ShowEditProduct(EditProductView editControl)
    {
        Content = editControl;
    }
}
