using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementApp.Models;

namespace UserManagementApp.Services
{
    public interface IUserService
    {
        void CreateUser(User user);

        /// <summary>
        /// Gets the users from file.
        /// </summary>
        /// <returns>The list of users.</returns>
        public List<User> GetUsers();
        /// <summary>
        /// Retrieves the user from file with the given ID.
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The user</returns>
        public User GetUser(int id);
        /// <summary>
        /// Updates the user's data.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="editedUser">The user object containing the new user data.</param>
        public void EditUser(int id, User editedUser);
        /// <summary>
        /// Checks if the given credentials are correct.
        /// </summary>
        /// <param name="userName">The given username</param>
        /// <param name="password">The given password</param>
        /// <returns>True if the credentials are correct, false otherwise</returns>
        public bool CredentialsCorrect(string userName, string password);
        /// <summary>
        /// Writes the users to xml.
        /// </summary>
        public void WriteUsersToXml();
    }
}
