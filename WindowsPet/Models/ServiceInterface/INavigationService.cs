using System;

namespace WindowsPet.Models.ServiceInterface
{
    public interface INavigationService
    {
        object? CurrentView { get; }
        event Action<object>? CurrentViewChanged;
        void NavigateTo<T>() where T : class;
        void NavigateTo(Type viewType);
        void NavigateTo(object view);
        T GetView<T>() where T : class;
    }
}
