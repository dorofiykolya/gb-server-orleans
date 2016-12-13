using Database.DatabaseDataSetTableAdapters;

namespace Database
{
    public class DatabaseUsers
    {
        private static readonly DatabaseUsers Instance = new DatabaseUsers();

        public DatabaseUsers()
        {
            Adapter = new UsersTableAdapter();
        }

        public UsersTableAdapter Adapter { get; }

        public static int GetUserIdByDeveloperId(string developerId)
        {
            var data = Instance.Adapter.GetDataByDeveloperId(developerId);
            return data.Count != 0 ? data[0].UserId : -1;
        }
    }
}
