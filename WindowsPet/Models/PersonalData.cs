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
		

        #region UserName
        private string? _name;

		public  string? Name
		{
			get { return _name; }
			set { _name = value; }
		}
        #endregion
        #region UserEmail
        private string? _email;

		public string? Email
		{
			get { return _email; }
			set { _email = value; }
		}
        #endregion
        
		#region
		private string? _userpasswd;

		public string? UserPassword
		{
			get { return _userpasswd; }
			set { _userpasswd = value; }
		}
		#endregion
		#region
		private string? _token;
		[Key]
		public string? Token
		{
			get { return _token; }
			set { _token = value; }
		}

		private decimal _credit;

		public decimal Credit
		{
			get { return _credit; }
			set { _credit = value; }
		}

		#endregion

		public List<Pet>? UserPets=new();

        // 我加了哪些人好友
        public ICollection<Friend> Friend { get; set; } = new List<Friend>();

        public ICollection<Friend> FriendOf { get; set; } = new List<Friend>();

        public ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = new List<FriendRequest>();

    }
    public class Friend
    {
        // 關係表
        public string UserId { get; set; }              // 自己
        public PersonalData User { get; set; } = null!;

        public string FriendId { get; set; }            // 好友
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
        public string FromUserId { get; set; }
        // 接收者
        public string ToUserId { get; set; }

        public string FromUserName { get; set; }

        // 狀態：Pending / Accepted / Rejected
        public FriendRequestStatus Status { get; set; }

        public DateTime RequestTime { get; set; } = DateTime.Now; 

        [JsonIgnore]
        public ICollection<PersonalData> RequestUser = new List<PersonalData>();

        

    }
    public static class CurrentUser
    {
        #region
        private static string? _token;

        public static string? Token
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

		#endregion
	}
	
}
