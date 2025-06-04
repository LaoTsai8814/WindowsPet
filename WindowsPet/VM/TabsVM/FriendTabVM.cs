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
        public ICommand AcceptCommand { get; set; }
        public ICommand RejectCommand { get; set; }

        private ObservableCollection<FriendRequest> _pendingFriendRequest;
        public ObservableCollection<FriendRequest> PendingFriendRequest
        {
            get => _pendingFriendRequest;
            set
            {
                _pendingFriendRequest = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Friend> _friends;
        public ObservableCollection<Friend> Friends
        {
            get => _friends;
            set
            {
                _friends = value;
                OnPropertyChanged();
            }
        }


        public FriendTabVM()
        {
            AddFriendCommand = new RelayCommands(OnAddFriend);
            PendingFriendRequest = new ObservableCollection <FriendRequest> (AppDbContext.Instance.GetPendingFriendRequest());
            
            Friends = new ObservableCollection<Friend>(AppDbContext.Instance.GetUserFriends());
            AcceptCommand = new RelayCommands(OnAccept);
            RejectCommand = new RelayCommands(OnReject);
        }

        private async void OnReject(object? obj)
        {
            
            
        }

        private async void OnAccept(object? obj)
        {
            
        }

        private async void OnAddFriend(object? obj)
        {
            
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
