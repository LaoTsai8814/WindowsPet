using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;

namespace WindowsPet.Models
{
    public class PersonalData
    {
        [Key]
        public Guid Token { get; set; }
        public string Name { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public decimal Credit { get; set; }
		public List<Pet>? UserPets=new();
        // 我加了哪些人好友
        public ICollection<Friend> Friend { get; set; } = new List<Friend>();
        public ICollection<Friend> FriendOf { get; set; } = new List<Friend>();
        public ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = new List<FriendRequest>();
    }
    public class Friend
    {
        // 關係表
        public Guid UserId { get; set; }              // 自己
        public PersonalData User { get; set; } = null!;

        public Guid FriendId { get; set; }            // 好友
        public PersonalData FriendUser { get; set; } = null!;

        public Guid? Token { get; set; }             // 好友的 Token（快取用）
        public string Name { get; set; } = "";       // 好友名字（快取）
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 非必要，如果你確實要快取寵物清單：
        public ICollection<Pet>? FriendOwningPets { get; set; }
    }
    public enum FriendRequestStatus
    {
        Pending,
        Accepted,
        Rejected
    }
    public class FriendRequest
    {

        public int Id { get; set; }

        // 送出者
        public Guid FromUserId { get; set; }
        // 接收者
        public Guid ToUserId { get; set; }

        public string FromUserName { get; set; }

        // 狀態：Pending / Accepted / Rejected
        public FriendRequestStatus Status { get; set; }

        public DateTime RequestTime { get; set; } = DateTime.Now; 

        [JsonIgnore]
        public ICollection<PersonalData> RequestUser = new List<PersonalData>();

        

    }
    public static class CurrentUser
    {
        
        private static Guid _token;

        public static Guid Token
        {
            get { return _token; }
            set { _token = value; }
        }
		private static decimal _credit;

		public static decimal Credit
		{
			get { return _credit; }
			set { _credit = value; }
		}

		
	}
	
}
