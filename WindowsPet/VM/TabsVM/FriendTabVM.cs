using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WindowsPet.Command;
using WindowsPet.Models;
using WindowsPet.Models.RepositoryInterface.DatabaseRepositoryInterface;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.VM.TabsVM
{
    public class FriendTabVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IFriendRepository _friendRepository;
        private readonly IFriendService _friendService;

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
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

        public FriendTabVM(IFriendRepository friendRepository, IFriendService friendService)
        {
            _friendRepository = friendRepository;
            _friendService = friendService;

            AddFriendCommand = new RelayCommands(OnAddFriend);
            PendingFriendRequest = new ObservableCollection<FriendRequest>(_friendRepository.GetPendingFriendRequests());
            Friends = new ObservableCollection<Friend>(_friendRepository.GetUserFriends());
            AcceptCommand = new RelayCommands(OnAccept);
            RejectCommand = new RelayCommands(OnReject);
        }

        private void OnReject(object? obj)
        {
        }

        private void OnAccept(object? obj)
        {
        }

        private void OnAddFriend(object? obj)
        {
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
