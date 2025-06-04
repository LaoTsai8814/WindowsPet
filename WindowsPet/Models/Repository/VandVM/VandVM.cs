using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using WindowsPet.Models.RepositoryInterface.VandVM;
using WindowsPet.VM;
namespace WindowsPet.Models.Repository.VandVM
{
    public abstract class BaseViewProvider
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ConcurrentDictionary<Type, object> _views = new();

        protected BaseViewProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T? GetView<T>(Action<object>? onNavigate = null) where T : class
        {
            var type = typeof(T);
            if (_views.TryGetValue(type, out var view))
            {
                onNavigate?.Invoke(view);
                return view as T;
            }
            else
            {
                var newView = _serviceProvider.GetRequiredService<T>();
                _views[type] = newView;
                onNavigate?.Invoke(newView);
                return newView;
            }
        }

        public T? GetViewObject<T>() where T : class
        {
            var type = typeof(T);
            if (_views.TryGetValue(type, out var view))
            {
                return view as T;
            }
            else
            {
                var newView = _serviceProvider.GetRequiredService<T>();
                _views[type] = newView;
                return newView;
            }
        }
    }
    public class View : BaseViewProvider, RepositoryInterface.VandVM.IView
    {
        ConcurrentDictionary<Type, object> ViewList = new();
        public View(IServiceProvider sp) : base(sp) { }

        public T? GetView<T>() where T : class =>
            base.GetView<T>(MainWindowVM._changeViewAction);
    }
    public class Tab : BaseViewProvider, RepositoryInterface.VandVM.IView
    {
        public Tab(IServiceProvider sp) : base(sp) { }
        public T? GetView<T>() where T : class =>
            base.GetView<T>(HomeVM.ChangeTab);
    }
    public class ViewModel : RepositoryInterface.VandVM.IViewModel
    {
        protected readonly IServiceProvider _serviceProvider;
        public ViewModel(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }

        ConcurrentDictionary<object, object> _viewModelDictionary = new();

        public T? GetViewModel<T>(object? view) where T : class
        {
            if (_viewModelDictionary.TryGetValue(view!, out var vm))
                return vm as T;

            var newVM = _serviceProvider.GetRequiredService<T>();
            _viewModelDictionary[view!] = newVM;
            return newVM;
        }
    }
}
