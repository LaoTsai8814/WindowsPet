using System;

namespace WindowsPet.Models.ServiceInterface
{
    public interface ITabNavigationService
    {
        object? CurrentTab { get; }
        event Action<object>? CurrentTabChanged;
        void NavigateTo<T>() where T : class;
        void NavigateTo(Type tabType);
        void NavigateTo(object tab);
        T GetTab<T>() where T : class;
    }
}
