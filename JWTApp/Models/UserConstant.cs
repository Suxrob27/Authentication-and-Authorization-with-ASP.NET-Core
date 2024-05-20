namespace JWTApp.Models
{
    public class UserConstant
    {
        public static List<UserModel> Users = new List<UserModel>()
        { 
         new UserModel { Id = 1, Name = "Suxrob", Email= "suxrobvjl1@gmail.com",Password = "suxrob123",Role = "Backend Dev"},
         new UserModel { Id = 2, Name = "Otabek", Email= "example@gmail.com",Password = "otabek123",Role = "Backend Dev"},
        };
    }
}
