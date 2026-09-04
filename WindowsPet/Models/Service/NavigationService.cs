using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.Models.Service
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, object> _views = new();
        private object? _currentView;

        public object? CurrentView => _currentView;

        public event Action<object>? CurrentViewChanged;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<T>() where T : class
        {
            NavigateTo(typeof(T));
        }

        public void NavigateTo(Type viewType)
        {
            var view = _views.GetOrAdd(viewType, t => _serviceProvider.GetRequiredService(t));
            Console.WriteLine($"Navigating to {viewType.Name}");
            _currentView = view;
            CurrentViewChanged?.Invoke(view);
        }

        public void NavigateTo(object view)
        {
            _currentView = view;
            CurrentViewChanged?.Invoke(view);
        }

        public T GetView<T>() where T : class
        {
            var view = _views.GetOrAdd(typeof(T), t => _serviceProvider.GetRequiredService(t));
            return (T)view;
        }
    }
}
