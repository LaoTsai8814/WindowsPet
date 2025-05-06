using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPet.VM;

namespace WindowsPet.Models
{
    internal class TabManager
    {
        private static TabManager? _instance;
        public static TabManager Instance => _instance ??= new();
        ConcurrentDictionary<Type,object> TabList = new();
        public T? GetTab<T>() where T : class
        {
            var type = typeof(T);
            if (TabList.TryGetValue(type, out var view))
            {
                // If the view is already created, return it
                // This is a simple way to check if the view is of the correct type
                HomeVM.ChangeTab?.Invoke(view);
                return view as T;
            }
            else
            {
                var newView = Activator.CreateInstance<T>();
                TabList[type] = newView;
                HomeVM.ChangeTab?.Invoke(TabList[type]);
                return newView;
            }
        }
        public T? GetTabObject<T>() where T : class
        {
            var type = typeof(T);
            if (TabList.TryGetValue(type, out var view))
            {
                // If the view is already created, return it
                // This is a simple way to check if the view is of the correct type
                HomeVM.ChangeTab?.Invoke(view);
                return view as T;
            }
            else
            {
                var newView = Activator.CreateInstance<T>();
                TabList[type] = newView;
                return newView;
            }
        }
    }
}
