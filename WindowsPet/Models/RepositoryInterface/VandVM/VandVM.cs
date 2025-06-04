using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models.RepositoryInterface.VandVM
{
    public interface IView
    {
        T? GetView<T>() where T:class;

        T? GetViewObject<T>() where T:class;
    }
    
    public interface IViewModel
    {
        T? GetViewModel<T>(object? obj=null) where T : class;
    }
    
}
