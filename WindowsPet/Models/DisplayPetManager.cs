using System;
using System.Collections.Generic;
using WindowsPet.Models.ServiceInterface;
using YourGodDamnPet;

namespace WindowsPet.Models
{
    public class DisplayPetManager : IDisplayPetManager
    {
        private readonly Stack<YourGodDamnPet.MainWindow> _windowStack = new();
        private PetStartup? _petStartup;

        public void DisplayPet(Uri peturi)
        {
            if (peturi == null) return;
            _petStartup ??= new PetStartup();
            _petStartup.ShowWindow(peturi, out var mainWindow);
            _windowStack.Push(mainWindow);
        }

        public void RemoveDisplayPet(Uri peturi)
        {
            if (_windowStack.Count > 0)
            {
                var window = _windowStack.Pop();
                window.Close();
            }
        }
    }
}
