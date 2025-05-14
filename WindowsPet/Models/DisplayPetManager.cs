using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourGodDamnPet;

namespace WindowsPet.Models
{
    internal class DisplayPetManager
    {
        private static DisplayPetManager? _instance;
        public static DisplayPetManager? Instance => _instance ??= new();

        Stack<YourGodDamnPet.MainWindow> WindowStack = new();
        PetStartup petstartup;



        internal void DisplayPet(Uri peturi)
        {
            petstartup ??= new();
            petstartup.ShowWindow(peturi,out var mainwindow);
            WindowStack.Push(mainwindow);
        }

        internal void RemoveDisplayPet(Uri peturi)
        {
            var window =  WindowStack.Pop();
            
            window.Close();
        }





    }
}
