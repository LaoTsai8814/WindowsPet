using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models
{
    internal class ViewModelManager
    {
        /// <summary>
        /// Key :View
        /// Value : ViewModel
        /// </summary>
        private static ViewModelManager? _viewManager;
        public static ViewModelManager Instance => _viewManager ??= new();

        ConcurrentDictionary<object, object> _viewModelDictionary = new();

        public T? GetViewModel<T>(object view) where T : class
        {
            var type = typeof(T);
            if (_viewModelDictionary.TryGetValue(view, out var viewModel))
            {
                // If the view is already created, return it
                // This is a simple way to check if the view is of the correct type
                return viewModel as T;
            }
            else
            {
                var newViewModel = Activator.CreateInstance<T>();
                _viewModelDictionary[view] = newViewModel;
                return newViewModel;
            }
        }
        ViewModelManager()
        {
        }
    }
}
