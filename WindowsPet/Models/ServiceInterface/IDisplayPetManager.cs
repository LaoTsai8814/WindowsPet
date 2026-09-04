using System;

namespace WindowsPet.Models.ServiceInterface
{
    public interface IDisplayPetManager
    {
        void DisplayPet(Uri peturi);
        void RemoveDisplayPet(Uri peturi);
    }
}
