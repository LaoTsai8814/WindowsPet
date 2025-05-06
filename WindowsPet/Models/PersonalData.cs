using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models
{
    public class PersonalData
    {
		
        public int Id { get; set; }

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

		public string? Token
		{
			get { return _token; }
			set { _token = value; }
		}
		#endregion

		public List<Pet>? UserPets;







    }
}
