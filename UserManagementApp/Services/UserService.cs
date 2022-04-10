using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UserManagementApp.Models;

namespace UserManagementApp.Services
{
    public class UserService : IUserService
    {
        private const string path = "Users.txt";
        public void CreateUser(User user)
        {
            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            int id = GetNextId();

            string paswordHash = ComputeSha256Hash(user.Password);
            File.AppendAllText(path, $"{id}; {user.UserName}; {paswordHash}; {user.SureName}; {user.FirstName}; {user.DateOfBirth}; {user.PlaceOfBirth}; {user.PlaceOfLiving}" + Environment.NewLine);
        }

        private int GetNextId()
        {
            List<User> userList = GetUsers();
            int maxId = 0;

            foreach (var user in userList)
            {
                if (user.Id > maxId)
                    maxId = user.Id;
            }

            return maxId + 1;
        }

        private string ComputeSha256Hash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                return ByteArrayToString(bytes);
            }
        }


        private string ByteArrayToString(byte[] byteArray)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < byteArray.Length; i++)
            {
                builder.Append(byteArray[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public List<User> GetUsers()
        {
            string[] lines = { };
            //If file doesn't exist
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                
            }
            List<User> userList = new List<User>();
            foreach (var line in lines)
            {
                string[] str = line.Split("; ");
                User user = new User
                {
                    Id = int.Parse(str[0]),
                    UserName = str[1],
                    PasswordHash = str[2],
                    SureName = str[3],
                    FirstName = str[4],
                    DateOfBirth = DateTime.Parse(str[5]),
                    PlaceOfBirth = str[6],
                    PlaceOfLiving = str[7],
                };
                userList.Add(user);
            }
            return userList;
        }

        public User GetUser(int id)
        {
            List<User> userList = GetUsers();
            foreach (var user in userList)
            {
                if (user.Id == id)
                    return user;
            }

            return null;
        }

        public void EditUser(int id, User editedUser)
        {
            List<User> userList = GetUsers();
            User userToEdit = new User();
            foreach (var user in userList)
            {
                if (user.Id == id)
                {
                    userToEdit = user;
                    break;
                }
            }
            editedUser.PasswordHash = userToEdit.PasswordHash;
            userList.Remove(userToEdit);
            userList.Add(editedUser);

            WriteUsersToFile(userList);
        }

        private void WriteUsersToFile(List<User> userList)
        {
            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            File.WriteAllText(path, string.Empty);

            foreach (var user in userList)
            {
                File.AppendAllText(path, $"{user.Id}; {user.UserName}; {user.PasswordHash}; {user.SureName}; {user.FirstName}; {user.DateOfBirth}; {user.PlaceOfBirth}; {user.PlaceOfLiving}" + Environment.NewLine);
            }
        }

        public bool CredentialsCorrect(string userName, string password)
        {
            string passwordHash = ComputeSha256Hash(password);
            List<User> userList = GetUsers();
            foreach (var user in userList)
            {
                if (user.UserName.Equals(userName) && user.PasswordHash.Equals(passwordHash))
                    return true;
            }
            return false;
        }

        public void WriteUsersToXml()
        {
            List<User> userList = GetUsers();

            XmlSerializer serialiser = new XmlSerializer(typeof(List<User>));

            TextWriter filestream = new StreamWriter("users.xml");

            serialiser.Serialize(filestream, userList);

            filestream.Close();
        }
    }

}
