using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPet.Models.Repository;
using WindowsPet.Models.RepositoryInterface.Database;
using WindowsPet.Models.ServiceInterface;
using WindowsPet.Views.Tabs;
using WindowsPet.VM.TabsVM;
using static WindowsPet.Models.FileManager;

namespace WindowsPet.Models.Service
{
    public class PetService: IPetService
    {

        private readonly IPetRepository petRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoriesRepository categoriesRepository;

        public PetService(IPetRepository petRepository,IUserRepository userRepository,ICategoriesRepository categoriesRepository)
        {
            this.petRepository = petRepository;
            this.userRepository = userRepository;
            this.categoriesRepository = categoriesRepository;
        }

        public void AddPetListToUser(Guid token, List<Pet> pets)
        {
            // Add a pet list to the user
            if (pets == null)
            {
                return;
            }
            try
            {
                var user = userRepository.GetByToken(token);
                if (user.UserPets == null)
                    user.UserPets = new List<Pet>();
                if (user != null)
                {
                    foreach (var pet in pets)
                    {
                        var trackedPet = petRepository.GetById(pet.PetToken);
                        if (trackedPet != null)
                        {
                            if (!user.UserPets.Any(u => u.PetToken == trackedPet.PetToken))
                            {
                                user.UserPets?.Add(trackedPet);
                            }
                        }
                        else
                        {
                            // 如果是新寵物（未存在 DB），可以選擇先加入 Pets 資料表
                            petRepository.Add(pet);
                            user.UserPets?.Add(pet);
                        }
                    }
                }
                petRepository.Save();
                userRepository.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void AddPetToUser(Guid token, Pet addpet)
        {
            try
            {
                // Add a pet to the user
                var user = userRepository.GetByToken(token);
                if (user == null)
                {
                    // Handle the case where the user is not found
                    return;
                }
                
                if (addpet != null)
                    user.UserPets.Add(addpet);

                userRepository.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //throw new NotImplementedException();
        }
        public void AddSpecificPetListToTable(List<Pet>? petList,PetCategories Type)
        {
            if (petList == null || petList.Count == 0) return;
            try
            {
                categoriesRepository.GetCategoryNameById(Type.Id);
               
            }
            catch (Exception ex)
            {
                categoriesRepository.AddCategory(Type.Id, Type.Type);
                categoriesRepository.Save();
            }

            try
            {
                foreach (var pet in petList)
                {
                    // 檢查資料庫中是否已存在該寵物
                    var existingPet = petRepository.GetById(pet.PetToken); // 這裡假設 GetById 方法會返回 null 如果找不到該寵物
                    if (existingPet == null)
                    {
                        pet.FileFolder = Path.Combine(LocalStorageSetting.LocalCache, Type.Type, pet.PetToken.ToString());
                        categoriesRepository.GetCategoryNameById(Type.Id).Pets.Add(pet); // 將寵物加入到對應的類別中
                        petRepository.Add(pet); // 加入到資料庫的 DbSet<Pet>
                    }
                }

                petRepository.Save(); // 實際寫入資料庫
            }
            catch (Exception ex)
            {
                Console.WriteLine($"新增熱門寵物時失敗: {ex.Message}");
            }
        }
        public void DeletePetFromUser(Guid token, Pet pet)
        {
            throw new NotImplementedException();
        }
        public void DeletePetListFromUser(Guid token, List<Pet> pets)
        {
            throw new NotImplementedException();
        }
        public void UpdatePetFromUser(Guid token, Pet pet)
        {
            throw new NotImplementedException();
        }
        public void UpdatePetListFromUser(Guid token, List<Pet> pets)
        {
            throw new NotImplementedException();
        }
        public void UpdatePetStatusFromUser(Guid token, Pet pet)
        {
            throw new NotImplementedException();
        }
        public bool IsPetOwnByUser(Guid UserId,Guid PetId)
        {
            try
            {
                var pet = userRepository.GetByToken(UserId).UserPets?.FirstOrDefault(p => p.PetToken == PetId);
                if (pet == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            throw new NotImplementedException();
        }

        public bool IsPetPurchased(Guid token, string? petname)
        {

            // 檢查 token 是否有效
            // Check if the pet is purchased by the user
            try
            {
                var user = userRepository.GetByToken(token);
                if (user != null)
                {
                    return user.UserPets?.Any(p => p.Name == petname) ?? false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            //throw new NotImplementedException();
        }

        public List<Pet> GetPetsByCategory(PetCategories PetCategory)
        {
            try
            {
                var pet = petRepository.GetByCategory(PetCategory);
                if (pet != null)
                {
                    return pet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            throw new NotImplementedException();
        }

        
    }
}
