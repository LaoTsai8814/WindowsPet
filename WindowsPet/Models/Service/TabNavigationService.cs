using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.Models.Service
{
    public class TabNavigationService : ITabNavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, object> _tabs = new();
        private object? _currentTab;

        public object? CurrentTab => _currentTab;

        public event Action<object>? CurrentTabChanged;

        public TabNavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<T>() where T : class
        {
            NavigateTo(typeof(T));
        }

        public void NavigateTo(Type tabType)
        {
            var tab = _tabs.GetOrAdd(tabType, t => _serviceProvider.GetRequiredService(t));
            _currentTab = tab;
            CurrentTabChanged?.Invoke(tab);
        }

        public void NavigateTo(object tab)
        {
            _currentTab = tab;
            CurrentTabChanged?.Invoke(tab);
        }

        public T GetTab<T>() where T : class
        {
            var tab = _tabs.GetOrAdd(typeof(T), t => _serviceProvider.GetRequiredService(t));
            return (T)tab;
        }
    }
}
