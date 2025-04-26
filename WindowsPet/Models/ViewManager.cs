using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsPet.VM;

namespace WindowsPet.Models
{
    internal class ViewManager
    {
        private static ViewManager? _instance;
        public static ViewManager Instance => _instance ??= new();
        ConcurrentDictionary<Type, object> ViewList = new();

        public T? GetView<T>() where T : class
        {
                var type = typeof(T);
                if (ViewList.TryGetValue(type, out var view))
                {
                    // If the view is already created, return it
                    // This is a simple way to check if the view is of the correct type
                    MainWindowVM._changeViewAction?.Invoke(view);
                    return view as T;
                }
                else
                {
                    var newView = Activator.CreateInstance<T>();
                    ViewList[type] = newView;
                    MainWindowVM._changeViewAction?.Invoke(ViewList[type]);
                    return newView;
                }               
        }
        private ViewManager(){}
    }
}
