using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;

namespace WindowsPet.VM.TabsVM
{
    internal class FriendTabVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; }
        }

        public ICommand AddFriendCommand { get; set; }

        public ObservableCollection<FriendRequest> PendingFriendRequest { get; set; } = new();

        public ObservableCollection<Friend> Friends { get; set; } = new();


        public FriendTabVM()
        {
            AddFriendCommand = new RelayCommands(OnAddFriend);
            PendingFriendRequest = new ObservableCollection <FriendRequest> (AppDbContext.Instance.GetPendingFriendRequest());
            Friends = new ObservableCollection<Friend>(AppDbContext.Instance.GetUserFriends());

        }

        private async void OnAddFriend(object? obj)
        {
            if(String.IsNullOrEmpty(SearchText))
            {
                // Show error message
                return;
            }
            await JsonSerialize.SerializeAndSendJson(new SearchFriendRequest
            {
                Token = SearchText,
                UserToken = CurrentUser.Token
            });
            //throw new NotImplementedException();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
